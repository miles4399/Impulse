using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class MovementAttributes : ScriptableObject
{
    [Header("Movement")]
    public float Speed = 5f;
    public float Acceleration = 10f;
    public float TurnSpeed = 10f;
    public float GroundDashForce = 1000f;
    public float AirDashForce = 250f;
    public float SpeedBoostTime = 2f;
    public float NormalDashMultiplier = 10f;
    public float DashCoolDown = 0.5f;
    public float DashTime = 0.5f;
    public float RailDashMultiplier = 4f;
    public LayerMask WallLayer;

    [Header("Grinding")]
    public float SnapSpeed = 10f;
    public float GrindSpeed = 5f;
    public float RailExitSpeed = 20f;
    public float GrindGracePeriod = 0.5f;

    [Header("Airborne")] 
    public float Gravity = -20f;
    public float JumpHeight = 2f;
    public float AirControl = 0.1f;
    public bool AirTurning = false;
    
    [Header("Grounding")]
    public float GroundCheckRadius = 0.25f;
    public Vector3 GroundCheckStart = new Vector3(0f, 0.35f, 0f);
    public Vector3 GroundCheckEnd = new Vector3(0f, 0.1f, 0f);
    public float MaxSlopeAngle = 40f;
    public float GroundedFudgeTime = 0.25f;
    public LayerMask GroundMask = 1 << 0;

    [Header("Wall Running")]
    public LayerMask RunnableWall;
    public float WallRunDistance = 1f;
    public float WallRunSpeed;
    public float WallRunGracePeriod = 0.5f;
    public float WallRunJumpVerticalModifier = 0.5f;
    public float WallRunJumpHorizontalModifier = 1f;

    [Header("AI")]
    public float StopDistance = 5f;
}
