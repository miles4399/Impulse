using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSpeedFeedBack : MonoBehaviour
{
    [SerializeField] private float _maxSpeed = 1f;
    [SerializeField] private Material _blurMat;
    public UnityEvent<float> OnPlayerSpeed;
    private Rigidbody _RB;

    private void Awake()
    {
        _RB = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        float speed = _RB.velocity.magnitude;
        float speedPercentage = Mathf.Clamp01(speed / (_maxSpeed));
        _blurMat.SetFloat("_BlurAmount", speedPercentage);
        OnPlayerSpeed.Invoke(speedPercentage);
    }
}
