using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Server
{
    public class ServerTEST : MonoBehaviour
    {

        [SerializeField] private Button _button;
        [SerializeField] private Button _buttonPost;
        [SerializeField] private TextMeshProUGUI _text;

        private string _data;
        private void Awake()
        {
            _button.onClick.AddListener(RefreshGetData);
            _buttonPost.onClick.AddListener(RefreshPostData);
        }

        private void RefreshGetData() => StartCoroutine(GetDataCoroutine());
        private void RefreshPostData() => StartCoroutine(PostDataCoroutine());

        private IEnumerator GetDataCoroutine()
        {
            string url = "https://api-dev.inout.games/games/jumper/getState";
            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();
                
                _text.text = request.downloadHandler.text;
            }
        }
        
        private IEnumerator PostDataCoroutine()
        {
            string url = "https://api-dev.inout.games/games/jumper/getState";
            
            WWWForm from = new WWWForm();
            from.AddField("ttle", "test data");
            
            using (UnityWebRequest request = UnityWebRequest.Post(url, from))
            {
                yield return request.SendWebRequest();
                
                _text.text = request.downloadHandler.text;
            }
        }
    }
}