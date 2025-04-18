﻿using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

#pragma warning disable 649

public class PlayerController : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] private GameObject _target;
    [SerializeField] private GrappleHook _grappleHook;
    [SerializeField] private DebugCheckpoints _reloader;
    [SerializeField] private Timer _timer;
    [SerializeField] private bool _autoPossess = true;

       
    private Camera _mainCamera;
    private CharacterMovement _characterMovement;
    


    private Vector2 _lookInput;
    private Vector2 _moveInput;
    private bool _possessed = false;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _mainCamera = Camera.main;
        if(_autoPossess && _target != null) Possess(_target);
    }

    public void Possess(GameObject target)
    {
        if (_target.TryGetComponent(out CharacterMovement characterMovement))
        {
            _characterMovement = characterMovement;
            _possessed = true;
            Debug.Log($"{gameObject.name} possessed {_target.name}", _target);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} failed to possess {_target.name}", _target);
            _characterMovement = null;
            _possessed = false;
        }

    }

    public void Depossess()
    {
        _characterMovement = null;
        _grappleHook = null;
        _possessed = false;
    }

    public void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        _characterMovement?.StartJump();
    }

    public void OnGrapple(InputValue value)
    {
        _grappleHook?.StartGrappleSwing();
        _grappleHook?.StartGrapplePull();
    }

    public void OnEndGrapple(InputValue value)
    {
        _grappleHook?.StopGrappleSwing();
        //_grappleHook?.StopGrapplePull();
    }

    public void OnDash(InputValue vaule)
    {
        _characterMovement?.StartDash();
    }

    public void OnRestart(InputValue value)
    {
        if(!_characterMovement.IsOnRail && !_characterMovement.IsDashing && !_grappleHook.IsGrappled)
        {
        _reloader.Reload();
        _timer.ResetTimer();

        }
    }


    private void Update()
    {
        if (_possessed)
        {
            Vector3 up = Vector3.up;
            Vector3 right = _mainCamera.transform.right;
            Vector3 forward = Vector3.Cross(right, up);
            Vector3 moveInput = forward * _moveInput.y + right * _moveInput.x;
        
            _characterMovement.SetMoveInput(moveInput);
            if(!_characterMovement.IsWallRunning)_characterMovement.SetLookDirection(moveInput);
            _characterMovement.RawMoveInput = _moveInput;
        }
    }
}
