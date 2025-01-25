using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class UIManager : MonoBehaviour
{
    [Header("In Game UI")]
    [HideInInspector]
    public static int gameScore = 0;
    [HideInInspector]
    public string currentScene;

    public GameObject scoreUI;
    public GameObject timeUI;
    [HideInInspector]
    public float gameLength;
    private float currentTime = 0f;

    [Header("Completion Screen")]
    public GameObject completeScreen;
    public GameObject shoeManager;
    public GameObject finalScore;
    public GameObject stepleftScore;
    public GameObject steprightScore;
    public GameObject forwardleftScore;
    public GameObject forwardrightScore;
    public GameObject turnleftScore;
    public GameObject turnrightScore;
    public GameObject balanceScore;

    [Header("Music Variables")]
    public GameObject audioSource;
    [HideInInspector]
    public static int OrangeMusicScore = 0;
    [HideInInspector]
    public static int piano = 0;
    [HideInInspector]
    public static int drums = 0;
    [HideInInspector]
    public static int orchestral = 0;
    [HideInInspector]
    public static int bell = 0;


    private string gameDifficulty = "Stride";
    private bool menuDown = false;
    public bool inGame = true;

    private void Start()
    {
        if (inGame)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
       
        currentScene = SceneManager.GetActiveScene().name;

        switch (currentScene)
        {
            case "Squash":
                gameLength = 60f;
                break;
            case "Music":
                gameLength = 20;
                //GameLength gets set in MusicManager here
                break;
            default:
                break;
        }
            
    }

    void Update()
    {
        //calculate balance based on optimal percentage on area's 
        if (inGame)
        {
            if ((gameLength >= 0) && (currentTime < gameLength))
            {  
                switch (currentScene)
                {
                    case "Squash":
                        gameLength -= Time.deltaTime;
                        timeUI.GetComponent<TextMeshProUGUI>().SetText(Mathf.Round(gameLength).ToString());
                        scoreUI.GetComponent<TextMeshProUGUI>().SetText(gameScore.ToString());
                        break;
                    case "Music":
                        currentTime += Time.deltaTime;
                        timeUI.GetComponent<Slider>().value = currentTime/gameLength; //here change the movement of the slider  

                        if (piano < 5)
                        {
                            scoreUI.transform.GetChild(piano).gameObject.SetActive(true); //here change the stars 
                        }
                        else
                        {
                            piano = 5;
                        }

                        ///
                        if (drums < 5)
                        {
                            scoreUI.transform.GetChild(5 + drums).gameObject.SetActive(true); //here change the stars 
                        }
                        else
                        {
                            drums = 5;
                        }

                        /*For potential more instruments
                        if (bell < 5)
                        {
                            scoreUI.transform.GetChild(10 + bell).gameObject.SetActive(true); //here change the stars 
                        }
                        else
                        {
                            bell = 5;
                        }

                        ///
                        if (orchestral < 5)
                        {
                            scoreUI.transform.GetChild(15 + orchestral).gameObject.SetActive(true); //here change the stars 
                        }
                        else
                        {
                            orchestral = 5;
                        }
                        */
                        break;
                    default:
                        break;
                }
            }
            else
            {
                audioSource.SetActive(true);
                completeScores();
                completeScreen.SetActive(true);
                StartCoroutine(menuAnim());
            }
        }     
    }

    public void restart()
    {
        gameScore = 0;
        piano = 0;
        drums = 0;
        //bell = 0;
        //orchestral = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void startScene(string sceneName)
    {
        gameScore = 0;
        piano = 0;
        drums = 0;
        //bell = 0;
        //eorchestral = 0;
        SceneManager.LoadScene(sceneName);
    }

    public void quitGame()
    {
        Application.Quit();
    }

    public void pauser()
    {
        if (Time.timeScale == 1f)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void menuManaged(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
    }

    public void setDifficulty(string difficulty)
    {
        gameDifficulty = difficulty;
    }

    public void completeScores()
    {  
        stepleftScore.GetComponent<TextMeshProUGUI>().SetText(shoeManager.GetComponent<ShoeManager>().StepLeftCount.ToString());
        steprightScore.GetComponent<TextMeshProUGUI>().SetText(shoeManager.GetComponent<ShoeManager>().StepRightCount.ToString());
        forwardleftScore.GetComponent<TextMeshProUGUI>().SetText(shoeManager.GetComponent<ShoeManager>().ForwardLeftCount.ToString());
        forwardrightScore.GetComponent<TextMeshProUGUI>().SetText(shoeManager.GetComponent<ShoeManager>().ForwardRightCount.ToString());
        turnleftScore.GetComponent<TextMeshProUGUI>().SetText(shoeManager.GetComponent<ShoeManager>().TurnLeftCount.ToString());
        turnrightScore.GetComponent<TextMeshProUGUI>().SetText(shoeManager.GetComponent<ShoeManager>().TurnRightCount.ToString());
        finalScore.GetComponent<TextMeshProUGUI>().SetText(gameScore.ToString());

        switch (gameDifficulty)
        {
            case "Rise":
                stepleftScore.transform.parent.gameObject.SetActive(true);
                steprightScore.transform.parent.gameObject.SetActive(true);
                break;
            case "Stride":
                forwardleftScore.transform.parent.gameObject.SetActive(true);
                forwardrightScore.transform.parent.gameObject.SetActive(true);
                break;
            case "Twist":
                turnleftScore.transform.parent.gameObject.SetActive(true);
                turnrightScore.transform.parent.gameObject.SetActive(true);
                break;
        }
        
    }

    IEnumerator menuAnim()
    {
        if (!menuDown)
        {
            completeScreen.GetComponent<Animator>().Play("ScreenDown");
            menuDown = true;
        }
        yield return new WaitForSeconds(1.5f);
        Time.timeScale = 0;
    }
}
