using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Infrastructure
{
    public class SceneLoader
    {
        private readonly ICoroutineRunner _coroutineRunner;

        public SceneLoader(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }
        
        public void Load(string sceneName, Action onSceneLoad = null)
        {
            _coroutineRunner.StartCoroutine(LoadScene(sceneName, onSceneLoad));
        }
        
        private IEnumerator LoadScene(string sceneName, Action onSceneLoad)
        {
            if (SceneManager.GetActiveScene().name == sceneName)
            {
                onSceneLoad?.Invoke();
                yield break;
            }
            
            AsyncOperation waitNextScene = SceneManager.LoadSceneAsync(sceneName);

            while (!waitNextScene.isDone)
            {
                yield return null;
            }
            
            onSceneLoad?.Invoke();
        }
    }
}