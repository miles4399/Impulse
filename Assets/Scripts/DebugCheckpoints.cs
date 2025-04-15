using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DebugCheckpoints : MonoBehaviour
{
    [SerializeField] private CharacterMovement _character;

    private GameObject[] _debugCheckpoints;
    private int _index = 0;
    private Health _health;
    

    private bool _checkpointUp = true;
    private Vector3 _currentRespawnPoint;
    private Vector3 _reloadPoint;
    private Quaternion _currentRespawnPointRot;

    private void Start()
    {
        _debugCheckpoints = GameObject.FindGameObjectsWithTag("DebugCheckpoints").OrderBy(x => x.name).ToArray();
        _health = GetComponent<Health>();
        _currentRespawnPoint = transform.position;
        _reloadPoint = transform.position;
    }

    private void Update()
    {
        // Moving to the previous checkpoint
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            // Set the current checkpoint to the previous one in the array
            _index--;

            // Allows to cycle through checkpoints backwards infinitely
            if (_index == -1)
            {
                _index = _debugCheckpoints.Length;
            }

            MoveToCheckpoint();
        }

        // Changing the checkpoint to the next one
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            _index++;
            MoveToCheckpoint();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            _checkpointUp = false;
        }

        if (Input.GetKeyDown(KeyCode.Alpha9)) Debug.Log(_currentRespawnPointRot);


    }

    // KillYVolume function
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "KillYVolume")
        {
            _health.Death();
            Respawn();
        }
    }



    // Checkpoint update function
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "CheckpointUp" && _checkpointUp == true)
    //    {
    //        _index++;

    //        // don't change checkpoints repeatedly as you enter the same area
    //        other.gameObject.SetActive(false);
    //    }
    //}

    public void SetCheckpoint(Transform checkpointPos)
    {
        _currentRespawnPoint = checkpointPos.position;
        _currentRespawnPointRot = checkpointPos.rotation;
        
    }

    public void MoveToCheckpoint()
    {
        // Teleport the player to the current checkpoint
        // % divides and gives the remainder, allow for us to loop through checkpoints without going out of range

        transform.position = _debugCheckpoints[_index % _debugCheckpoints.Length].transform.position;
        

        
    }

    public void Respawn()
    {
        transform.position = _currentRespawnPoint;
        Vector3 direction = new Vector3(0f, 0f, 0f);
        //_character.SetLookDirection ( _currentRespawnPointRot *direction);
    }

    public void Reload()
    {

        _currentRespawnPoint = _reloadPoint;
        transform.position = _reloadPoint;
    }
}
