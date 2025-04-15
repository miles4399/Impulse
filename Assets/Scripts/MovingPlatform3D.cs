using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform3D : MovingPlatform
{
    public override Vector3 Velocity { get => _rb.velocity; protected set => _rb.velocity = value; }

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
}