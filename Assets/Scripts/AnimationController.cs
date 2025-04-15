using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField] private float _dampTime = 0.1f;
    
    private Animator _animator;
    private CharacterMovement _characterMovement;
    private GrappleHook _grappleHook;

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        _characterMovement = GetComponentInParent<CharacterMovement>();
        _grappleHook = GetComponentInParent<GrappleHook>();
        _characterMovement.Jumped.AddListener(OnJumped);
        _characterMovement.Dashed.AddListener(OnDashed);
    }

    private void Update()
    {
        float speed = Mathf.Min(_characterMovement.MoveInput.magnitude, _characterMovement.Velocity.Flatten().magnitude / _characterMovement.MovementAttributes.Speed);
        _animator.SetFloat("Speed", speed, _dampTime, Time.deltaTime);
        _animator.SetBool("IsGrounded", _characterMovement.IsGrounded);
        _animator.SetBool("IsOnRail", _characterMovement.IsOnRail);
        if(_grappleHook != null) _animator.SetBool("IsGrappleSwing", _grappleHook.IsGrappled);
        // send wall run state
        
        
         _animator.SetBool("IsWallRunning", _characterMovement.IsWallRunning);

        
    }


    private void OnJumped()
    {
        _animator.SetTrigger("Jump");
    }

    private void OnDashed()
    {
        _animator.SetTrigger("Dash");
    }

    private void OnDisable()
    {
        _characterMovement.Jumped.RemoveListener(OnJumped);
    }
}