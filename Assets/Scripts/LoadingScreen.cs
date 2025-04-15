using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private string _levelName;
    [SerializeField] private Slider _slider;
    private void Start()
    {
        LoadLevel(_levelName);
    }

    public void LoadLevel (string sceneName)
    {
        StartCoroutine(LoadAsynchronously(sceneName));
        
    }

    private IEnumerator LoadAsynchronously (string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            Debug.Log(operation.progress);
            
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            _slider.value = progress;

            yield return null;
        }
    }
}
