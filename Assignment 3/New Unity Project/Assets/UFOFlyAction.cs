using UnityEngine;
using System.Collections;

public class UFOFlyAction : SSAction
{
	public Vector3 speed;
	public static readonly float gravity = 9.8F;

	public static UFOFlyAction GetSSAction () {
		UFOFlyAction action = ScriptableObject.CreateInstance<UFOFlyAction> ();
		return action;
	}

	public override void Start ()
	{ 
		enable = true;
		speed = gameobject.GetComponent<UFOData> ().speed * gameobject.GetComponent<UFOData> ().direction;
		if (gameobject.GetComponent<Rigidbody> () != null)
			Destroy (gameobject.GetComponent<Rigidbody> ());
		gameobject.transform.rotation = Quaternion.identity;
	}

	public override void Update ()
	{
		var newSpeed = speed + gravity * Time.deltaTime * Vector3.down;
		this.transform.position += (0.5F * (newSpeed + speed) * Time.deltaTime);
		speed = newSpeed;
		// Compute the next position of UFO
		// Refresh current speed

		if (this.transform.position.y <= -4) {
			this.destoried = true;
			this.enable = false;
			this.callback.SSActionEvent (this);
		} // UFO lands
	}
}

