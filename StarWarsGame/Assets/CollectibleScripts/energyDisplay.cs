using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class energyDisplay : MonoBehaviour
{
    public Slider energySlider;
    public Slider healthSlider;
    public Text scoreText;

    public static int gameScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        energySlider.value = (float)energy.playerEnergy;
        healthSlider.value = (float)energy.playerHealth;
        scoreText.text = "Score: " + energy.playerScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        energySlider.value = (float)energy.playerEnergy;
        healthSlider.value = (float)energy.playerHealth;
        scoreText.text = "Score: " + energy.playerScore.ToString();
    }
}
