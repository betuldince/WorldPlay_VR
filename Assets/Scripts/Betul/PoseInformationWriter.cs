using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class PoseInformationWriter : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject leftController;
    public GameObject rightController;
    public GameObject headSet;
    public GameObject spawner;
    private int letterCount;
    private bool writeHeader = true;
    public string filename = "";
    private string filePath = "";
    private TextWriter tw;
    void Start()
    {
        filePath = Application.dataPath + "/"+filename+".csv";
        tw = new StreamWriter(filePath, false);

        
    }

    // Update is called once per frame
    void Update()
    {
        string header = "";
        if (spawner.transform.childCount > 0 && writeHeader)
        {
            letterCount = spawner.transform.childCount;
            string[] nameLetter = new string[letterCount];
            int i = 0;
            foreach (Transform child in spawner.transform)
            {
                nameLetter[i] = child.name.Substring(0, 1);
                i++;
            }
           
            for (int j = 0; j < letterCount; j++)
            {
                header += nameLetter[j];
                header += "_x";
                header += ',';
                header += nameLetter[j];
                header += "_y";
                header += ',';
                header += nameLetter[j];
                header += "_z";
                header += ',';
            }
            //header += nameLetter[letterCount - 1];
          tw.WriteLine("Position_HeadSet_x,Position_HeadSet_y,Position_HeadSet_z,Position_Right_x,Position_Right_y,Position_Right_z,Position_Left_x,Position_Left_y," +"Position_Left_z"+","+header);
            writeHeader = false;
        }else if(spawner.transform.childCount == 0)
        {
            return;
        }
 
 


        Vector3 headSetPos = headSet.transform.position;
        Vector3 leftControllerPos = leftController.transform.position;
        Vector3 rightControllerPos = rightController.transform.position;
        string letterPositions = "";
        foreach (Transform child in spawner.transform)
        {
            letterPositions += child.position.x;
            letterPositions += ",";
            letterPositions += child.position.y;
            letterPositions += ",";
            letterPositions += child.position.z;
            letterPositions += ",";
            //Debug.Log(child.name.Substring(0,2)+child.position);
        }
        tw.WriteLine(headSetPos.x + "," + headSetPos.y + "," + headSetPos.z + "," + rightControllerPos.x + "," + rightControllerPos.y + "," + rightControllerPos.z + ","+ leftControllerPos.x+ "," + leftControllerPos.y+ "," + leftControllerPos.z+ ","+letterPositions);
        
    }
    public void OnApplicationQuit()
    {
        tw.Close();
    }
}
