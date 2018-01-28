using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour {

    public TextMeshProUGUI ScoreUI;
    // Use this for initialization
    void Start () {
        ScoreUI.text = "Score: " + GameManager.Score;
    }
	

    public void ResetGame()
    {
        GameManager.isDead = false;
        GameManager.Score = 0;

        SceneManager.LoadScene(0);
    }
}
