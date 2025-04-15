using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCameraMatcher : MonoBehaviour
{
    [SerializeField] private CinemachineStateDrivenCamera _stateCamera;

    private void Update()
    {
        ICinemachineCamera live = _stateCamera.LiveChild;
        if(live is CinemachineFreeLook freeLook)
        {
            for (int i = 0; i < _stateCamera.ChildCameras.Length; i++)
            {
                CinemachineVirtualCameraBase otherCam = _stateCamera.ChildCameras[i];
                if(otherCam is CinemachineFreeLook otherFreeLook)
                {
                    otherFreeLook.m_XAxis.Value = freeLook.m_XAxis.Value;
                    otherFreeLook.m_YAxis.Value = freeLook.m_YAxis.Value;
                }
            }
        }
    }
}