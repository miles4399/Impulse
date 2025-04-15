using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LaserGun : MonoBehaviour
{
    [SerializeField] private float _fireRate;
    [SerializeField] private float _projectileSpeed;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField, FMODUnity.EventRef] private string FMODEventAIShoot;
    [SerializeField] private UnityEvent _onAIShoot;

    private float _shootTimer = 0f;
    private EnemyAIController _enemyAIController;
    

    void Awake()
    {
        _enemyAIController = GetComponentInParent<EnemyAIController>();
    }


    void Update()
    {
        _shootTimer -= Time.deltaTime;
    }

    public void Shoot()
    {
        
        if (_shootTimer <= 0)
        {
            _onAIShoot.Invoke();    //Audio
            _shootTimer = _fireRate;
            GameObject ProjectileIns = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);
            ProjectileIns.GetComponent<Rigidbody>().AddForce(_enemyAIController.Direction.normalized * _projectileSpeed);
        }
    }
}
