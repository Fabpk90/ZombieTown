using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Cameras;
using UnityStandardAssets.Utility;

public class PlayerInput : MonoBehaviour {

    private Vector3 movementVec;

    public float lastTimePossesed = 0f;
    public float lastTimeYell = 0f;
    public float lastTimeContaminated = 0f;

    public List<HumanBehaviour> humanInRangeBite = new List<HumanBehaviour>();

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        movementVec.x = Input.GetAxis("Horizontal");
        movementVec.z = Input.GetAxis("Vertical");

        transform.parent.GetComponent<NavMeshAgent>().destination = transform.position + (movementVec * 2f);

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

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
	}

    private void Yell()
    {

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_house_contamination");

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
                    z.transform.parent.GetComponent<NavMeshAgent>().destination = transform.position;
                }
                
            }
        }
    }

    public void FindNearestZombie()
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

            GameManager.config.Camera.GetComponent<FollowTarget>().target = (nearestZombie.transform);

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
            HumanBehaviour hToContaminate = null;

            float distance;
            foreach(HumanBehaviour h in humanInRangeBite)
            {
                if(h)
                {
                    distance = Vector3.Distance(transform.position, h.transform.position);
                    if (nearestDistance > distance)
                    {
                        nearestDistance = distance;
                        hToContaminate = h;
                    }
                }
               
            }

            hToContaminate.Contaminate();
            humanInRangeBite.Remove(hToContaminate);

            if (humanInRangeBite.Count == 0)
                GameManager.DeactivateBiteHUD();

            
        }
    }

    /// <summary>
    /// Used when a human is contaminated, to see who he can contaminated right away
    /// </summary>
    public void SearchForHumanInRange()
    {
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, GameManager.config.rangeBite, transform.position);
        HumanBehaviour h = null;
        foreach (RaycastHit ray in hits)
        {
            h = ray.collider.GetComponent<HumanBehaviour>();
            if (h)
            {
                humanInRangeBite.Add(h);
                GameManager.ActivateBiteHUD();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
      //  print(other.gameObject);

        if (other.GetComponent<HumanBehaviour>())
        {
            GameManager.ActivateBiteHUD();
            humanInRangeBite.Add(other.GetComponent<HumanBehaviour>());
        }
        else if (humanInRangeBite.Count == 0)
            GameManager.DeactivateBiteHUD();
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<HumanBehaviour>())
        {
            humanInRangeBite.Remove(other.GetComponent<HumanBehaviour>());

            if (humanInRangeBite.Count == 0)
                GameManager.DeactivateBiteHUD();
        }
    }




}
