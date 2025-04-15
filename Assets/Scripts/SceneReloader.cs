using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneReloader : MonoBehaviour
{
    [SerializeField] Health _character;
    private string _currentScene;
    

    void Start()
    {
        Scene scene = SceneManager.GetActiveScene();
        _currentScene = scene.name;
    }



    public void Reload()
    {
        //_character.Death();

    }
}
