using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanBehaviour : MonoBehaviour {

    public Ronde rondeObj;

	// Use this for initialization
	void Start () {
        Color c = GetComponent<SkinnedMeshRenderer>().material.color;
        c.r = 0;

        GetComponent<SkinnedMeshRenderer>().material.color = c;
    }

    virtual public void InUpdate()
    {
        if (!transform.parent.GetComponent<NavMeshAgent>().pathPending) //used for knowing if it has arrived at his destination
        {
            if (transform.parent.GetComponent<NavMeshAgent>().remainingDistance <= transform.parent.GetComponent<NavMeshAgent>().stoppingDistance)
            {
                if (!transform.parent.GetComponent<NavMeshAgent>().hasPath || transform.parent.GetComponent<NavMeshAgent>().velocity.sqrMagnitude == 0f)
                {
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
    }

    private void Update()
    {
        InUpdate();
    }

    public void Contaminate()
    {
        GameManager.Score += GameManager.config.scoreContaHuman * GameManager.config.scoreMulti;

        gameObject.AddComponent<PlayerInput>().enabled = false;

        gameObject.AddComponent<Zombie>();
        gameObject.GetComponent<Zombie>().rondeObj = rondeObj;

        Destroy(gameObject.GetComponent<HumanBehaviour>());
    }

}
