using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerInput))]
public class Zombie : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (transform.tag != "Player")
            GameManager.AddZombie(this);
        else
            TakePossesion();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakePossesion()
    {
        GetComponent<PlayerInput>().lastTimePossesed = Time.time;
        GetComponent<NavMeshAgent>().enabled = false;
        GetComponent<PlayerInput>().enabled = true;

        GameManager.DeactivateBiteHUD();

        gameObject.AddComponent<SphereCollider>().radius = GameManager.config.rangeBite;
        GetComponent<SphereCollider>().isTrigger = true;

        

       // GetComponent<PlayerInput>().SearchForHumanInRange();


        Color c = GetComponent<MeshRenderer>().material.color;
        c.g = 0;
        c.b = 0;

        GetComponent<MeshRenderer>().material.color = c;
    }

    public void LeavePossesion()
    {

        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<PlayerInput>().enabled = false;
        GetComponent<PlayerInput>().humanInRangeBite = new List<Human>();

        DestroyObject(gameObject.GetComponent<SphereCollider>());

        Color c = GetComponent<MeshRenderer>().material.color;
        c.g = 1;
        c.b = 1;

        GetComponent<MeshRenderer>().material.color = c;
    }
}
