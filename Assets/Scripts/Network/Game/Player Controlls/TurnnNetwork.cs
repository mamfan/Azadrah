using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class TurnnNetwork : MonoBehaviour
{
    public PlayerNetwork _player;
    private int state;

    public void stateSet(int st)
    {
        switch (st)
        {
            case 1:
                _player.transform.GetChild(0).DORotate(new Vector3(-90, 90, 20), .5f);
                break;
            case -1:
                _player.transform.GetChild(0).DORotate(new Vector3(-90, 90, -20), .5f);
                break;
            case 0:
                _player.transform.GetChild(0).DORotate(new Vector3(-90, 90, 0), .5f);
                break;
        }
        state = st;
    }

    private void Update()
    {
        _player.Turn(state);
    }
}