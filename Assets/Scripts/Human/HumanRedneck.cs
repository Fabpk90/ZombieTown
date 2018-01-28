using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Human
{
    class HumanRedneck : HumanBehaviour
    {
        public bool isHouseGarding;

        private void OnTriggerEnter(Collider other)
        {
            //print(other.gameObject.GetComponent<Zombie>());
            if (!other.isTrigger && other.gameObject.GetComponent<Zombie>())
            {
                transform.LookAt(other.transform);
                GetComponentInParent<Animator>().SetBool("isShooting", true);

                StartCoroutine(disableShoot(1.2f, GetComponentInParent<Animator>()));
                if(other.gameObject.GetComponent<PlayerInput>())
                {
                    other.gameObject.GetComponent<PlayerInput>().FindNearestZombie();
                }
                GameManager.RemoveZombie(other.gameObject.GetComponent<Zombie>());
                Destroy(other.transform.parent.gameObject);
            }
        }

        //sorry for what you are going to read, no time no good
        public override void InUpdate()
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

                        if (rondeObj.timeSinceLastRonde >= ( 
                            isHouseGarding ? GameManager.config.cooldownRondeRedNeckHouse 
                            : GameManager.config.cooldownRondeRedNeckStreet ))
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

        System.Collections.IEnumerator disableShoot(float waitTime, Animator anim)
        {
            bool ok = false;
             while(!ok)
            {
                yield return new WaitForSeconds(waitTime);
                anim.SetBool("isShooting", false);
                ok = true;

            }

            yield return null;
        }
    }
}

