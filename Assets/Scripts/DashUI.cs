using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DashUI : MonoBehaviour
{
    [SerializeField] private DashControl _chargeAmount;

    private GameObject _player;
    private Image _image;
    private TextMeshProUGUI _dashCount;


    void Start()
    {
        _image = GetComponentInChildren<Image>();
        _dashCount = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {       
        if(_dashCount != null)
        {
            _image.fillAmount = _chargeAmount.ChargePercentage;
            _dashCount.text = _chargeAmount._currentChargeAmount.ToString();

        }
    }

}
