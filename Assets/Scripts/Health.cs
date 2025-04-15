using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Health : MonoBehaviour
{
    [SerializeField] private float _currentHp = 1f;
    [SerializeField] private float _maxHp = 1f;
    [SerializeField] private int _team = 0;
    
    [SerializeField] private Health _health;
    
   


    private Rigidbody _rigdyBody;

    public bool DebugGodMode;
    public UnityEvent OnDeath;
    public UnityEvent OnHit;
    public int team => _team;
    public float healthPercentage => _currentHp / _maxHp;

    private void Awake()
    {
        _rigdyBody = GetComponent<Rigidbody>();   
    }

    private void OnDisable()
    {
       // StopAllCoroutines();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            Damage(1f);
        }

    }

    public void Damage(float amount)
    {
        if (_currentHp <= 0f || DebugGodMode == true)
        {
            return;
        }

        float combinedValue = _currentHp - amount;

        _currentHp = Mathf.Clamp(combinedValue, 0, _maxHp);

        if (_currentHp <= 0f) Death();

        OnHit.Invoke();
  
    }

    public void Death()
    {
        //DebugCheckpoints.instance.MoveToCheckpoint();
        _rigdyBody.velocity = new Vector3(0, 0, 0);
        OnDeath.Invoke();
        _currentHp = 1f;
        
        return;
    }

    //public void EnableInvincible(float timer)
    //{
        //StartCoroutine(InvincibilityTimer(timer));

   // }

    //private IEnumerator InvincibilityTimer(float timer)
   // {
        //DebugGodMode = true;
       // yield return new WaitForSeconds(timer);
       // DebugGodMode = false;
   // }
}
