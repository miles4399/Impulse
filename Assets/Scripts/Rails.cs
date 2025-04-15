using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Components;

public class Rails : MonoBehaviour
{
    [SerializeField] private GameObject _curve;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _snapDistance = 1f;
    public static Rails Instance { get; private set; }
    private void Awake()
    {
        Instance = this;

    }

    private BGCcMath _curveMath;
    private float _distance = 0f;

    private void Start()
    {
        _curveMath = _curve.GetComponent<BGCcMath>();
    }

    private void Update()
    {
        // get closest point on rail
        Vector3 closestPoint = _curveMath.CalcPositionByClosestPoint(transform.position, out float currentDistance);
        Debug.DrawLine(transform.position, closestPoint);

        float distanceToRail = Vector3.Distance(transform.position, closestPoint);
        if (distanceToRail < _snapDistance)
        {
            //increase distance
            _distance = currentDistance + _speed * Time.deltaTime;

            // calculate position and tangeant
            transform.position = _curveMath.CalcPositionAndTangentByDistance(_distance, out Vector3 tangent);
            transform.rotation = Quaternion.LookRotation(tangent);
        }
    }

}
