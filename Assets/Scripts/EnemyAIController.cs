using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAIController : MonoBehaviour
{
    private enum State
    {
        Chase,
        Static,
        Retreat,
        Fire
    }

    private enum Behaviour
    {
        Static,
        Passive,
        Aggressive,
        Dummy
    }

    [SerializeField] private string _targetTag = "Player";
    [SerializeField] private State _state;
    [SerializeField] private Behaviour _behaviour;
    [SerializeField] private float _fireRange;
    [SerializeField] private float _aggroRange;
    [SerializeField, FMODUnity.EventRef] private string FMODEventAIDeath;
    [SerializeField] private UnityEvent _onAIDeath;

    



    private GameObject _target;
    private CharacterMovement _characterMovement;
    private Coroutine _currentState;
    private bool _isInFireRange;
    private bool _isInAggroRange;
    private LaserGun _laserGun;
    
    public Vector3 Direction { get; private set; }


    private void Awake()
    {
        _target = GameObject.FindWithTag(_targetTag);
        _characterMovement = GetComponent<CharacterMovement>();
        _laserGun = GetComponentInChildren<LaserGun>();
    }

    void Start()
    {
        //if(_behaviour == Behaviour.Aggressive) NextState(ChaseState());
        if (_behaviour == Behaviour.Static) _state = State.Static;
           
    }

    private void NextState(IEnumerator nextState)
    {
        if (_currentState != null) StopCoroutine(_currentState);
        _currentState = StartCoroutine(nextState);
    }

    private IEnumerator ChaseState()
    {
        _state = State.Chase;
        _characterMovement.MoveSpeedMultiplier = 1f;
        while (true)
        {
            
            _characterMovement.MoveTo(_target.transform.position);
            if(_isInFireRange)
            {
                NextState(FireState());
            }

            yield return null;
        }
    }

    private IEnumerator Retreat()
    {
        while(true)
        {
            yield return null;
        }
    }

    private IEnumerator FireState()
    {
        _state = State.Fire;
        _characterMovement.MoveSpeedMultiplier = 0.2f;
        
        while(_isInFireRange)
        {
            if(_behaviour == Behaviour.Aggressive) _characterMovement.MoveTo(_target.transform.position);
            Vector3 newPos = transform.position + -Direction;
            if (_behaviour == Behaviour.Passive) _characterMovement.MoveTo(newPos);

            _laserGun?.Shoot();

            if(!_isInFireRange )
            {
                if (_behaviour == Behaviour.Aggressive) NextState(ChaseState());
                else if (_behaviour == Behaviour.Static || _behaviour == Behaviour.Passive)
                {
                    StopAllCoroutines();
                    _characterMovement.Stop();
                }
            }


            yield return null;
        }

       
    }

    public void Death()
    {
        _onAIDeath.Invoke();    //Audio
        Destroy(this.gameObject);
    }


    void Update()
    {
        _isInFireRange = Vector3.Distance(transform.position, _target.transform.position) <= _fireRange;
        _isInAggroRange = Vector3.Distance(transform.position, _target.transform.position) <= _aggroRange;

        
        Direction = _target.transform.position - transform.position;

        Vector3 tempTransform = _target.transform.position;
        tempTransform.y = transform.position.y;
        if(_isInAggroRange) transform.LookAt(tempTransform);
        if (_isInFireRange) 
        {
            if(_behaviour == Behaviour.Static || _behaviour == Behaviour.Passive) NextState(FireState());
        }
        if (_behaviour == Behaviour.Aggressive) 
        {
            if(_isInAggroRange) NextState(ChaseState());
          
        }

    }

    

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _fireRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _aggroRange);
    }
}
