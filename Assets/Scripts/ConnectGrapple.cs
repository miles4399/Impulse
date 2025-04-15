using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConnectGrapple : MonoBehaviour
{
    [SerializeField] private bool _isSwing;
    [SerializeField] private string _playerTag = "Player";
 
    [SerializeField] private LayerMask _enviormentLayer;
    [SerializeField] private float _exitHeight;
    [SerializeField] private Image _arrow;

    public Image _outerImage;
    
    private Animator _animator;

    private SpringJoint _springJoint;
    private Transform _bridge;
    private Canvas _grappleCanvas;
    private SphereCollider _sphereCollider;
    private float _distance;
    private GameObject _player;
    private GrappleHook _grappleHook;
    private Color _colorA;
    private Color _colorB;
    private float _grappleUIDistance;
    private Renderer _renderer;
    private bool _UISet = false;





    private bool _isPointVisible;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _player = GameObject.FindWithTag(_playerTag);
        _renderer = GetComponent<Renderer>();
    }





    public void Start()
    {
        
        _sphereCollider = GetComponent<SphereCollider>();
        _springJoint = GetComponent<SpringJoint>();
        _bridge = GetComponent<Transform>();
        _grappleCanvas = GetComponentInChildren<Canvas>();
        //_grappleCanvas.gameObject.SetActive(false);
        _outerImage?.gameObject.SetActive(true);
        //_innerImage?.gameObject.SetActive(false);
        _animator.enabled = false;
        _grappleUIDistance = _sphereCollider.radius * 1.5f;
        _arrow.gameObject.SetActive(false);


        if (_outerImage != null) _outerImage.color = Color.yellow;

    }



    private void Update()
    {

        //Debug.Log(_renderer.isVisible);

        bool isVisible = CheckVisibility(Camera.main.transform.position);




        if (_distance > _grappleUIDistance || Mathf.Abs(_player.transform.position.y - this.transform.position.y) <= 1 || _player.transform.position.y >= this.transform.position.y)
        {
            _outerImage.gameObject.SetActive(false);


        }
        else
        {
            _outerImage.gameObject.SetActive(true);

        }

 

        _distance = Vector3.Distance(_player.transform.position, transform.position) * 0.95f;
        if(_distance < _grappleUIDistance)
        {

            if (_grappleHook != null && _grappleHook.IsGrappled)
            {
                _outerImage.gameObject.SetActive(false);

            }
            //if (_renderer.isVisible &&_UISet)
            //{


            //    _arrow.gameObject.SetActive(false);
            //}
            //else if (!_UISet) _arrow.gameObject.SetActive(false);
            //else _arrow.gameObject.SetActive(true);
            
            Vector3 screenPos = Camera.main.WorldToScreenPoint(this.transform.position);
            Vector3 fixScreenPos = new Vector3(Mathf.Abs(screenPos.x), screenPos.y, screenPos.z);
            float posX = Mathf.Clamp(screenPos.x, Screen.width * 0.2f, Screen.width * 0.8f);
            float posY = Mathf.Clamp(screenPos.y, Screen.height * 0.8f, Screen.height * 0.9f);

 

            Vector3 point = Camera.main.transform.InverseTransformPoint(this.transform.position);
            float angle = -Mathf.Atan2(point.x, point.y) * Mathf.Rad2Deg - 30f;


            _arrow.transform.eulerAngles = new Vector3(0, 0, angle);


            //_arrow.transform.position = new Vector3(posX, posY, 0f);

        }
        else
        {
            //_arrow.gameObject.SetActive(false);
        }










    }

    public void UIOn()
    {
        _UISet = true;
        _arrow.gameObject.SetActive(true);
    }
    public void UIOff()
    {
        _UISet = false;
        _arrow.gameObject.SetActive(false);
    }

    private bool CheckVisibility(Vector3 CameraPos)         //Check if theres anywall between player and grapple point
    {
        Debug.DrawLine(transform.position, CameraPos, Color.red);

        Vector3 direction = (CameraPos - transform.position).normalized;
        Ray viewRay = new Ray(transform.position, direction);
        if (Physics.Raycast(viewRay, _distance, _enviormentLayer)) return false;


        Debug.DrawLine(transform.position, CameraPos, Color.green);
        return true;
        
    }

    private float CalcPercentage()
    {
        float minDist = _sphereCollider.radius;
        float maxDist = _grappleUIDistance;
        float TotalDist = maxDist - minDist;
        float currentDist = _distance - minDist;
        float percentage = currentDist / TotalDist;
        return percentage;
    }





    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out GrappleHook grappleHook))
        {
            //_grappleCanvas.gameObject.SetActive(true);
            if (_outerImage != null)
            {
                _outerImage?.gameObject.SetActive(true);
                _outerImage.color = Color.red;
                _animator.enabled = true;
            }
            if(_isSwing)
            {
                grappleHook.SetSwing(_springJoint, _exitHeight);
                _grappleHook = grappleHook;
            }
            else
            {
                grappleHook.SetPull(_bridge);
                grappleHook.SetPullPivot(_grappleCanvas.transform);
            }


        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out GrappleHook grappleHook))
        {
            //_grappleCanvas.gameObject.SetActive(false);
            if (_outerImage != null) 
            {
                _outerImage?.gameObject.SetActive(true);
                _outerImage.color = Color.yellow;
                _animator.enabled = false;
            }
            if (_isSwing)
            {
                grappleHook.StopGrappleSwing();
                grappleHook.SetSwing(null, 0);
                _grappleHook = null;
            }
            else
            {
                grappleHook.SetPull(null);
            }
            _grappleCanvas?.gameObject.SetActive(true);
            //_innerImage?.gameObject.SetActive(false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 hiehgtPoint = transform.position;
        hiehgtPoint.y = _exitHeight;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, hiehgtPoint);
        Gizmos.DrawWireSphere(hiehgtPoint, 0.5f);
    }



}
