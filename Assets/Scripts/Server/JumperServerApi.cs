﻿using System;
using System.Collections;
using System.Text;
using GameModels;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Server
{
    public interface IJumperServerApi
    {
        event Action<GetStateResponse> StateGet;
        event Action<BetResponse> BetGet;
        event Action<JumpResponse> JumpGet;
        event Action<CashoutResponse> CashoutGet;
        
        void AuthRequest(AuthRequest authRequest, Action callback = null, Action badCallback = null);
        void GetState(Action callback = null , Action badCallback = null );
        void ToBet(BetRequest betRequest, Action callback = null, Action badCallback = null);
        void Jump(Action callback = null, Action badCallback = null);
        void Cashout(Action callback = null, Action badCallback = null);
    }

    public class JumperJumperServerApi : IJumperServerApi
    {
        public event Action<GetStateResponse> StateGet;
        public event Action<BetResponse> BetGet;
        public event Action<JumpResponse> JumpGet;
        public event Action<CashoutResponse> CashoutGet;

        private string _authToken;

        [Inject] private ICoroutineRunner _coroutineRunner;
        [Inject] private IGameHandler _gameHandler;
        
        public void AuthRequest(AuthRequest authRequest, Action callback = null, Action badCallback = null) => _coroutineRunner.StartCoroutine(AuthRequestCoroutine(authRequest, callback, badCallback));
        public void GetState(Action callback = null, Action badCallback = null) => _coroutineRunner.StartCoroutine(GetStateRequestCoroutine(callback, badCallback));
        public void ToBet(BetRequest betRequest, Action callback = null, Action badCallback = null) => 
            _coroutineRunner.StartCoroutine(BetRequestCoroutine(betRequest, callback, badCallback));
        public void Jump(Action callback = null, Action badCallback = null) => _coroutineRunner.StartCoroutine(JumpRequestCoroutine(callback, badCallback));
        public void Cashout(Action callback = null, Action badCallback = null) => _coroutineRunner.StartCoroutine(CashoutRequestCoroutine(callback, badCallback));


        private IEnumerator AuthRequestCoroutine(AuthRequest authRequest, Action callback, Action badCallback)
        {
            var from = new WWWForm();
            var url = "https://api-dev.inout.games/api/auth";
            from.AddField("operator", authRequest.@operator);
            from.AddField("auth_token", authRequest.authToken);
            from.AddField("currency", authRequest.currency);
            using var request = UnityWebRequest.Post(url, from);
            
            yield return request.SendWebRequest();
            
            var response = JsonUtility.FromJson<AuthResponse>(request.downloadHandler.text);
            Debug.LogError(request.downloadHandler.text);

            if (response.success)
            {
                _authToken = response.result;
                callback?.Invoke();
            }
            else
            {
                badCallback?.Invoke();
            }
        }

        private IEnumerator GetStateRequestCoroutine( Action callback, Action badCallback)
        {
            var from = new WWWForm();
            var url = "https://api-dev.inout.games/games/jumper/getState";
            using var request = UnityWebRequest.Post(url, from);
            request.SetRequestHeader("Authorization", "Bearer " + _authToken);
            
            yield return request.SendWebRequest();
            
            var response = JsonUtility.FromJson<GetStateResponse>(request.downloadHandler.text);
            response.success = request.downloadHandler.isDone;
            
            if (response.success)
            {
                Debug.LogError(request.downloadHandler.text);
                _gameHandler.InitializeHandle(request.downloadHandler.text == "{}" ? null : response);

                StateGet?.Invoke(response);
                callback?.Invoke();
            }
            else
            {
                badCallback?.Invoke();
            }
        }
        
        private IEnumerator BetRequestCoroutine(BetRequest betRequest, Action callback, Action badCallback)
        {
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
                Debug.LogError(request.downloadHandler.text);

                if (response.success)
                {
                    _gameHandler.BonusBetHandle(response);
                    callback?.Invoke();
                }
                else
                    badCallback?.Invoke();
            }
            else
            {
                var response = JsonUtility.FromJson<BetResponse>(request.downloadHandler.text);
                response.success = request.downloadHandler.isDone;
                Debug.LogError(request.downloadHandler.text);

                if (response.success)
                {
                    BetGet?.Invoke(response);
                    _gameHandler.BetHandle(response);
                    callback?.Invoke();
                }
                else
                    badCallback?.Invoke();
            }
        }
        
        private IEnumerator JumpRequestCoroutine(Action callback, Action badCallback)
        {
            var from = new WWWForm();
            var url = "https://api-dev.inout.games/games/jumper/nextStep";
            using var request = UnityWebRequest.Post(url, from);
            request.SetRequestHeader("Authorization", "Bearer " + _authToken);
            
            yield return request.SendWebRequest();
            
            var response = JsonUtility.FromJson<JumpResponse>(request.downloadHandler.text);
            response.success = request.downloadHandler.isDone;
            Debug.LogError(request.downloadHandler.text);

            if (response.success)
            {
                callback?.Invoke();
                _gameHandler.JumpHandle(response);
                JumpGet?.Invoke(response);
            }
            else
            {
                badCallback?.Invoke();
            }
        }
        private IEnumerator CashoutRequestCoroutine(Action callback, Action badCallback)
        {
            var from = new WWWForm();
            var url = "https://api-dev.inout.games/games/jumper/cashout";
            using var request = UnityWebRequest.Post(url, from);
            request.SetRequestHeader("Authorization", "Bearer " + _authToken);
            
            yield return request.SendWebRequest();
            
            var response = JsonUtility.FromJson<CashoutResponse>(request.downloadHandler.text);
            response.success = request.downloadHandler.isDone;
            Debug.LogError(request.downloadHandler.text);

            if (response.success)
            {
                callback?.Invoke();
                _gameHandler.CashOutHandle(response);
                CashoutGet?.Invoke(response);
            }
            else
            {
                badCallback?.Invoke();
            }
        }
    }
}