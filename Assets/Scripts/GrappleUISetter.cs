using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleUISetter : MonoBehaviour
{
    [SerializeField] ConnectGrapple _grapplePoint;

    public bool On = false;

    private void OnTriggerEnter(Collider collider)
    {
        //if (On)
        //{
        //    _grapplePoint.UIOn();
        //    Debug.Log("yes");
        //}
        //else
        //{
        //    _grapplePoint.UIOff();
        //    Debug.Log("No");
        //}
    }
}
