using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarSystem : MonoBehaviour {

	public Transform Sun;
	public Transform Mercury;
	public Transform Venus;
	public Transform Earth;
	public Transform Moon;
	public Transform Mars;
	public Transform Jupiter;
	public Transform Saturn;
	public Transform Uranus;
	public Transform Neptune;
	public Transform Pluto;
	private float speed = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Sun.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 30);
		Mercury.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 30);
		Venus.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 80);
		Earth.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 20);
		Moon.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 30);
		Mars.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 20);
		Jupiter.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 10);
		Saturn.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 10);
		Uranus.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 10);
		Neptune.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 10);
		Pluto.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 15);

		Mercury.RotateAround (Sun.transform.position, Vector3.up + 0.1F * Vector3.left, speed * 360 * Time.deltaTime / 87);
		Venus.RotateAround (Sun.transform.position, Vector3.up - 0.05F * Vector3.left, speed * 360 * Time.deltaTime / 224);
		Earth.RotateAround (Sun.transform.position, Vector3.up + 0.13F * Vector3.left, speed * 360 * Time.deltaTime / 365);
		Moon.RotateAround (Earth.transform.position, Vector3.up + 0.2F * Vector3.left, speed * 360 * Time.deltaTime / 30);
		Mars.RotateAround (Sun.transform.position, Vector3.up - 0.18F * Vector3.left, speed * 360 * Time.deltaTime / 687);
		Jupiter.RotateAround (Sun.transform.position, Vector3.up + 0.09F * Vector3.left, speed * 360 * Time.deltaTime / 1000);
		Saturn.RotateAround (Sun.transform.position, Vector3.up - 0.21F * Vector3.left, speed * 360 * Time.deltaTime / 1300);
		Uranus.RotateAround (Sun.transform.position, Vector3.up + 0.1F * Vector3.left, speed * 360 * Time.deltaTime / 1500);
		Neptune.RotateAround (Sun.transform.position, Vector3.up + 0.2F * Vector3.left, speed * 360 * Time.deltaTime / 1800);
		Pluto.RotateAround (Sun.transform.position, Vector3.up + 0.15F * Vector3.left, speed * 360 * Time.deltaTime / 2000);


	}
}
