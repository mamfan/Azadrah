using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InfinitRoad : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            transform.SetPositionAndRotation(transform.GetChild(0).position, new Quaternion(0, 0, 0, 0));
        }
        else if (SceneManager.GetActiveScene().name == "Main Menu" && other.transform.parent.CompareTag("Player"))
        {
            transform.SetPositionAndRotation(transform.GetChild(0).position, new Quaternion(0, 0, 0, 0));
        }
    }
}