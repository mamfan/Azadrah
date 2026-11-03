using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class RoomManager : MonoBehaviourPunCallbacks
{
    // public static RoomManager Instance;

    private void Awake()
    {
        // DontDestroyOnLoad
        RoomManager[] roomManagers = FindObjectsOfType<RoomManager>();
        if (roomManagers.Length > 1)
        {
            foreach (var r in roomManagers)
            {
                if (r.gameObject.scene.name != "DontDestroyOnLoad")
                {
                    Destroy(r.gameObject);
                }
            }
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // void DestroyInstance()
    // {
    //     // if (Instance)
    //     // {
    //     //     Destroy(gameObject);
    //     // }
    //     // else
    //     // {
    //         DontDestroyOnLoad(gameObject);
    //         // Instance = this;
    //         // print(gameObject.scene.name);
    //     // }
    // }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (scene.buildIndex == 3)
        {
            PhotonNetwork.Instantiate("PlayerManager", Vector3.zero,
                Quaternion.identity);
        }
    }
}