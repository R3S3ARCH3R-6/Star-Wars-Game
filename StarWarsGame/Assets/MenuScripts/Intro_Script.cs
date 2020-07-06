using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intro_Script : MonoBehaviour
{
    public RawImage logo;
    float fadeTime = 8f;
    float colorChanger = 0f;
    float sceneChangeTime = 7f;
    // Start is called before the first frame update
    void Start()
    {
        logo.color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        while(fadeTime >= 0)
        {
            colorChanger += 0.01f;
            logo.color = new Color(1f, 1f, 1f, colorChanger);
            fadeTime -= Time.deltaTime;
        }

        StartCoroutine(MainMenuChanger(sceneChangeTime));
    }

    private IEnumerator MainMenuChanger(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        MainMenuSwitch();
    }

    public void MainMenuSwitch()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
