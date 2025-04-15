using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockCursor : MonoBehaviour
{
    private enum TimeScale
    {
        Zero,
        One
    }

    private enum CursorLockState
    {
        None,
        Lock,
        Confined
    }

    private enum CursorVisibility
    {
        visible,
        notVisible
    }


    [SerializeField] private TimeScale _timeScale;
    [SerializeField] private CursorLockState _cursorLockState;
    [SerializeField] private CursorVisibility _cursorVisiblity;

     

    void Start()
    {
        if (_timeScale == TimeScale.One) Time.timeScale = 1f;
        else Time.timeScale = 0f;

        if(_cursorVisiblity == CursorVisibility.visible) Cursor.visible = true;
        else Cursor.visible = false;

        if (_cursorLockState == CursorLockState.None) Cursor.lockState = CursorLockMode.None;
        else if (_cursorLockState == CursorLockState.Confined) Cursor.lockState = CursorLockMode.Confined;
        else Cursor.lockState = CursorLockMode.Locked;








    }


}
