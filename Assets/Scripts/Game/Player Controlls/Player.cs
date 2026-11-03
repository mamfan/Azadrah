using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using RTLTMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private bool _gameOver;

    public AudioSource gasSound;
    public AudioSource breakSound;
    public AudioSource accidentSound;
    public Transform gameplayUI;
    public RTLTextMeshPro scoreText;
    public RTLTextMeshPro speedText;

    public Transform gameoverUI;
    public Transform pauseUI;
    public Transform unPauseUI;

    // public RTLTextMeshPro gameoverScoreText;
    // public RTLTextMeshPro gameoverHighScoreText;
    public float speed = 1;
    public float maxSpeed = 1;
    public float minSpeed = 1;
    public float turnSpeed = 1;
    public float gasPower = 0.02f;
    public float breakPower = 0.03f;
    public float nonePower = 0.01f;
    public float minSpeedToSpawnCar = 30;

    public float distance;

    public Transform comboUi;
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
    public Transform maxSpeedCounter;
    
    void Start()
    {
        // LODGroup.ForceLOD(-1);
        _lastCombo = StartCoroutine(ComboStrike());
        if (!gasSound.isPlaying) gasSound.Play();
        playerCar = PlayerPrefs.GetInt("carnum");
        gameplayUI.gameObject.SetActive(true);
        gameoverUI.gameObject.SetActive(false);
        pauseUI.gameObject.SetActive(false);
        unPauseUI.gameObject.SetActive(false);
        _theCar = Instantiate(skins[playerCar], transform);
        _wh = _theCar.GetComponentsInChildren<Transform>();
        _theCar.GetComponent<TrafficRunner>().enabled = false;
        _theCar.transform.DOLocalMove(new Vector3(0, 0, 0), Time.deltaTime);
        _theCar.tag = "Player";
        transform.GetChild(0).GetComponent<CarDestroier>().ParentChange(_theCar.transform);
        _comboSlider = comboUi.GetComponentInChildren<Slider>();
        _comboSlider.maxValue = comboTime;
        Time.timeScale = 1;
    }

    void Update()
    {
        if (!_gameOver)
        {
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

            distance = transform.position.x / 5000;

            scoreText.text = distance.ToString("F");
            speedText.text = ((int)speed * 2).ToString();

            if (gasSound.isPlaying) gasSound.pitch = ((speed / maxSpeed) * 2) + 1;

            for (int i = 1; i < _wh.Length; i++) _wh[i].Rotate(speed * 24 * Time.deltaTime, 0, 0);

            // if (Input.GetKeyDown(KeyCode.DownArrow)) breakSound.Play();
            //
            // if (Input.GetKey(KeyCode.RightArrow)) Turn(1);
            // else if (Input.GetKey(KeyCode.LeftArrow)) Turn(-1);
            // else Turn(0);
            //
            // if (Input.GetKey(KeyCode.UpArrow)) GetGas();
            // else if (Input.GetKey(KeyCode.DownArrow)) GetBreak();

            if (speed < minSpeed) GetGas();

            if (speed > maxSpeed)
            {
                if (!_maxSpeed)
                {
                    _maxSpeed = true;
                    float moveVal = Screen.width * .18f;
                    maxSpeedCounter.DOMoveX(moveVal, .5f);
                }

                _maxSpeedCounter += Time.deltaTime;
                _maxSpeedLocalCounter += Time.deltaTime;
                maxSpeedCounter.GetComponentInChildren<RTLTextMeshPro>().text = _maxSpeedLocalCounter.ToString("F0");
            }
            else if (_maxSpeed)
            {
                maxSpeedCounter.DOMoveX(0, .5f);
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
            float carSpeed = other.GetComponent<TrafficRunner>().speed;
            if (carSpeed < speed)
            {
                StopCoroutine(_lastCombo);
                _totalCombos++;
                _comboAdd++;
                comboUi.GetComponentInChildren<RTLTextMeshPro>().text = (_comboAdd - 1).ToString();
                _comboSlider.DOValue(comboTime, .5f);
                _lastCombo = StartCoroutine(ComboStrike());
            }
        }
    }

    IEnumerator ComboStrike()
    {
        float moveVal = Screen.width * .18f;
        if (_comboAdd > 1) comboUi.DOMoveX(moveVal, .5f);
        yield return new WaitForSeconds(comboTime);
        comboUi.DOMoveX(0, .5f);
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

    public void SetPause()
    {
        gasSound.Pause();
        Time.timeScale = 0;
        pauseUI.gameObject.SetActive(true);
        gameplayUI.gameObject.SetActive(false);
        gameoverUI.gameObject.SetActive(false);
        unPauseUI.gameObject.SetActive(false);
    }

    public void SetUnPause()
    {
        gasSound.UnPause();
        pauseUI.gameObject.SetActive(false);
        gameplayUI.gameObject.SetActive(true);
        gameoverUI.gameObject.SetActive(false);
        unPauseUI.gameObject.SetActive(false);
        Time.timeScale = 1;
        // StartCoroutine(UnPause());
    }

    // IEnumerator UnPause()
    // {
    //     pauseUI.gameObject.SetActive(false);
    //     gameplayUI.gameObject.SetActive(false);
    //     gameoverUI.gameObject.SetActive(false);
    //     unPauseUI.gameObject.SetActive(true);
    //     unPauseUI.GetComponentInChildren<RTLTextMeshPro>().text = "3";
    //     yield return new WaitForSeconds(1);
    //     unPauseUI.GetComponentInChildren<RTLTextMeshPro>().text = "2";
    //     yield return new WaitForSeconds(1);
    //     unPauseUI.GetComponentInChildren<RTLTextMeshPro>().text = "1";
    //     yield return new WaitForSeconds(1);
    //     unPauseUI.GetComponentInChildren<RTLTextMeshPro>().text = "برو!";
    //     yield return new WaitForSeconds(.5f);
    //     Time.timeScale = 1;
    //     pauseUI.gameObject.SetActive(false);
    //     gameplayUI.gameObject.SetActive(true);
    //     gameoverUI.gameObject.SetActive(false);
    //     unPauseUI.gameObject.SetActive(false);
    // }

    public IEnumerator gameOver()
    {
        if (!_gameOver)
        {
            _gameOver = true;
            if (gasSound.isPlaying) gasSound.Stop();
            accidentSound.Play();
            int coin = PlayerPrefs.GetInt("coin");
            int income = (int)((distance * 100) + (_maxSpeedCounter * .3f) + (_totalCombos * 1.5f));
            gameoverUI.GetChild(1).GetChild(0).GetComponent<RTLTextMeshPro>().text = distance.ToString("F");
            gameoverUI.GetChild(2).GetChild(0).GetComponent<RTLTextMeshPro>().text = _maxSpeedCounter.ToString("F0");
            gameoverUI.GetChild(3).GetChild(0).GetComponent<RTLTextMeshPro>().text = _totalCombos.ToString();
            gameoverUI.GetChild(4).GetChild(0).GetComponent<RTLTextMeshPro>().text = income.ToString();
            PlayerPrefs.SetInt("coin", coin + income);
            gameoverUI.GetChild(1).DOScale(Vector3.zero, Time.deltaTime);
            gameoverUI.GetChild(2).DOScale(Vector3.zero, Time.deltaTime);
            gameoverUI.GetChild(3).DOScale(Vector3.zero, Time.deltaTime);
            gameoverUI.GetChild(5).DOScale(Vector3.zero, Time.deltaTime);
            gameoverUI.GetChild(4).DOScale(Vector3.zero, Time.deltaTime);
            gameoverUI.GetChild(6).DOScale(Vector3.zero, Time.deltaTime);
            gameoverUI.GetChild(7).DOScale(Vector3.zero, Time.deltaTime);
            yield return new WaitForSeconds(.2f);
            gameplayUI.gameObject.SetActive(false);
            pauseUI.gameObject.SetActive(false);
            unPauseUI.gameObject.SetActive(false);
            Time.timeScale = 1;
            gameoverUI.gameObject.SetActive(true);
            gameoverUI.GetChild(1).DOScale(new Vector3(-1, 1, 1), .5f);
            yield return new WaitForSeconds(.3f);
            gameoverUI.GetChild(2).DOScale(new Vector3(-1, 1, 1), .5f);
            yield return new WaitForSeconds(.3f);
            gameoverUI.GetChild(3).DOScale(new Vector3(-1, 1, 1), .5f);
            yield return new WaitForSeconds(.3f);
            gameoverUI.GetChild(5).DOScale(Vector3.one, .5f);
            yield return new WaitForSeconds(.3f);
            gameoverUI.GetChild(4).DOScale(Vector3.one, .5f);
            yield return new WaitForSeconds(.3f);
            gameoverUI.GetChild(6).DOScale(Vector3.one, .5f);
            yield return new WaitForSeconds(.3f);
            gameoverUI.GetChild(7).DOScale(Vector3.one, .5f);
            // gameoverScoreText.text = distance.ToString("F");
            // if (distance > PlayerPrefs.GetInt("highscore")) PlayerPrefs.SetFloat("highscoref", distance);
            // gameoverHighScoreText.text = PlayerPrefs.GetFloat("highscoref").ToString("F");
        }
    }
}