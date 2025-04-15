using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Shake : MonoBehaviour
{
    CinemachineImpulseSource impulse;
    public static Shake Instance { get; private set; }
    private void Start()
    {
        impulse = transform.GetComponent<CinemachineImpulseSource>();

    }
    private void Awake()
    {
        Instance = this;
    }
    public void shake()
    {
        impulse.GenerateImpulse(3f);
    }
}
