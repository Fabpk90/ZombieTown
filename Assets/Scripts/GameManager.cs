using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static List<Zombie> zombiePossesed;

    [Range(0f, 5f)]
    public float cooldownPossesion;
    static public float cooldownPossesionCode;

    public Zombie player;

    private void Awake()
    {
        zombiePossesed = new List<Zombie>();
        cooldownPossesionCode = cooldownPossesion;
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
