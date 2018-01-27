using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Human : MonoBehaviour {

    public Ronde rondeObj;

	// Use this for initialization
	void Start () {
        Color c = GetComponent<MeshRenderer>().material.color;
        c.r = 0;

        GetComponent<MeshRenderer>().material.color = c;
    }

    private void Update()
    {
        if (!GetComponent<NavMeshAgent>().pathPending)
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

    public void Contaminate()
    {
        GameManager.Score += GameManager.config.scoreContaHuman * GameManager.config.scoreMulti;

        gameObject.AddComponent<PlayerInput>().enabled = false;

        gameObject.AddComponent<Zombie>();
        gameObject.GetComponent<Zombie>().rondeObj = rondeObj;

        Destroy(gameObject.GetComponent<Human>());
    }
}
