using System;
using System.Collections;
using UnityEngine.SceneManagement;
using Zenject;

public interface ISceneLoader
{
    void Load(string name, Action onLoaded = null);
}

public class SceneLoader : ISceneLoader
{
    [Inject] private ICoroutineRunner _coroutineRunner;

    public void Load(string name, Action onLoaded = null) => _coroutineRunner.StartCoroutine(LoadScene(name, onLoaded));
    
    private IEnumerator LoadScene(string nextSceneName, Action onLoaded = null)
    {
        if (SceneManager.GetActiveScene().name == nextSceneName)
        {
            onLoaded?.Invoke();
            yield break;
        }
        
        var waitNextScene = SceneManager.LoadSceneAsync(nextSceneName);

        while (!waitNextScene.isDone)
            yield return null;

        yield return null;
        onLoaded?.Invoke();
    }
}