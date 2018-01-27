using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFMOD : MonoBehaviour {

	// Use this for initialization
	void Start () {
        FMODUnity.RuntimeManager.PlayOneShot("event:/SFX/sfx_zombie_call");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
