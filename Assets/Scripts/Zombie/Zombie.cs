using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(PlayerInput))]
public class Zombie : MonoBehaviour {
    public Ronde rondeObj;

    private bool isWalking;

    // Use this for initialization
    void Start () {
        if (transform.tag != "Player")
            GameManager.AddZombie(this);
        else
            TakePossesion();
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
        GetComponentInParent<Animator>().SetBool("isWalking", isWalking);

    }

    public void TakePossesion()
    {
        GetComponent<PlayerInput>().lastTimePossesed = Time.time;
        GetComponent<PlayerInput>().enabled = true;

        GameManager.DeactivateBiteHUD();

        gameObject.AddComponent<SphereCollider>().radius = GameManager.config.rangeBite;
        GetComponent<SphereCollider>().isTrigger = true;

       // GetComponent<PlayerInput>().SearchForHumanInRange();

        Color c = GetComponent<SkinnedMeshRenderer>().material.color;
        c.g = 0;
        c.b = 0;

        GetComponent<SkinnedMeshRenderer>().material.color = c;
    }

    public void LeavePossesion()
    {
        GetComponent<PlayerInput>().enabled = false;
        GetComponent<PlayerInput>().humanInRangeBite = new List<HumanBehaviour>();

        DestroyObject(gameObject.GetComponent<SphereCollider>());

        Color c = GetComponent<SkinnedMeshRenderer>().material.color;
        c.g = 1;
        c.b = 1;

        GetComponent<SkinnedMeshRenderer>().material.color = c;
    }
}
