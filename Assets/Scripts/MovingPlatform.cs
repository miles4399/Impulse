using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// moves platforms using Rigidbodies, MovingPlatform2D/MovingPlatform3D for Rigibody2D Rigidbody3D
public abstract class MovingPlatform : MonoBehaviour
{
    // points to loop through
    [SerializeField] private Vector3[] _points = new Vector3[] { -Vector3.right, Vector3.right };
    [SerializeField] private float _speed = 2f;
    // moves to next point within distance
    [SerializeField] private float _pointReachedDistance = 0.05f;
    // slows movement within distance
    [SerializeField] private float _easingDistance = 0.5f;

    public virtual Vector3 Velocity { get; protected set; }

    private Vector3 _startPosition;
    private int _pointIndex;
    private Vector3 _nextPoint => _startPosition + _points[_pointIndex % _points.Length];
    private Vector3 _previousPoint => _startPosition + _points[(_pointIndex + _points.Length - 1) % _points.Length];

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // checks if point is reached
        float distance = Vector3.Distance(transform.position, _nextPoint);
        if (distance < _pointReachedDistance) _pointIndex++;

        // calculates movement direction/speed/easing
        Vector3 dir = (_nextPoint - transform.position).normalized;
        float distanceToPrevious = Vector3.Distance(transform.position, _previousPoint);
        float previousEasing = distanceToPrevious / _easingDistance;
        float nextEasing = distance / _easingDistance;
        float easing = Mathf.Min(previousEasing, nextEasing);
        easing = Mathf.Clamp(easing, 0.1f, 1f);
        Velocity = dir * _speed * easing;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        for (int i = 0; i < _points.Length; i++)
        {
            Vector3 point = transform.position + _points[i];
            Vector3 nextPoint = transform.position + _points[(i + 1) % _points.Length];
            Gizmos.DrawWireSphere(point, 0.1f);
            Gizmos.DrawLine(point, nextPoint);
        }
    }
}
