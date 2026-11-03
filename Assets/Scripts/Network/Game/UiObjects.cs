using System;
using System.Collections;
using System.Collections.Generic;
using RTLTMPro;
using UnityEngine;

public class UiObjects : MonoBehaviour
{
    public RTLTextMeshPro scoreText, speedText, timeText;
    public Transform gameplay, gameover, combo, maxSpeed;
    public GaasNetwork gas;
    public TurnnNetwork turn;
    public BreakkNetwork breik;
    public AudioSource gasSound;
    public AudioSource breakSound;
    public AudioSource accidentSound;
}