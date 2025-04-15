using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DashControl : MonoBehaviour
{
    [SerializeField] private float _chargeTime;
    [SerializeField] private int _maxChargeAmount;

    [SerializeField, FMODUnity.EventRef] private string _dashRechargePath;
    [SerializeField] private UnityEvent _dashRecharge;

    private float _currentChargeTime = 0f;
    private int _currentCharge = 0;

    public int _currentChargeAmount => _currentCharge;
    public float ChargePercentage => _currentChargeTime / _chargeTime;

    public bool NeedCharge = true;
    
    void Start()
    {
        StartCoroutine(Charge());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad9)) ToggleNoCharge();
    }

    private IEnumerator Charge()
    {
        while (_currentChargeTime < _chargeTime)
        {
            _currentChargeTime += Time.deltaTime;
            yield return null;
        }

        _dashRecharge.Invoke();
        Recharge();
        
    }

    private void Recharge()
    {
        StopAllCoroutines();
        _currentCharge++;
        if (_currentCharge == _maxChargeAmount) return;
        _currentChargeTime = 0f;
        StartCoroutine(Charge());
    }

    public void UseDash()
    {
        StopAllCoroutines();
        if (_currentCharge > 0)_currentCharge--;
        
        if(_currentChargeTime >= _chargeTime) _currentChargeTime = 0f;

        StartCoroutine(Charge());
    }

    public void ToggleNoCharge()
    {
        if(NeedCharge == true)
        {
            NeedCharge = false;
            Debug.Log("No Charge On");
        }
        else if (NeedCharge == false)
        {
            NeedCharge = true;
            Debug.Log("No Charge Off");
        }
    }
}
