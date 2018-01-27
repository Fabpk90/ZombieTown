using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Human
{
    class HumanRedneck : HumanBehaviour
    {


        private void OnTriggerEnter(Collider other)
        {
            //print(other.gameObject.GetComponent<Zombie>());
            if (other.gameObject.GetComponent<Zombie>())
            {
                transform.LookAt(other.transform);
                print("pan!");
            }
        }
    }
}

