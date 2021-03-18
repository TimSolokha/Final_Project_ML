using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chests : MonoBehaviour {
    public GameObject target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frameS
    void OnTriggerEnter(Collider other)
    {
        Destroy(target);

    }
}
