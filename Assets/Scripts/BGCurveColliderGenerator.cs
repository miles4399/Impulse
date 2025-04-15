using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BansheeGz.BGSpline.Components;

public class BGCurveColliderGenerator : MonoBehaviour
{
    [SerializeField] private int _colliderCount = 10;
    [SerializeField] private float _boxSize = 1f;
    [SerializeField] private string _assignLayer = "Rail";

    [ContextMenu("Generate Colliders")]
    public void Generate()
    {
        DestroyExisting();

        BGCcMath math = GetComponent<BGCcMath>();
        int layer = LayerMask.NameToLayer(_assignLayer);

        for (int i = 0; i < _colliderCount; i++)
        {
            float percentage = (float)i / (_colliderCount - 1);
            Vector3 position = math.CalcPositionAndTangentByDistanceRatio(percentage, out Vector3 tangent);
            GameObject colliderGO = new GameObject("SplineCollider");
            colliderGO.transform.SetParent(transform);
            colliderGO.transform.position = position + Vector3.up * _boxSize * 0.5f;
            colliderGO.transform.rotation = Quaternion.LookRotation(tangent);
            colliderGO.layer = layer;
            colliderGO.AddComponent<BoxCollider>();
            BoxCollider collider = colliderGO.GetComponent<BoxCollider>();
            collider.size = Vector3.one * _boxSize;
            collider.isTrigger = true;
        }
    }

    private void DestroyExisting()
    {
        BoxCollider[] colliders = GetComponentsInChildren<BoxCollider>();
        for (int i = 0; i < colliders.Length; i++)
        {
            DestroyImmediate(colliders[i].gameObject);
        }
    }
}