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
        SceneManager.LoadScene("SampleScene"); 
    }

    void ShowRules()
    {
        SceneManager.LoadScene("RulesScene"); 
    }

    void ShowStory()
    {
        SceneManager.LoadScene("StoryScene"); 
    }
}
