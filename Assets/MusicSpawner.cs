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
        Instantiate(musicNotes[Random.Range(0, musicNotes.Length)], spawnPoints[Random.Range(0,2)].transform.position, spawnPoints[0].transform.rotation);
        Invoke("Music", Random.Range(3,5));
    }
}
