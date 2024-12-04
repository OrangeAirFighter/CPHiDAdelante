using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RobotInput : IInput
{

    GameObject networkObject;
    NetworkCommunicationController networkController;

    
    public double[,] GetForces()
    {
        double[,] result = new double[2,7];
        for (int i = 0; i <7; i++)
        {
            result[0, i] = 0; // Data from WiiBB A
            result[1, i] = 0; // Data from WiiBB B

        }

        result = networkController.GetForceData();
        return result;

    }


    
    public void startController()
    {
        networkObject = GameObject.Find("NetworkCommunicationObject");
        networkController = networkObject.GetComponent<NetworkCommunicationController>();
        
    }


}

