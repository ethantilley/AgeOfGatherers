﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;
public class GameManager : MonoBehaviour {
    #region In Game Time
    [Header("In Game Time")]
    [Range(0.1f, 5f)]
    public float timeScale = 1;
    public Stopwatch worldAge;
    public int daysPast = 0;
    public Light gameSun;
    public float dayLength = 300;

    [Range(0, 1)]
    public float currentTimeOfDay = 0, morning = 0.23f, night = 0.75f;

    public float timeRate = 1;

    float sunInitIntensity;

    public static GameManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(this);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        worldAge = new Stopwatch();
        worldAge.Start();
        sunInitIntensity = gameSun.intensity;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(0);
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        Time.timeScale = timeScale;

        UpdateSun();

        currentTimeOfDay += (Time.deltaTime / dayLength) * (timeRate);

        if (currentTimeOfDay >= 1)
        {
            currentTimeOfDay = 0;
            ++daysPast;
        }
    }

    void UpdateSun()
    {
        gameSun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);
        float intensityMultiplier = 1;

        if (currentTimeOfDay <= morning || currentTimeOfDay >= night)        
            intensityMultiplier = Mathf.Lerp(intensityMultiplier, 0, 1 * Time.deltaTime);
        else if (currentTimeOfDay < morning)
            intensityMultiplier = Mathf.Clamp01(Mathf.Lerp(intensityMultiplier, (currentTimeOfDay - morning) * (1 / 0.02f), 1 * Time.deltaTime));
        else if (currentTimeOfDay > night)
            intensityMultiplier = Mathf.Clamp01(Mathf.Lerp(intensityMultiplier, (1 - ((currentTimeOfDay - night) * (1 / 0.02f))), 1 * Time.deltaTime)); 
        
        gameSun.intensity = sunInitIntensity * intensityMultiplier;
    }

#endregion
}
