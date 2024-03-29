﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour {


    public static WeatherManager instance;

    enum Weather { Sunny, Rainy, Cloudy };

    Weather _weather;

    Weather weather
    {
        get
        {
            return _weather;
        }

        set
        {
            _weather = value;
            Debug.Log("new weather: " + weather.ToString());
         
            UpdateEnvironmentStats();
            Debug.Log("soil humidity: " + soilHumidity);

            StartCoroutine(WeatherTransition());
            
        }
    }

    [Header("Settings")]
    public float humidityStep = 0.1f;
    public float sunnyLuminosity = 1, rainyLuminosity = 0, cloudyLuminosity = 0;
    [Range(0, 1)]
    public float sunnyProbability;
    [Range(0, 1)]
    public float rainProbability;
    float weatherTransitionSpritDelay = 1;

    //environment stats
    [HideInInspector] public float soilHumidity = 0.6f;  //between 0 and 1
    [HideInInspector] public float luminosity;   //between 0 and 1


    [Header("References to game objects")]
    public SpriteRenderer sky;
    public SpriteRenderer ground;
    public GameObject rainParticleSystem;

    [Header("References to assets")]
    public Sprite rainySprite;
    public Sprite cloudySprite;
    public Sprite sunnySprite;
    public AudioClip rainyClip;
    public AudioClip cloudyClip;
    public AudioClip sunnyClip;
    public Sprite[] groundSprites;

    AudioSource myAudioSource;


    

    private void Awake()
    {
        instance = this;
        myAudioSource = GetComponent<AudioSource>();
        soilHumidity = 0.5f;
        rainParticleSystem.SetActive(false);
    }

    private void Update()
    {
       
    }

    IEnumerator WeatherTransition()
    {
        switch (weather)
        {
            case Weather.Sunny:
                myAudioSource.clip = sunnyClip;
                break;
            case Weather.Cloudy:
                myAudioSource.clip = cloudyClip;
                break;
            case Weather.Rainy:
                myAudioSource.clip = rainyClip;
                break;
        }

        myAudioSource.Play();

        yield return new WaitForSeconds(weatherTransitionSpritDelay);

        switch (weather)
        {
            case Weather.Sunny:
                sky.sprite = sunnySprite;
                rainParticleSystem.SetActive(false);
                break;
            case Weather.Cloudy:
                sky.sprite = cloudySprite;
                rainParticleSystem.SetActive(false);
                break;
            case Weather.Rainy:
                sky.sprite = rainySprite;
                rainParticleSystem.SetActive(true);
                break;
        }

        int humidityGraphicalStage = (int) Mathf.Floor(soilHumidity / (1f / groundSprites.Length));
       // Debug.Log("humidity graphical stage: " + humidityGraphicalStage);
        if (humidityGraphicalStage == groundSprites.Length)
            humidityGraphicalStage--;
        ground.sprite = groundSprites[humidityGraphicalStage]; 
    }


    public void SelectNewWeather()
    {
        float rand1 = Random.Range(0f, 1f);

        if (rand1 < 0.5)
            return;

            float rand2 = Random.Range(0f, 1f);
        if (rand2 < sunnyProbability)
            weather = Weather.Sunny;
        else if (rand2 < sunnyProbability + rainProbability)
            weather = Weather.Rainy;
        else
            weather = Weather.Cloudy;
    }




    public void UpdateEnvironmentStats()
    {
        switch (weather)
        {
            case Weather.Sunny:
                soilHumidity = Mathf.Clamp(soilHumidity - humidityStep, 0, 1);
                luminosity = sunnyLuminosity;
                break;
            case Weather.Cloudy:
                luminosity = cloudyLuminosity;
                break;
            case Weather.Rainy:
                soilHumidity = Mathf.Clamp(soilHumidity + humidityStep, 0, 1);
                luminosity = rainyLuminosity;
                break;
        }

    }

 

  

}
