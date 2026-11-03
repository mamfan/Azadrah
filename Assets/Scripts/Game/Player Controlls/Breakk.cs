using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakk : MonoBehaviour
{
    public AudioSource breakAudio;
    private Player _player;
    private bool state;

    private void Start()
    {
        _player = FindObjectOfType<Player>();
    }

    public void stateSet(bool st)
    {
        state = st;
        if (st) breakAudio.Play();
    }

    private void Update()
    {
        if (
            Input.GetKey(KeyCode.DownArrow)
            || Input.GetKey(KeyCode.Space)
            || Input.GetKey(KeyCode.S)
        )
        {
            if (!state)
            {
                stateSet(true);
            }
        }
        else
        {
            if (state)
            {
                stateSet(false);
            }
        }

        if (state)
        {
            _player.GetBreak();
        }
    }
}