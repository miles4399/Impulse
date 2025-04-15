using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;

public class Rail : MonoBehaviour
{
    private BGCurve _curve;
    private BGCcMath _curveMath;

    private void Awake()
    {
        _curve = GetComponent<BGCurve>();
        _curveMath = GetComponent<BGCcMath>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out CharacterMovement characterMovement))
        {
            _curveMath.CalcPositionByClosestPoint(characterMovement.transform.position, out float currentDistance);
            characterMovement.EnterRail(this, currentDistance);
        }
    }

    public Vector3 GetPositionAndRotationByDistance(float distance, out Vector3 railForward)
    {
        Vector3 position = _curveMath.CalcPositionAndTangentByDistance(distance, out Vector3 tangent);
        railForward = tangent;
        return position;
    }

    public float GetRailLength()
    {
        return _curveMath.GetDistance();
    }
}
