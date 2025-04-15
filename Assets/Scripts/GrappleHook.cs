using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class GrappleHook : MonoBehaviour
{
    [SerializeField] private SpringJoint _grappleSwing;
    private float _exitHeight;
    [SerializeField] private Transform _grapplePull;
    [SerializeField] private float _pushForce = 100f;
    [SerializeField] private float _pullForce = 100f;
    [SerializeField] private float _exitSpeed = 10f;
    [SerializeField, FMODUnity.EventRef] private string FMODEventGrappleLaunch;
    [SerializeField] private UnityEvent _onGrappleLaunch;
    [SerializeField] private Transform _grappleHand;




    private CharacterMovement _characterMovement;
    private Rigidbody _rigidbody;
    private LineRenderer _rope;
    private Transform _pullPivot;
    
    
    
    public bool IsGrappled = false;
    public bool IsPulling = false;


    public void Start()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _rigidbody = GetComponent<Rigidbody>();
        _rope = GetComponentInChildren<LineRenderer>();

    }

    public void Update()
    {
        

    }

    private void FixedUpdate()
    {
        if (!IsGrappled) return;
        Vector3 grapplePointDir = (_grappleSwing.transform.position - _characterMovement.transform.position).normalized;
        Vector3 right = Vector3.Cross(Vector3.up, grapplePointDir);
        right = Camera.main.transform.right;
        Vector3 swingTangent = Vector3.Cross(-grapplePointDir, right);
        Debug.DrawRay(_characterMovement.transform.position, swingTangent * 3, Color.magenta);
        float swingMultiplier = swingTangent.y < 0f ? 1f : 0f;
        _rigidbody.AddForce(swingTangent * _pushForce * swingMultiplier);
        _rigidbody.AddForce(grapplePointDir * _pullForce);
        _rigidbody.AddForce(Vector3.up * _pullForce * swingMultiplier);
        //Debug.Log(swingTangent);
        
    }

    public void StartGrappleSwing()
    {
        if (_grappleSwing == null) return;
        if (_grappleSwing.transform.position.y < transform.position.y) return;
        _characterMovement.StartJump();
        
        _onGrappleLaunch.Invoke();
        IsGrappled = true;
        
        _grappleSwing.connectedBody = _rigidbody;



        StartCoroutine(DrawRopeSwing());
    }

    public void StartGrapplePull()
    {
        if (_grapplePull == null) return;
        if(_grapplePull.rotation.z <= 0) return;

        //Canvas canvas = _grapplePull.gameObject.GetComponentInChildren<Canvas>();
        //canvas.gameObject.SetActive(true);

        IsPulling = true;
        StartCoroutine(RotateBridge());

        
        
        

    }

    private IEnumerator RotateBridge()
    {
        _rope.enabled = true;
        
        while (IsPulling && _grapplePull != null && _grapplePull?.rotation.z >= 0)
        {
            _grapplePull.Rotate(0, 0, -3f);
            _rope.SetPosition(0, this.gameObject.transform.position);
            _rope.SetPosition(1, _pullPivot.position);
            
            yield return null;
        }
        _rope.enabled = false;
    }

    public void StopGrappleSwing()
    {
        if (IsGrappled == false) return;
        _grappleSwing.connectedBody = null;
        

        IsGrappled = false;
        Vector3 currentVel = _rigidbody.velocity;
        // Vector3 newVel = new Vector3(currentVel.x * 1.5f, currentVel.y, currentVel.z * 1.5f);
        float heightDiff = Mathf.Clamp(_exitHeight - transform.position.y, 0f, Mathf.Infinity);
        float exitYVelocity = Mathf.Sqrt(2f * 28f * heightDiff);
        Vector3 newVel = currentVel.normalized * _exitSpeed;
        newVel.y = exitYVelocity;
        _rigidbody.velocity = newVel;
        StopCoroutine(DrawRopeSwing());

    }



    public void SetPull(Transform bridge)
    {
        _grapplePull = bridge;
        
    }

    public void SetPullPivot(Transform pivot)
    {
        _pullPivot = pivot;
    }


    public void SetSwing(SpringJoint joint, float exitHeight)
    {
        _grappleSwing = joint;
        _exitHeight = exitHeight;
        
        

    }

    private IEnumerator DrawRopeSwing()
    {
        _rope.enabled = true;
        while(IsGrappled)
        {
            _rope.SetPosition(0, _grappleHand.transform.position); ;
            _rope.SetPosition(1, _grappleSwing.gameObject.transform.position);
            yield return null;
        }

        _rope.enabled = false;
    }

    //private IEnumerator DrawRopePull()
    //{
    //    _rope.enabled = true;
    //    while (IsGrappled)
    //    {
    //        _rope.SetPosition(0, this.gameObject.transform.position);
    //        _rope.SetPosition(1, _grapplePull.gameObject.transform.position);
    //        yield return null;
    //    }
    //    _rope.enabled = false;
    //}
}
