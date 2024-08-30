using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Weapon : MonoBehaviour
{
    [Header("Setup")][SerializeField] private Vector3 damagePoint = Vector3.zero;
    [Range(.1f, 2f)][SerializeField] private float damageRadius = .5f;

    [Header("Events")]
    [SerializeField] UnityEvent onHit;

    public Vector3 DamagePoint => transform.TransformPoint(damagePoint);
    public float DamageRadius => damageRadius;

    public void OnHit()
    {
        onHit.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(DamagePoint, DamageRadius);
    }

}
