using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;

public class SpawnCarStaticsNetwork : MonoBehaviour
{
    public float minTime = 4;
    public float maxTime = 8;
    public Transform[] players;
    private bool _gameStarted;

    private void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            print("I AM THE MAAASTER!");
            StartCoroutine(SyncX());
        }
        else
        {
            print("THE CLIENT IS MEEE!");
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (!_gameStarted)
        {
            if (players.Length < 2)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
                players[0].GetComponent<PlayerNetwork>()._endt = (float)PhotonNetwork.Time + 30;
                // StartCoroutine(players[0].GetComponent<PlayerNetwork>().GameTimer());
                _gameStarted = true;
            }   
        }
    }

    IEnumerator SyncX()
    {
        while (true)
        {
            if (_gameStarted) transform.position = new Vector3(Math.Max(players[0].position.x, players[1].position.x), 0, 0);
            yield return new WaitForSeconds(.5f);
        }
    }
}