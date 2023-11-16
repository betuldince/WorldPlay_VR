using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ContainerCollider : MonoBehaviour {

    public static int theScore;
    private AudioSource containerAudio;
    public AudioClip win; //set this in inspector with audio file
    public AudioClip loose;
    public Text ScoreText;
    private GameObject scoreDigit;
    public static bool continueTimer = true;
    public static bool beforeClearBoolean=false;
    private int beforeTime = 150;
    /// <summary>
    /// RAHUL ABHISHEK
    ///
    public AudioClip levelComplete;
    private GameObject scoreChar;
    private GameObject myItem1;
    public static int count;
    GameObject thePlayer;
    ContainerGenerator playerScript;
    public static int temp1;
    GameObject scoreDigitObj;
    /// </summary>

    private void Start()
    {
        //Debug.Log("Hello1");
        
        containerAudio = GetComponent<AudioSource>();
        theScore = 0;
        //count = 0;

        thePlayer = GameObject.Find("ContainerBoxParent");
      //  playerScript = thePlayer.GetComponent<ContainerGenerator>();
        //temp1 = 0;

        /*uncomment this for score*/

        //ScoreText = (Text)GameObject.Find("ScoreCanvas").GetComponent("Text");
    }

    public static void setBlanks(int val)
    {
        temp1 = val;
        Debug.Log("temp " + temp1);
    }

    private void OnCollisionEnter(Collision collision)
        {
        char match_first = (collision.gameObject.name)[0];
        string temp_str = match_first.ToString();


        string collisionObject = temp_str;
        string gameObjectTag = gameObject.tag;

        Debug.Log(collisionObject);
        Debug.Log(gameObjectTag);

        if (collisionObject.Equals(gameObjectTag))
        {
            
            //clean code in if (collisionObject.Equals(gameObjectTag))
            /* RE-ACTIVATE THIS PART!!!!!!!!!!!!!!!!!!!!!!!!
            Score si = Score.getInstance();
            si.incrementCurrRoundScore();
            si.displayPoints();
            //Displaying Digits Ends here

            // Displaying the Score Prefabs ends here

            containerAudio.clip = win;
            containerAudio.Play();
            Debug.Log("Play Win");
            */
            Transform solutionObject = gameObject.transform;

            // Debug.Log("Child!");
            Debug.Log("HEYY");
            solutionObject.GetChild(5).gameObject.SetActive(true);
            Debug.Log("Count :" + solutionObject.childCount);

            //Debug.Log(gameObject.transform.GetChild(0));
            Destroy(collision.gameObject);
            Debug.Log("Destroying after touch");

            /*
             This is the per round blanks, if no of blanks == total number of blanks display something
             Now this thing is being handled by the ContainerGenerator Script, it takes care of round completions

            if (temp1 == count)
            {
                Debug.Log("Win Scene"+"temp1 "+temp1);
                // SceneManager.LoadScene("Showcase");
                continueTimer = false;
                containerAudio.clip = levelComplete;

                beforeClearBoolean = true;
                containerAudio.Play();
                count = 0;
                //congrats.clearSceneAndDisplay();

                
                //count = 0;
            }
            */
        }
        else
        {
            Debug.Log("Play Loose");
            containerAudio.clip = loose;
            containerAudio.Play();
        }

    }
    private void Update()
    {
        if (beforeTime == 0)
        {
            beforeClearBoolean = false;
        }
        if (beforeClearBoolean)
            beforeTime -= 1;
        
    }
}
