using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;


[RequireComponent(typeof(CapsuleCollider))]
public class CameraMagnet : MonoBehaviour
{
    [SerializeField] private CinemachineTargetGroup _targetGroup;
    [SerializeField] private float _radius = 4f;
    [SerializeField] private float _strength = 3f;
    [SerializeField] private float _clamp = 0.5f;
    [SerializeField] private float _lerpSpeed = 5f;
    [SerializeField] private float _angle = 90f;
    [SerializeField] private string _targetTag = "Player";

    private Transform _target;
    private bool _setWeight;
    private CapsuleCollider _collider;
    private float _weight;

    private void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        _collider.isTrigger = true;
        _collider.radius = _radius;
        gameObject.layer = 1 << 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_targetGroup == null) return;
        if (!other.CompareTag(_targetTag)) return;
        _target = _targetGroup.m_Targets[0].target;
        _targetGroup.AddMember(transform, 0f, _radius);
        _setWeight = true;
        _weight = 0f;
    }

    private void OnTriggerExit(Collider other)
    {
        if (_targetGroup == null) return;
        if (!other.CompareTag(_targetTag)) return;
        _targetGroup.RemoveMember(transform);
        _setWeight = false;
    }

    private void Update()
    {
        if (!_setWeight) return;

        float distance = Vector3.Distance(_target.position, transform.position);
        float targetWeight = Mathf.Clamp((1f - distance / _radius) * _strength, 0f, _clamp);
        _weight = Mathf.Lerp(_weight, targetWeight, Time.deltaTime * _lerpSpeed);
        int index = _targetGroup.FindMember(transform);
        _targetGroup.m_Targets[index].weight = _weight;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}


