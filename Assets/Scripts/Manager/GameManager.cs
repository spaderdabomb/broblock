using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Collections;
using GameUI;
using JSAM;

[DefaultExecutionOrder(-50)]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [field: SerializeField] public GameObject GroundContainer { get; private set; }
    [field: SerializeField] public GameObject KeyContainer { get; private set; }
    [field: SerializeField] public LevelData levelData { get; private set; }
    [field: SerializeField] public PlayerData playerData { get; private set; }

    public Dictionary<KeyType, int> keyDictionary = new();

    [HideInInspector] public GameObject[] enemyPrefabs;
    [HideInInspector] public GameObject[] gatePrefabs;
    [HideInInspector] public GameObject[] keyPrefabs;

    [HideInInspector] public float score;

    private bool wonGame = false;
    private bool isPaused = false;
    private float previousTimeScale;
    bool musicStart = false;

    private void Awake()
    {
        Instance = this;

        foreach (KeyType key in Enum.GetValues(typeof(KeyType)))
        {
            keyDictionary.Add(key, 0);
        }

        enemyPrefabs = Resources.LoadAll<GameObject>("Prefabs/Enemies");
        gatePrefabs = Resources.LoadAll<GameObject>("Prefabs/Gates");
        keyPrefabs = Resources.LoadAll<GameObject>("Prefabs/Keys");

        playerData.InitDefaults();
    }

    void Start()
    {
        Time.timeScale = 1f;
        previousTimeScale = Time.timeScale;
    }

    void Update()
    {
        FBPP.SetFloat(DataManager.score, score);
        if (!musicStart)
        {
            StartMusic();
        }
    }

    public bool AddKeyToDict(KeyType keyType)
    {
        bool keyAdded = false;
        int currentKeyCount = keyDictionary[keyType];
        if (currentKeyCount < GlobalData.maxKeyStack)
        {
            keyDictionary[keyType]++;
            keyAdded = true;
        }

        return keyAdded;
    }

    public void LoseGame()
    {
        FBPP.Save();
        UIManager.Instance.uiGameSceneMain.ShowLoseGameUI();
        PauseGame();
    }

    public void WinGame()
    {
        int maxLevel = Math.Max(FBPP.GetInt(DataManager.currentMaxLevel, DataManager.currentMaxLevelDefault), levelData.currentLevel + 1);
        FBPP.SetInt(DataManager.currentMaxLevel, maxLevel);
        FBPP.Save();
        UIManager.Instance.uiGameSceneMain.ShowWinGameUI();
        PauseGame();
    }

    public Vector3 RoundVectorToInt(Vector3 vector)
    {
        return new Vector3(
            Mathf.RoundToInt(vector.x),
            Mathf.RoundToInt(vector.y),
            Mathf.RoundToInt(vector.z)
        );
    }

    public void TogglePauseMenu()
    {
        bool pauseMenuShowing = UIManager.Instance.uiGameSceneMain.TogglePauseMenu();
        if (!isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        previousTimeScale = Time.timeScale;
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        if (wonGame) return;

        isPaused = false;
        Time.timeScale = previousTimeScale;
    }

    public Level GetCurrentLevelData()
    {
        Level currentLevel = levelData.levels[levelData.currentLevel - 1];
        return currentLevel;
    }

    public void StartMusic()
    {
        AudioManager.StopMusic(AudioLibraryMusic.Upbeat_Intense);
        AudioManager.PlayMusic(AudioLibraryMusic.FullSpeed);
        musicStart = true;
    }
}
