/// \file
/// \brief ClientController is used to control the client behaviour.
/// 
/// It is designed to be as customizable as possible. The customizability allows for a increased playability depending on the player's individual preferences. 
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;
//using UnityEngine.Networking.NetworkSystem;

using UnityEngine.SceneManagement;


/// <summary>
///The class used to define the format of the message received from the network. 
/// </summary>
///Should be declared identically to the gameParameters class in serverController.
public class gameParameters //MessageBase
{
    /// <summary>
    /// The count of the rounds. 
    /// </summary>

    public int count;
    public string respawnTime;
    /// <summary>
    /// The solution string. Attached to \link SpawnerSettings::solutionString \endlink. 
    /// </summary>
    public string[] input;
    /// <summary>
    /// The hint string. 
    /// </summary>
    public string inputForHint;
    /// <summary>
    /// A bitmap used to represent which letters of the solution string are to be displayed and which are to be hidden. 
    /// </summary>
    public int[] bitMapSubString0, bitMapSubString1, bitMapSubString2, bitMapSubString3;
    /// <summary>
    /// THe number of letters in the solution string which are to be hidden. 
    /// </summary>
    public int[] No_of_blanks;
    /// <summary>
    /// The number of obstacles for each round.
    /// </summary>
    public int[] No_of_obstacles;
    /// <summary>
    /// The interval between the spawned objects disappearing and reappearing. Attached to \link SpawnerSettings::spawnInterval \endlink.
    /// </summary>
    public int[] SpawnInterval;
    /// <summary>
    /// If the spawned objects always face the user's eye always or not. Attached to \link SpawnerSettings::alphabetsFaceUsersEye \endlink.
    /// </summary>
    public bool[] AlphabetsFaceUser;
    /// <summary>
    /// Enable or diable a spinning effect for the spawned objects. Attached to \link SpawnerSettings::spin \endlink.
    /// </summary>
    public bool[] spin;
    /// <summary>
    /// If the alphabets pertaining to the solving the puzzle are to be spawned twice. 
    /// </summary>
    public bool[] repeatSolution;
    /// <summary>
    /// Sets the speed at which the objects should shuffle (fly) around. Attached to \link SpawnerSettings::flyingSpeed \endlink.
    /// </summary>
    public int FlyingSpeed;
    /// <summary>
    /// Sets the speed at which the alphabets should rotate. Attached to \link SpawnerSettings::rotationSpeed \endlink.
    /// </summary>
    public int RotationSpeed;
    /// <summary>
    /// Represents the ten dgree difficulty of a word as denoted by the TwinWord Langauge Scoring API. 
    /// </summary>
    public int[] Difficulty;
    /// <summary>
    /// The height of the ramp from the ground.
    /// </summary>
    public float rampHeight;
	/// <summary>
	/// The minimum and maximum height of the alphabets.
	/// </summary>
	public float minHeight, maxHeight;
	/// <summary>
	/// 0: No calibration.
	/// 1: Run with all rounds.
	/// 2: Run only calibration.
	/// </summary>
	public int calibration;
    /// <summary>
    /// The last calibration for the user, 0 if no prior calibrations exist. 
    /// </summary>
    public float mostRecentCalibration;    
}

/// <summary>
/// The class used to control the WordplayVR Client behaviour.
/// </summary>
public class clientController : MonoBehaviour {
    /// <summary>
    ///NetworkClient is the class used to establsih a connection as a client. 
    /// </summary>
    NetworkClient myClient;
    /// <summary>
    ///Set the server's IP address.
    /// </summary>
    public string serverAddress = "127.0.0.1";
    public HeightAdjust heightAdjust;
    /// <summary>
    ///The number used to identify a Start message from the WordplayVR-Remote Server.
    /// </summary>
    public static short MSG_GAME_PARAMETERS_START = 1005;
    /// <summary>
    ///The number used to identify a Stop message from the WordplayVR-Remote Server.
    /// </summary>
    public static short MSG_GAME_PARAMETERS_STOP = 1007;
    /// <summary>
    ///The number used to identify a Update message from the WordplayVR-Remote Server.
    /// </summary>
	public static short MSG_GAME_PARAMETERS_UPDATE = 1006;
    /// <summary>
    ///The number used to send a Game end update message to the WordplayVR-Remote Server.
    /// </summary>
    public static short MSG_GAME_PARAMETERS_GEU = 1010;
    /// <summary>
    ///The number used to send a Calibration complete update message to the WordplayVR-Remote Server.
    /// </summary>
    public static short MSG_GAME_PARAMETERS_CCU = 1011;
    /// <summary>
    ///The number used to send a IP update message to the WordplayVR-Remote Server.
    /// </summary>
    public static short MSG_GAME_PARAMETERS_IPU = 1012;

     
    /// <summary>
    /// The server Discovery component, derived from NetworkDiscovery. 
    /// </summary>
   // ServerDiscovery sD;
    /// <summary>
    /// If the start up configuration has been done after the ClientServer has been discovered.
    /// </summary>
    bool discoveredStartUp = false;
    /// <summary>
    /// If calibration has been sent to the WordplayVR-Remote Server.
    /// </summary>
    bool calibrationSaved = false;

    private gameParameters msg;
    private ParameterSelection parameterSelection;

    /// <summary>
    /// We destroy the current instance if we find a clientController already running. We need this to 
    /// prevent duplicates clientControllers being created when we restart the game as the old one is a 
    /// "DontDestryOnLoad" and persists on restart.
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
    }
 
    // Use this for initialization
    void Start() {
        /*
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.RegisterHandler(MSG_GAME_PARAMETERS_START, startGame);
        myClient.RegisterHandler(MSG_GAME_PARAMETERS_STOP, stopGame);
        myClient.RegisterHandler(MSG_GAME_PARAMETERS_UPDATE, updateToGame);
        myClient.RegisterHandler(MsgType.Disconnect, OnDisconnected);
        //var config = new ConnectionConfig();
        sD = gameObject.AddComponent<ServerDiscovery>();
        //Debug.Log("start called");
        //string ip = LocalIPAddress();
        //Debug.Log("The ip is " + ip);
        */
        parameterSelection = FindObjectOfType<ParameterSelection>();
        startGame();
    }

    // Update is called once per frame
    void Update() {
        ///If the server has been discovered and the discovery start up has not yet been preformed.
        /* (sD.discovered && !discoveredStartUp)
        {
            discoveredStartUp = true;
            serverAddress = sD.networkAddress;
            Debug.Log("Connecting to WordplayVR-Remote Server.");
            myClient.Connect(serverAddress, 4444);
            
        }*/

        ///If the game is running
        if (gameStateContainer.state != -1)
        {
            ///Check if last round is completed
            if (gameStateContainer.state == gameParametersContainer.gameParam.count + 1)
            {
                gameEndUpdate();
                gameStateContainer.state++;
            }
            ///Check if calibration has been just completed
            if (gameStateContainer.state == 1 && gameParametersContainer.gameParam.calibration != 0 && !calibrationSaved)
            {
                calibrationEndUpdate();
                calibrationSaved = true;
            }
        }


    }

    /// <summary>
    /// Once the server accepts the connection request, we send our ip for database purposes. 
    /// </summary>
    ///
    /*
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to WordplayVR-Remote Server successfully.");
        sendIP();
    }*/

    /// <summary>
    /// Once the connection is disrupted, we restart the NetworkDiscovery Client so it can once again
    /// wait for a message from the NetworkDiscovery Server. 
    /// </summary>
    /*
    public void OnDisconnected(NetworkMessage netMsg)
    {
        //ErrorMessage error = netMsg.ReadMessage<ErrorMessage>();
        Debug.Log("Connection error!");
        //myClient.Connect(serverAddress, 4444);        
        //Debug.Log("Attempting to connect to server again..."); ;
        Debug.Log("Restarted Network Discovery Client.");
        discoveredStartUp = false;
        sD.discovered = false;        
    }
    */
    /// <summary>
    /// On receiving a start message from the WordplayVR-Remote server we save the parameters
    /// and start the game.
    /// </summary>
    public void startGame( )
    {
        Debug.Log("Received Start Message.");
        //gameParameters msg = netMsg.ReadMessage<gameParameters>();

        // set gameParameters & gameParametersContainer manually

        msg = new gameParameters();
        msg.count = 4;
        msg.input = new string[4];
        msg.SpawnInterval = new int[4];
        msg.AlphabetsFaceUser = new bool[4];
        msg.spin = new bool[4];
        msg.repeatSolution = new bool[4];
        msg.Difficulty = new int[4];
        msg.No_of_blanks = new int[4];
        msg.No_of_obstacles = new int[4];
        msg.inputForHint ="";

        for (int i = 0; i < 4; i++)
        {
  
                msg.SpawnInterval[i] = parameterSelection.SpawnInterval[i];
                msg.input[i] = parameterSelection.input[i];
                msg.AlphabetsFaceUser[i] = parameterSelection.AlphabetsFaceUser[i];
                msg.spin[i] = parameterSelection.spin[i];
                msg.repeatSolution[i] =parameterSelection.repeatSolution[i];
                msg.Difficulty[i] = parameterSelection.Difficulty[i];
                msg.No_of_blanks[i] = parameterSelection.No_of_blanks[i];
                msg.FlyingSpeed = parameterSelection.FlyingSpeed;
                msg.RotationSpeed = parameterSelection.RotationSpeed;
               
 
        }


 

        List<int[]> bitMapSubString = new List<int[]>();
        for (int j = 0; j < 4; j++)
        {
            ///Create a temp int bitmask of length of the corresponding input string, add it to bitmapList.
            int[] temp = new int[msg.input[j].Length];
            bitMapSubString.Add(temp);
            System.Random rnd = new System.Random();
            ///Initialize bitmap to all 1's. 
            for (int i = 0; i < bitMapSubString[j].Length; i++)
                bitMapSubString[j][i] = 1;

            int rand_id;
            ///We need more than 2 characters to be displayed in the puzzle (specifically the first and the last characters). 
            if (msg.No_of_blanks[j] > msg.input[j].Length - 2)
                msg.No_of_blanks[j] = msg.input[j].Length / 2;

            ///Run until enough 'blanks' are created. 
            while (msg.No_of_blanks[j] != 0)
            {
                //Randomly search for spots which are not 0 already.
                rand_id = rnd.Next(1, msg.input[j].Length - 1);
                if (bitMapSubString[j][rand_id] != 0)
                {
                    bitMapSubString[j][rand_id] = 0;
                    msg.No_of_blanks[j]--;

                }
            }
        }

        ///Refill the No_of_blanks parameter as it was modified while processing it. 
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
                msg.No_of_blanks[i] = 5; //calibration
            else
                msg.No_of_blanks[i] = 3; //other cases
        }
        msg.bitMapSubString0 = bitMapSubString[0];
        msg.bitMapSubString1 = bitMapSubString[1];
        msg.bitMapSubString2 = bitMapSubString[2];
        msg.bitMapSubString3 = bitMapSubString[3];
        /*
        if (calibrationOnly)
            msg.calibration = 2;
        else
            msg.calibration = (int)calibrationToggle.value;
            *///Calibration
        msg.rampHeight = 5;
        msg.minHeight = 0.4f;
        msg.maxHeight =1.5f;
       // msg.mostRecentCalibration = dbQueryManager.mostRecentCalibration;

        ///Save parameters.
        gameParametersContainer.gameParam = msg;

       

        ///Set ramp height.
        heightAdjust = GameObject.Find("MainParentObject").GetComponent<HeightAdjust>();
        ///Subtract ramp height offset.
        heightAdjust.floorHeight = msg.rampHeight - 15.6f;
        ///Start the game by incrementing the state.
        gameStateContainer.state++;
        
        ///Checking if calibration is to be performed.
        if (gameParametersContainer.gameParam.calibration == 0)
        {   ///If calibration is not to be done and if a non-zero calibration value is provided by the tablet, use that. 
            if (gameParametersContainer.gameParam.mostRecentCalibration != 0)
            {   ///Directly change state to round 1 since calibration is to be skipped.
                gameStateContainer.state++;
            }
            ///Set the calibration value from local storage if not provided by the tablet, retreive last value from playerprefs.
            else if (PlayerPrefs.HasKey("LastCalibrationTime") && PlayerPrefs.GetFloat("LastCalibrationTime") != 0)
            {
                Debug.Log("Locally saved calibration data found!");
                gameParametersContainer.gameParam.mostRecentCalibration = PlayerPrefs.GetFloat("LastCalibrationTime");
                ///Directly change state to round 1 since calibration is to be skipped.
                gameStateContainer.state++;
            }
            else
            {   ///50 is the default value we set if a local calibration is not found.
                gameParametersContainer.gameParam.mostRecentCalibration = 50;
                ///Directly change state to round 1 since calibration is to be skipped.
                gameStateContainer.state++;
            }           
        }           
    }

    /// <summary>
    /// Placeholder for receiving NetowrkMessages from the WordplayVR-Remote Server afetr game has started.
    /// </summary>
    /*
    public void updateToGame(NetworkMessage netMsg)
    {
        //TODO: Updating required game parameters dynamically after the game has started.
    }
    */
    /// <summary>
    /// On receiving a Stop message we reload the scene and reset the static parameters.
    /// </summary>    
    public void stopGame()//NetworkMessage netMsg)
    {
        Debug.Log("Stop message received.");
        //SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        //Resources.UnloadUnusedAssets();
        gameStateContainer.state = -1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        //SceneManager.UnloadScene("NetworkedGame");
        //SceneManager.LoadScene("NetworkedGame");
    }

    /// <summary>
    /// Send our IP to WordplayVR-Remote Server after we discover its IP on receiving a broadcast
    /// message from its NetworkDiscovery Server. 
    /// </summary>
    /*
    public void sendIP()
    {        
        ipUpdate ipU = new ipUpdate();
        ipU.PC_IP = LocalIPAddress();
        myClient.Send(MSG_GAME_PARAMETERS_IPU, ipU);
    }

    /// <summary>
    /// Method to get the ip of the device in the local network. 
    /// </summary>    
    public static string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "0.0.0.0";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
                break;
            }
        }
        return localIP;
    }
    */
    /// <summary>
    /// Send the game completion stats to the WordplayVR-Remote Server. 
    /// </summary>
    public void gameEndUpdate()
    {
        gameStateUpdate endState = new gameStateUpdate();
        endState.score = new double[3];
        endState.time = new double[3];
        endState.roundsCompleted = 0;
        for (int i = 0; i < 3; i++)
        {
            endState.score[i] = gameStateContainer.score[i];
            endState.time[i] = gameStateContainer.time[i];
            if (endState.score[i] != 0)
                endState.roundsCompleted++;
        }
        Debug.Log("Sending data for: "+endState.roundsCompleted+" rounds.");
        if (endState.roundsCompleted == 3)
            endState.win = true;
        else
            endState.win = false;

      //  myClient.Send(MSG_GAME_PARAMETERS_GEU, endState);
    }
    /// <summary>
    /// Send a completed calibration data to the WordplayVR-Remote Server. 
    /// </summary>
    public void calibrationEndUpdate()
    {
        calibrationCompleteUpdate cUpdate = new calibrationCompleteUpdate();
        cUpdate.calibrationTime = 300-Countdown.prevTimer; //gameStateContainer.calibrationTime;
        Debug.Log("Sending calibration data: ");
      //  myClient.Send(MSG_GAME_PARAMETERS_CCU, cUpdate);
        ///Restart game.
        if(gameParametersContainer.gameParam.calibration == 2)
        {
            gameStateContainer.state = -1;
            SceneManager.LoadScene("NetworkedGame");
        }        
    }

}
/// <summary>
/// This NetworkDiscovery Client performs "ServerDiscovery". This is done by waiting for the broadcast from the Network Discovery Server
/// and then also saves the IP Address of the client. 
/// </summary>
/*
public class ServerDiscovery //: NetworkDiscovery
{
    /// <summary>
    /// Poll this to check if ClientDiscovery component has been located. 
    /// </summary>
    public bool discovered = false;
    /// <summary>
    /// The ip address of the machine in which the ClientDiscovery component runs in.
    /// </summary>
    public string networkAddress;

    /// <summary>
    /// Once the broadcast is received from the server, we save the server's address.
    /// </summary>
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        Debug.Log("WordplayVR-Remote Server Found at " + fromAddress);
        discovered = true;
        networkAddress = fromAddress;
    }

    void Start()
    { 
        showGUI = false;
        startServer();
    }
    /// <summary>
    /// Initializes and starts the Network Discovery Client. We passively wait to receive broadcasts from
    /// other network discovery servers.
    /// </summary>
    public void startServer()
    {
        Initialize();
        StartAsClient();
        Debug.Log("NetworkDiscovery Server started.");
    }
}*/
