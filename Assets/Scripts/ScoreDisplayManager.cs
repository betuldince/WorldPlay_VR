using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayManager : MonoBehaviour {

    public Text Score, TimeTaken, ObjectsHit;
    public GameObject scoreDisplay;

	// Use this for initialization
	void Start () {
        scoreDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void displayScore(int roundCompleted)
    {
        scoreDisplay.SetActive(true);
        Score.text = gameStateContainer.score[roundCompleted].ToString("F1");
        TimeTaken.text = gameStateContainer.time[roundCompleted].ToString("F0");
        ObjectsHit.text = gameStateContainer.collisions[roundCompleted].ToString("F0");
        
    }

    public void hideScore()
    {
        scoreDisplay.SetActive(false);
    }
}
