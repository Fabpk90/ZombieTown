using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Human : MonoBehaviour {

    public Ronde rondeObj;

    private Zombie zombieToEscapeFrom;

    //store the time when a zombie is saw
    private float timeLastEscape = 0f;

	// Use this for initialization
	void Start () {
        Color c = GetComponent<MeshRenderer>().material.color;
        c.r = 0;

        GetComponent<MeshRenderer>().material.color = c;
    }

    private void Update()
    {
        //if a zombie has been seen, escape!
        if (zombieToEscapeFrom != null)
        {
            if (timeLastEscape + GameManager.config.cooldownRunningAway >= Time.time)
            {
                Vector3 direction = transform.position - zombieToEscapeFrom.transform.position;
                GetComponent<NavMeshAgent>().destination += direction.normalized;
            }
            else
                zombieToEscapeFrom = null;

        } // else do your thing
        else if (!GetComponent<NavMeshAgent>().pathPending) //used for knowing if it has arrived at his destination
        {
            if (GetComponent<NavMeshAgent>().remainingDistance <= GetComponent<NavMeshAgent>().stoppingDistance)
            {
                if (!GetComponent<NavMeshAgent>().hasPath || GetComponent<NavMeshAgent>().velocity.sqrMagnitude == 0f)
                {
                    rondeObj.timeSinceLastRonde += Time.deltaTime;

                    if (rondeObj.timeSinceLastRonde >= GameManager.config.cooldownRonde)
                    {
                        rondeObj.timeSinceLastRonde = 0f;
                        Vector3 v = rondeObj.nextPointRonde();
                        if (v != Vector3.zero)
                            GetComponent<NavMeshAgent>().destination = v;
                    }
                }
            }
        }

        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Zombie>())
        {
            print(other.GetComponent<Zombie>().transform.name);
            timeLastEscape = Time.time;
            zombieToEscapeFrom = other.GetComponent<Zombie>();
        }
    }



    public void Contaminate()
    {
        GameManager.Score += GameManager.config.scoreContaHuman * GameManager.config.scoreMulti;

        gameObject.AddComponent<PlayerInput>().enabled = false;

        gameObject.AddComponent<Zombie>();
        gameObject.GetComponent<Zombie>().rondeObj = rondeObj;

        Destroy(gameObject.GetComponent<Human>());
    }
}
