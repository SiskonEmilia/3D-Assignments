using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Parabola2 : MonoBehaviour {

	private Rigidbody rigidbody;
	private Vector3 initSpeed;

	// Use this for initialization
	void Start () {
		rigidbody = this.GetComponent<Rigidbody> ();
		initSpeed = new Vector3 (3, 10, 0);
		rigidbody.velocity = initSpeed;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
