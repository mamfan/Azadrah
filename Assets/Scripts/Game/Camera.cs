using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject[] cars;
    private GameObject _player;
    private float _x;

    private void Start()
    {
        _player = FindObjectOfType<Player>().gameObject;
    }

    private void LateUpdate()
    {
        Vector3 p = transform.position;
        _x = _player.transform.position.x - p.x;
        transform.Translate(_x, p.y, p.z);
    }
}