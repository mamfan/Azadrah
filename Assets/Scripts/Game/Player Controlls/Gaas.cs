using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaas : MonoBehaviour
{
    private Player _player;
    private bool state;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    public void stateSet(bool st)
    {
        state = st;
    }

    private void Update()
    {
        if (
            Input.GetKey(KeyCode.UpArrow)
            || Input.GetKey(KeyCode.W)
        )
        {
            stateSet(true);
        }
        else
        {
            stateSet(false);
        }


        if (state) _player.GetGas();
        else _player.GetNonePower();
    }
}