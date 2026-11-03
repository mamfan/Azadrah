using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrafficRunnerNetwork : MonoBehaviour
{
    public float speed = 1;
    public float spawnChance = 0.1f;
    // private PlayerNetwork _player;
    private bool _isPlayer;
    private Transform[] _wh;

    private Transform spawnStatics;
    // private bool _isHarif;
    // Photon.Realtime.Player harif;
    // private float _xPos;

    void Start()
    {
        // if (!GetComponent<PhotonView>().Owner.IsMasterClient)
        // {
        //     PhotonNetwork.Destroy(gameObject);
        // }
        spawnStatics = FindObjectOfType<SpawnCarStaticsNetwork>().transform;
        transform.DOMoveY(0.65f, Time.deltaTime);
        _wh = transform.GetComponentsInChildren<Transform>();
        // _player = FindObjectOfType<PlayerNetwork>();
        if (!_isPlayer)
        {
            // float maxS = _player.maxSpeed - 10;
            // float minS = _player.minSpeedToSpawnCar;
            speed = Random.Range(30, 40);
        }

        // if (!GetComponent<PhotonView>().Owner.IsMasterClient)
        // {
        //     if (PhotonNetwork.IsMasterClient ||
        //         (!PhotonNetwork.IsMasterClient && GetComponent<PhotonView>().sceneViewId < 3))
        //     {
        //         _isHarif = true;
        //         foreach (var p in PhotonNetwork.PlayerList)
        //         {
        //             if (!p.IsLocal) harif = p;
        //         }
        //     }
        // }
    }

    void Update()
    {
        // if (_isHarif)
        // {
        //     print(harif.CustomProperties["speed"]);
        //     speed = (float)harif.CustomProperties["speed"];
        //     _xPos = (float)harif.CustomProperties["xpos"];
        //     transform.Translate(0, -speed * Time.deltaTime, 0);
        //     for (int i = 1; i < _wh.Length; i++) _wh[i].Rotate(speed * 24 * Time.deltaTime, 0, 0);
        // }
        // else 
        if (!_isPlayer && transform.position.z < 50)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            // if (transform.position.y < -4)
            // {
            //     Destroy(gameObject);
            // }
            if (transform.position.x > spawnStatics.position.x + 400)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
        
        for (int i = 1; i < _wh.Length; i++) _wh[i].Rotate(speed * 24 * Time.deltaTime, 0, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!transform.CompareTag("playerCh"))
        {
            if (collision.gameObject.CompareTag("Car"))
            {
                float speed2 = collision.gameObject.GetComponent<TrafficRunnerNetwork>().speed;
                if (transform.position.x > collision.transform.position.x && speed < speed2)
                {
                    float speed3 = speed;
                    speed = speed2;
                    collision.gameObject.GetComponent<TrafficRunnerNetwork>().speed = speed3;
                }
            }
            // else if (collision.transform.CompareTag("Player"))
            // {
            //     speed = 0;
            //     _player.speed = 0;
            //     StartCoroutine(_player.gameOver());
            //     // if (_player.speed > _player.minSpeedToSpawnCar) _player.gameOver();
            //     // else StartCoroutine(carDeath());
            // }
        }
    }

    // private void OnCollisionStay(Collision collisionInfo)
    // {
    //     if (collisionInfo.gameObject.CompareTag("GurdRail"))
    //     {
    //         _player.GetBreak();
    //         if (_player.speed > _player.minSpeedToSpawnCar)
    //         {
    //             StartCoroutine(_player.gameOver());
    //         }
    //     }
    // }

    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag("MainCamera")) Destroy(gameObject);
    // }
}