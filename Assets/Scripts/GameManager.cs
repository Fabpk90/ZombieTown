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
        [Range(1, 20)]
        public float cooldownPossesion;
        [Range(1, 20)]
        public float cooldownYell;
        [Range(1, 20)]
        public float cooldownContamitation;

        [Range(1, 20)]
        public float cooldownRonde;

        [Range(1, 20)]
        public float cooldownRondeRedNeckStreet;
        [Range(1, 20)]
        public float cooldownRondeRedNeckHouse;

        [Range(1, 20)]
        public float cooldownRunningAway;
        [Range(1, 20)]
        public float rangeYell;
        [Range(1, 20)]
        public float rangeBite;

        [Range(1, 20)]
        public float cooldownShot;

        public int scoreContaHuman;
        public int scoreContaHumanArme;

        public int scoreMulti = 1;
        [Range(1, 900)]
        public float timeGame;

        public GameObject biteHUD;
        public GameObject Camera;
        public GameObject ScoreUI;
        public GameObject TimerUI;

        public Avatar ZombieSkeleton;
        public Mesh ZombieMesh;
    }

    public static int Score;

    public static List<Zombie> zombiePossesed;
    public static ConfigClass config;

    public ConfigClass configEditor;

    public GameObject player;

    

    private float timeElapsed = 0f;

    public static bool isDead = false;

    private void Awake()
    {
        zombiePossesed = new List<Zombie>();

        config = configEditor;
    }

    [FMODUnity.EventRef]
    public string rolling = "event:/AMBIANCE/amb_city_zombie";       //Create the eventref and define the event path
    FMOD.Studio.EventInstance scoreEv;                //rolling event
    FMOD.Studio.ParameterInstance ScoreParam;    //speed param object

    FMOD.Studio.EventInstance musicLevel;
    FMOD.Studio.EventInstance musicbgm;

    private bool isMin = false;

    // Use this for initialization
    void Start ()
    {
        scoreEv = FMODUnity.RuntimeManager.CreateInstance(rolling);
        scoreEv.getParameter("score", out ScoreParam);
        scoreEv.start();

        musicLevel = FMODUnity.RuntimeManager.CreateInstance("event:/MUSIC/Level_mus");

       musicbgm = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/sfx_ui_round_1");

        musicLevel.start();
        musicbgm.start();

        if (player != null)
            zombiePossesed.Add(player.GetComponentInChildren<Zombie>());
	}

    private void Update()
    {
        timeElapsed += Time.deltaTime;

        config.ScoreUI.GetComponent<TextMeshProUGUI>().text = "Score: " + Score;

        if(isMin)
            config.TimerUI.GetComponent<TextMeshProUGUI>().text = "" + ((int)timeElapsed) / 60 + ":" + ((int)timeElapsed) % 60;
        else
        {
            config.TimerUI.GetComponent<TextMeshProUGUI>().text = ""+((int)timeElapsed) % 60;
            if (timeElapsed / 60f >= 1)
            {
                isMin = true;
            }
        }

        ScoreParam.setValue(Score / 100f);

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

    static public bool RemoveZombie(Zombie zombie)
    {
        bool ok = false;
        ok = zombiePossesed.Remove(zombie);

        if (zombiePossesed.Count == 0)
        {
            isDead = true;
            SceneManager.LoadScene("DeathScene");
        }
            

        return ok;
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
