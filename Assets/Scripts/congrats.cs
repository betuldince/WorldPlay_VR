using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class congrats : MonoBehaviour
{

    GameObject winChar;
    GameObject ContainerBoxes;
    static GameObject confetti;
    public static AudioSource general;
    public static AudioClip congo;
    public static AudioClip gameOver;
    public static AudioClip round_complete;
    public static GameObject containerBox, hint;
    public static bool waitTimer = false;
    public static bool waitTimerRound2 = false;
    public static int currentRoundTimer = 0;
    public static int score = 0;
    public int roundCompleteWaitTime = 1;
    int countWaitTimer;
    public static GameObject pointRefrence;

    Countdown count;
    float time = 0;
    // Use this for initialization
    void Start()
    {
        ///Used to initialize the audio clips
        general = GetComponent<AudioSource>();
        congo=Resources.Load("Audio/success_sound") as AudioClip;
        gameOver = Resources.Load("Audio/Gameover2") as AudioClip;
        round_complete = Resources.Load("Audio/level_complete") as AudioClip;
        //Debug.Log("Congrats Script");
        confetti = GameObject.Find("Confetti");
        pointRefrence = GameObject.Find("Planks");
        countWaitTimer = roundCompleteWaitTime;
        containerBox = GameObject.Find("ContainerBox");
        hint = GameObject.Find("Hint");
    }

    // Update is called once per frame
    void Update()
    {
        if (waitTimer || waitTimerRound2)
        {
            time += Time.deltaTime;
            if(time >= 1)
            {
                time = 0;
                countWaitTimer--;
            }
            
        }
        if (countWaitTimer == 0)
        {
            waitTimer = false;
            waitTimerRound2 = false;
            countWaitTimer = roundCompleteWaitTime;
            enableNow();
        }
    }
    public static void enableNow()
    {
        ScoreDisplayManager sdm = GameObject.Find("Canvas").GetComponent<ScoreDisplayManager>();    
        //This was problem
      //  sdm.hideScore();
        // Also Destroy Score tags here

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("winChar");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }

        gameObjects = null;
        gameObjects = GameObject.FindGameObjectsWithTag("ScoreChar");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }

        gameObjects = null;
        gameObjects = GameObject.FindGameObjectsWithTag("ScoreDigit");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }


        containerBox.SetActive(true);
        hint.SetActive(true);
        Countdown.showTimer = true;

    }
    /// <summary>
    /// Deletes the score related game objects.
    /// </summary>
    public static void disableSceneObjects() {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("ScoreDigit");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
        gameObjects=null;

        gameObjects = GameObject.FindGameObjectsWithTag("ScoreChar");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }

        gameObjects = GameObject.FindGameObjectsWithTag("digit");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
        //GameObject containerBox = GameObject.Find("ContainerBox");
        containerBox.SetActive(false);

        //GameObject hint = GameObject.Find("Hint");
        hint.SetActive(false);
    }
    // This is to handled by the score script now
    public static void enabledisableSceneObjects()
    {
        //containerBox = GameObject.Find("ContainerBox");
        containerBox.SetActive(false);

        //hint = GameObject.Find("Hint");
        hint.SetActive(false);
    }
    /// <summary>
    /// The function to be called for displaying the score scene, called with the round number. 
    /// </summary>
    public static void roundclearSceneAndDisplay(int currRound)
    {
        //Working
        
        enabledisableSceneObjects();
        ///Get timer and disable it.
        Countdown countdown = Countdown.getInstance();
        Countdown.showTimer = false;
        countdown.sendTimerToScore(currRound);
        countdown.resetTimer(currRound);        
        // Call the Score script here which paints the score on screen

        Material newMat = Resources.Load("UA_Cover", typeof(Material)) as Material;

        ///Confetti is disabled in the current build.
        //confetti.SetActive(true);
        //Working
        //if (!GameObject.Find("Confetti").activeInHierarchy)
        //  GameObject.Find("Confetti").SetActive(true);

        ///Play round success sound.
        general.clip = round_complete;
        general.Play();

        // Clear the timer digits before displaying the text for successive rounds

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("digit");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }
        gameObjects = null;

        ///Display "Well done", "Round complete", and "Starting in...".
        string s = "Well Done!!!";
        float increment = 0f;
        if (currRound != 1)
        {
            
            float[] zPos = { 1.5f, 3.25f, 4.23f, 5.06f, 4.98f, 7.92f, 8.91f, 9.81f, 10.93f, 11.81f, 12.38f, 14.05f, 14.71f, 16.07f, 17.37f, 18.87f, 20.00f, 21.30f };
            for (int i = 0; i < s.Length; i++)
            {
                char letterNow = s[i];
                if (letterNow == ' ') continue;
                GameObject winChar = (Instantiate(Resources.Load("RoundVisual/Letter_" + letterNow)) as GameObject);
                winChar.transform.position = new Vector3(20f, pointRefrence.transform.position.y + 20.00f, 10 - zPos[i]);
                Vector3 temp = winChar.transform.rotation.eulerAngles;
                temp.y = 90.00f;
                winChar.transform.rotation = Quaternion.Euler(temp);
                winChar.transform.localScale += new Vector3(1f, 1f, 1f);
                winChar.tag = "winChar";
                winChar.GetComponent<Renderer>().material = newMat;
            }

            s = "Round " + currRound + " Complete!";
            increment = 0f;
            float[] zPos1 = { 1.5f, 2.75f, 3.86f, 5.06f, 6.19f, 7.67f, 8.08f, 9.81f, 11.18f, 12.14f, 13.05f, 14.38f, 15.29f, 15.9f, 16.79f, 17.54f, 19.00f, 20.09f, 22f, 23f, 24f, 25f, 26f, 27f, 28f, 29f, 30f, 31f, 32f, 33f };
            for (int i = 0; i < s.Length; i++)
            {
                char letterNow = s[i];
                if (letterNow == ' ') continue;
                GameObject winChar = (Instantiate(Resources.Load("RoundVisual/Letter_" + letterNow)) as GameObject);
                winChar.transform.position = new Vector3(20f, pointRefrence.transform.position.y + 15.00f, 13 - zPos1[i]);
                Vector3 temp = winChar.transform.rotation.eulerAngles;
                temp.y = 90.00f;
                winChar.transform.rotation = Quaternion.Euler(temp);
                winChar.transform.localScale += new Vector3(1f, 1f, 1f);
                winChar.tag = "winChar";
                winChar.GetComponent<Renderer>().material = newMat;
            }
        }
        

        s = "Starting Round " +(currRound);
        increment = 0f;
        float[] zPos2 = { 1.5f, 2.92f, 3.77f, 4.73f, 6.02f, 6.76f, 7.58f, 8.65f, 11.18f, 11.81f, 12.97f, 13.8f, 14.88f, 16.07f, 17.37f, 18.45f, 20.00f, 21f, 22f, 23f, 24f, 25f, 26f, 27f, 28f, 29f, 30f, 31f, 32f, 33f };
        for (int i = 0; i < s.Length; i++)
        {
            char letterNow = s[i];
            if (letterNow == ' ') continue;
            GameObject winChar = (Instantiate(Resources.Load("RoundVisual/Letter_" + letterNow)) as GameObject);
            winChar.transform.position = new Vector3(20f, pointRefrence.transform.position.y + 10.00f, 13 - zPos2[i]);
            Vector3 temp = winChar.transform.rotation.eulerAngles;
            temp.y = 90.00f;
            winChar.transform.rotation = Quaternion.Euler(temp);
            winChar.transform.localScale += new Vector3(1f, 1f, 1f);
            winChar.tag = "winChar";
            winChar.GetComponent<Renderer>().material = newMat;
        }

        // The wait after which we enable the scene objects calling the enableNow()
        if (currRound == 1)
            waitTimer = true;
        if (currRound == 2)
            waitTimerRound2 = true;
        if (currRound == 3)
            waitTimerRound2 = true;

    }
    public static void clearSceneAndDisplay()
    {
        //Working
        disableSceneObjects();

        Countdown countdown = Countdown.getInstance();

        countdown.sendTimerToScore(3);

        Material newMat = Resources.Load("UA_Cover", typeof(Material)) as Material;

        general.clip = congo;
        general.Play();

        string s = "Congratulations!!";
        //float increment = 0f;
        float[] zPos = { 1.5f, 3f, 4.19f, 5.48f, 6.77f, 7.67f, 8.91f, 9.81f, 11.18f, 11.81f, 13.05f, 14.05f, 14.71f, 16.07f, 17.37f, 18.87f, 20.00f };
        for (int i = 0; i < s.Length; i++)
        {
            char letterNow = s[i];
            GameObject winChar = (Instantiate(Resources.Load("G0_" + i + "_" + letterNow)) as GameObject);
            winChar.transform.position = new Vector3(14f, pointRefrence.transform.position.y + 10.00f, 10 - zPos[i]);
            Vector3 temp = winChar.transform.rotation.eulerAngles;
            temp.y = 90.00f;
            winChar.transform.rotation = Quaternion.Euler(temp);
            winChar.transform.localScale += new Vector3(1f, 1f, 1f);
            winChar.tag = "winChar";
            winChar.GetComponent<Renderer>().material = newMat;
        }
    }
    public static void displayGameOver()
    {
        disableSceneObjects();
        Material newMat = Resources.Load("UA_Cover", typeof(Material)) as Material;

        general.clip = gameOver;
        general.Play();
        string s = "Game Over!";
        float increment = 0f;
        float[] zPos = { 1.5f, 3f, 4.19f, 5.73f, 6.77f, 7.67f, 8.91f, 9.81f, 10.84f, 11.81f, 13.05f, 14.05f, 14.71f, 16.07f, 17.37f, 18.87f, 20.00f };
        for (int i = 0; i < s.Length; i++)
        {
            char letterNow = s[i];
            if (i == 4)
                continue;
            GameObject winChar = (Instantiate(Resources.Load("Game_Over/G0_" + i + "_" + letterNow)) as GameObject);
            
            winChar.transform.position = new Vector3(14f, pointRefrence.transform.position.y + 15.00f, 10 - zPos[i]);
            Vector3 temp = winChar.transform.rotation.eulerAngles;
            temp.y = 90.00f;
            winChar.transform.rotation = Quaternion.Euler(temp);
            winChar.transform.localScale += new Vector3(1f, 1f, 1f);
            winChar.tag = "winChar";
            winChar.GetComponent<Renderer>().material = newMat;
        }
    }

 }