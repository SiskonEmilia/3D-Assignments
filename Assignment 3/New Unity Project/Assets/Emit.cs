using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emit : SSAction {

	public bool isDone = false;
	Vector3 force; // The direction of force

	public ISceneController scenecontroller = Director.GetInstance ().currentSceneController;

	public override void Start () {
		enable = true;
		// Set the initial position

		force = new Vector3 (2 * Random.Range (-1, 1), Random.Range (0.25f, 0.5f), 4 * (scenecontroller as FirstSceneControl).round);
		// Set the force, the scale depends on current round
	}

	public static Emit GetSSAction() {
		Emit action = ScriptableObject.CreateInstance<Emit> ();
		return action;
	}
	
	// Update is called once per frame
	public override void Update () {
		// There's nothing to update
		if (!this.destoried) {
			if (!isDone) {
				gameobject.AddComponent<Rigidbody> ();
				gameobject.GetComponent<Rigidbody> ().velocity = Vector3.zero;
				gameobject.GetComponent<Rigidbody> ().AddForce (force, ForceMode.Impulse);
				isDone = true;
			}
		}

		if (this.transform.position.y <= -4 || this.transform.position.z >= 15) {
			this.destoried = true;
			this.enable = false;
			this.callback.SSActionEvent (this);
		} // UFO lands
	}

}
