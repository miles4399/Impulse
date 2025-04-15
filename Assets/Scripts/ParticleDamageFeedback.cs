using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamageFeedback : MonoBehaviour
{
    private ParticleSystem _damageFeedback;

    private void Start()
    {
        _damageFeedback = GetComponent<ParticleSystem>();
    }

    public void PlayParticles(float duration)
    {
        if (_damageFeedback.isPlaying) _damageFeedback.Stop();
        ParticleSystem.MainModule main = _damageFeedback.main;
        main.duration = duration;
        _damageFeedback.Play();
    } 

  
}
