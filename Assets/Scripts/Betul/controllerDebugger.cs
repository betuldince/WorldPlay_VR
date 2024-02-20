using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using System.IO;

public class controllerDebugger : MonoBehaviour
{
    // Start is called before the first frame update-+
   
    private SteamVR_Action_Pose poseActionR;
    private SteamVR_Action_Pose poseActionL;
    public GameObject cameraRig;

    public string filename ="";
    private TextWriter tw;
    void Start()
    {
        poseActionR = SteamVR_Input.GetAction<SteamVR_Action_Pose>("Pose_r");
        poseActionL = SteamVR_Input.GetAction<SteamVR_Action_Pose>("Pose" );
        filename = Application.dataPath + "/test4.csv";
        tw = new StreamWriter(filename, false);
        tw.WriteLine("Position_Right_x,Position_Right_y,Position_Right_z,Position_Left_x,Position_Left_y," +
            "Position_Left_z,Rotation_Right_x,Rotation_Right_y,Rotation_Right_z,Rotation_Left_x,Rotation_Left_y,Rotation_Left_z");
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 headSetPos = cameraRig.transform.position;
        Debug.Log("Head set" + headSetPos);
        tw.WriteLine(poseActionR.localPosition+","+ poseActionL.localPosition + "," + poseActionR.localRotation.eulerAngles + "," + poseActionL.localRotation.eulerAngles);

 

    }
    public void OnApplicationQuit()
    {
        tw.Close();
    }
}
