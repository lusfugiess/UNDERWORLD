using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public string gameSceneName = "GameplayScene";

    public void PlayGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void Start()
    {
        if (!Cursor.visible)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        
        
    }
}
