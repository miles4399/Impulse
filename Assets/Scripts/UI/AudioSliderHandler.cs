using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSliderHandler : MonoBehaviour
{
    [SerializeField] private string _busName;
    [SerializeField] private string _prefName;

    private FMOD.Studio.Bus _busRef;

    protected void Start()
    {
        Slider slider = GetComponent<Slider>();
        slider.value = PlayerPrefs.GetFloat(_prefName);
        _busRef = FMODUnity.RuntimeManager.GetBus($"Bus:/{_busName}");
        _busRef.setVolume(slider.value);
    }

    public void SetBusVolume(float value)
    {
        PlayerPrefs.SetFloat(_prefName, value);
        _busRef.setVolume(value);
    }

}
