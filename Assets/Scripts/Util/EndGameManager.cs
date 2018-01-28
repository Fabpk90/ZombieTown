using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour {

    public TextMeshProUGUI ScoreUI;

    FMOD.Studio.EventInstance musicLevel;
    FMOD.Studio.EventInstance musicbgm;
    // Use this for initialization
    void Start () {
        ScoreUI.text = "Score: " + GameManager.Score;

        if (GameManager.isDead)
        {
            musicLevel = FMODUnity.RuntimeManager.CreateInstance("event:/MUSIC/Jingle_loose");
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_youloose");
        }    
        else
        {
            musicLevel = FMODUnity.RuntimeManager.CreateInstance("event:/MUSIC/Jingle_win");
            FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_youwin00");
        }
           

        musicbgm = FMODUnity.RuntimeManager.CreateInstance("event:/AMBIANCE/amb_zombie");

        musicLevel.start();
        musicbgm.start();
    }
	

    public void ResetGame()
    {
        GameManager.isDead = false;
        GameManager.Score = 0;

        SceneManager.LoadScene(0);
    }
}
