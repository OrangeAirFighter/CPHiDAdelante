using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Net;
using System.Net.Sockets;

using System.Text;
using System;
using System.IO;


public class WiiBalanceReader : MonoBehaviour
{

    private IInput inputController;

   
    double[,] forces = new double[2,6];
    double FtotA = 0;

    public Text txtFSA1;
    public Text txtFSA2;
    public Text txtFSA3;
    public Text txtFSA4;
    public Text txtFSAw;
    public Slider sliderFSAw;
    public Slider battStatA;
    public Slider sliderFSAfb;

    public Text txtFSB1;
    public Text txtFSB2;
    public Text txtFSB3;
    public Text txtFSB4;
    public Text txtFSBw;
    public Slider sliderFSBw;
    public Slider battStatB;
    public Slider sliderFSBfb;

    public GameObject Ball;
    Rigidbody ballRB;

    float ballLimit = 2.0f;
    float cBall = 4f;

    float oldForceA = 0;
    int countSameA = 0;
    float oldForceB = 0;
    int countSameB = 0;

    bool keyDown = false;
    bool logging = false;
    string logfile;


    // Start is called before the first frame update
    void Start()
    {
        inputController = new RobotInput();
        inputController.startController();

        ballRB = Ball.GetComponent<Rigidbody>();


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        forces = inputController.GetForces(); // Get forces from both WiiBB's
        //FtotA = forces[0,1] + forces[0, 2] + forces[0, 3] + forces[0, 4];
        //Debug.Log("board: " + forces[0, 0] + " - F1: " + forces[0, 1] + " - F2: " + forces[0, 2] + " - F3: " + forces[0, 3] + " - F4: " + forces[0, 4]);
        //Debug.Log("board: " + forces[1, 0] + " - F1: " + forces[1, 1] + " - F2: " + forces[1, 2] + " - F3: " + forces[1, 3] + " - F4: " + forces[1, 4]);

        if (oldForceA == (float)forces[0, 1])
        {
            countSameA = countSameA + 1;
        }
        else
        {
            countSameA = 0;
        }
        

        if (forces[0, 0] == 0 || countSameA > 100  ) // if forcevalues don't change for a while, board seems to be disconnected....
        {
            txtFSAw.text = "Not connected";
        }
        else
        {
            txtFSA1.text = string.Format("{0,6:###.##}", forces[0, 1]);
            txtFSA2.text = string.Format("{0,6:###.##}", forces[0, 2]);
            txtFSA3.text = string.Format("{0,6:###.##}", forces[0, 3]);
            txtFSA4.text = string.Format("{0,6:###.##}", forces[0, 4]);
            txtFSAw.text = string.Format("{0,6:###.##}", forces[0, 5]);

            sliderFSAw.value = (float)forces[0, 5];
            battStatA.value = (float)forces[0, 6];
            sliderFSAfb.value = ((float)forces[0, 1] + (float)forces[0, 3]) - ((float)forces[0, 2] + (float)forces[0, 4]);

        }

        oldForceA = (float)forces[0, 1];

        // Balance Board B (right)

        if (oldForceB == (float)forces[1, 1])
        {
            countSameB = countSameB + 1;
        }
        else
        {
            countSameB= 0;
        }

        if (forces[1, 0] == 0 || countSameB > 100)
        {
            txtFSBw.text = "Not connected";
        }
        else
        {
            txtFSB1.text = string.Format("{0,6:###.##}", forces[1, 1]);
            txtFSB2.text = string.Format("{0,6:###.##}", forces[1, 2]);
            txtFSB3.text = string.Format("{0,6:###.##}", forces[1, 3]);
            txtFSB4.text = string.Format("{0,6:###.##}", forces[1, 4]);
            txtFSBw.text = string.Format("{0,6:###.##}", forces[1, 5]);

            sliderFSBw.value = (float)forces[1, 5];
            battStatB.value = (float)forces[1, 6];
            sliderFSBfb.value = ((float)forces[1, 2] + (float)forces[1, 4]) - ((float)forces[1, 1] + (float)forces[1, 3]);

        }

        oldForceB = (float)forces[1, 1];

        // eerste spelvorm, balanceer bal op lat....
        float ballPos = (float)forces[1, 5] - (float)forces[0, 5];
        //float ballPos = ((float)forces[0, 2]  + (float)forces[0, 4]) - ((float)forces[0, 1] + (float)forces[0, 3]);

        //if (Mathf.Abs(ballPos) <= ballLimit)
        //{
        //ballRB.position = new Vector3(cBall * ballPos, 0, 0);
        Ball.transform.position = new Vector3(cBall * ballPos, 0, 0);
        //}


        // Type 'L' to start logging data to standard file (C:\temp\WiiBB.....)
        // Note: please create 'C:\temp' is it not exists, or alternatively change the logfile variable below....
        if (Input.GetKey(KeyCode.L) && keyDown == false)
        {
            logging = true;
            keyDown = true;
            logfile = @"C:\temp\WiiBB_" + DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss_fff") + ".txt";

        }

        
        // Type 'Esc' to stop logging
        if (Input.GetKey(KeyCode.Escape))
        {
            logging = false;
            keyDown = false;

        }

        if (logging)
        {
            txtFSAw.color = new Color(1, 0, 0, 1);
            txtFSBw.color = new Color(1, 0, 0, 1);

            //Start new stream and log to it
            if (!File.Exists(logfile))
            {
                // Create a file to write to and start with Header.
                string createText = "Date       Time         : WiiBB_A f1 f2 f3 f4 Weight - WiiBB_B f1 f2 f3 f4 Weight" + Environment.NewLine;
                File.WriteAllText(logfile, createText);
            }
            
            string appendText = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " " + forces[0,1].ToString("N4") + " " + forces[0, 2].ToString("N4")
                                                                                 + " " + forces[0, 3].ToString("N4") + " " + forces[0, 4].ToString("N4")
                                                                                 + " " + forces[0, 5].ToString("N4") + " " + forces[1, 1].ToString("N4")
                                                                                 + " " + forces[1, 2].ToString("N4") + " " + forces[1, 3].ToString("N4")
                                                                                 + " " + forces[1, 4].ToString("N4") + " " + forces[1, 5].ToString("N4")
                                                                                 + Environment.NewLine;

            File.AppendAllText(logfile, appendText);



        }
        else
        {
            txtFSAw.color = new Color(1, 0.7863293f, 0, 1);
            txtFSBw.color = new Color(1, 0.7863293f, 0, 1);

        }

    }
}
