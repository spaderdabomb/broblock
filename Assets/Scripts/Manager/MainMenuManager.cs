using GameUI;
using JSAM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    public UIDocument mainMenuUIDocument;
    public VisualTreeAsset buttonLevelAsset;
    public LevelData levelData;

    [HideInInspector] UIMainMenu uiMainMenu;
    private VisualElement root;

    bool musicStart = false;

    private void Awake()
    {
        Instance = this;
        Time.timeScale = 1.0f;
        root = mainMenuUIDocument.rootVisualElement;
    }

    private void OnEnable()
    {
        root.RegisterCallback<GeometryChangedEvent>(GeometryChangedCallback);
    }

    private void OnDisable()
    {
        uiMainMenu.RemoveCallbacks();
    }

    void Start()
    {
        VisualElement root = mainMenuUIDocument.rootVisualElement;
        uiMainMenu = new UIMainMenu(root);
    }

    // Update is called once per frame
    void Update()
    {
        if (!musicStart)
        {
            AudioManager.StopMusic(AudioLibraryMusic.FullSpeed);
            AudioManager.PlayMusic(AudioLibraryMusic.Upbeat_Intense);
            musicStart = true;
        }
    }
    private void GeometryChangedCallback(GeometryChangedEvent evt)
    {
        root.UnregisterCallback<GeometryChangedEvent>(GeometryChangedCallback);

        uiMainMenu.OnGeometryChangedFirstPass();
    }
}
