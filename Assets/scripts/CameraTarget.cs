using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	public Transform target;
	// Update is called once per frame
	void Update () {
		this.transform.LookAt(target);
	}
}
