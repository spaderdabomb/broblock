using UnityEngine.UIElements;
using UnityEngine;
using Unity.VisualScripting.FullSerializer;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using JSAM;

namespace GameUI
{
    public partial class UIGameSceneMain
    {
        private VisualElement root;
        public VisualElement[] keySlots;
        private Dictionary<KeyType, int> keyTypeToSlotNum = new();
        private Color emptyKeyColor;
        private float healthBarBaseWidth;
        private float throwPowerBarBaseWidth;

        private bool onGUIcalled = false;
        private List<Button> allButtons;

        public UIGameSceneMain(VisualElement newRoot)
        {
            root = newRoot;
            emptyKeyColor = new Color(1f, 1f, 1f, 0.2f);
            AssignQueryResults(root);

            keySlots = new VisualElement[5] {keySlot1, keySlot2, keySlot3, keySlot4, keySlot5};

            int i = 0;
            foreach (KeyType keyType in Enum.GetValues(typeof(KeyType)))
            {
                keyTypeToSlotNum.Add(keyType, i);
                i++;
            }

            InitCallbacks();
        }

        private void InitCallbacks()
        {
            allButtons = root.Query<Button>().ToList();

            buttonSettings.clickable.clicked += SettingsClicked;
            buttonResume.clickable.clicked += ResumeClicked;
            buttonRestart.clickable.clicked += RestartClicked;
            buttonMainMenu.clickable.clicked += MainMenuClicked;
            buttonQuit.clickable.clicked += QuitClicked;
            buttonBack.clickable.clicked += BackClicked;

            buttonWinMainMenu.clickable.clicked += MainMenuClicked;
            buttonWinReplay.clickable.clicked += RestartClicked;
            buttonWinNextLevel.clickable.clicked += NextLevelClicked;

            buttonLoseMainMenu.clickable.clicked += MainMenuClicked;
            buttonLoseReplay.clickable.clicked += RestartClicked;

            foreach (Button button in allButtons)
            {
                button.clickable.clicked += OnAnyButtonClicked;
                button.RegisterCallback<MouseEnterEvent>(OnAnyButtonHover);
            }
        }

        public void OnGeometryChangedFirstPass()
        {
            Debug.Log(throwPowerBar.resolvedStyle.width);
            healthBarBaseWidth = healthBar.resolvedStyle.width;
            throwPowerBarBaseWidth = throwPowerBar.resolvedStyle.width;
            UIManager.Instance.playerData.ThrowPowerCurrent = 0f;
        }

        public void AddKeyToUI(SpawnedKey spawnedKey)
        {
            int keySlotNum = keyTypeToSlotNum[spawnedKey.keySO.keyType];
            VisualElement keySlotVE = keySlots[keySlotNum];
            keySlotVE.style.unityBackgroundImageTintColor = spawnedKey.keySO.color;
        }

        public void SetKeyUIToEmpty(KeyType keyType)
        {
            int keySlotNum = keyTypeToSlotNum[keyType];
            VisualElement keySlotVE = keySlots[keySlotNum];
            keySlotVE.style.unityBackgroundImageTintColor = emptyKeyColor;
        }

        public void ShowWinGameUI()
        {
            menuContainer.style.display = DisplayStyle.Flex;
            menuWin.style.display= DisplayStyle.Flex;

            menuPause.style.display = DisplayStyle.None;
            menuSettings.style.display = DisplayStyle.None;
            topLeftContainer.style.display = DisplayStyle.None;
        }

        public void ShowLoseGameUI()
        {
            menuContainer.style.display = DisplayStyle.Flex;
            menuLose.style.display = DisplayStyle.Flex;

            menuPause.style.display = DisplayStyle.None;
            menuSettings.style.display = DisplayStyle.None;
            topLeftContainer.style.display = DisplayStyle.None;
        }

        public bool TogglePauseMenu()
        {
            bool wasPauseMenuShowing = (menuContainer.style.display == DisplayStyle.Flex) ? true : false;
            if (wasPauseMenuShowing)
            {
                menuContainer.style.display = DisplayStyle.None;
                menuPause.style.display = DisplayStyle.None;

                topLeftContainer.style.display = DisplayStyle.Flex;
            }
            else
            {
                menuContainer.style.display = DisplayStyle.Flex;
                menuPause.style.display = DisplayStyle.Flex;

                topLeftContainer.style.display = DisplayStyle.None;
            }

            return !wasPauseMenuShowing;
        }

        private void SettingsClicked()
        {
            menuPause.style.display = DisplayStyle.None;
            menuSettings.style.display = DisplayStyle.Flex;
        }

        private void RestartClicked()
        {
            FBPP.Save();
            SceneManager.LoadScene("GameScene");
        }

        private void MainMenuClicked()
        {
            FBPP.Save();
            SceneManager.LoadScene("MainMenu");
        }

        private void ResumeClicked()
        {
            GameManager.Instance.TogglePauseMenu();
        }

        private void QuitClicked()
        {
            FBPP.Save();

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }

        private void BackClicked()
        {
            menuPause.style.display = DisplayStyle.Flex;
            menuSettings.style.display = DisplayStyle.None;
        }

        private void NextLevelClicked()
        {
            GameManager.Instance.levelData.currentLevel += 1;
            SceneManager.LoadScene("GameScene");
        }

        private void OnAnyButtonHover(MouseEnterEvent evt)
        {
            AudioManager.PlaySound(AudioLibrarySounds.digi_plink);
        }

        private void OnAnyButtonClicked()
        {
            AudioManager.PlaySound(AudioLibrarySounds.click_suppressed);
        }

        public void UpdatePlayerHealthUI(float newPlayerHealth)
        {
            float newWidth = healthBarBaseWidth * newPlayerHealth / GameManager.Instance.playerData.healthBase;
            healthBar.style.width = newWidth;
        }

        public void UpdatePlayerPowerUI(float newPlayerPower)
        {
            float newWidth = throwPowerBarBaseWidth * newPlayerPower / GameManager.Instance.playerData.throwPowerBase;
            throwPowerBar.style.width = newWidth;
        }
    }
}
