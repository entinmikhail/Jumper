using System;
using System.Collections;
using UnityEngine;

public interface ICoroutineRunner
{
    Coroutine StartCoroutine(IEnumerator coroutine);
    void StartAfterDelay(float waitSeconds, Action action);
}

public class CoroutineRunner : MonoBehaviour, ICoroutineRunner
{
    private void Awake() => DontDestroyOnLoad(this);


    public void StartAfterDelay(float waitSeconds, Action action)
    {
        StartCoroutine(CoroutineAfterDelay(waitSeconds, action));
    }
    
    private IEnumerator CoroutineAfterDelay(float waitSeconds, Action action)
    {
        yield return new WaitForSeconds(waitSeconds);
        action.Invoke();
    }
}