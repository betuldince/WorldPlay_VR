using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    
    [SerializeField] private Text uiText;
    [SerializeField] public static int mainTimer;
    public static AudioClip audioClip;
    public static AudioSource audioSource;
    GameObject myItem1;
    GameObject myItem2;
    GameObject myItem3;
    GameObject myItem4;
    GameObject totalDigit;
    private bool loadGameOver = false;
    GameObject scoreDigit;
    congrats congoObj;
    //static bool flagRound1 = false;
    private bool canCount = true;
    private bool doOnce = false;
    public static GameObject confetti;
    public static int blanks;
    //private bool congratsActive = false;

    public static GameObject pointRefrence;

    // Cleaning Code Variables
    public static double round1Timer;
    public static Countdown countdownInstance = null;
    private int timer;
    private int pseudoTimer;
    //private int transitTimer;
    //int temp;
    Score scoreInstance;
    public static int prevTimer;
    //GameObject scoreChar;
    public bool gameStart = false;
    public static float elapsedTime = 0;
    public static bool showTimer = true;

    // Making the class Singleton
    private Countdown()
    {
    }
    public static Countdown getInstance()
    {
        if (countdownInstance == null)
        {
            //Debug.Log("Create Object of CountDown");
            countdownInstance = new Countdown();
        }
        return countdownInstance;
    }
    public int getTimer()
    {
        return timer;
    }
    public void resetTimer(int currRound)
    {
        Countdown count = Countdown.getInstance();
        double calcNew = currRound * round1Timer;
        Debug.Log("Reset time to currRound(" + currRound + ") * round1Timer(" + round1Timer + ") = " + calcNew);
        countdownInstance.timer = (int)calcNew;
        countdownInstance.pseudoTimer = 300;
        //countdownInstance.transitTimer = 1000;
        Score.currRoundScore = 0;
    }
    public void sendTimerToScore(int currRound)
    {
        if (currRound == 1)
        {
            ///Set timer from previous calibration if received from remote. 
            if (gameParametersContainer.gameParam.calibration == 0 && gameParametersContainer.gameParam.mostRecentCalibration != 0)
                round1Timer = gameParametersContainer.gameParam.mostRecentCalibration;
            else
                round1Timer = 300 - prevTimer;

            //Debug.Log("Set round1timer to: " + round1Timer);         
        }            
        scoreInstance = Score.getInstance();
        //Debug.Log("PrevTimer: " +prevTimer);
        scoreInstance.calcScoreFromTimer(prevTimer,currRound);
    }
    void Start()
    {        
        canCount = true;
        doOnce = false;
        elapsedTime = 0;
        showTimer = true;
        pointRefrence = GameObject.Find("Planks");
        Debug.Log("Countdown Script");
        countdownInstance = Countdown.getInstance();
        confetti = GameObject.Find("Confetti");
        countdownInstance.timer = 300;
        //countdownInstance.temp = 60;
        countdownInstance.pseudoTimer = 300;
       // countdownInstance.transitTimer = 200;
        audioSource = GameObject.Find("TimerController").GetComponent<AudioSource>();
        // audioClip = Resources.Load("Audio/iphone_text_tone") as AudioClip;
        audioClip = Resources.Load("Audio/timer-1-sec-cut") as AudioClip;
        audioSource.clip = audioClip;

    }
    /*
     * Might need this if we decide later to have per blank score
    public static void setBlanksInCountdown(int val)
    {
        blanks = val;
        //debug.log("blanks " + blanks);
    }
    */
    void Update()
    {
        elapsedTime += Time.deltaTime;
        //Debug.Log(elapsedTime);

        if (gameStart)
        {
            countdownInstance.pseudoTimer -= 1;
            //countdownInstance.transitTimer = 0;
        }
        ///Blinking effect?
        if ((countdownInstance.timer >= 57 && countdownInstance.timer <= 60) || (countdownInstance.timer >= 26 && countdownInstance.timer <= 30) || (countdownInstance.timer >= 6 && countdownInstance.timer <= 10))
        {
            if (countdownInstance.pseudoTimer == 40)
            {
                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("digit");
                for (var i = 0; i < gameObjects.Length; i++)
                {
                    Destroy(gameObjects[i]);
                }
            }
        }

        if (elapsedTime>=1 && ContainerGenerator.continueTimer && gameStart && showTimer)
         {
            countdownInstance.HandleIt();
            elapsedTime = 0;
         }
    }
    /// <summary>
    /// Called after every second when the timer is running
    /// </summary>
    void HandleIt()
    {
        //Debug.Log("Timer in Handle It");
        //Debug.Log(timer);
        //Debug.Log(canCount);
        if (timer >= 0 && canCount)
        {
            int modifiedTimer = timer + 1;
            if (timer != mainTimer)
            {
                GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("digit");

                for (var i = 0; i < gameObjects.Length; i++)
                {
                    Destroy(gameObjects[i]);
                }
            }
            string timerString = timer.ToString();
            float increment = 0f;
            prevTimer = timer;
            // Extract each digit and get its prefab to be displayed.
            for (int i = 0; i < timerString.Length; i++)
            {
                int newTimer = (int)(timerString[i] - '0');

                myItem1 = (Instantiate(Resources.Load("Digit_" + newTimer)) as GameObject);
                myItem1.transform.position = new Vector3(0.15f + increment, pointRefrence.transform.position.y+10.00f, 19.92f);
                myItem1.transform.localScale += new Vector3(2f, 2f, 2f);
                myItem1.tag = "digit";
                
                myItem2 = (Instantiate(Resources.Load("Digit_" + newTimer)) as GameObject);
                myItem2.transform.position = new Vector3(20.00f, pointRefrence.transform.position.y + 10.00f, 0.00f - increment);
                Vector3 temp = myItem2.transform.rotation.eulerAngles;
                temp.y = 87.78f;
                myItem2.transform.rotation = Quaternion.Euler(temp);
                myItem2.transform.localScale += new Vector3(2f, 2f, 2f);

                myItem2.tag = "digit";
                
                myItem3 = (Instantiate(Resources.Load("Digit_" + newTimer)) as GameObject);
                myItem3.transform.position = new Vector3(0.15f - increment, pointRefrence.transform.position.y + 10.00f, -18.00f);

                temp = myItem3.transform.rotation.eulerAngles;
                temp.y = 187.00f;
                myItem3.transform.rotation = Quaternion.Euler(temp);
                myItem3.transform.localScale += new Vector3(2f, 2f, 2f);

                myItem3.tag = "digit";

                myItem4 = (Instantiate(Resources.Load("Digit_" + newTimer)) as GameObject);
                myItem4.transform.position = new Vector3(-19.00f, pointRefrence.transform.position.y + 10.00f, 0.31f + increment);

                temp = myItem4.transform.rotation.eulerAngles;
                temp.y = -90.78f;
                myItem4.transform.rotation = Quaternion.Euler(temp);
                myItem4.transform.localScale += new Vector3(2f, 2f, 2f);

                increment += 2f;

                myItem4.tag = "digit";

            }
            if ((timer >= 57 && timer<=60) || (timer >= 27 && timer <= 30) || (timer >= 7 && timer <= 10))
            {
                audioSource.Play();
                //Debug.Log("Playing Here");
                //temp--;
            }

            /*
             * If we need sound on the last few seconds uncomment this part
            if (timer == 5)
                GetComponent<AudioSource>().Play();
            else if (timer == 3)
                GetComponent<AudioSource>().Play();

             // Takes care of the blinking effect
           */

            timer -= 1;
            pseudoTimer = 300;

            if (timer == 0)
                loadGameOver = true;
        }
        else if (timer < 0 && !doOnce)
        {

            canCount = false;
            doOnce = true;
            timer = 1;
            if (loadGameOver)
            {
                congrats.displayGameOver();
                gameStateContainer.state = gameParametersContainer.gameParam.count + 1;
            }
                
        }
    }
    public void ResetBtn()
    {
        countdownInstance.timer = mainTimer;
        canCount = true;
        doOnce = false;

    }

}