using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Parabola3 : MonoBehaviour {

	private CharacterController charactercontroller;
	private Vector3 speed;

	// Use this for initialization
	void Start () {
		charactercontroller = this.GetComponent<CharacterController> ();
		speed = new Vector3 (2, 5, 0);
	}
	
	// Update is called once per frame
	void Update () {
		speed -= 3.0F * Time.deltaTime * Vector3.up;
		charactercontroller.Move (speed * Time.deltaTime);
	}
}
