using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    public static bool GameIsPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void LoadScene(string sceneName)
    {
        if (GameIsPaused)
        {
            SceneManager.UnloadSceneAsync("Lobby");
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
        Resume();
    }

    public void LoadSceneAdditive(string sceneName)
    {
        PauseGame();
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ShowPanel(GameObject panel)
    {
        if (panel != null) panel.SetActive(true);
    }

    public void HidePanel(GameObject panel)
    {
        if (panel != null) panel.SetActive(false);
    }


    void OnGUI()
    {
        if (GameIsPaused)
        {
            Text text = GameObject.Find("StartPauseButtonText").GetComponent<Text>();
            if(text.text == "START")
            {
                GameObject.Find("StartPauseButtonText").GetComponent<Text>().text = "Resume";
            }
        }
        
    }

    void Resume()
    {
        Time.timeScale = 1;
        GameIsPaused = false;
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        GameIsPaused = true;
    }

    bool IsPaused()
    {
        return Time.timeScale == 0;
    }
}
