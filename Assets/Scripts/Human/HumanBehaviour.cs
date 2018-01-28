using Assets.Scripts.Human;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanBehaviour : MonoBehaviour {

    public Ronde rondeObj;

    protected bool isWalking;

    private FMOD.Studio.EventInstance idleSFX;

    // Use this for initialization
    void Start () {
        Color c = GetComponent<SkinnedMeshRenderer>().material.color;
        c.r = 0;

        GetComponent<SkinnedMeshRenderer>().material.color = c;

        idleSFX = FMODUnity.RuntimeManager.CreateInstance("event:/SFX/sfx_idle_human");
    }

    virtual public void InUpdate()
    {
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

    private void Update()
    {
        InUpdate();
    }

    virtual public void Contaminate()
    {
        GameManager.Score += GetComponent<HumanPussy>() != null ? GameManager.config.scoreContaHuman :
            GameManager.config.scoreContaHumanArme
            * GameManager.config.scoreMulti;

        gameObject.AddComponent<PlayerInput>().enabled = false;

        gameObject.AddComponent<Zombie>();
        gameObject.GetComponent<Zombie>().rondeObj = rondeObj;

        //gameObject.GetComponentInParent<Animator>().avatar = GameManager.config.ZombieSkeleton;
        gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh = (Mesh) GameManager.config.ZombieMesh;

        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_contamination");
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_contamination_ui");

        //destroying sight
        foreach (CapsuleCollider sp in gameObject.GetComponents<CapsuleCollider>())
        {
            print(sp.isTrigger);
            if (sp.isTrigger)
                DestroyObject(sp);
        }
        if (GetComponent<HumanPussy>())
            Destroy(gameObject.GetComponent<HumanPussy>());
        else
            Destroy(gameObject.GetComponent<HumanRedneck>());
    }

}
