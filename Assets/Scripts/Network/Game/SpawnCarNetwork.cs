using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Photon.Pun;

public class SpawnCarNetwork : MonoBehaviour
{
    private GameObject[] _cars;
    private float _minTime;
    private float _maxTime;
    private int _carInBound;
    public PlayerNetwork _player;
    private float _x;
    private float _totalChance;
    private SpawnCarStaticsNetwork _statics;
    private float _timeReducer;

    void Start()
    {
        if (!PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Destroy(gameObject);
        }
        _statics = transform.GetComponentInParent<SpawnCarStaticsNetwork>();
        _minTime = _statics.minTime;
        _maxTime = _statics.maxTime;
        // _player = FindObjectOfType<PlayerNetwork>();
        _cars = FindObjectOfType<CameraNetwork>().cars;
        foreach (var car in _cars) _totalChance += car.GetComponent<TrafficRunnerNetwork>().spawnChance;
        if (PhotonNetwork.IsMasterClient) StartCoroutine(SpawnTraffic());
    }

    float ChanceCalc(int targetCar)
    {
        if (targetCar == 0) return 0;
        float res = 0;
        for (int i = 0; i < targetCar; i++)
        {
            res += _cars[i].GetComponent<TrafficRunnerNetwork>().spawnChance;
        }

        return res;
    }

    IEnumerator SpawnTraffic()
    {
        while (true)
        {
            if (_carInBound == 0)
            {
                float carNum = Random.Range(0, _totalChance);
                for (int i = 0; i < _cars.Length; i++)
                {
                    if (carNum > ChanceCalc(i) && carNum <= ChanceCalc(i + 1))
                    {
                        PhotonNetwork.Instantiate(_cars[i].name, transform.position, _cars[i].transform.rotation);
                        break;
                    }
                }

                if (_timeReducer < _minTime - 2) _timeReducer += 0.05f;
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