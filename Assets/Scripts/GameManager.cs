using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public float rangeYell;
        public float rangeBite;

        public GameObject biteHUD;
    }

    public static List<Zombie> zombiePossesed;
    public static ConfigClass config;

    public ConfigClass configEditor;

    public Zombie player;

    private void Awake()
    {
        zombiePossesed = new List<Zombie>();

        config = configEditor;
    }

    // Use this for initialization
    void Start ()
    {
        zombiePossesed.Add(player);
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
