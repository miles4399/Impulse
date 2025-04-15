using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
    {
    private float _currentTime;
    private bool _timeGoing;
    private TextMeshProUGUI _timerCounter;
    private TimeSpan _timePlaying;
    private float _fastestTime;
    private bool _time1Reach;
    private bool _time2Reach;
    private bool _time3Reach;


    private void Awake()
    {
        _timerCounter = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        _timerCounter.text = "Time: 00:00.00";
        _timeGoing = false;
        BeginTimer();
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            float time1 = 0f;
            time1 += Time.deltaTime;
            _time1Reach = false;
            if (time1 < 1f) _time1Reach = true;
            else _time1Reach = false;

        }
        if ( _time1Reach  && Input.GetKeyDown(KeyCode.V))
        {
            float time2 = 0f;
            time2 += Time.deltaTime;
            _time2Reach = false;
            if (time2 < 1f) _time2Reach = true;
            else _time2Reach = false;
        }
        if (_time2Reach && Input.GetKeyDown(KeyCode.N))
        {
            float time3 = 0f;
            time3 += Time.deltaTime;
            _time3Reach = false;
            if (time3 < 1f) _time3Reach = true;
            else _time3Reach = false;
        }
        if (_time3Reach && Input.GetKeyDown(KeyCode.C) && Input.GetKeyDown(KeyCode.B)) EndTime();


    }

    public void BeginTimer()
    {
        _timeGoing = true;
        _currentTime = 0f;
        StartCoroutine(UpdateTimeer());
    }

    public void ResetTimer()
    {

        _currentTime = 0f;
        _timeGoing = false;
        _timerCounter.text = "Time: 00:00.00";
        _timeGoing = true;
    }

    public void EndTime()
    {
        _timeGoing = false;
        if (_currentTime <= PlayerPrefs.GetFloat("timeValue") || PlayerPrefs.HasKey("timeRecord") == false)
        {
            PlayerPrefs.SetFloat("timeRecord", _currentTime);
        }
        PlayerPrefs.SetFloat("timeValue", _currentTime);

        SceneManager.LoadScene("ScoreScene");



    }



    private IEnumerator UpdateTimeer()
    {
        while(_timeGoing)
        {
            _currentTime += Time.deltaTime;
            _timePlaying = TimeSpan.FromSeconds(_currentTime);
            
            _timerCounter.text = _timePlaying.ToString("mm':'ss'.'ff");
            yield return null;
        }
    }

    

}
