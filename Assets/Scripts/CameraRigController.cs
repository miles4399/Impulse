using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRigController : MonoBehaviour
{
    [SerializeField] private CharacterMovement _movement;
    [SerializeField] private GrappleHook _swing;

    public Transform cameraTarget;

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>(); 
    }
    private void Update()
    {
        _animator.SetBool("IsOnRail", _movement.IsOnRail);

        _animator.SetBool("IsWallRunning", _movement.IsWallRunning);

        _animator.SetBool("IsGrappling",_swing.IsGrappled);

        _animator.SetBool("IsDashing", _movement.IsDashing);

    }
   
}
