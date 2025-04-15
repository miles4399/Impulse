using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventExample : MonoBehaviour
{
    [SerializeField, FMODUnity.EventRef] private string FMODEvent;
    [SerializeField] private UnityEvent Landed; //This makes the event visible in the editor 

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            Landed.Invoke(); // I have to call Invoke for the Event to work
        }
    }


}
