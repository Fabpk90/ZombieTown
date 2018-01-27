using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Ronde
{
    public GameObject[] ronde;

    public int indexRonde = 0;
    public float timeSinceLastRonde = 0f;

    public Vector3 nextPointRonde()
    {
        if (ronde.Length > 0)
        {
            indexRonde = (indexRonde + 1) % ronde.Length;
           return ronde[indexRonde].transform.position;
        }

        return Vector3.zero;

    }
}

public class GameManager : MonoBehaviour {

    [System.Serializable]
    public class ConfigClass
    {
        public float cooldownPossesion;
        public float cooldownYell;
        public float cooldownContamitation;

        public float cooldownRonde;

        public float cooldownRunningAway;

        public float rangeYell;
        public float rangeBite;

        public int scoreContaHuman;
        public int scoreContaHumanArme;

        public int scoreMulti = 1;

        public float timeGame;

        public GameObject biteHUD;
        public GameObject Camera;
    }

    public static int Score;

    public static List<Zombie> zombiePossesed;
    public static ConfigClass config;

    public ConfigClass configEditor;

    public Zombie player;

    public TextMeshProUGUI ScoreUI;

    private float timeElapsed = 0f;

    public static bool isDead = false;

    private void Awake()
    {
        zombiePossesed = new List<Zombie>();

        config = configEditor;
    }

    // Use this for initialization
    void Start ()
    {
        if (player != null)
            zombiePossesed.Add(player);
	}

    private void Update()
    {
        ScoreUI.text = "Score: " + Score;

        timeElapsed += Time.deltaTime;

        //time out
        if (timeElapsed >= config.timeGame && !isDead)
        {
            isDead = true;
            SceneManager.LoadScene("DeathScene");
        }
            
    }

    static public void AddZombie(Zombie zombie)
    {
        zombiePossesed.Add(zombie);
    }

    static public void ActivateBiteHUD()
    {
        config.biteHUD.SetActive(true);
    }

    static public void DeactivateBiteHUD()
    {
        config.biteHUD.SetActive(false);
    }
}
