using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoeManager : MonoBehaviour
{
    public static Material shoeMaterialLeft;
    public static Material shoeMaterialRight;

    public GameObject balanceBoardReader;
    public GameObject leftShoe;
    public GameObject rightShoe;

    public GameObject TopLeftSpawn;
    public GameObject TopRightSpawn;
    public GameObject BottomLeftSpawn;
    public GameObject BottomRightSpawn;

    private bool riseCase;

    private float TopLeft;
    private float TopRight;
    private float BottomLeft;
    private float BottomRight;
    private float TotalWeight;
    private float TotalWeight_B;

    public static float weightFactor = 0.8f; //percentage of total weight

    private string previousMovement = "Null";
    private string currentMovement ="BasePose";

    [HideInInspector]
    public float StepLeftCount = 0;
    [HideInInspector]
    public float StepRightCount = 0;
    [HideInInspector]
    public float ForwardLeftCount = 0;
    [HideInInspector]
    public float ForwardRightCount = 0;
    [HideInInspector]
    public float TurnRightCount = 0;
    [HideInInspector]
    public float TurnLeftCount = 0;

    private string gameDifficulty = "Stride";

    private Color startColor;

    private void Start()
    {
        leftShoe.GetComponent<Renderer>().material = shoeMaterialLeft;
        rightShoe.GetComponent<Renderer>().material = shoeMaterialRight;
        Invoke("stepChecker", 0f);
        startColor = leftShoe.GetComponent<Renderer>().material.color;
    }

    private void FixedUpdate()
    {
        if (riseCase)
        {
            leftShoe.transform.position += new Vector3(0,0.1f,0);
            leftShoe.GetComponent<Renderer>().material.color -= new Color(-1,-1,-1,-1);
        }

        //Test Code

        if (Input.GetKeyDown(KeyCode.T))
        {
            leftShoe.transform.position = TopLeftSpawn.transform.position;
            rightShoe.transform.position = BottomRightSpawn.transform.position;
            SetShoeRotation(rightShoe, BottomRightSpawn.transform.rotation);
            SetShoeRotation(leftShoe, BottomLeftSpawn.transform.rotation);
        }
    }

    public void setDifficulty(string difficulty)
    {
        gameDifficulty = difficulty;
    }

    public void stepChecker()
    {
        //Instance of balanceboard
        NetworkCommunicationController balanceboard = balanceBoardReader.GetComponent<NetworkCommunicationController>();

        //Calculate all Data Sectors 
        TotalWeight = balanceboard.Fweight;
        TotalWeight_B = balanceboard.Fweight_B;

        TopLeft = (balanceboard.F2  + balanceboard.F4) / TotalWeight;
        TopRight = (balanceboard.F2_B + balanceboard.F4_B) / TotalWeight_B;
        BottomLeft = (balanceboard.F1 + balanceboard.F3) / TotalWeight;
        BottomRight = (balanceboard.F1_B + balanceboard.F4_B) / TotalWeight_B;

        //Value's for boolean checks
        float threshold = 0.1f;
        float thresholdTurn = (TotalWeight + TotalWeight_B) * weightFactor;

        //rightShoe.GetComponent<Collider>().enabled = true;
        switch (gameDifficulty)
        {
/////////////////////////////////////////////////////////////////////////////////////////////////////////
            case "Rise":
                //is threshold still necessary?
                //might need to adapt position for this currently only one will be forward
                riseCase = true;
                //if (Mathf.Abs(TopLeft - BottomLeft) > threshold && Mathf.Abs(BottomRight - TopRight) > threshold)
                {
                    if (TotalWeight > TotalWeight_B)
                    {
                        currentMovement = "ForwardLeft";
                    }
                    else
                    {
                        currentMovement = "ForwardRight";
                    }
                }
                break;
/////////////////////////////////////////////////////////////////////////////////////////////////////////
            case "Stride":
                //Boolean checks for position
                if (BottomLeft > TopLeft && BottomRight > TopRight)
                {
                    currentMovement = "basePos";
                }

                else if (Mathf.Abs(TopLeft - BottomLeft) > threshold && Mathf.Abs(BottomRight - TopRight) > threshold)
                {
                    if (TopLeft > BottomLeft && BottomRight > TopRight)
                    {
                        currentMovement = "ForwardLeft";
                    }
                    else if (BottomLeft > TopLeft && TopRight > BottomRight)
                    {
                        currentMovement = "ForwardRight";
                    }
                }
                break;
/////////////////////////////////////////////////////////////////////////////////////////////////////////
            case "Twist":
                //Boolean checks for position
                if (BottomLeft > TopLeft && BottomRight > TopRight)
                {
                    currentMovement = "basePos";
                }

                else if (Mathf.Abs(TotalWeight - TotalWeight_B) > thresholdTurn)
                {
                    if (TotalWeight > TotalWeight_B)
                    {
                        currentMovement = "TurnStepLeft";
                    }
                    else if (TotalWeight < TotalWeight_B)
                    {
                        currentMovement = "TurnStepRight";
                    }
                }
                /*
                else if (Mathf.Abs(TopLeft - BottomLeft) > threshold && Mathf.Abs(BottomRight - TopRight) > threshold)
                {
                    if (TopLeft > BottomLeft && BottomRight > TopRight)
                    {
                        currentMovement = "ForwardLeft";
                    }
                    else if (BottomLeft > TopLeft && TopRight > BottomRight)
                    {
                        currentMovement = "ForwardRight";
                    }
                }*/
                break;
/////////////////////////////////////////////////////////////////////////////////////////////////////////
            default:
                Debug.Log("Option Not Possible");
                break;
        }

        //Check for change in position
        if (previousMovement != currentMovement)
        {
            //Switch case for each movement
            switch (currentMovement)
            {
                case "TurnStepLeft":
                    rightShoe.transform.position = TopLeftSpawn.transform.position;
                    leftShoe.transform.position = BottomLeftSpawn.transform.position;
                    SetShoeRotation(rightShoe, TopLeftSpawn.transform.rotation);
                    SetShoeRotation(leftShoe, TopLeftSpawn.transform.rotation);
                    TurnLeftCount += 1;
                    break;
                case "TurnStepRight":
                    leftShoe.transform.position = TopRightSpawn.transform.position;
                    rightShoe.transform.position = BottomRightSpawn.transform.position;
                    SetShoeRotation(rightShoe, TopRightSpawn.transform.rotation);
                    SetShoeRotation(leftShoe, TopRightSpawn.transform.rotation);
                    TurnRightCount += 1;
                    break;
                case "ForwardLeft":
                    leftShoe.transform.position = TopLeftSpawn.transform.position;
                    rightShoe.transform.position = BottomRightSpawn.transform.position;
                    SetShoeRotation(rightShoe, BottomRightSpawn.transform.rotation);
                    SetShoeRotation(leftShoe, BottomLeftSpawn.transform.rotation);
                    if (riseCase)
                    {
                        leftShoe.GetComponent<Renderer>().material.color = startColor;
                        StepLeftCount += 1;
                    } else
                    {
                        ForwardLeftCount += 1;
                    }
                    break;
                case "ForwardRight":
                    rightShoe.transform.position = TopRightSpawn.transform.position;
                    leftShoe.transform.position = BottomLeftSpawn.transform.position;
                    SetShoeRotation(rightShoe, BottomRightSpawn.transform.rotation);
                    SetShoeRotation(leftShoe, BottomLeftSpawn.transform.rotation);
                    if (riseCase)
                    {
                        StepRightCount += 1;
                    }
                    else
                    {
                        ForwardRightCount += 1;
                    }
                    break;
                default: //basepos or if not exist
                    if (riseCase)
                    {
                        leftShoe.transform.position = TopLeftSpawn.transform.position;
                        rightShoe.transform.position = TopRightSpawn.transform.position;
                        SetShoeRotation(rightShoe, BottomRightSpawn.transform.rotation);
                        SetShoeRotation(leftShoe, BottomLeftSpawn.transform.rotation);
                    } else
                    {
                        leftShoe.transform.position = BottomLeftSpawn.transform.position;
                        rightShoe.transform.position = BottomRightSpawn.transform.position;
                        SetShoeRotation(rightShoe, BottomRightSpawn.transform.rotation);
                        SetShoeRotation(leftShoe, BottomLeftSpawn.transform.rotation);
                    }
                    
                    break;
            }
        }

        previousMovement = currentMovement;
        Invoke("stepChecker", 1f);
    }

    private void SetShoeRotation(GameObject shoe, Quaternion targetRotation)
    {
        // Preserve the current x-axis rotation, and update only the y and z axes.
        Vector3 currentRotation = shoe.transform.rotation.eulerAngles;
        shoe.transform.rotation = Quaternion.Euler(currentRotation.x, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);
        
    }
}
