using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button playButton;
    public Button rulesButton;
    public Button storyButton;

    private void Start()
    {
        playButton.onClick.AddListener(PlayGame);
        rulesButton.onClick.AddListener(ShowRules);
        storyButton.onClick.AddListener(ShowStory);
    }

    void PlayGame()
    {
        SceneManager.LoadScene("SampleScene"); // Înlocuiește "GameScene" cu numele scenei tale de joc
    }

    void ShowRules()
    {
        SceneManager.LoadScene("RulesScene"); // Înlocuiește "RulesScene" cu numele scenei tale pentru reguli
    }

    void ShowStory()
    {
        SceneManager.LoadScene("StoryScene"); // Înlocuiește "StoryScene" cu numele scenei tale pentru poveste
    }
}
