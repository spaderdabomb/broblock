using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BootManager : MonoBehaviour
{
    public static BootManager Instance;

    [SerializeField] private GameObject dataManager;
    [SerializeField] private GameObject inputManager;
    [SerializeField] private GameObject debugManager;
    [SerializeField] private GameObject audioManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        InstantiateManagers();
    }

    private void InstantiateManagers()
    {
        GameObject spawnedDataManager = Instantiate(dataManager);
        GameObject spawnedInputManager = Instantiate(inputManager);
        GameObject spawnedDebugManager = Instantiate(debugManager);
        GameObject spawnedaudioManager = Instantiate(audioManager);
    }
}
