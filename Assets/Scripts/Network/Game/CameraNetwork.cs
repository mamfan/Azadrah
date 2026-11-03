using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraNetwork : MonoBehaviour
{
    public GameObject[] cars;
    public GameObject _player;
    private float _x;

    private void LateUpdate()
    {
        if (_player != null)
        {
            Vector3 p = transform.position;
            _x = _player.transform.position.x - p.x;
            transform.Translate(_x, p.y, p.z);
        }
    }
}