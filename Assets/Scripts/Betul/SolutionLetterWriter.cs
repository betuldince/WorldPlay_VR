using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class SolutionLetterWriter : MonoBehaviour
{
    // Start is called before the first frame update
 
     
 
    private bool writeHeader = true;
    private string objectName = "";
    private string filePath = "";
    private bool solObj = false;
    private TextWriter tw;
    void Start()
    {
        SpawnerSettings s = FindObjectOfType<SpawnerSettings>();
        List<GameObject> sol = s.SolutionGameObjects;
        foreach (GameObject solName in sol )
        {
            if (gameObject.name[0].CompareTo(solName.name[0])==0)
            {
                solObj = true;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {

        if (gameObject.name[1].CompareTo('(') == 0)
        {
            return;
        }else if (writeHeader)
        {
            objectName = gameObject.name;
            string filename = "";
            if (solObj)
            {
                filename = "Round" + gameStateContainer.state + '-' + "Sol-"+ objectName;
            }
            else
            {
                filename = "Round" + gameStateContainer.state + '-' + "NonSol-" + objectName;
            }

            filePath = Application.dataPath + "/CSVfiles/" + filename + ".csv";
            tw = new StreamWriter(filePath, false);
            tw.WriteLine("Time Stamp," + filename + "_x," + filename + "_y," + filename + "_z");
            writeHeader = false;
        }


        DateTime curr = DateTime.Now;
        string timestamp = curr.ToString("yyyyMMdd_HHmmss");
        tw.WriteLine(timestamp+","+gameObject.transform.position.x + "," + gameObject.transform.position.y + "," + gameObject.transform.position.z);

    }
    public void OnApplicationQuit()
    {
        tw.Close();
    }
}
