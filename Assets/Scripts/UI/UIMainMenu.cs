using UnityEngine.UIElements;
using JSAM;
using System.Collections.Generic;

public partial class UIMainMenu
{
    private List<Button> allUIButtons;
    private VisualElement root;
    public UIMainMenu(VisualElement newRoot)
    {
        root = newRoot;
        AssignQueryResults(root);

        SetCallbacks();
        SetupLevelSelect();
    }

    private void SetCallbacks()
    {
        allUIButtons = root.Query<Button>().ToList();

        // Main menu buttons
        buttonSettings.clickable.clicked += SettingsClicked;
        buttonAchievements.clicked += AchievementsClicked;
        buttonHighscores.clicked += HighscoresClicked;
        buttonPlay.clicked += PlayClicked;

        // Back buttons
        buttonSettingsBack.clickable.clicked += BackToMainMenuClicked;
        buttonAchievementsBack.clickable.clicked += BackToMainMenuClicked;
        buttonHighscoresBack.clickable.clicked += BackToMainMenuClicked;
        buttonLevelSelectBack.clickable.clicked += BackToMainMenuClicked;

        // Sliders
        sliderMasterVolume.RegisterValueChangedCallback(OnMasterVolumeChange);
        sliderMusicVolume.RegisterValueChangedCallback(OnMusicVolumeChange);
        sliderSFXVolume.RegisterValueChangedCallback(OnSFXVolumeChange);

        foreach (Button button in allUIButtons)
        {
            button.RegisterCallback<MouseEnterEvent>(OnAnyButtonHover);
            button.clickable.clicked += OnAnyButtonClicked;
        }
    }

    public void RemoveCallbacks()
    {
        buttonSettings.clickable.clicked -= SettingsClicked;
        buttonAchievements.clicked -= AchievementsClicked;
        buttonHighscores.clicked -= HighscoresClicked;
        buttonPlay.clicked -= PlayClicked;

        buttonSettingsBack.clickable.clicked -= BackToMainMenuClicked;
        buttonAchievementsBack.clickable.clicked -= BackToMainMenuClicked;
        buttonHighscoresBack.clickable.clicked -= BackToMainMenuClicked;
        buttonLevelSelectBack.clickable.clicked -= BackToMainMenuClicked;

        foreach (Button button in allUIButtons)
        {
            button?.UnregisterCallback<MouseEnterEvent>(OnAnyButtonHover);
            button.clickable.clicked -= OnAnyButtonClicked;
        }
    }

    public void OnGeometryChangedFirstPass()
    {
        sliderMasterVolume.value = FBPP.GetFloat(DataManager.masterVolume, DataManager.masterVolumeDefault);
        sliderMusicVolume.value = FBPP.GetFloat(DataManager.musicVolume, DataManager.musicVolumeDefault);
        sliderSFXVolume.value = FBPP.GetFloat(DataManager.sfxVolume, DataManager.sfxVolumeDefault);
    }


    private void SetupLevelSelect()
    {
        for (int i = 0; i < MainMenuManager.Instance.levelData.numLevels; i++)
        {
            VisualElement buttonLevelCloned = MainMenuManager.Instance.buttonLevelAsset.CloneTree();
            ButtonLevel buttonLevel = new ButtonLevel(buttonLevelCloned);
            buttonLevel.InitializeButton(i);
            scrollviewLevelSelect.Add(buttonLevelCloned);
        }
    }

    private void AchievementsClicked()
    {
        menuAchievements.style.display = DisplayStyle.Flex;
        menuHighscores.style.display = DisplayStyle.None;
        menuSettings.style.display = DisplayStyle.None;
        menuLevelSelect.style.display = DisplayStyle.None;
        menuMain.style.display = DisplayStyle.None;
    }

    private void SettingsClicked()
    {
        menuAchievements.style.display = DisplayStyle.None;
        menuHighscores.style.display = DisplayStyle.None;
        menuSettings.style.display = DisplayStyle.Flex;
        menuLevelSelect.style.display = DisplayStyle.None;
        menuMain.style.display = DisplayStyle.None;
    }

    private void HighscoresClicked()
    {
        menuAchievements.style.display = DisplayStyle.None;
        menuHighscores.style.display = DisplayStyle.Flex;
        menuSettings.style.display = DisplayStyle.None;
        menuLevelSelect.style.display = DisplayStyle.None;
        menuMain.style.display = DisplayStyle.None;
    }

    private void PlayClicked()
    {
        menuAchievements.style.display = DisplayStyle.None;
        menuHighscores.style.display = DisplayStyle.None;
        menuSettings.style.display = DisplayStyle.None;
        menuLevelSelect.style.display = DisplayStyle.Flex;
        menuMain.style.display = DisplayStyle.None;
    }

    private void BackToMainMenuClicked()
    {
        menuAchievements.style.display = DisplayStyle.None;
        menuHighscores.style.display = DisplayStyle.None;
        menuSettings.style.display = DisplayStyle.None;
        menuLevelSelect.style.display = DisplayStyle.None;
        menuMain.style.display = DisplayStyle.Flex;
    }

    private void OnMasterVolumeChange(ChangeEvent<float> evt)
    {
        AudioManager.MasterVolume = evt.newValue;
        FBPP.SetFloat(DataManager.masterVolume, evt.newValue);
        FBPP.Save();
    }

    private void OnMusicVolumeChange(ChangeEvent<float> evt)
    {
        AudioManager.MusicVolume = evt.newValue;
        FBPP.SetFloat(DataManager.musicVolume, evt.newValue);
        FBPP.Save();
    }

    private void OnSFXVolumeChange(ChangeEvent<float> evt)
    {
        AudioManager.SoundVolume = evt.newValue;
        FBPP.SetFloat(DataManager.sfxVolume, evt.newValue);
        FBPP.Save();
    }

    private void OnAnyButtonHover(MouseEnterEvent evt)
    {
        AudioManager.PlaySound(AudioLibrarySounds.digi_plink);
    }

    private void OnAnyButtonClicked()
    {
        AudioManager.PlaySound(AudioLibrarySounds.click_suppressed);
    }
}

