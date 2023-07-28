using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class ButtonLevel
{
    public int buttonNum;
    public ButtonLevel(VisualElement root)
    {
        AssignQueryResults(root);

        buttonLevel.clickable.clicked += OnClicked;
    }

    public void InitializeButton(int index)
    {
        buttonLevel.text = (index + 1).ToString();
        buttonNum = index + 1;
        if (FBPP.GetInt(DataManager.currentMaxLevel, DataManager.currentMaxLevelDefault) < buttonNum)
        {
            buttonLevel.SetEnabled(false);
        }
    }

    private void OnClicked()
    {
        MainMenuManager.Instance.levelData.currentLevel = buttonNum;
        SceneManager.LoadScene("GameScene");
    }
}
