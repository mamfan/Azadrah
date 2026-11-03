using System;
using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using ExitGames.Client.Photon;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using RTLTMPro;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class NetworkLauncher : MonoBehaviourPunCallbacks
{
    public RTLTextMeshPro userName;
    private RTLTextMeshPro _networkStatusText;
    private NetworkUiManager _uiManager;
    private bool _started;

    void Start()
    {
        print("connecting...");
        _uiManager = FindObjectOfType<NetworkUiManager>();
        _networkStatusText = _uiManager.loading.GetComponentInChildren<RTLTextMeshPro>();
        _networkStatusText.text = "در حال اتصال به سرور...";
        PhotonNetwork.ConnectUsingSettings();
    }
    
    public override void OnConnectedToMaster()
    {
        _networkStatusText.text = "در حال اتصال به لابی...";
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        _networkStatusText.text = "متصل شد!";
        _uiManager.NetworkConnected();
    }

    public void StartRoom()
    {
        print("start");
        _uiManager.mainUi.GetComponentInChildren<Button>().interactable = false;
        string un = userName.text;
        if (un.Length > 3 && !PhotonNetwork.InRoom)
        {
            PhotonNetwork.LocalPlayer.CustomProperties.Add("skin", PlayerPrefs.GetInt("carnum"));
            PhotonNetwork.NickName = un;
            print("joining");
            RoomOptions ropts = new RoomOptions { IsOpen = true, IsVisible = true, MaxPlayers = 2 };
            Hashtable roomProps = new Hashtable();
            roomProps.Add("jayeze", 200);
            roomProps.Add("endtime", 0);
            ropts.CustomRoomProperties = roomProps;
            PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: ropts);
        }
        else _uiManager.mainUi.GetComponentInChildren<Button>().interactable = true;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        StartCoroutine(CreateRoomFail(returnCode, message));
        _uiManager.mainUi.GetComponentInChildren<Button>().interactable = false;
    }

    IEnumerator CreateRoomFail(short code, string ms)
    {
        _uiManager.Loading();
        _networkStatusText.text = $"Error!\nCode: {code}\nmessage: {ms}";
        yield return new WaitForSeconds(10);
        _uiManager.NetworkConnected();
    }

    public override void OnJoinedRoom()
    {
        print("joined");
        // PhotonNetwork.LocalPlayer.CustomProperties.Add("endtime", PhotonNetwork.Time + 35);
        print(PhotonNetwork.CurrentRoom.Name);
        print(PhotonNetwork.CurrentRoom.MaxPlayers);
        print(PhotonNetwork.NickName);
        print(PhotonNetwork.LocalPlayer.CustomProperties["skin"]);
        PlayerPrefs.SetFloat("endtime",(float)PhotonNetwork.Time + 35);
        _uiManager.Waiting();
    }

    private void Update()
    {
        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
            {
                if (!_started)
                {
                    PhotonNetwork.CurrentRoom.IsVisible = false;
                    print("started");
                    _started = true;
                    Debug.Log(PhotonNetwork.CurrentRoom.CustomProperties["endtime"]);
                    FindObjectOfType<DemoLoadScene>().LoadSceneAsync("GameMulti");
                }
            }
        }
    }
}