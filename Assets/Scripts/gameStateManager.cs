using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

/// <summary>
///A class to store the message passed from the network. Made public static so that the message can be accessed from other scripts such as SpawnerController.
/// </summary>
public static class gameParametersContainer
{
    public static gameParameters gameParam;
}

/// <summary>
/// A static class that holds important game state information.
/// </summary>
public static class gameStateContainer
{
    /// <summary>
    /// -1: Game not running
    /// 0 : Game started, in calibration round
    /// Greater than 1: Game in corresponding round 
    /// </summary>
    public static int state = -1;
    /// <summary>
    /// The score for each round
    /// </summary>
    public static double[] score = { 0, 0, 0 };
    /// <summary>
    /// The time taken to complete each round
    /// </summary>
    public static double[] time = { 0, 0, 0 };
    /// <summary>
    /// The number of collisions in each round
    /// </summary>
    public static double[] collisions = { 0, 0, 0 };
    /// <summary>
    /// The calibration time
    /// </summary>
    public static double calibrationTime = 0;
}

/// <summary>
/// A message class used to communicate game completion stats over the network. Primarily sent to  
/// WordplayVR-Remote Server. 
/// </summary>
/// Must be declared identical to WordplayVR-Remote/gameStateUpdate.
public class gameStateUpdate // : MessageBase
{
    public double[] score, time;
    public int roundsCompleted;
    public bool win;
}
/// <summary>
/// A message class used to communicate a completed calibration over the network. Primarily sent to  
/// WordplayVR-Remote Server. 
/// </summary>
/// Must be declared identical to WordplayVR-Remote/calibrationCompleteUpdate.
public class calibrationCompleteUpdate //: MessageBase
{
    public double calibrationTime;
}
/// <summary>
/// A message class used to communicate an IP Address string over the network. Primarily sent to  
/// WordplayVR-Remote Server after we discover its IP on receiving a broadcast message from its
/// NetworkDiscovery Server. 
/// </summary>
/// Must be declared identical to WordplayVR-Remote/ipUpdate.
public class ipUpdate //: MessageBase
{
    public string PC_IP;
}
/// <summary>
/// Handles the changes in gameStateContainer.state value.
/// </summary>
public class gameStateManager : MonoBehaviour {

    public List<GameObject> spawner;

    /// <summary>
    /// handler starts with -1, if gameStateContainer.state value is higher, means game has started or progressed. 
    /// </summary>
    int handler = -1;
	
	/// Update is called once per frame
	void Update () {
        
        if (handler < gameStateContainer.state)
        {   ///Handling game start.
            gameStateContainer.state = 0;
            if (gameStateContainer.state == 0)
            {   ///Start the game (container box generation and spawner).
                GameObject.Find("ContainerBox").GetComponent<ContainerGenerator>().start = true;
                ///Start the hint generator
                GameObject.Find("Hint").GetComponent<HintGenerator>().start = true;
                ///Start the time controller. 
                GameObject.Find("TimerController").GetComponent<Countdown>().gameStart = true;
                ///Enable the spawner.
                
                spawner[1].SetActive(true);
                if (gameParametersContainer.gameParam.calibration == 0)
                    Debug.Log("GSM: First round started.");
                else
                    Debug.Log("GSM: Calibration round started.");
            }
            ///Handling other round progressions, except if only calibration is to be performed.
            else if(gameStateContainer.state < gameParametersContainer.gameParam.count && gameParametersContainer.gameParam.calibration != 2)
            {
                ///If calibration is not to be performed, game starts at state 1 and not 0. 
                if(gameStateContainer.state == 1 && gameParametersContainer.gameParam.calibration == 0)
                {
                    GameObject.Find("TimerController").GetComponent<Countdown>().gameStart = true;
                    GameObject.Find("ContainerBox").GetComponent<ContainerGenerator>().start = true;
                    GameObject.Find("Hint").GetComponent<HintGenerator>().start = true;
                    Countdown countdown = Countdown.getInstance();
                    Countdown.showTimer = false;
                    countdown.sendTimerToScore(1);
                    countdown.resetTimer(1);
                    StartCoroutine(loadNextSpawner());
                    congrats.roundclearSceneAndDisplay(gameStateContainer.state);
                }
                else
                {
                    ///Disable the previous round spawner. 
                    spawner[gameStateContainer.state - 1].SetActive(false);
                    //spawner[gameStateContainer.state].SetActive(true);
                    ///Enable the spawner for the new round after a delay. 
                    StartCoroutine(loadNextSpawner());
                    congrats.roundclearSceneAndDisplay(gameStateContainer.state);
                    Debug.Log("GSM: New round started: " + gameStateContainer.state);
                }
                    
            }

            ///Handling last round ending. We perform the necessary changes to the scene
            ///and increment the state to indicate the clientController that game is in final
            ///state and can send the game end update to the server. 
            else if(gameStateContainer.state == gameParametersContainer.gameParam.count){
                Debug.Log("GSM: Last round");
                spawner[gameStateContainer.state - 1].SetActive(false);
                ///Call final display function. 
                congrats.clearSceneAndDisplay();
                gameStateContainer.state++;
            }

            ///After any case is handled, increment handler variable to indicate we are up to date in 
            ///handling changes to game state. 
            handler++;
        }
	}

    /// <summary>
    /// Enable the spawner for the new round after a delay.
    /// </summary>
    IEnumerator loadNextSpawner()
    {
        yield return new WaitForSeconds(1);
        spawner[gameStateContainer.state].SetActive(true);
    }
}
