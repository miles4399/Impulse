using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _previousText;
    [SerializeField] private TextMeshProUGUI _fastestText;
    [SerializeField, FMODUnity.EventRef] private string FMODEventOnNewRecord;
    [SerializeField] private UnityEvent _onNewRecord;
    private TimeSpan _previousTime;
    private TimeSpan _fastestTime;

    private void Awake()
    {

        _previousTime = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("timeValue"));
        _fastestTime = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("timeRecord"));
        _previousText.text = _previousTime.ToString("mm':'ss'.'ff");
        _fastestText.text = _fastestTime.ToString("mm':'ss'.'ff");
        

    }

    void Start()
    {
        if (PlayerPrefs.GetFloat("timeValue") == PlayerPrefs.GetFloat("timeRecord")) _onNewRecord.Invoke();     //Audio
        //Debug.Log(PlayerPrefs.GetFloat(_previousTime));
    }

    public void ResetScore()
    {
        PlayerPrefs.DeleteKey("timeVaule");
        PlayerPrefs.DeleteKey("timeRecord");
    }



}
