using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerNetwork : MonoBehaviour
{
    private bool _gameOver;

    public Photon.Realtime.Player harif;

    private UiObjects _uiObjects;

    private bool _starting;
    public float _endt;
    private float _t;

    public PhotonView PV;

    public float speed = 1;
    public float maxSpeed = 1;
    public float minSpeed = 1;
    public float turnSpeed = 1;
    public float gasPower = 0.02f;
    public float breakPower = 0.03f;
    public float nonePower = 0.01f;
    public float minSpeedToSpawnCar = 30;

    public float distance;

    // public Transform comboUi;
    public float comboTime = 5;

    private Slider _comboSlider;
    private int _totalCombos;
    private int _comboAdd = 1;
    private Coroutine _lastCombo;

    private Transform[] _wh;
    public GameObject[] skins;
    public int playerCar;

    public GameObject _theCar;

    private bool _maxSpeed;
    private float _maxSpeedCounter;
    private float _maxSpeedLocalCounter;
    // public Transform maxSpeedCounter;

    void Awake()
    {
        PV = GetComponent<PhotonView>();
        
        if (PV.IsMine)
        {
            FindObjectOfType<CameraNetwork>()._player = gameObject;
            _uiObjects = FindObjectOfType<UiObjects>();
            _lastCombo = StartCoroutine(ComboStrike());
            playerCar = PlayerPrefs.GetInt("carnum");
            if (!_uiObjects.gasSound.isPlaying) _uiObjects.gasSound.Play();
            _uiObjects.gameplay.gameObject.SetActive(true);
            _uiObjects.gameover.gameObject.SetActive(false);
            _comboSlider = _uiObjects.combo.GetComponentInChildren<Slider>();
            _comboSlider.maxValue = comboTime;
            _uiObjects.gas._player = this;
            _uiObjects.breik._player = this;
            _uiObjects.turn._player = this;
            foreach (var s in FindObjectsOfType<SpawnCarNetwork>()) s._player = this;
            FindObjectOfType<SpawnCarStaticsNetwork>().players[0] = transform;
        }
        else
        {
            foreach (var player in PhotonNetwork.PlayerList)
            {
                if (!player.Equals(PhotonNetwork.LocalPlayer)) harif = player;
            }
            FindObjectOfType<SpawnCarStaticsNetwork>().players[1] = transform;
            playerCar = (int)harif.CustomProperties["skin"];
        }

        _theCar = Instantiate(skins[playerCar], transform);
        _wh = _theCar.GetComponentsInChildren<Transform>();
        _theCar.GetComponent<TrafficRunner>().enabled = false;
        _theCar.transform.parent = transform;
        _theCar.transform.DOLocalMove(new Vector3(0, 0, 0), Time.deltaTime);
        _theCar.tag = "Player";
        _theCar.layer = 3;
        for (int i = 1; i < _wh.Length; i++)
        {
            _wh[i].tag = "Player";
            _wh[i].gameObject.layer = 3;
        }

        if (!PV.IsMine) Destroy(_theCar.GetComponent<Rigidbody>());

        // transform.GetChild(0).GetComponent<CarDestroierNetwork>().ParentChange(_theCar.transform);
    }

    // private void Start()
    // {
    //     if (PV.IsMine)
    //     {
    //         // _endt = (float)harif.CustomProperties["endtime"];
    //         // float pt = (float)PhotonNetwork.LocalPlayer.CustomProperties["endtime"];
    //         // if (_endt < pt)
    //         // {
    //         //     harif.CustomProperties["endtime"] = pt;
    //         //     _endt = (float)harif.CustomProperties["endtime"];
    //         // }
    //         _endt = PlayerPrefs.GetFloat("endtime");
    //
    //         Debug.Log("endt: " + _endt);
    //         StartCoroutine(GameTimer());
    //     }
    // }
    
    public IEnumerator GameTimer()
    {
        _t = (float)PhotonNetwork.Time;
        Debug.Log("endt: " + _endt);
        Debug.Log("t: " + _t);
        while (_t < _endt)
        {
            _t = (float)PhotonNetwork.Time;
            _uiObjects.timeText.text = (_endt - _t).ToString("F0");
            yield return new WaitForSeconds(1);
        }
    
        if (_t > _endt)
        {
            _t = _endt;
            _uiObjects.timeText.text = "0";
        }
    
        _gameOver = true;
    }

    private void FixedUpdate()
    {
        if (!PV.IsMine) transform.GetChild(0).DOMoveY(0.65f, Time.deltaTime);
        else
        {
            foreach (var road in FindObjectsOfType<InfinitRoadNetwork>())
            {
                if (road.player == null) road.player = transform.GetChild(0);
            }

            // int harifStarted = (int)harif.CustomProperties["started"];
            // if (!Time.timeScale.Equals(harifStarted)) Time.timeScale = harifStarted;
            //
            // if (harifStarted == 1 && !_starting)
            // {
            // StartCoroutine(GameTimer());
            //     _starting = true;
            // }
        }
    }

    void Update()
    {
        for (int i = 1; i < _wh.Length; i++) _wh[i].Rotate(speed * 24 * Time.deltaTime, 0, 0);

        if (transform.position.z < -9)
        {
            transform.DOMoveZ(-9, Time.deltaTime);
            _theCar.transform.DOLocalMoveZ(0, Time.deltaTime);
        }

        if (transform.position.z > 9)
        {
            transform.DOMoveZ(9, Time.deltaTime);
            _theCar.transform.DOLocalMoveZ(0, Time.deltaTime);
        }

        if (!_gameOver && PV.IsMine)
        {
            distance = transform.position.x / 5000;

            _uiObjects.scoreText.text = distance.ToString("F");
            _uiObjects.speedText.text = ((int)speed * 2).ToString();

            if (_uiObjects.gasSound.isPlaying) _uiObjects.gasSound.pitch = ((speed / maxSpeed) * 2) + 1;

            if (Input.GetKey(KeyCode.RightArrow)) Turn(1);
            else if (Input.GetKey(KeyCode.LeftArrow)) Turn(-1);

            if (Input.GetKey(KeyCode.UpArrow)) GetGas();
            else if (Input.GetKey(KeyCode.DownArrow)) GetBreak();

            if (speed < minSpeed) GetGas();

            if (speed > maxSpeed)
            {
                if (!_maxSpeed)
                {
                    _maxSpeed = true;
                    float moveVal = Screen.width * .18f;
                    _uiObjects.maxSpeed.DOMoveX(moveVal, .5f);
                }

                _maxSpeedCounter += Time.deltaTime;
                _maxSpeedLocalCounter += Time.deltaTime;
                _uiObjects.maxSpeed.GetComponentInChildren<RTLTextMeshPro>().text =
                    _maxSpeedLocalCounter.ToString("F0");
            }
            else if (_maxSpeed)
            {
                _uiObjects.maxSpeed.DOMoveX(0, .5f);
                _maxSpeedLocalCounter = 0;
                _maxSpeed = false;
            }

            if (_comboSlider.value > 0) _comboSlider.value -= Time.deltaTime;

            transform.Translate(speed * Time.deltaTime, 0, 0);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            float carSpeed = other.GetComponent<TrafficRunnerNetwork>().speed;
            if (carSpeed < speed)
            {
                StopCoroutine(_lastCombo);
                _totalCombos++;
                _comboAdd++;
                _uiObjects.combo.GetComponentInChildren<RTLTextMeshPro>().text = (_comboAdd - 1).ToString();
                _comboSlider.DOValue(comboTime, .5f);
                _lastCombo = StartCoroutine(ComboStrike());
            }
        }
    }

    IEnumerator ComboStrike()
    {
        float moveVal = Screen.width * .18f;
        if (_comboAdd > 1) _uiObjects.combo.DOMoveX(moveVal, .5f);
        yield return new WaitForSeconds(comboTime);
        _uiObjects.combo.DOMoveX(0, .5f);
        _comboAdd = 1;
    }

    public void GetNonePower()
    {
        if (speed > minSpeed + 1) speed -= nonePower;
    }

    public void GetBreak()
    {
        if (speed > minSpeed) speed -= breakPower;
    }

    public void GetGas()
    {
        if (speed < maxSpeed) speed += gasPower;
    }

    public void Turn(int t)
    {
        switch (t)
        {
            case 1:
                transform.Translate(0, 0, -turnSpeed * Time.deltaTime);
                break;
            case -1:
                transform.Translate(0, 0, turnSpeed * Time.deltaTime);
                break;
        }
    }

    public IEnumerator gameOver()
    {
        // if (!_gameOver)
        // {
        //     _gameOver = true;
        //     if (gasSound.isPlaying) gasSound.Stop();
        //     accidentSound.Play();
        //     int coin = PlayerPrefs.GetInt("coin");
        //     int income = (int)((distance * 100) + (_maxSpeedCounter * .3f) + (_totalCombos * 1.5f));
        //     gameoverUI.GetChild(1).GetChild(0).GetComponent<RTLTextMeshPro>().text = distance.ToString("F");
        //     gameoverUI.GetChild(2).GetChild(0).GetComponent<RTLTextMeshPro>().text = _maxSpeedCounter.ToString("F0");
        //     gameoverUI.GetChild(3).GetChild(0).GetComponent<RTLTextMeshPro>().text = _totalCombos.ToString();
        //     gameoverUI.GetChild(4).GetChild(0).GetComponent<RTLTextMeshPro>().text = income.ToString();
        //     PlayerPrefs.SetInt("coin", coin + income);
        yield return new WaitForSeconds(.2f);
        //     gameplayUI.gameObject.SetActive(false);
        //     gameoverUI.gameObject.SetActive(true);
        //     // gameoverScoreText.text = distance.ToString("F");
        //     // if (distance > PlayerPrefs.GetInt("highscore")) PlayerPrefs.SetFloat("highscoref", distance);
        //     // gameoverHighScoreText.text = PlayerPrefs.GetFloat("highscoref").ToString("F");
        // }
    }
}