using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;                      //make the cursor visible
        Cursor.lockState = CursorLockMode.None;     //lock the cursor to screen
        
    }

    // Update is called once per frame
 
    public void startbutton()
    {   
        SceneManager.LoadScene("Outside Level Scene 1");
    }

    //TODO- OPTIONS MENU
    public void optionsbutton()
    {

    }

    public void quitbutton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
