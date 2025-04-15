using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenGlitchFeedback : MonoBehaviour
{
    [SerializeField] private Material _glitchFeedback;
    [SerializeField] private float _strengthValue = 0.1f;
    [SerializeField] private float _duration = 0.5f;

    private void Start()
    {
        _glitchFeedback.SetFloat("_Strength", 0f);
    }

    public void StartGlitch()
    {
        _glitchFeedback.SetFloat("_Strength", _strengthValue);
        StartCoroutine(WaitForGlitchTimer());
    }

    private IEnumerator WaitForGlitchTimer()
    {
        yield return new WaitForSeconds(_duration);
        _glitchFeedback.SetFloat("_Strength", 0f);
    }
}
