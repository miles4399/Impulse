using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillYVolume : MonoBehaviour
{
    [SerializeField] private Vector3 _respawnPoint;
    [SerializeField] private GameObject _player;

    private void OnTriggerEnter(Collider other)
    {
        _player.transform.position = _respawnPoint;
    }
}
