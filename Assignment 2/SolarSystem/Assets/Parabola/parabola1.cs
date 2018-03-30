using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parabola1 : MonoBehaviour {

	static private float g = 2.38F;
	private float speedx;
	private float speedy;
	private float x;
	private float y;

	// Use this for initialization
	void Start () {
		speedx = 1.5F;
		speedy = 4;
	}
	
	// Update is called once per frame
	void Update () {
		float newSpeedy = speedy - g * Time.deltaTime;
		x = this.transform.position.x + speedx * Time.deltaTime;
		y = (speedy + newSpeedy) / 2 * Time.deltaTime + this.transform.position.y;
		if (y <= 0) {
			y = -y;
			newSpeedy = -newSpeedy;
		}
		speedy = newSpeedy;
		this.transform.position = new Vector3(
			x, y, 0.0F
		);
	}
}
