using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInput : MonoBehaviour {

    private Vector3 movementVec;

    public float lastTimePossesed = 0f;
    public float lastTimeYell = 0f;
    public float lastTimeContaminated = 0f;

    public List<Human> humanInRangeBite = new List<Human>();

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
        else if(Time.time >= (lastTimeContaminated + GameManager.config.cooldownContamitation) && Input.GetButtonDown("Contaminate"))
        {
            Contaminate();
        }
	}

    private void Yell()
    {
        lastTimeYell = Time.time;

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
        lastTimePossesed = Time.time;

        //if there is more zombie than the player
        if(GameManager.zombiePossesed.Count > 1)
        {
            float nearestDistance = 10000f; //TODO: lol, change this mess
            Zombie nearestZombie = null;

            float distance;

            foreach(Zombie z in GameManager.zombiePossesed)
            {
                //not comparing this by himself lolilol
                if(z != GetComponent<Zombie>())
                {

                    distance = Vector3.Distance(transform.position, z.transform.position);

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

    /// <summary>
    /// Contaminate the nearest Human in range
    /// </summary>
    private void Contaminate()
    {
        lastTimeContaminated = Time.time;

        if(humanInRangeBite.Count >= 1)
        {
            float nearestDistance = 10000f;
            Human hToContaminate = null;

            float distance;
            foreach(Human h in humanInRangeBite)
            {
                distance = Vector3.Distance(transform.position, h.transform.position);
                if (nearestDistance > distance)
                {
                    nearestDistance = distance;
                    hToContaminate = h;
                }
            }

            humanInRangeBite.Remove(hToContaminate);

            if (humanInRangeBite.Count == 0)
                GameManager.DeactivateBiteHUD();

            hToContaminate.Contaminate();  
        }
    }

    /// <summary>
    /// Used when a human is contaminated, to see who he can contaminated right away
    /// </summary>
    public void SearchForHumanInRange()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, GameManager.config.rangeBite, transform.position);
        Human h = null;
        foreach (RaycastHit ray in hits)
        {
            h = ray.collider.GetComponent<Human>();
            if (h)
            {
                humanInRangeBite.Add(h);
                GameManager.ActivateBiteHUD();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Human>())
        {
            GameManager.ActivateBiteHUD();
            humanInRangeBite.Add(other.GetComponent<Human>());
        }
        else if (humanInRangeBite.Count == 0)
            GameManager.DeactivateBiteHUD();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<Human>())
        {
            humanInRangeBite.Remove(other.GetComponent<Human>());

            if (humanInRangeBite.Count == 0)
                GameManager.DeactivateBiteHUD();
        }
    }




}
