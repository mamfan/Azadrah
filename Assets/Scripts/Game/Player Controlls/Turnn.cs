using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Turnn : MonoBehaviour
{
    private Player _player;
    private int state;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

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
        if (
            Input.GetKey(KeyCode.RightArrow)
            || Input.GetKey(KeyCode.D)
        )
        {
            stateSet(1);
        }
        else if (
            Input.GetKey(KeyCode.LeftArrow)
            || Input.GetKey(KeyCode.A)
        )
        {
            stateSet(-1);
        }
        else
        {
            stateSet(0);
        }

        _player.Turn(state);
    }
}