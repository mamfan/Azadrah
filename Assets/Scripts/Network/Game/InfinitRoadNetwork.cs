using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class InfinitRoadNetwork : MonoBehaviour
{
    public GameObject theRoad;
    public Transform player;

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player") && other.transform == player)
        {
            Instantiate(theRoad, transform.GetChild(0).position, new Quaternion(0, 0, 0, 0));
            Destroy(gameObject);
        }
    }
}