using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRotation : MonoBehaviour
{
    public void FixedUpdate()
    {
        transform.Rotate(0, 1, 0 * Time.realtimeSinceStartup);
    }
}
