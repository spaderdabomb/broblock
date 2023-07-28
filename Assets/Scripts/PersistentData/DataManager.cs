using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    // Persistent data keys
    public static string score = "score";
    public static string currentMaxLevel = "currentMaxLevel";
    public static string achievementsKey = "achievements";
    public static string masterVolume = "masterVolume";
    public static string musicVolume = "musicVolume";
    public static string sfxVolume = "sfxVolume";

    // Default values
    public static int currentMaxLevelDefault = 1;
    public static float masterVolumeDefault = 1f;
    public static float musicVolumeDefault = 1f;
    public static float sfxVolumeDefault = 1f;

    public static bool[] achievementsArray = new bool[GlobalData.numAchievements];

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetConfig();
            LoadData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private static void SetConfig()
    {
        // build your config
        var config = new FBPPConfig()
        {
            SaveFileName = "broblock-data.txt",
            AutoSaveData = false,
            ScrambleSaveData = false,
            // EncryptionSecret = "spadersecretcode",
            // SaveFilePath = "C:/Repositories/unity-2d-builtin-template"
        };
        // pass it to FBPP
        FBPP.Start(config);
    }

    private static void LoadData()
    {
        // print(GlobalData.numAchievements);
    }

    private static void LoadArray(string key, bool[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            FBPP.GetBool(key + i.ToString(), false);
        }
    }

    private static void SaveArray(string key, bool[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            FBPP.SetBool(key + i.ToString(), array[i]);
        }
    }
}
