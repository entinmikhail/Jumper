using System.Collections;
using UnityEngine;

public interface ICoroutineRunner
{
    Coroutine StartCoroutine(IEnumerator coroutine);
}

public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
{
    private void Awake() => DontDestroyOnLoad(this);
}