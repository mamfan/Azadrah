using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CarDestroier : MonoBehaviour
{
    public void ParentChange(Transform parent)
    {
        transform.parent = parent;
        transform.DOMove(new Vector3(0, 0, 0), Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car")) other.GetComponent<TrafficRunner>().speed = 3;
    }
}