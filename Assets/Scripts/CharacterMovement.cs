using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.AI;
using UnityEngine.Events;
#pragma warning disable 649

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private MovementAttributes _movementAttributes;
    [SerializeField] private Transform _orientation;
    [SerializeField] private GameObject _ObjectToRotate;
    [SerializeField] private Transform _raycastTransform;

    [SerializeField, FMODUnity.EventRef] private string FMODEventRailLanded;
    [SerializeField] private UnityEvent _railLanded;
    [SerializeField, FMODUnity.EventRef] private string FMODEventDash;
    [SerializeField] private UnityEvent _onDash;
    [SerializeField, FMODUnity.EventRef] private string FMODEventRailJump;
    [SerializeField] private UnityEvent _onRailJump;
    [SerializeField, FMODUnity.EventRef] private string FMODEventExitRail;
    [SerializeField] private UnityEvent _onRailExit;
    [SerializeField, FMODUnity.EventRef] private string FMODEventEnterWallRun;
    [SerializeField] private UnityEvent _onWallRunEnter;
    [SerializeField, FMODUnity.EventRef] private string FMODEventExitWallRun;
    [SerializeField] private UnityEvent _onWallRunExit;
    [SerializeField, FMODUnity.EventRef] private string FMODEventMovementStart;
    [SerializeField] private UnityEvent _onMoveStart;
    [SerializeField, FMODUnity.EventRef] private string FMODEventMovementEnd;
    [SerializeField] private UnityEvent _onMoveEnd;



    //controls and calls the particle effects
    [SerializeField] private VolumeProfile _profile;
    [SerializeField] private ParticleSystem _speedparticleSystem;
    [SerializeField] private ParticleSystem _trailparticleSystem;
    [SerializeField] private ParticleSystem _grindparticleSystem;

    [SerializeField] private float _shieldLerpMod = 0.25f;

    private ChromaticAberration _chromaticAberration;
    private LensDistortion _lensDistortion;
    private Vignette _vignette;

    private GameObject _player;
    public MovementAttributes MovementAttributes => _movementAttributes;
    public bool IsGrounded { get; private set; } = true;
    public bool IsFudgeGrounded => Time.timeSinceLevelLoad < _lastGroundedTime + MovementAttributes.GroundedFudgeTime;
    public Vector3 GroundNormal { get; private set; } = Vector3.up;
    public Vector3 MoveInput { get; private set; }
    public Vector3 LocalMoveInput { get; private set; }
    public Vector3 LookDirection { get; private set; }

    public bool CanMove { get; set; } = true;
    public float MoveSpeedMultiplier { get; set; } = 1f;

    public float GrindSpeedMultiplier { get; set; } = 1f;
    public float ForcedMovement { get; set; } = 0f;
    public float TurnSpeedMultiplier { get; set; } = 1f;
    public Rail Rail { get; private set; }
    public bool IsOnRail => Rail != null;
    public float RailDistance { get; private set; }
    public float RailDirection { get; private set; }
    public bool HasMoveInput => MoveInput.magnitude > 0.2f;
    public Vector2 RawMoveInput;
    public bool IsWallRunning = false;
    public bool IsDashing = false;
    public bool _isWallRight { get; private set; } 
    public bool _isWallLeft { get; private set; }



    private Rigidbody _rigidbody;
    private NavMeshAgent _navMeshAgent;
    private GrappleHook _grappleHook;
    private SphereCollider _dashHitBox;
    private DashControl _dashControl;
    private float _lastGroundedTime = -Mathf.Infinity;


    private float _railExitTime;
    private float _defaultGravity;
    //private float _wallExitTimeRight;
    //private float _wallExitTimeLeft;
    private Vector3 _wallRunNormal;
    private bool _blockWallRun;
    private bool _isDashCoolDown = false;
    private bool _isSpeedBoosting = false;
    private bool _didMoveInput = false;
    private bool _didWallRun = false;
    private bool _blockDash = false;

    public UnityEvent Jumped;
    public UnityEvent Dashed;
    private Quaternion _wallRunRot;

    public Vector3 Velocity
    {
        get => _rigidbody.velocity;
        private set => _rigidbody.velocity = value;
    }

    
    
    private void Awake()
    {

        _dashControl = GetComponent<DashControl>();
        _dashHitBox = GetComponent<SphereCollider>();
        _grappleHook = GetComponent<GrappleHook>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.useGravity = false;
        
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;
        
        LookDirection = transform.forward;

        //_dashHitBox.enabled = false;

    }

    public void SetMoveInput(Vector3 input)
    {
        Vector3 flattened = input.Flatten();
        MoveInput = Vector3.ClampMagnitude(flattened, 1f);
        LocalMoveInput = transform.InverseTransformDirection(MoveInput);
    }

    public void SetLookDirection(Vector3 direction)
    {
        if(direction.magnitude < 0.05f) return;
        LookDirection = direction.Flatten().normalized;
    }


    public void StartJump()
    {
        // everything below this line needs fixing
        if(CanMove && IsGrounded || IsWallRunning || IsOnRail)
        {
            Jump();
        }



    }

    private void Jump()
    {
        if (IsOnRail)
        {
            _onRailJump.Invoke();
            ExitRail();
            _rigidbody.isKinematic = false;
        }
        
        float jumpVelocity = Mathf.Sqrt(2f * -MovementAttributes.Gravity * MovementAttributes.JumpHeight);
        Velocity = new Vector3(Velocity.x, jumpVelocity, Velocity.z);
        if (IsWallRunning) Velocity = new Vector3(Velocity.x, jumpVelocity * MovementAttributes.WallRunJumpVerticalModifier, Velocity.z) + _wallRunNormal * jumpVelocity * MovementAttributes.WallRunJumpHorizontalModifier;
        

        _blockWallRun = true;
        StopWallRun();

        Jumped.Invoke();
    }

    public void BlockDash()
    {
        _blockDash = true;
    }

    public void UnBlockDash()
    {
        _blockDash = false;
    }

       
    // Called when press input
    public void StartDash()
    {
        if (_isDashCoolDown || IsWallRunning || _grappleHook.IsGrappled == true || _blockDash == true)
        {
            return;
            
        }
        _onDash.Invoke();       //Audio
        Dashed.Invoke();


        if (_speedparticleSystem != null)
        {
            _trailparticleSystem.Play();
        }

        if (IsOnRail)
        {
            StartCoroutine(RailDash());
        }
        else
        {           
            _dashHitBox.enabled = true;
            Vector3 dashDirection = Camera.main.transform.forward;
            dashDirection.y = 0f;
            dashDirection.Normalize();
            _rigidbody.velocity = new Vector3(0f, 0f, 0f);
            Vector3 startPos = transform.position;
            Vector3 endPos = dashDirection * _movementAttributes.NormalDashMultiplier + startPos;

            StartCoroutine(Dash(startPos, endPos));
            SetLookDirection(dashDirection);
            
            Debug.DrawLine(startPos, endPos, Color.red, 10f);

        }
    }



    private IEnumerator RailDash()
    {
        _blockDash = true;
        _isDashCoolDown = true;
        GrindSpeedMultiplier = _movementAttributes.RailDashMultiplier;
        _dashHitBox.enabled = true;
        yield return new WaitForSeconds(_movementAttributes.DashTime);
        GrindSpeedMultiplier = 1f;
        _dashHitBox.enabled = false;
        _trailparticleSystem.Stop();
        yield return new WaitForSeconds(_movementAttributes.DashCoolDown);
        _isDashCoolDown = false;
        _blockDash = false;
    }

    private IEnumerator Dash(Vector3 startPos, Vector3 endPos)
    {
        IsDashing = true;
        _isDashCoolDown = true;
        float elapsedTime = 0f;
        Vector3 sphereDirection = (endPos - startPos).normalized;
        Vector3 sphereStart = transform.position + transform.up * 0.75f + (sphereDirection * -0.05f);
        Ray collisionRay = new Ray(sphereStart, sphereDirection);
        RaycastHit hit;
        if (Physics.SphereCast(collisionRay, 0.05f, out hit, (endPos - sphereStart).magnitude, _movementAttributes.WallLayer) )
        {
            //if (Vector3.Angle(hit.normal, -sphereDirection) < 45f)
            if(Mathf.Abs(hit.normal.y) <= 0.1f)
            {
                endPos = hit.point - (transform.up * 0.75f);

            }
            else
            {
                float slopeRad = (Vector3.Angle(hit.normal, -sphereDirection)) * Mathf.Deg2Rad;
                float raiseAmount = (Vector3.Distance(startPos, endPos) / Mathf.Tan(slopeRad));
                endPos = endPos + transform.up * raiseAmount;
                Debug.DrawLine(startPos, endPos, Color.red, 10f);
            }

        }
        while (elapsedTime < _movementAttributes.DashTime )
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = elapsedTime / _movementAttributes.DashTime;


            Vector3 dashPos = Vector3.Lerp(startPos, endPos, percentageComplete);
  
            
            _rigidbody.MovePosition (dashPos);
            yield return null;
        }
        
        
        IsDashing = false;
        
        _dashHitBox.enabled = false;
        
        StartCoroutine(SpeedBoost());

        yield return new WaitForSeconds(_movementAttributes.DashCoolDown);
        _isDashCoolDown = false;

        if (_speedparticleSystem != null)
        {
            _trailparticleSystem.Stop();
        }

    }




    private IEnumerator SpeedBoost()
    {
        MoveSpeedMultiplier = 1.6f;
        _isSpeedBoosting = true;
        yield return new WaitForSeconds(_movementAttributes.SpeedBoostTime);
        MoveSpeedMultiplier = 1f;
        _isSpeedBoosting = false;

    }




    //Navmesh movement for AI
    public bool MoveTo(Vector3 destination)
    {
        if (PositionReached(destination)) return true;

        _navMeshAgent.SetDestination(destination);
        return false;
    }

    //Navmesh movement for AI
    public bool PositionReached(Vector3 position)
    {
        Vector3 myPositionFlat = transform.position;
        myPositionFlat.y = 0f;
        position.y = 0f;

        return Vector3.Distance(myPositionFlat, position) < _movementAttributes.StopDistance;
        
    }

    //Navmesh movement for AI
    public void Stop()
    {
        _navMeshAgent.ResetPath();
        SetMoveInput(Vector3.zero);
    }

    private void FixedUpdate()
    {

        CheckForWall();
        DoOnce();

        if (IsWallRunning) return;

        if (_navMeshAgent.hasPath)
        {
            if (PositionReached(_navMeshAgent.destination)) Stop();
        }

        Vector3 input = MoveInput;
        if (ForcedMovement > 0f) input = transform.forward * ForcedMovement;
        Vector3 right = Vector3.Cross(transform.up, input);
        Vector3 forward = Vector3.Cross(right, GroundNormal);
        Vector3 targetVelocity = forward * (MovementAttributes.Speed * MoveSpeedMultiplier);
        Vector3 velocityDiff = targetVelocity.Flatten() - Velocity.Flatten();

        if (IsOnRail)          
        {
            
            if (_vignette != null || _profile.TryGet(out _vignette))
            {
                if (_vignette.intensity.value < 0.35f)
                {
                    _vignette.intensity.value += Time.fixedDeltaTime * 0.25f;
                }
            }

            RailDistance += MovementAttributes.GrindSpeed * RailDirection * Time.fixedDeltaTime * GrindSpeedMultiplier;
            Vector3 railPosition = Rail.GetPositionAndRotationByDistance(RailDistance, out Vector3 railForward);
            Debug.DrawLine(transform.position, railPosition, Color.green);
            _rigidbody.isKinematic = true;
            _rigidbody.MovePosition(railPosition);
            _rigidbody.MoveRotation(Quaternion.LookRotation(railForward * RailDirection));

            if ((RailDirection > 0f && Rail.GetRailLength() < RailDistance) || RailDistance < 0f)
            {
                ExitRail();
                _rigidbody.isKinematic = false;
                _rigidbody.velocity = railForward * RailDirection * MovementAttributes.RailExitSpeed;
            }

            SetLookDirection(railForward * RailDirection); 
            return;
        }


        float control = IsGrounded || IsOnRail ? 1f : MovementAttributes.AirControl;
        if (!IsGrounded && !IsOnRail) control *= MoveInput.magnitude;
        Vector3 acceleration = velocityDiff * (MovementAttributes.Acceleration * control);
        if(!IsOnRail && !IsWallRunning && !IsDashing) acceleration += GroundNormal * MovementAttributes.Gravity;
        Debug.DrawRay(transform.position, GroundNormal);
        Debug.DrawRay(transform.position, forward);
        if(!IsWallRunning)  _rigidbody.AddForce(acceleration);

        if (!IsOnRail &&!IsWallRunning && (IsGrounded || MovementAttributes.AirTurning))
        {
            Quaternion targetRotation = Quaternion.LookRotation(LookDirection);
            Quaternion rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * MovementAttributes.TurnSpeed * TurnSpeedMultiplier);
            _rigidbody.MoveRotation(rotation);
        }

        
    }

    public void EnterRail(Rail rail, float distance)
    {
        if (IsGrounded) return;
        if (Time.time < _railExitTime + MovementAttributes.GrindGracePeriod || Rail == rail) return;
        _railLanded.Invoke();       //Audio
        Rail = rail;
        RailDistance = distance;
        _rigidbody.MovePosition(rail.GetPositionAndRotationByDistance(distance, out Vector3 railForward));
        float dot = Vector3.Dot(transform.forward, railForward);
        RailDirection = Mathf.Sign(dot);

        if (_chromaticAberration != null || _profile.TryGet(out _chromaticAberration))
        {
            _chromaticAberration.intensity.value = 1f;
        }
        if (_lensDistortion != null || _profile.TryGet(out _lensDistortion))
        {
            _lensDistortion.intensity.value = -0.3f;
            _lensDistortion.scale.value = 0.95f;
        }
        if (_speedparticleSystem != null)
        {
            _speedparticleSystem.Play();
        }
        if (_grindparticleSystem !=null)
        {
            _grindparticleSystem.Play();
        }
    }

    public void ExitRail()
    {
        Rail = null;
        _railExitTime = Time.time;
        _onRailExit.Invoke();       //Audio

        if (_vignette != null || _profile.TryGet(out _vignette))
        {
            _vignette.intensity.value = 0.25f;
        }
        if (_chromaticAberration != null || _profile.TryGet(out _chromaticAberration))
        {
            _chromaticAberration.intensity.value = 0f;
        }

        if (_lensDistortion != null || _profile.TryGet(out _lensDistortion))
        {
            _lensDistortion.intensity.value = 0f;
            _lensDistortion.scale.value = 1f;
        }
        if (_speedparticleSystem != null)
        {
            _speedparticleSystem.Stop();
        }
        if (_grindparticleSystem != null)
        {
            _grindparticleSystem.Stop();
        }
    }

    private void Update()
    {

        if(Input.GetKeyDown(KeyCode.G))
        {

        }

        IsGrounded = CheckGrounded();

        

        if (_navMeshAgent.hasPath)
        {
            Vector3 nextPathPoint = _navMeshAgent.path.corners[1];
            Vector3 pathDir = (nextPathPoint - transform.position).normalized;
            SetMoveInput(pathDir);
            //SetLookDirection(pathDir);
        }

        _navMeshAgent.nextPosition = transform.position;

        if(!CanMove)
        {
            SetMoveInput(Vector3.zero);
            SetLookDirection(transform.forward);
        }



    }

    Coroutine wallRotationCoroutine;
    public IEnumerator SmoothWallRotation(Quaternion newRotation) {
        float rotationTime = 0.75f;
        float curTime = 0.0f;
        while(curTime < rotationTime && IsWallRunning)
        {
            
            curTime += Time.deltaTime;
            float t = curTime / rotationTime;
            t = t * t * (3f - 2f * t);
            _ObjectToRotate.transform.rotation = Quaternion.Slerp(_ObjectToRotate.transform.rotation, newRotation, t);
            yield return null;
        }
        yield return null;
    }

    private void DoOnce()
    {

        
        if(HasMoveInput == true)
        {
            if(_didMoveInput == false)
            {
                _onMoveStart.Invoke();  //audio
                
                _didMoveInput = true;
                return;
            }
        }

        if (HasMoveInput == false)
        {
            if (_didMoveInput == true)
            {
                _onMoveEnd.Invoke();    //audio
                _didMoveInput = false;
                return;
            }
        }

        if (IsWallRunning == true)
        {
            if (_didWallRun == false)
            {
                _didWallRun = true;
                _onWallRunEnter.Invoke();    //audio


                //_wallRunRot = Quaternion.Slerp(_ObjectToRotate.transform.rotation, Quaternion.FromToRotation(Vector3.down, _wallRunNormal), 0.5f);
                //_ObjectToRotate.transform.localRotation = Quaternion.Inverse(Quaternion.Euler(0f, 0f, _wallRunRot.z * Mathf.Rad2Deg));
                //Debug.LogError($"I am Wall Run! {Quaternion.Inverse(Quaternion.Euler(0f, 0f, _wallRunRot.z * Mathf.Rad2Deg))}");

                Vector3 forward = Vector3.Cross(Vector3.down, _wallRunNormal);
                if (_isWallRight) forward *= -1f;
                Debug.DrawRay(_ObjectToRotate.transform.position, forward, Color.magenta);
                Quaternion forwardRotation = Quaternion.LookRotation(forward, Vector3.up);
                float tiltAngle = _isWallRight ? 45f : -45f;
                Quaternion tiltRotation = Quaternion.Euler(0f, 0f, tiltAngle);
                Quaternion finalRotation = forwardRotation * tiltRotation;
                wallRotationCoroutine = StartCoroutine(SmoothWallRotation(finalRotation));
                //_ObjectToRotate.transform.rotation = finalRotation;

                return;
            }
        }

        if (IsWallRunning == false)
        {
            if (_didWallRun == true)
            {
                _didWallRun = false;

                _onWallRunExit.Invoke();    //audio
                if(wallRotationCoroutine != null)
                {
                    StopCoroutine(wallRotationCoroutine);
                }
                ResetModelRotation();
                return;
            }
        }
    }

    /// <summary>
    /// Resets the character's model's rotation. Can be used to fix bugs with respawn system.
    /// </summary>
    public void ResetModelRotation()
    {
        _wallRunRot = Quaternion.identity;
        _ObjectToRotate.transform.localRotation = _wallRunRot;
    }

    private void WallRunInput()
    {

        if (_isWallRight || _isWallLeft) StartWallRun();
        if(IsWallRunning) Debug.DrawRay(transform.position, _wallRunNormal * 3f, Color.magenta);
    
    }

    private void StartWallRun()
    {
        if (_blockWallRun) return;
        IsWallRunning = true;



        if (_vignette != null || _profile.TryGet(out _vignette))
        {
            if (_vignette.intensity.value < 0.35f)
            {
                _vignette.intensity.value += Time.fixedDeltaTime * 0.25f;
            }
        }

        if (_chromaticAberration != null || _profile.TryGet(out _chromaticAberration))
        {
            _chromaticAberration.intensity.value = 1f;
        }
        if (_lensDistortion != null || _profile.TryGet(out _lensDistortion))
        {
            _lensDistortion.intensity.value = -0.3f;
            _lensDistortion.scale.value = 0.95f;
        }
        if (_speedparticleSystem != null)
        {
            _speedparticleSystem.Play();
        }




        Vector3 Cross = Vector3.Cross(Vector3.down , _wallRunNormal);
        //Vector3 CrossY = new Vector3(0f, Cross.y, 0f);
        //SetLookDirection(CrossY);

        if (_isWallLeft)
        {
            _rigidbody.velocity = Cross * MovementAttributes.WallRunSpeed;
        }
        else if (_isWallRight)
        {
            _rigidbody.velocity = -Cross * MovementAttributes.WallRunSpeed;
            //SetLookDirection(-Cross);
        }


    }

    public void StopWallRun()
    {
        IsWallRunning = false;
        ResetModelRotation();
        if (_player == null) return;

        if (_vignette != null || _profile.TryGet(out _vignette))
        {
            _vignette.intensity.value = 0.25f;
        }
        if (_chromaticAberration != null || _profile.TryGet(out _chromaticAberration))
        {
            _chromaticAberration.intensity.value = 0f;
        }

        if (_lensDistortion != null || _profile.TryGet(out _lensDistortion))
        {
            _lensDistortion.intensity.value = 0f;
            _lensDistortion.scale.value = 1f;
        }
        if (_speedparticleSystem != null)
        {
            _speedparticleSystem.Stop();
        }
    }

    private void CheckForWall()
    {
        if (_raycastTransform == null) return;
        RaycastHit hit;
        //Check to see if theres a runnable wall to the left or the right
        _isWallRight = Physics.Raycast(_raycastTransform.position, _raycastTransform.right, out hit, 1f, MovementAttributes.RunnableWall);
        if (_isWallRight) 
        {
            _wallRunNormal = hit.normal;
        }
        if(!_isWallRight)
        {
            _isWallLeft = Physics.Raycast(_raycastTransform.position, -_raycastTransform.right, out hit, 1f, MovementAttributes.RunnableWall);
            if (_isWallLeft) _wallRunNormal = hit.normal;
        }

        Debug.DrawRay(_raycastTransform.position, _raycastTransform.right * MovementAttributes.WallRunDistance, _isWallRight ? Color.green : Color.red);
        Debug.DrawRay(_raycastTransform.position, -_raycastTransform.right * MovementAttributes.WallRunDistance, _isWallLeft ? Color.green : Color.red);


        //leave wall running
        if (!_isWallRight && !_isWallLeft) _blockWallRun = false;

        WallRunInput();
        if (!IsWallRunning) return;
        if (!_isWallLeft && !_isWallRight) StopWallRun();



    }

    private bool CheckGrounded()
    {
        Vector3 start = transform.TransformPoint(MovementAttributes.GroundCheckStart);
        Vector3 end = transform.TransformPoint(MovementAttributes.GroundCheckEnd);
        Vector3 diff = end - start;
        Vector3 dir = diff.normalized;
        float distance = diff.magnitude;
        if (Physics.SphereCast(start, MovementAttributes.GroundCheckRadius, dir, out RaycastHit hit, distance, MovementAttributes.GroundMask))
        {
            bool angleValid = Vector3.Angle(Vector3.up, hit.normal) < MovementAttributes.MaxSlopeAngle;
            if (angleValid)
            {
                _lastGroundedTime = Time.timeSinceLevelLoad;
                GroundNormal = hit.normal;
                return true;
            }
        }

        GroundNormal = Vector3.up;
        return false;
    }


    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Vector3 start = transform.TransformPoint(MovementAttributes.GroundCheckStart);
        Vector3 end = transform.TransformPoint(MovementAttributes.GroundCheckEnd);
        Gizmos.DrawWireSphere(start, MovementAttributes.GroundCheckRadius);
        Gizmos.DrawWireSphere(end, MovementAttributes.GroundCheckRadius);
        if(_navMeshAgent != null && _navMeshAgent.hasPath)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _navMeshAgent.destination);
        }
    }
}