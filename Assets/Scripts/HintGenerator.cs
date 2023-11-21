using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintGenerator : MonoBehaviour
{

    public GameObject parentContainer;
    public int length = 0;
    private int count = 0;
    public bool start = false;
    bool started = false;
    //private SendInputString Obj;
    string inputChild;
    string myTag;

    string hintInput;
    string hintTag;
    public GameObject hintInst;
     
    /// <summary>
    /// handled starts with -1, if gameStateContainer.state value is higher, means game has started or progressed. 
    /// </summary>
    int handler = -1;

   public GameObject A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z;

    void SetContainerPhysics()
    {
        Debug.Log("SetContainerPhysics");
        //this.gameObject.transform.RotateAround(transform.position, Vector3.up, -180);

    }

    public void gameStart()
    {

        //hintInput = gameParametersContainer.gameParam.inputForHint.ToUpper();
        hintInput = "CALIBRATION";
        Debug.Log("hint: " + hintInput);
        length = hintInput.Length;
        SetContainerPhysics();
        CreateContainers();
    }

    public void clearHint()
    {
        while (transform.childCount != 0)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }
    public void createHint()
    {

        string dump = "";
        if (gameStateContainer.state == 0)
        {
            dump = "calibration";

        }
        else if (gameStateContainer.state == 1)
        {
            dump = "Adverbs of quantity";

        }
        else if (gameStateContainer.state == 2)
        {
            dump = "game";

        }
        else if (gameStateContainer.state == 3)
        {
            dump = "animal";

        }

        //hintInput = gameParametersContainer.gameParam.inputForHint.ToUpper(); // i changed this
        hintInput = dump.ToUpper();
        Debug.Log("Hint: " + hintInput);
        length = hintInput.Length;
        if (gameParametersContainer.gameParam.calibration == 0 && gameStateContainer.state == 1)
            SetContainerPhysics();
        CreateContainers();
    }
    public void Update()
    {
        //if (!started)
        //{
        //    if(start)
        //    {
        //        gameStart();
        //        started = true;
        //    }

        //}
        
        if (handler < gameStateContainer.state)
        {
            if (gameStateContainer.state == 0)
                gameStart();
            else
            {
                clearHint();
                createHint();
            }
            handler = gameStateContainer.state;
        }
    }

    public void CreateContainers()
    {
        count = 0;
        //update value of y for multiple words on the "space" letter
        while (length > count)
        {
            Vector3 temp = transform.position;

          //  Vector3 temp2 = transform.position;

            Quaternion rotat = transform.rotation;

            temp.z -= 0.1f * (count + 1);
           // temp2 = temp;

            myTag = hintInput[count].ToString();

            hintInst.tag = this.myTag;
                     
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

                //hintTag = hintInput[count].ToString();

                //Vector3 hintVec = transform.position;
                //Quaternion hintRotat = transform.rotation;

                //hintInst.tag = this.hintTag;
                ////hintInst = A;
                GameObject hints = Instantiate(tempObj, temp, rotat);
                hints.transform.SetParent(hintInst.transform, true);
              //  count++;
                //

                


                tempObj = null;

            }
            count++;
        }
    }
}

