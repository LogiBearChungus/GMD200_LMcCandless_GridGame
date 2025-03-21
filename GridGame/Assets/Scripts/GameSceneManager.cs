using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadLevelSelect()
    {
        SceneManager.LoadScene("LevelSelect"); // Ensure the scene name matches in Unity
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu"); // Ensure the scene name matches in Unity
    }

    public void LoadRulesScene()
    {
        SceneManager.LoadScene("Rules"); // Ensure the scene name matches in Unity
    }

    public void LoadLevel1()
    {
        SceneManager.LoadScene("Level1"); // Ensure the scene name matches in Unity
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2"); // Ensure the scene name matches in Unity
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("Level3"); // Ensure the scene name matches in Unity
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is closing"); // This will show in the console but not in a built game
    }
}
