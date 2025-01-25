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
    private bool shrinkingLeft;
    private bool shrinkingRight;

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

    private Vector3 startSize;

    private void Start()
    {
        shrinkingLeft = true;
        shrinkingRight = true;
        leftShoe.GetComponent<Renderer>().material = shoeMaterialLeft;
        rightShoe.GetComponent<Renderer>().material = shoeMaterialRight;
        Debug.Log(shoeMaterialLeft);
        Debug.Log(shoeMaterialRight);
        StartCoroutine("stepChecker");
        startColor = leftShoe.GetComponent<Renderer>().material.color;
        startSize = leftShoe.transform.localScale;
    }

    private void FixedUpdate()
    {   
        if (riseCase)
        {
            shrinkShoe();
        }
    }
    public void setDifficulty(string difficulty)
    {
        gameDifficulty = difficulty;
    }

    public IEnumerator stepChecker()
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
                riseCase = true;
                {
                    if (true)
                    {
                        if (TotalWeight > TotalWeight_B)
                        {
                            currentMovement = "ForwardLeft";
                        }
                        else
                        {
                            currentMovement = "ForwardRight";
                        }
                    } else
                    {
                        currentMovement = "basePos";
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
                
                break;
/////////////////////////////////////////////////////////////////////////////////////////////////////////
            default:
                Debug.Log("Option Not Possible");
                break;
        }

        Debug.Log(currentMovement);
        //Check for change in position
        if (previousMovement != currentMovement)
        {

            leftShoe.gameObject.tag = "Player";
            rightShoe.gameObject.tag = "Player";
            
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
                    
                    SetShoeRotation(rightShoe, BottomRightSpawn.transform.rotation);
                    SetShoeRotation(leftShoe, BottomLeftSpawn.transform.rotation);
                    if (riseCase)
                    {
                        shrinkingRight = true; 
                        shrinkingLeft = false;
                        growShoe();
                        rightShoe.transform.position = TopRightSpawn.transform.position;
                        StepLeftCount += 1;
                    } else
                    {
                        ForwardLeftCount += 1;
                        rightShoe.transform.position = BottomRightSpawn.transform.position;
                    }
                    break;
                case "ForwardRight":
                    rightShoe.transform.position = TopRightSpawn.transform.position;
                      
                    SetShoeRotation(rightShoe, BottomRightSpawn.transform.rotation);
                    SetShoeRotation(leftShoe, BottomLeftSpawn.transform.rotation);
                    if (riseCase)
                    {     
                        shrinkingLeft = true; 
                        shrinkingRight = false;
                        growShoe();
                        leftShoe.transform.position = TopLeftSpawn.transform.position;
                        StepRightCount += 1;
                    }
                    else
                    {
                        leftShoe.transform.position = BottomLeftSpawn.transform.position;
                        ForwardRightCount += 1;
                    }
                    break;
                default: //basepos or if not exist
                    if (riseCase)
                    {
                        shrinkingRight = true;
                        shrinkingLeft = true;
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
        yield return new WaitForSeconds(0.3f);
        leftShoe.gameObject.tag = "Untagged";
        rightShoe.gameObject.tag = "Untagged";
        StartCoroutine("stepChecker");
    }

    private void SetShoeRotation(GameObject shoe, Quaternion targetRotation)
    {
        // Preserve the current x-axis rotation, and update only the y and z axes.
        Vector3 currentRotation = shoe.transform.rotation.eulerAngles;
        shoe.transform.rotation = Quaternion.Euler(currentRotation.x, targetRotation.eulerAngles.y, targetRotation.eulerAngles.z);    
    }
    
    public void shrinkShoe()
    {
        if (shrinkingLeft)
        {
            if (leftShoe.transform.localScale.y > 1f)
            {
                leftShoe.transform.localScale -= (Vector3.one * 3);
            }
            else
            {
                leftShoe.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }

        if (shrinkingRight) {
            if (rightShoe.transform.localScale.y > 1f)
            {
                rightShoe.transform.localScale -= (Vector3.one * 3);
            }
            else
            {
                rightShoe.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        } 
    }

    public void growShoe()
    {   if (!shrinkingLeft) {
            leftShoe.transform.localScale = startSize;
            SetShoeRotation(leftShoe, BottomLeftSpawn.transform.rotation);
        }

        if (!shrinkingRight)
        {
            rightShoe.transform.localScale = startSize;
            SetShoeRotation(rightShoe, BottomRightSpawn.transform.rotation);
        }
    }
}
