using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakkNetwork : MonoBehaviour
{
    public AudioSource breakAudio;
    public PlayerNetwork _player;
    private bool state;

    public void stateSet(bool st)
    {
        state = st;
        if (st) breakAudio.Play();
    }

    private void Update()
    {
        if (state)
        {
            _player.GetBreak();
        }
    }
}