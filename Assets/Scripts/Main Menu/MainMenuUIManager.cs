using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIManager : MonoBehaviour
{
    public GameObject mainMenuCanvas;
    public GameObject shopCanvas;
    public GameObject selectCanvas;
    public GameObject rahnamaCanvas;
    public GameObject aboutCanvas;
    public Transform mainCam;
    public Transform camPosMain;
    public Transform camPosShop;
    public Transform camPosSelect;

    private PlayerMenu _playerMenu;

    private void Start()
    {
        _playerMenu = FindObjectOfType<PlayerMenu>();
        MainMenu();
    }

    void InitScale(GameObject menu)
    {
        _playerMenu.playerCar = PlayerPrefs.GetInt("carnum");
        _playerMenu.CarStatusUpdate();
        foreach (var c in menu.GetComponentsInChildren<Transform>())
        {
            if (c.name != "Back") c.DOScale(Vector3.zero, Time.deltaTime);
        }
    }
    
    void DoScale(GameObject menu)
    {
        foreach (var c in menu.GetComponentsInChildren<Transform>())
        {
            if (c.name == "PreviewCartBtn" || c.name == "StartGameBtn") c.DOScale(new Vector3(-1, 1, 1), 1);
            else if (c.name != "Back") c.DOScale(Vector3.one, 1);
        }
    }
    
    public void ShopMenu()
    {
        InitScale(shopCanvas);
        mainCam.DOMove(camPosShop.position, 1);
        mainCam.DORotateQuaternion(camPosShop.rotation, 1);
        mainMenuCanvas.SetActive(false);
        selectCanvas.SetActive(false);
        rahnamaCanvas.SetActive(false);
        aboutCanvas.SetActive(false);
        shopCanvas.SetActive(true);
        DoScale(shopCanvas);
    }

    public void MainMenu()
    {
        InitScale(mainMenuCanvas);
        mainCam.DOMove(camPosMain.position, 1);
        mainCam.DORotateQuaternion(camPosMain.rotation, 1);
        mainMenuCanvas.SetActive(true);
        selectCanvas.SetActive(false);
        rahnamaCanvas.SetActive(false);
        aboutCanvas.SetActive(false);
        shopCanvas.SetActive(false);
        DoScale(mainMenuCanvas);
    }
    
    public void RahnamaMenu()
    {
        InitScale(rahnamaCanvas);
        mainCam.DOMove(camPosMain.position, 1);
        mainCam.DORotateQuaternion(camPosMain.rotation, 1);
        mainMenuCanvas.SetActive(false);
        selectCanvas.SetActive(false);
        rahnamaCanvas.SetActive(true);
        aboutCanvas.SetActive(false);
        shopCanvas.SetActive(false);
        DoScale(rahnamaCanvas);
    }
    
    public void AboutMenu()
    {
        InitScale(aboutCanvas);
        mainCam.DOMove(camPosMain.position, 1);
        mainCam.DORotateQuaternion(camPosMain.rotation, 1);
        mainMenuCanvas.SetActive(false);
        selectCanvas.SetActive(false);
        rahnamaCanvas.SetActive(false);
        aboutCanvas.SetActive(true);
        shopCanvas.SetActive(false);
        DoScale(aboutCanvas);
    }

    public void SelectMenu()
    {
        InitScale(selectCanvas);
        mainCam.DOMove(camPosSelect.position, 1);
        mainCam.DORotateQuaternion(camPosSelect.rotation, 1);
        mainMenuCanvas.SetActive(false);
        selectCanvas.SetActive(true);
        rahnamaCanvas.SetActive(false);
        aboutCanvas.SetActive(false);
        shopCanvas.SetActive(false);
        DoScale(selectCanvas);
    }

    public void exitGame()
    {
        Application.Quit();
    }
}