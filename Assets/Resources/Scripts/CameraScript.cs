using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public GameObject target;

	void Start () {
		
	}
	
	void Update () {
        gameObject.transform.position = target.transform.position + Vector3.back * 8;
    }
}
