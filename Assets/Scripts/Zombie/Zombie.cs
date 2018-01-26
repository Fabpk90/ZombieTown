using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class Zombie : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (transform.tag != "Player")
            GameManager.AddZombie(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakePossesion()
    {
        GetComponent<PlayerInput>().lastTimePossesed = Time.time;
        GetComponent<PlayerInput>().enabled = true;
    }
}
