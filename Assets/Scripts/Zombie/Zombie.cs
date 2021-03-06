﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerInput))]
public class Zombie : MonoBehaviour {
    public Ronde rondeObj;

    private bool isWalking;

    private FMOD.Studio.EventInstance idleSFX;

    // Use this for initialization
    void Start () {
        if (transform.tag != "Player")
            GameManager.AddZombie(this);
        else
            TakePossesion();

        idleSFX = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/sfx_idle_zombie");
    }
	
	// Update is called once per frame
	void Update () {

        isWalking = true;
        

        if (!transform.parent.GetComponent<NavMeshAgent>().pathPending) //used for knowing if it has arrived at his destination
        {
            if (transform.parent.GetComponent<NavMeshAgent>().remainingDistance <= transform.parent.GetComponent<NavMeshAgent>().stoppingDistance)
            {
                if (!transform.parent.GetComponent<NavMeshAgent>().hasPath || transform.parent.GetComponent<NavMeshAgent>().velocity.sqrMagnitude == 0f)
                {
                    isWalking = false;
                    rondeObj.timeSinceLastRonde += Time.deltaTime;

                    if (rondeObj.timeSinceLastRonde >= GameManager.config.cooldownRonde)
                    {
                        rondeObj.timeSinceLastRonde = 0f;
                        Vector3 v = rondeObj.nextPointRonde();
                        if (v != Vector3.zero)
                            transform.parent.GetComponent<NavMeshAgent>().destination = v;
                    }
                }
            }
        }

        if (!isWalking)
            idleSFX.start();
        else
            idleSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        GetComponentInParent<Animator>().SetBool("isWalking", isWalking);

    }

    public void TakePossesion()
    {
        GetComponent<PlayerInput>().lastTimePossesed = Time.time;
        GetComponent<PlayerInput>().enabled = true;

        GameManager.DeactivateBiteHUD();

        gameObject.AddComponent<SphereCollider>().radius = GameManager.config.rangeBite;
        GetComponent<SphereCollider>().isTrigger = true;

       // GetComponent<PlayerInput>().SearchForHumanInRange
    }

    public void LeavePossesion()
    {
        GetComponent<PlayerInput>().enabled = false;
        GetComponent<PlayerInput>().humanInRangeBite = new List<HumanBehaviour>();

        foreach(SphereCollider sp in GetComponents<SphereCollider>())
        {
            if(sp.isTrigger)
                DestroyObject(sp);
        }

    }
}
