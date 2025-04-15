using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpeedControl : MonoBehaviour
{
    [SerializeField] private float _velocity = 7f;
    [SerializeField] private float _rate = 35f;

    private ParticleSystem _ps;
    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
    }
    public void SetSpeedPercentage(float percentage)
    {
        var Emission = _ps.emission;
        Emission.rateOverTime = _rate * percentage;
        var main = _ps.main;
        main.emitterVelocity = _velocity * transform.forward * percentage;
    }
}
