using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SetCheckpoint : MonoBehaviour
{
    public bool SpawnAtCollider;                       

    [BoxGroup("CheckPointObject", VisibleIf = "revertBool")][SerializeField] private GameObject _checkpoint;

    private bool revertBool => !SpawnAtCollider;

    private void Start()
    {
        //SpawnAtCollider = !_revertBool;
    }





    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out DebugCheckpoints checkpoint))
        {
            if(SpawnAtCollider) checkpoint.SetCheckpoint(this.transform);
            else checkpoint.SetCheckpoint(_checkpoint.transform);

        }


    }

}
