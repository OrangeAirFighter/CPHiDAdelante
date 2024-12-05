using System.Xml.Linq;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [HideInInspector]
    public float songLength; 

    private bool musicSelected = false;
    private AudioSource[] audios;
    private AudioClip[] selectedMusicSounds;
    public AudioClip[] twinkleSounds;

    private UIManager UIManager;

    private void Start()
    {
        audios = GetComponentsInChildren<AudioSource>();
        UIManager = GameObject.Find("UIManager").GetComponent<UIManager>();
    }

    void Update()
    {
        if (musicSelected)
        {
            audios[UIManager.piano].resource = selectedMusicSounds[UIManager.piano];
            audios[UIManager.piano].volume = 1;
            audios[5 + UIManager.drums].resource = selectedMusicSounds[5 + UIManager.drums];
            audios[5 + UIManager.drums].volume = 1;
            /*audios[10 +UIManager.bell].resource = selectedMusicSounds[10 + UIManager.bell];
            audios[10 + UIManager.bell].volume = 1;
            audios[15 + UIManager.orchestral].resource = selectedMusicSounds[15 + UIManager.orchestral];
            audios[15 + UIManager.orchestral].volume = 1;*/
        }
    }

    public void selectSong(string song)
    {
        switch (song)
        {
            case "Twinkle":
                selectedMusicSounds = twinkleSounds;
                break;
            case "other":
                break;
            default:
                break;
        }
    }

    public void playMusic()
    {
        UIManager.gameLength = selectedMusicSounds[0].length;

        for (int i = 0; i<audios.Length; i++)
        {
            audios[i].resource = selectedMusicSounds[i];
            audios[i].Play();
        }
        musicSelected = true;
    }
}
