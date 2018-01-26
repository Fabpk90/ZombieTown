using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInput : MonoBehaviour {

    private Vector3 movementVec;

    public float lastTimePossesed = 0f;
    public float lastTimeYell = 0f;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        movementVec.x = Input.GetAxis("Horizontal");
        movementVec.z = Input.GetAxis("Vertical");

        transform.position += movementVec;

        if(Time.time >= (lastTimePossesed + GameManager.config.cooldownPossesion) && Input.GetAxis("ChangeZombie") > 0)
        {
            FindNearestZombie();
        }
        else if(Time.time >= (lastTimeYell + GameManager.config.cooldownYell) && Input.GetButtonDown("Submit"))
        {
            Yell();
        }
	}

    private void Yell()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, GameManager.config.rangeYell, transform.position);
        Zombie z = null;
        foreach (RaycastHit ray in hits)
        {
            z = ray.collider.GetComponent<Zombie>();
            if (z)
            {
                //not calling himself
                if(z != GetComponent<Zombie>())
                {
                    z.GetComponent<NavMeshAgent>().destination = transform.position;
                }
                
            }
        }
    }

    private void FindNearestZombie()
    {
        //if there is more zombie than the player
        if(GameManager.zombiePossesed.Count > 1)
        {
            float nearestDistance = 10000; //TODO: lol, change this mess
            Zombie nearestZombie = null;

            float distance;

            foreach(Zombie z in GameManager.zombiePossesed)
            {
                //not comparing this by himselft lolilol
                if(z != GetComponent<Zombie>())
                {
                    print(z.transform.name);
                    distance = Vector3.Distance(transform.position, z.transform.position);
                    print("distance " + distance);
                    if (nearestDistance > distance)
                    {
                        nearestDistance = distance;
                        nearestZombie = z;
                    }
                }
               
            }

            nearestZombie.TakePossesion();
            GetComponent<Zombie>().LeavePossesion();
        }
    }
}
