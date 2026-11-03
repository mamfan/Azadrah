using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkUiManager : MonoBehaviour
{
    public GameObject loading;
    public GameObject mainUi;
    public GameObject waitUi;
    
    void Start()
    {
        Loading();
    }
    
    public void Loading()
    {
        loading.SetActive(true);
        mainUi.SetActive(false);
        waitUi.SetActive(false);
    }
    
    public void NetworkConnected()
    {
        loading.SetActive(false);
        mainUi.SetActive(true);
        waitUi.SetActive(false);
    }
    
    public void Waiting()
    {
        loading.SetActive(false);
        mainUi.SetActive(false);
        waitUi.SetActive(true);
    }
}
