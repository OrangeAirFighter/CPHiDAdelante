using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class destroyBug : MonoBehaviour
{
    public GameObject particleExplosion;
    public Sprite[] bugSplashUIList;

    public float movementSpeed = 10; 
    public Vector3 enemyRotation;

    public float waveAmplitude = 1f; // The height of the wave
    public float waveFrequency = 25f; // The frequency of the wave

    public GameObject bugSplashUI;

    private float timeElapsed;
    private void Start()
    {
        bugSplashUI.GetComponentInChildren<Image>().sprite = bugSplashUIList[Random.Range(0, bugSplashUIList.Length)];
        bugSplashUI.GetComponentInChildren<Image>().color = Color.black;
        enemyRotation = new Vector3(0, Random.Range(65, 115), 0);
        
        transform.Rotate(enemyRotation);
        
        Invoke("destroyer", 20f);
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        // Calculate forward movement
        Vector3 forwardMovement = transform.forward * Time.deltaTime * movementSpeed;

        // Calculate wave movement
        Vector3 waveMovement = transform.right * Mathf.Sin(timeElapsed * waveFrequency) * waveAmplitude * Time.deltaTime;

        // Combine movements
        transform.position += forwardMovement + waveMovement;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            UIManager.gameScore += 1;
            GameObject bugExplosion = Instantiate(particleExplosion, transform.position, particleExplosion.transform.rotation);
            GameObject bugSplash = Instantiate(bugSplashUI, transform.position, particleExplosion.transform.rotation);
            Destroy(gameObject);
        }
    }

    public void destroyer()
    {
    Destroy(gameObject); 
    }
}
