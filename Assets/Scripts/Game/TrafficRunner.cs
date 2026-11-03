using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrafficRunner : MonoBehaviour
{
    public float speed = 1;
    public float spawnChance = 0.1f;
    private Player _player;
    private bool _isPlayer;
    private Transform[] _wh;

    void Start()
    {
        _wh = transform.GetComponentsInChildren<Transform>();
        _player = FindObjectOfType<Player>();
        if (!_isPlayer)
        {
            float maxS = _player.maxSpeed * .8f;
            float minS = _player.minSpeedToSpawnCar;
            speed = Random.Range(minS, maxS);
        }
    }

    void Update()
    {
        if (!_isPlayer && transform.position.z < 50)
        {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            if (transform.position.y < -4)
            {
                Destroy(gameObject);
            }

            for (int i = 1; i < _wh.Length; i++) _wh[i].Rotate(speed * 24 * Time.deltaTime, 0, 0);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!transform.CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("Car"))
            {
                float speed2 = collision.gameObject.GetComponent<TrafficRunner>().speed;
                if (transform.position.x > collision.transform.position.x && speed < speed2)
                {
                    float speed3 = speed;
                    speed = speed2;
                    collision.gameObject.GetComponent<TrafficRunner>().speed = speed3;
                }
            }
            else if (collision.transform.CompareTag("Player"))
            {
                speed = 0;
                _player.speed = 0;
                StartCoroutine(_player.gameOver());
                // if (_player.speed > _player.minSpeedToSpawnCar) _player.gameOver();
                // else StartCoroutine(carDeath());
            }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCamera")) Destroy(gameObject);
    }
}