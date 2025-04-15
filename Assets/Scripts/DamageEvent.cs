using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEvent : MonoBehaviour
{
    [SerializeField] private float damageOff = 6f;
    [SerializeField] private float damageOn = 2f;

    [Header("Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    [SerializeField] private ParticleSystem [] _fireSystems;

    private Collider _collider;
    
    private bool _on = true;

    private void Start()
    {
        _collider = GetComponentInChildren<Collider>();
        StartCoroutine(DamageEventLoop());
    }

    private IEnumerator DamageEventLoop()
    {
        while (true)
        {
            if (_on == true)
            {
                _audioSource.clip = _audioClip;
                _audioSource.Play();

                foreach (ParticleSystem fire in _fireSystems)
                {
                    fire.Play();
                }
            }
            else if (_on == false)
            {
                _audioSource.Stop();

                foreach (ParticleSystem fire in _fireSystems)
                {
                    fire.Stop();
                }
            }

            _collider.enabled = _on;


            float wait = _on ? damageOn : damageOff;
            yield return new WaitForSeconds(wait);
            _on = !_on;
        }
    }
}
