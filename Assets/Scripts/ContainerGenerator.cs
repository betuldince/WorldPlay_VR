using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContainerGenerator : MonoBehaviour
{
    public bool solveCurrentRound = false;
    public bool start = false;
    public GameObject container;
    //public GameObject parentContainer;
    //public GameObject parentContainer1;
    public static bool beforeClearBoolean = false;
    //public GameObject parentContainer2;
    public static bool continueTimer = true;
    public List<int[]> bitMapSubString;
    public List<int> length;
    private int count = 0;
    private int index_in_input = 0;
    private bool started = false;
    List<GameObject> Containers = new List<GameObject>();
    String SubStringQuestion;
    List<string> inputChild = new List<string>();
    //string inputChild;
    string myTag;
    //set Spawner Objects
    public GameObject sp0;
    public GameObject sp1;
    public GameObject sp2;
    public GameObject A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z;

    /// <summary>
    /// deprecated
    /// </summary>
    //void SetContainerPhysics()
    //{
    //    parentContainer.gameObject.transform.RotateAround(transform.position, Vector3.up, -180);
    //    parentContainer1.gameObject.transform.RotateAround(transform.position, Vector3.up, -180);
    //    parentContainer2.gameObject.transform.RotateAround(transform.position, Vector3.up, -180);
    //}
    public void Start()
    {
        
    }
    void startGame()
    {
        bitMapSubString = new List<int[]>();
        length = new List<int>();
        //GameObject Parent = GameObject.Find("Parent");
        //SendInputString ParentInputObject = Parent.GetComponent<SendInputString>();
        //if (ParentInputObject)
        //{
            
        for (int j = 0; j < 4; j++)
        {
            inputChild.Add(gameParametersContainer.gameParam.input[j].ToUpper());
            length.Add(inputChild[j].Length);
                

        }
        bitMapSubString.Add(gameParametersContainer.gameParam.bitMapSubString0);
        bitMapSubString.Add(gameParametersContainer.gameParam.bitMapSubString1);
        bitMapSubString.Add(gameParametersContainer.gameParam.bitMapSubString2);
        bitMapSubString.Add(gameParametersContainer.gameParam.bitMapSubString3);
//            SetContainerPhysics();
            //Debug.Log("message recieved is" + inputChild);
        //CreateContainers(parentContainer, index_in_input);
        //CreateContainers(parentContainer1, index_in_input + 1);
        //CreateContainers(parentContainer2, index_in_input + 2);
        for(int i = 0; i < gameParametersContainer.gameParam.count; i++)
        {
            ///Create the container box parent.
            Containers.Add(new GameObject("ContainerBoxParent" + i));
            ///Set it as the ContainerBox's child.
            Containers[i].transform.parent = transform;
            ///Set position to origin.
            Containers[i].transform.localPosition = new Vector3(0, 0, 0);
            ///Call the function to start up the container.
            CreateContainers(Containers[i], i);
            ///Random rotation feature.
            int randomSide = UnityEngine.Random.Range(0, 4);
            randomSide = 0;
            ///Do nothing if default side is chosen.
            ///LEFT
            if (randomSide == 1)
            {
                Containers[i].transform.localPosition = new Vector3(-2.549f, 1.199001f, 1.507f);
                Containers[i].transform.localEulerAngles = new Vector3(0, -90, 0);
            }
            ///RIGHT
            else if (randomSide == 2)
            {
                Containers[i].transform.localPosition = new Vector3(0.237f, 1.199001f, 1.51f);
                Containers[i].transform.localEulerAngles = new Vector3(0, 90, 0);
            }
            ///BACK
            else if (randomSide == 3)
            {
                Containers[i].transform.localPosition = new Vector3(-2.723f, 1.199001f, -1.408f);
                Containers[i].transform.localEulerAngles = new Vector3(0, 180, 0);
            }
            
        }
        
        //Containers.Add(new GameObject("ContainerBoxParent")); 
        //Containers[1].transform.parent = transform;
        //Containers[1].transform.localPosition = new Vector3(0, 0, 0);
        //CreateContainers(Containers[1], 1);
        //Containers.Add(new GameObject("ContainerBoxParent"));
        //Containers[2].transform.parent = transform;
        //Containers[2].transform.localPosition = new Vector3(0, 0, 0);
        //CreateContainers(Containers[2], 2);

        //}
        //parentContainer = Containers[0];
        //parentContainer1 = Containers[1];
        //parentContainer2 = Containers[2];
        //SetContainerPhysics();

        ///Disable all containers.
        for(int i = 0; i < gameParametersContainer.gameParam.count; i++)
            Containers[i].SetActive(false);
        ///Enable the container corresponding to the first round.
        if(gameParametersContainer.gameParam.calibration == 0)
            Containers[1].SetActive(true);
        else
            Containers[0].SetActive(true);
    }
    void GamePlay3Rounds()
    {
               
        int i ,flag;
        ///check if round 1 is completed and load round 2 
        //while (i < Containers[0].transform.childCount) {
        //    if (Containers[0].transform.GetChild(i).gameObject.transform.GetChild(5).gameObject.activeInHierarchy == true)
        //            flag += 1;
        //    i++;
        //}
        for(i = 0; i < gameParametersContainer.gameParam.count; i++)
        {
            flag = 0;
            ///Check for number of solved letters.
            foreach (Transform containr in Containers[i].transform)
                if (containr.gameObject.transform.GetChild(5).gameObject.activeInHierarchy == true)
                    flag++;
            ///If solved the round.
            if (flag == Containers[i].transform.childCount || (flag !=0 && solveCurrentRound))
            {
                if(i + 1 != gameParametersContainer.gameParam.count && gameParametersContainer.gameParam.calibration != 2)
                {
                    ///Disable containers for previous round and enable the containers for next round.
                    Containers[i].SetActive(false);
                    //congrats.roundclearSceneAndDisplay(1);
                    Containers[i + 1].SetActive(true);
                }                
                ///Check for if it is the final round.
                else
                {
                    Containers[i].SetActive(false);
                    // Debug.Log("Win Scene");
                    continueTimer = false;
                    beforeClearBoolean = true;
                    //congrats.clearSceneAndDisplay();
                }
                ///Increase the state if more than calibrations is to be done. 
                if(gameParametersContainer.gameParam.calibration != 2)
                    gameStateContainer.state++;
                ///Reset game if only calibration is to be done. 
                else
                {
                    //gameStateContainer.state = gameParametersContainer.gameParam.count + 1;
                    //gameStateContainer.state = -1;
                    //SceneManager.LoadScene("NetworkedGame");
                    gameStateContainer.state++;
                }

                ///Reset the solve variable    
                solveCurrentRound = false;
            }

        }
            


        //Debug.Log("Flag0: "+ flag + ", Containers[0].transform.childCount: "+ Containers[0].transform.childCount);
        //if (flag == Containers[0].transform.childCount)
        //{
        //    Containers[0].SetActive(false);
        //    //congrats.roundclearSceneAndDisplay(1);
        //    Containers[1].SetActive(true);
        //    //sp1.SetActive(true);
        //    //sp0.SetActive(false);
        //    gameStateContainer.state++;
        //    //updateScore(10);
        //}



        /////load round 3
        //i = flag = 0;
        //while (i < Containers[1].transform.childCount)
        //{
        //    if (Containers[1].transform.GetChild(i).gameObject.transform.GetChild(5).gameObject.activeInHierarchy == true)
        //        flag += 1;
        //    i++;
        //}
        ////Debug.Log("Flag1: " + ", Containers[1].transform.childCount: " + Containers[1].transform.childCount);
        //if (flag == Containers[1].transform.childCount)
        //{
        //    Containers[1].SetActive(false);

        //    congrats.roundclearSceneAndDisplay(2);

        //    Containers[2].SetActive(true);
        //    sp2.SetActive(true);
        //    sp1.SetActive(false);
        //    gameStateContainer.state++;
        //    //updateScore(20);
        //    //GetComponent<AudioSource>().Play();

        //}



        ////finish scene
        //i = flag = 0;

        //while (i < Containers[2].transform.childCount)
        //{
        //    if (Containers[2].transform.GetChild(i).gameObject.transform.GetChild(5).gameObject.activeInHierarchy == true)
        //        flag += 1;
        //    i++;
        //}
        //if (flag == Containers[2].transform.childCount)
        //{
        //    Containers[2].SetActive(false);
        //   // Debug.Log("Win Scene");
        //    continueTimer = false;
        //    beforeClearBoolean = true;
        //    //congrats.clearSceneAndDisplay();

        //   // updateScore(30);
        //}

    }

    void Update() {
        if (!started)
        {
            if (start)
            {
                startGame();
                started = true;
            }
        }
        else
            GamePlay3Rounds();
    }

    void updateScore(int theScore)
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("ScoreDigit");

        for (var i = 0; i < gameObjects.Length; i++)
        {
            Destroy(gameObjects[i]);
        }

        string scoreString = theScore.ToString();
        float increment = 0f;
        float locationNow = 0f;

        // Extract each digit and get its prefab to be displayed.
        for (int i = 0; i < scoreString.Length; i++)
        {
            int scoreDigit = (int)(scoreString[i] - '0');

           // Debug.Log("New Score Digit is:" + scoreDigit);

            GameObject myItem1 = (Instantiate(Resources.Load("Digit_" + scoreDigit)) as GameObject);
            myItem1.transform.position = new Vector3(25f, 17f, 17f - increment);
            Vector3 temp = myItem1.transform.rotation.eulerAngles;
            temp.y = 87.78f;
            myItem1.transform.rotation = Quaternion.Euler(temp);
            myItem1.transform.localScale += new Vector3(1f, 1f, 1f);
            myItem1.tag = "ScoreDigit";

            increment += 2f;
            locationNow = 17.00f - increment;

        }
    }

    /// <summary>
    /// creates container boxes
    /// </summary>
    public void CreateContainers(GameObject ParentObj, int index)
    {
        count = 0;
        //update value of y for multiple words on the "space" letter
        while (length[index] > count)
        {
            Vector3 temp = ParentObj.transform.position;
            Vector3 temp2 = ParentObj.transform.position;
            Quaternion rotat = ParentObj.transform.rotation;
            temp.z -= 0.25f * (count + 1);
            temp2 = temp;
            myTag = inputChild[index][count].ToString();
            //Debug.Log(myTag+"check for space");
            container.tag = this.myTag;
            //Debug.Log("NAmE:" + container.name);
            // container.transform.parent = this.gameObject.transform;// position;

            GameObject foo = Instantiate(container, temp, rotat);
            foo.transform.parent = ParentObj.gameObject.transform;

            GameObject tempObj = null;
            int flag = 0;
            if (myTag == "A")
            {
                tempObj = A;
                flag = 1;
            }
            else if (myTag == "B")
            {
                tempObj = B;
                flag = 1;
            }
            else if (myTag == "C")
            {
                tempObj = C;
                flag = 1;
            }
            else if (myTag == "D")
            {
                tempObj = D;
                flag = 1;
            }
            else if (myTag == "E")
            {
                tempObj = E;
                flag = 1;
            }
            else if (myTag == "F")
            {
                tempObj = F;
                flag = 1;
            }
            else if (myTag == "G")
            {
                tempObj = G;
                flag = 1;
            }
            else if (myTag == "H")
            {
                tempObj = H;
                flag = 1;
            }
            else if (myTag == "I")
            {
                tempObj = I;
                flag = 1;
            }
            else if (myTag == "J")
            {
                tempObj = J;
                flag = 1;
            }
            else if (myTag == "K")
            {
                tempObj = K;
                flag = 1;
            }
            else if (myTag == "L")
            {
                tempObj = L;
                flag = 1;
            }
            else if (myTag == "M")
            {
                tempObj = M;
                flag = 1;
            }
            else if (myTag == "N")
            {
                tempObj = N;
                flag = 1;
            }
            else if (myTag == "O")
            {
                tempObj = O;
                flag = 1;
            }
            else if (myTag == "P")
            {
                tempObj = P;
                flag = 1;
            }
            else if (myTag == "Q")
            {
                tempObj = Q;
                flag = 1;
            }
            else if (myTag == "R")
            {
                tempObj = R;
                flag = 1;
            }
            else if (myTag == "S")
            {
                tempObj = S;
                flag = 1;
            }
            else if (myTag == "T")
            {
                tempObj = T;
                flag = 1;
            }
            else if (myTag == "U")
            {
                tempObj = U;
                flag = 1;
            }
            else if (myTag == "V")
            {
                tempObj = V;
                flag = 1;
            }
            else if (myTag == "W")
            {
                tempObj = W;
                flag = 1;
            }
            else if (myTag == "X")
            {
                tempObj = X;
                flag = 1;
            }
            else if (myTag == "Y")
            {
                tempObj = Y;
                flag = 1;
            }
            else if (myTag == "Z")
            {
                tempObj = Z;
                flag = 1;
            }



            if (flag == 1)
            {
                tempObj.tag = myTag;
                tempObj.SetActive(true);

                temp2.x += 0.033F;
                temp2.y += -0.09F;
                temp2.z += 0.164F;

                //tempObj.transform.parent = container.transform;
                // tempObj.transform.position = container.transform.position;




                GameObject childObj = Instantiate(tempObj, temp2, rotat);

                childObj.transform.SetParent(foo.transform, true);
                childObj.transform.position = temp2;


                // childObJ.transform.parent = container.transform;
                ///If solution character, disable it else disable the container. 
                if (bitMapSubString[index][count] == 0)
                    childObj.SetActive(false);
                else
                {
                    foo.transform.GetChild(0).gameObject.SetActive(false);
                    foo.transform.GetChild(1).gameObject.SetActive(false);
                    foo.transform.GetChild(2).gameObject.SetActive(false);
                    foo.transform.GetChild(3).gameObject.SetActive(false);
                    foo.transform.GetChild(4).gameObject.SetActive(false);

                    //foo.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    //foo.transform.GetChild(1).gameObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    //foo.transform.GetChild(2).gameObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    //foo.transform.GetChild(3).gameObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
                    //foo.transform.GetChild(4).gameObject.GetComponent<MeshRenderer>().material.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);

                }


                //  Instantiate(tempObj, temp2, rotat);

                //  container.transform.GetChild(0).gameObject.SetActive(true);
                //Debug.Log("Child"+container.transform.GetChild(0));



                tempObj = null;
            }
            count++;
        }
    }
}
