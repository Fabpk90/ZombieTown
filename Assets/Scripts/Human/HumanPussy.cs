using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Human
{
   public class HumanPussy : HumanBehaviour
    {
        private Zombie zombieToEscapeFrom;

        //store the time when a zombie is saw
        private float timeLastEscape = 0f;

        public override void InUpdate()
        {
            //if a zombie has been seen, escape!
            if (zombieToEscapeFrom != null)
            {
                if (timeLastEscape + GameManager.config.cooldownRunningAway >= Time.time)
                {
                    Vector3 direction = transform.position - zombieToEscapeFrom.transform.position;
                    transform.parent.GetComponent<NavMeshAgent>().destination += direction.normalized;
                }
                else
                    zombieToEscapeFrom = null;

            } // else do your thing
            else
                base.InUpdate();
        }

        private void OnTriggerEnter(Collider other)
        {
            //print(other.gameObject.GetComponent<Zombie>());
            if (other.gameObject.GetComponent<Zombie>())
            {
                timeLastEscape = Time.time;
                zombieToEscapeFrom = other.GetComponent<Zombie>();
            }
        }
    }
}
