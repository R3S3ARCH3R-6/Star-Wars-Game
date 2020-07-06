using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// ...
/// </summary>
public class SceneChanger : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        //The following code is used to allow the mouse to appear only on certain screens
        Cursor.visible = true;
            //the statement above allows the mouse to be seen
        Cursor.lockState = CursorLockMode.None;
            //the above statment stops the mouse from being locked into place or disabled
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Quits the game entirely. This script stops the program/game.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// This function brings the user to the scene "Dead Scene" when they die in the game.
    /// </summary>
    public void GameOver()
    {
        SceneManager.LoadScene("Dead Scene Final2");
    }

    /// <summary>
    /// This function brings the user to the scene "Win Scene" when they beat the game.
    /// </summary>
    public void Victory()
    {
        SceneManager.LoadScene("Win Scene Final2");
    }

    /// <summary>
    /// 
    /// </summary>
    public void StartScreen()
    {
        energy.playerScore = 0;
        SceneManager.LoadScene("Outside Level Scene 1");
    }

    public void MainMenu()
    {
        energy.playerScore = 0;
        SceneManager.LoadScene("MainMenu");
    }

    
}
