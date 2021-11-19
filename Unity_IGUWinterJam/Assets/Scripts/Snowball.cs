using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    [SerializeField] LayerMask snowLayerMask;
    [SerializeField] int snowPickupRadius = 8;

    int snowPoints = 0;

    void Update()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, 0.1f, Vector3.down*0.1f, 0.001f, snowLayerMask);

        foreach (var hit in hits)
        {
            var snowfield = hit.collider.gameObject.GetComponent<SnowField>();
            if (snowfield != null)
                CheckForSnow(snowfield);
        }
    }

    void CheckForSnow(SnowField snowField)
    {
        Vector2 where = snowField.GetPosition(transform);
        snowPoints += snowField.CheckForSnow(where.x, where.y, snowPickupRadius);
    }

    private void OnDrawGizmos()
    {
        Handles.Label(transform.position, snowPoints.ToString());
    }
}
