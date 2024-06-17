using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPoint : MonoBehaviour
{
    public float radius;
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
