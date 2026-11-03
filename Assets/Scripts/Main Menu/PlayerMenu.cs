using System;
using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMenu : MonoBehaviour
{
    public float speed = 1;
    private Transform[] _wh;
    public GameObject[] skins;
    public int playerCar;
    public Button shopActionBtn;

    private int _coin;
    public RTLTextMeshPro coinText;
    public GameObject buyBtn;
    public GameObject selectBtn;
    public GameObject selectedBtn;

    void Start()
    {
        _coin = PlayerPrefs.GetInt("coin");
        coinText.text = _coin.ToString();
        if (PlayerPrefs.GetInt("car0priced") == 0) PlayerPrefs.SetInt("car0priced", 1);
        playerCar = PlayerPrefs.GetInt("carnum");
        CarStatusUpdate();
        Time.timeScale = 1;
    }

    void Update()
    {
        for (int i = 1; i < _wh.Length; i++) _wh[i].Rotate(speed * 24 * Time.deltaTime, 0, 0);

        transform.Translate(speed * Time.deltaTime, 0, 0);
    }

    public void NextCar()
    {
        if (playerCar < skins.Length - 1)
        {
            playerCar++;
            CarStatusUpdate();
        }
    }

    public void PrevCar()
    {
        if (playerCar > 0)
        {
            playerCar--;
            CarStatusUpdate();
        }
    }

    public void CarStatusUpdate()
    {
        for (int i = 0; i < skins.Length; i++)
        {
            if (playerCar == i && !skins[i].activeSelf)
            {
                _wh = skins[i].GetComponentsInChildren<Transform>();
                skins[i].SetActive(true);
            }
            else if (playerCar != i && skins[i].activeSelf) skins[i].SetActive(false);
        }

        int priced = PlayerPrefs.GetInt("car" + playerCar + "priced");
        int selected = PlayerPrefs.GetInt("carnum");
        if (priced == 0)
        {
            int price = skins[playerCar].GetComponent<CarPrice>().price;
            buyBtn.SetActive(true);
            selectBtn.SetActive(false);
            selectedBtn.SetActive(false);
            buyBtn.transform.GetChild(0).GetComponent<RTLTextMeshPro>().text = price.ToString();
        }
        else
        {
            if (playerCar == selected)
            {
                buyBtn.SetActive(false);
                selectBtn.SetActive(false);
                selectedBtn.SetActive(true);
            }
            else
            {
                buyBtn.SetActive(false);
                selectBtn.SetActive(true);
                selectedBtn.SetActive(false);
            }
        }
    }

    public void SelectCar()
    {
        PlayerPrefs.SetInt("carnum", playerCar);
        CarStatusUpdate();
    }

    public void BuyCar()
    {
        int price = skins[playerCar].GetComponent<CarPrice>().price;
        if (_coin >= price)
        {
            _coin -= price;
            PlayerPrefs.SetInt("coin",_coin);
            PlayerPrefs.SetInt("car" + playerCar + "priced", 1);
            coinText.text = _coin.ToString();
            CarStatusUpdate();
        }
    }
}