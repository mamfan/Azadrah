using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaasNetwork : MonoBehaviour
{
    public PlayerNetwork _player;
    private bool state;

    public void stateSet(bool st)
    {
        state = st;
    }

    private void Update()
    {
        if (state) _player.GetGas();
        else _player.GetNonePower();
    }
}