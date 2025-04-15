using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinSwitch : MonoBehaviour
{
    [SerializeField] private Timer _timer;
    private void OnCollisionEnter(Collision other)
    {        
        _timer?.EndTime();
        SceneManager.LoadScene("ScoreScene");       
    }
}
