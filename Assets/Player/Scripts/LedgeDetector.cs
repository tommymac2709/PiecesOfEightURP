using UnityEngine;
using System;

public class LedgeDetector : MonoBehaviour
{
    public event Action<Vector3, Vector3> OnLedgeDetect;

    public  bool canGrabLedge = false;

    private void Start()
    {
        canGrabLedge = false;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Climbing"))
        {
            canGrabLedge = true;
        }

        if (canGrabLedge && Input.GetKey(KeyCode.W))
        {
            OnLedgeDetect?.Invoke(other.ClosestPoint(transform.position), other.transform.forward);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Climbing"))
        {
            canGrabLedge = false;
        }
    }


}
