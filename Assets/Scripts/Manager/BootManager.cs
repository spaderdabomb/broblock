using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class BootManager : MonoBehaviour
{
    public static BootManager Instance;

    [SerializeField] private GameObject dataManager;
    [SerializeField] private GameObject inputManager;
    [SerializeField] private GameObject debugManager;
    [SerializeField] private GameObject audioManager;

    public GameObject spawnedDataManager;
    public GameObject spawnedInputManager;
    public GameObject spawnedDebugManager;
    public GameObject spawnedaudioManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InstantiateManagers();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InstantiateManagers()
    {
        spawnedDataManager = Instantiate(dataManager);
        spawnedInputManager = Instantiate(inputManager);
        spawnedDebugManager = Instantiate(debugManager);
        spawnedaudioManager = Instantiate(audioManager);
    }
}
