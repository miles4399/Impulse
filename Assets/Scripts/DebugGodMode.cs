using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGodMode : MonoBehaviour
{
    [SerializeField] private KeyCode _key = KeyCode.Alpha1;
    [SerializeField] private Health _playerHealth;
   
    private void Update()
    {
        if (Input.GetKeyDown(_key))
        {
            _playerHealth.DebugGodMode = !_playerHealth.DebugGodMode;
            Debug.Log("PlayerGodMode: " + _playerHealth.DebugGodMode);
        }
    }
}
