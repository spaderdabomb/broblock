using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using GameUI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public UIDocument mainUIDocument;

    public float healthBarBaseWidth;
    public float throwPowerBarBaseWidth;

    private VisualElement root;

    [HideInInspector] public PlayerData playerData;
    [HideInInspector] public UIGameSceneMain uiGameSceneMain;

    private void Awake()
    {
        Instance = this;

        playerData = GameManager.Instance.playerData;
        root = mainUIDocument.rootVisualElement;
        uiGameSceneMain = new UIGameSceneMain(root);
    }

    private void OnEnable()
    {
        playerData.HealthChanged += uiGameSceneMain.UpdatePlayerHealthUI;
        playerData.ThrowPowerChanged += uiGameSceneMain.UpdatePlayerPowerUI;

        root.RegisterCallback<GeometryChangedEvent>(GeometryChangedCallback);
    }

    private void OnDisable()
    {
        playerData.HealthChanged -= uiGameSceneMain.UpdatePlayerHealthUI;
        playerData.ThrowPowerChanged -= uiGameSceneMain.UpdatePlayerPowerUI;
    }

    private void GeometryChangedCallback(GeometryChangedEvent evt)
    {
        root.UnregisterCallback<GeometryChangedEvent>(GeometryChangedCallback);

        uiGameSceneMain.OnGeometryChangedFirstPass();
    }

    private void Update()
    {
        
    }
}
