using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnCar : MonoBehaviour
{
    private GameObject[] _cars;
    private float _minTime;
    private float _maxTime;
    private int _carInBound;
    private Player _player;
    private float _x;
    private float _totalChance;
    private SpawnCarStatics _statics;
    private float _timeReducer;

    void Start()
    {
        _statics = transform.GetComponentInParent<SpawnCarStatics>();
        _minTime = _statics.minTime;
        _maxTime = _statics.maxTime;
        _player = FindObjectOfType<Player>();
        _cars = GetComponentInParent<Camera>().cars;
        foreach (var car in _cars) _totalChance += car.GetComponent<TrafficRunner>().spawnChance;
        StartCoroutine(SpawnTraffic());
    }

    float ChanceCalc(int targetCar)
    {
        if (targetCar == 0) return 0;
        float res = 0;
        for (int i = 0; i < targetCar; i++)
        {
            res += _cars[i].GetComponent<TrafficRunner>().spawnChance;
        }

        return res;
    }

    IEnumerator SpawnTraffic()
    {
        while (true)
        {
            float playerSpeed = _player.speed;
            float minSpeedToSpawnCar = _player.minSpeedToSpawnCar;
            if (playerSpeed >= minSpeedToSpawnCar)
            {
                if (_carInBound == 0)
                {
                    float carNum = Random.Range(0, _totalChance);
                    for (int i = 0; i < _cars.Length; i++)
                    {
                        if (carNum > ChanceCalc(i) && carNum <= ChanceCalc(i + 1))
                        {
                            Instantiate(_cars[i].gameObject, transform.position, _cars[i].transform.rotation);
                            break;
                        }
                    }
                }

                if (_timeReducer < _minTime - 2) _timeReducer += 0.1f;
            }
            else if (_timeReducer > 0) _timeReducer -= 0.05f;

            yield return new WaitForSeconds(Random.Range(_minTime, _maxTime) - _timeReducer);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car")) _carInBound++;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car")) _carInBound--;
    }
}