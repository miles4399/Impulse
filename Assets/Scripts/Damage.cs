using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField] private float _damage = 1f;               
    [SerializeField] private int _team = 0;                   
    [SerializeField] private float _minVelocity = 5f;
   
    private void OnTriggerEnter(Collider other)
    {
        TryDamage(other);
    }
     
    private void OnCollisionEnter(Collision other)
    {
        if (other.relativeVelocity.magnitude > _minVelocity)
        {
            TryDamage(other.collider);
        }
    }

    private void TryDamage(Collider other)
    {

        if (other.TryGetComponent(out Health target) == true && target.team != _team)
        {
            target.Damage(_damage);
        }

    }
}
