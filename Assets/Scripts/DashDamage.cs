using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashDamage : MonoBehaviour
{
    private CharacterMovement _characterMovement;

    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (_characterMovement.IsDashing)
        {
            if(other.TryGetComponent(out EnemyAIController enemy))
            {
                enemy.Death();
            }
            else if (other.TryGetComponent(out DestructableWall wall))
            {
                wall.Destroy();
            }
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (_characterMovement.IsDashing && collision.gameObject.layer == 11)
    //    {
    //        Destroy(collision.gameObject);
    //    }
    //}
}
