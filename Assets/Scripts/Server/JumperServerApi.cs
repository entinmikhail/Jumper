using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using BestHTTP.SocketIO3;
using BestHTTP.SocketIO3.Transports;
using BestHTTP.PlatformSupport.IL2CPP;
using GameModels;
using UIControllers;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Server
{
    
    [Preserve]
    public class BetsRangesData
    {
        [Preserve] public double MinBet;
        [Preserve] public double MaxBet;

        public BetsRangesData(double minBet, double maxBet)
        {
            MinBet = minBet;
            MaxBet = maxBet;
        }
    }
    
    
    [Preserve]
    public class OnBalanceChangeData
    {
        [Preserve] public string currency;
        [Preserve] public float balance;
    }
    
    public interface IJumperServerApi
    {
        event Action<GetStateResponse> StateGet;
        event Action<BetResponse> BetGet;
        event Action<JumpResponse> JumpGet;
        event Action<CashoutResponse> CashoutGet;
        event Action RequestSentEvent;
        event Action ResponseReceivedEvent;
        bool IsBusy { get; set; }

        void AuthRequest(Action callback = null, Action badCallback = null);
        void GetState(Action callback = null , Action badCallback = null );
        void ToBet(BetRequest betRequest, Action callback = null, Action badCallback = null);
        void Jump(Action callback = null, Action badCallback = null);
        void Cashout(Action callback = null, Action badCallback = null);
        void InitializeSocketIO();
        
        void Init(INotificationService notificationService);
    }

    public class JumperJumperServerApi : IJumperServerApi
    {
        public event Action<GetStateResponse> StateGet;
        public event Action<BetResponse> BetGet;
        public event Action<JumpResponse> JumpGet;
        public event Action<CashoutResponse> CashoutGet;
        public event Action RequestSentEvent;
        public event Action ResponseReceivedEvent;
        public bool IsBusy { get; set; }

        private string _authToken;
        private string _operatorId;
        private float _waitSeconds = 2;
        
        private INotificationService _notificationService;
        [Inject] private IButtonsLockService _buttonsLockService;
        [Inject] private ICoroutineRunner _coroutineRunner;
        [Inject] private IGameHandler _gameHandler;

        public void Init(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        public JumperJumperServerApi()
        {
            RequestSentEvent += () => IsBusy = true;
            ResponseReceivedEvent += () => IsBusy = false;
        }

        public void AuthRequest(Action callback = null, Action badCallback = null) => _coroutineRunner.StartCoroutine(AuthRequestCoroutine(callback, badCallback));
        public void GetState(Action callback = null, Action badCallback = null) => _coroutineRunner.StartCoroutine(GetStateRequestCoroutine(callback, badCallback));
        public void ToBet(BetRequest betRequest, Action callback = null, Action badCallback = null) => _coroutineRunner.StartCoroutine(BetRequestCoroutine(betRequest, callback, badCallback));
        public void Jump(Action callback = null, Action badCallback = null) => _coroutineRunner.StartCoroutine(JumpRequestCoroutine(callback, badCallback));
        public void Cashout(Action callback = null, Action badCallback = null) => _coroutineRunner.StartCoroutine(CashoutRequestCoroutine(callback, badCallback));


        public void InitializeSocketIO()
        {
            SocketOptions options = new SocketOptions();
            options.AutoConnect = false;
            options.ConnectWith = TransportTypes.WebSocket;

            var manager = new SocketManager(new Uri($"wss://api-dev.inout.games/io/?Authorization={_authToken}&operatorId={_operatorId}"), options);
            manager.Socket.On("connect", () => Debug.Log("connected"));
            
            manager.Socket.On<OnBalanceChangeData>("onBalanceChange", OnBalanceChanged);
            manager.Socket.On<Dictionary<string, object[]>>("betsRanges", OnBetsRanges);
            manager.Socket.On<Dictionary<string, object>>("currencies", OnCurrencies);
            
            manager.Open();
        }

        private void OnBetsRanges(Dictionary<string, object[]> data)
        {
            var values = data.First().Value;
            _gameHandler.OnBetsRanges(new BetsRangesData((double)values[0], (double)values[1]));
            Debug.LogError("OnBetsRanges");
        }

        private void OnCurrencies(Dictionary<string, object> data)
        {
            _gameHandler.OnCurrencies(data);
            Debug.LogError("OnCurrencies");
        }

        private void OnBalanceChanged(OnBalanceChangeData data)
        {
            _gameHandler.OnBalanceChanged(data);
            Debug.LogError("OnBalanceChanged");
        }

        private IEnumerator AuthRequestCoroutine(Action callback, Action badCallback)
        {
            RequestSentEvent?.Invoke();
            Uri appUrl = null;
#if UNITY_WEBGL && !UNITY_EDITOR
             appUrl = new Uri(Application.absoluteURL);

            if (appUrl == null)
                Debug.LogError("Uri does not hav data");
#endif
            
#if UNITY_EDITOR
            appUrl = new Uri("http://localhost/UnityBuilds/Jumper?operatorId=3bdda719-8b47-4e19-9282-4ea1df4b1da5&authToken=be67dc74323f3f1142f6152aa3ff0d32&currency=INR");
#endif


            _operatorId = HttpUtility.ParseQueryString(appUrl.Query).Get("operatorId");
            string authToken = HttpUtility.ParseQueryString(appUrl.Query).Get("authToken");
            string currency = HttpUtility.ParseQueryString(appUrl.Query).Get("currency");
            string lang = HttpUtility.ParseQueryString(appUrl.Query).Get("lang");
            
            var from = new WWWForm();
            var url = "https://api-dev.inout.games/api/auth";
            from.AddField("operator", _operatorId);
            from.AddField("auth_token", authToken);
            from.AddField("currency", currency);
            using var request = UnityWebRequest.Post(url, from);
            
            yield return request.SendWebRequest();
            
            var response = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);
            Debug.Log(request.downloadHandler.text);

            if (response.success)
            {
                _authToken = response.result;
                callback?.Invoke();
            }
            else
            {
                // _notificationService.ShowNotification(request.downloadHandler.text);
                badCallback?.Invoke();
            }
            
            ResponseReceivedEvent?.Invoke();
        }

        private IEnumerator GetStateRequestCoroutine(Action callback, Action badCallback)
        {
            RequestSentEvent?.Invoke();

            var from = new WWWForm();
            var url = "https://api-dev.inout.games/games/jumper/getState";
            using var request = UnityWebRequest.Post(url, from);
            request.SetRequestHeader("Authorization", "Bearer " + _authToken);
            
            yield return request.SendWebRequest();
            
            var response = JsonUtility.FromJson<GetStateResponse>(request.downloadHandler.text);
            response.success = request.downloadHandler.isDone;
            
            if (response.success)
            {
                Debug.Log(request.downloadHandler.text);
                _gameHandler.InitializeHandle(request.downloadHandler.text == "{}" ? null : response);

                StateGet?.Invoke(response);
                callback?.Invoke();
            }
            else
            {
                _notificationService.ShowNotification(request.downloadHandler.text);
                _notificationService.ShowNotification("Потеряно соединение, пожалуйста перезайдите");
                // _buttonsLockService.LockAllButtons();
                // AuthRequest();
                badCallback?.Invoke();
            }
            
            _coroutineRunner.StartAfterDelay(_waitSeconds, () =>
            {
                ResponseReceivedEvent?.Invoke();
            });
        }
        
        private IEnumerator BetRequestCoroutine(BetRequest betRequest, Action callback, Action badCallback)
        {
            // RequestSentEvent?.Invoke();

            var url = "https://api-dev.inout.games/games/jumper/bet";
            var request = new UnityWebRequest(url, "POST");
            var bodyJsonString = JsonUtility.ToJson(betRequest);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + _authToken);
            
            yield return request.SendWebRequest();
            
            if (betRequest.isWithBonus)
            {
                var response = JsonUtility.FromJson<BonusBetResponse>(request.downloadHandler.text);
                response.success = request.downloadHandler.isDone;
                Debug.Log(request.downloadHandler.text);

                if (response.success)
                {
                    _gameHandler.BonusBetHandle(response);
                    callback?.Invoke();
                }
                else
                {
                    _notificationService.ShowNotification(request.downloadHandler.text);
                    _notificationService.ShowNotification("Потеряно соединение, пожалуйста перезайдите");
                    // _buttonsLockService.LockAllButtons();

                    badCallback?.Invoke();
                }
            }
            else
            {
                var response = JsonUtility.FromJson<BetResponse>(request.downloadHandler.text);
                response.success = request.downloadHandler.isDone;
                Debug.Log(request.downloadHandler.text);

                if (response.success)
                {
                    BetGet?.Invoke(response);
                    _gameHandler.BetHandle(response);
                    callback?.Invoke();
                }
                else
                {
                    _notificationService.ShowNotification(request.downloadHandler.text);
                    _notificationService.ShowNotification("Потеряно соединение, пожалуйста перезайдите");
                    // _buttonsLockService.LockAllButtons();
                }
            }
            
            request.Dispose();
            
            // _coroutineRunner.StartAfterDelay(_waitSeconds, () =>
            // {
            //     ResponseReceivedEvent?.Invoke();
            // });
        }
        
        private IEnumerator JumpRequestCoroutine(Action callback, Action badCallback)
        {
            // RequestSentEvent?.Invoke();

            var from = new WWWForm();
            var url = "https://api-dev.inout.games/games/jumper/nextStep";
            using var request = UnityWebRequest.Post(url, from);
            request.SetRequestHeader("Authorization", "Bearer " + _authToken);
            
            yield return request.SendWebRequest();
            
            var response = JsonUtility.FromJson<JumpResponse>(request.downloadHandler.text);
            response.success = request.downloadHandler.isDone;
            Debug.Log(request.downloadHandler.text);

            if (response.success)
            {
                callback?.Invoke();
                _gameHandler.JumpHandle(response);
                JumpGet?.Invoke(response);
            }
            else
            {
                _notificationService.ShowNotification(request.downloadHandler.text);
                _notificationService.ShowNotification("Потеряно соединение, пожалуйста перезайдите");
                _buttonsLockService.LockAllButtons();


            }
            
            // _coroutineRunner.StartAfterDelay(_waitSeconds, () =>
            // {
            //     ResponseReceivedEvent?.Invoke();
            // });
        }
        
        private IEnumerator CashoutRequestCoroutine(Action callback, Action badCallback)
        {
            RequestSentEvent?.Invoke();

            var from = new WWWForm();
            var url = "https://api-dev.inout.games/games/jumper/cashout";
            using var request = UnityWebRequest.Post(url, from);
            request.SetRequestHeader("Authorization", "Bearer " + _authToken);
            
            yield return request.SendWebRequest();
            
            var response = JsonUtility.FromJson<CashoutResponse>(request.downloadHandler.text);
            response.success = request.downloadHandler.isDone;
            Debug.Log(request.downloadHandler.text);

            if (response.success)
            {
                callback?.Invoke();
                _gameHandler.CashOutHandle(response);
                CashoutGet?.Invoke(response);
            }
            else
            {
                _notificationService.ShowNotification(request.downloadHandler.text);
                _notificationService.ShowNotification("Потеряно соединение, пожалуйста перезайдите");
                _buttonsLockService.LockAllButtons();


                badCallback?.Invoke();
            }

            _coroutineRunner.StartAfterDelay(_waitSeconds, () =>
            {
                ResponseReceivedEvent?.Invoke();
            });
        }
    }
}