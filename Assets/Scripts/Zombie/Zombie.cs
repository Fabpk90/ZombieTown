using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerInput))]
public class Zombie : MonoBehaviour {
    public Ronde rondeObj;

    // Use this for initialization
    void Start () {
        if (transform.tag != "Player")
            GameManager.AddZombie(this);
        else
            TakePossesion();
	}
	
	// Update is called once per frame
	void Update () {
        if(!GetComponent<PlayerInput>().enabled)
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
        
    }

    public void TakePossesion()
    {
        GetComponent<PlayerInput>().lastTimePossesed = Time.time;
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
        GetComponent<PlayerInput>().enabled = false;
        GetComponent<PlayerInput>().humanInRangeBite = new List<Human>();

        DestroyObject(gameObject.GetComponent<SphereCollider>());

        Color c = GetComponent<MeshRenderer>().material.color;
        c.g = 1;
        c.b = 1;

        GetComponent<MeshRenderer>().material.color = c;
    }
}
