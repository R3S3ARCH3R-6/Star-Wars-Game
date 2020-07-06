using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScore : MonoBehaviour
{
    public Text finalscore;
    public static int gameScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        //energy.playerScore.ToString;
        finalscore.text = "Final Score: " + energy.playerScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
