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
            // change game graphics accordingly
        }
    }

    //settings
    float humidityStep = 0.1f;
    float sunnyLuminosity = 1, rainyLuminosity = 0, cloudyLuminosity = 0;
    [Range(0, 1)]
    float sunnyProbability;
    [Range(0, 1)]
    float rainProbability;

    //environment stats
    [HideInInspector] public float soilHumidity;  //between 0 and 1
    [HideInInspector] public float luminosity;   //between 0 and 1

    public void SelectRandomWeather()
    {
        float rand = Random.Range(0f, 1f);
        if (rand < sunnyProbability)
            weather = Weather.Sunny;
        else if (rand < sunnyProbability + rainProbability)
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

 

    private void Awake()
    {
        instance = this;
    }

}