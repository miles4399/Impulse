using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSwitch : MonoBehaviour
{
    [SerializeField] private string _sceneName;

    private void OnCollisionEnter(Collision other)
    {
        SceneManager.LoadScene(_sceneName);
    }
}
