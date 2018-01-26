using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [System.Serializable]
    public class ConfigClass
    {
        public float cooldownPossesion;
        public float cooldownYell;

        public float rangeYell;
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
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    static public void AddZombie(Zombie zombie)
    {
        zombiePossesed.Add(zombie);
    }
}
