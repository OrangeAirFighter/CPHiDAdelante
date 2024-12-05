using UnityEngine;

public class MusicSpawner : MonoBehaviour
{
    public GameObject[] spawnPoints;
    public GameObject[] musicNotes;

    private void Start()
    {
        Invoke("Music", 3f);
    }

    public void Music()
    {
        if (UIManager.piano >= 4 && UIManager.drums >= 4)
        {
            Instantiate(musicNotes[Random.Range(0, musicNotes.Length)], spawnPoints[Random.Range(0, 2)].transform.position, spawnPoints[0].transform.rotation);
        }
        else if (UIManager.drums >= 4)
        {
            Instantiate(musicNotes[Random.Range(0, musicNotes.Length-5)], spawnPoints[Random.Range(0, 2)].transform.position, spawnPoints[0].transform.rotation);
        }
        else if (UIManager.piano >= 4)
        {
            Instantiate(musicNotes[Random.Range(0+5, musicNotes.Length)], spawnPoints[Random.Range(0, 2)].transform.position, spawnPoints[0].transform.rotation);
        }
        else
        {
            Instantiate(musicNotes[Random.Range(0, musicNotes.Length)], spawnPoints[Random.Range(0, 2)].transform.position, spawnPoints[0].transform.rotation);
        }
            
        
        Invoke("Music", Random.Range(2,4));
    }
}
