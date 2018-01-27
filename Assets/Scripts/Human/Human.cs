using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Color c = GetComponent<MeshRenderer>().material.color;
        c.r = 0;

        GetComponent<MeshRenderer>().material.color = c;
    }
	
    public void Contaminate()
    {
        gameObject.AddComponent<PlayerInput>().enabled = false;
        gameObject.AddComponent<Zombie>();
        Destroy(gameObject.GetComponent<Human>());
    }
}
