using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum SSActionEventType: int { started, completed }

public interface ISSActionCallback{
	void SSActionEvent (SSAction source,
		SSActionEventType type = SSActionEventType.completed,
		int intPar = 0,
		string strPar = null,
		Object objPar = null);
} // In fact, this class is never used...

public class SSAction : ScriptableObject
{
	public bool enable = true;
	public bool destoried = false;

	public GameObject gameobject { get; set; }
	public Transform transform { get; set; }
	public ISSActionCallback callback { get; set; }

	protected SSAction() {}

	// Use this for initialization
	public virtual void Start ()
	{
		throw new System.NotImplementedException ();
	}
	
	// Update is called once per frame
	public virtual void Update ()
	{
		throw new System.NotImplementedException ();
	}
}

public class CCMoveToAction : SSAction {
	public Vector3 target;
	public float speed;

	public static CCMoveToAction GetSSAction (Vector3 target, float speed, GameObject gameobject) {
		CCMoveToAction action = ScriptableObject.CreateInstance<CCMoveToAction> ();
		action.target = target;
		action.speed = speed;
		action.gameobject = gameobject;
		action.transform = gameobject.transform;
		return action;
	}

	public override void Start() {
	}
	public override void Update() {
		this.transform.position = Vector3.MoveTowards (this.transform.position, target, speed * Time.deltaTime);
		if (this.transform.position == target) {
			this.destoried = true;
		}
	}
}

public class CCJumpToAction : SSAction {
	public Vector3 target;
	public float speedx;
	public float speedy;
	public static readonly float gravity = 100;

	public static CCJumpToAction GetSSAction (Vector3 target, float speed, GameObject gameobject) {
		CCJumpToAction action = ScriptableObject.CreateInstance<CCJumpToAction> ();
		action.gameobject = gameobject;
		action.transform = gameobject.transform;
		action.target = target;
		action.speedx = speed * ((target.x < action.transform.position.x) ? -1 : 1);
		float deltaTime = (target.x - action.transform.position.x) / action.speedx;
		action.speedx *= deltaTime * 2;
		deltaTime = 0.5F;
		action.speedy = Mathf.Abs((target.y - action.transform.position.y) / deltaTime + 0.5F * gravity * deltaTime);
		return action;
	}

	public override void Start ()
	{ 
		
	}

	public override void Update ()
	{
		var newSpeedy = speedy - gravity * Time.deltaTime;
		this.transform.position += (Vector3.right * speedx * Time.deltaTime + Vector3.up * 0.5F * (newSpeedy + speedy) * Time.deltaTime);
		speedy = newSpeedy;
		if (this.transform.position.x == target.x || this.transform.position.y <= target.y) {
			this.transform.position = target;
			this.destoried = true;
		}
	}
}

public class SSActionManager : MonoBehaviour, ISSActionCallback {
	private Dictionary <int, SSAction> actions = new Dictionary <int, SSAction>();
	private List <SSAction> waitngAdd = new List<SSAction> ();
	private List <int> waitingDelete = new List<int> ();

	protected void Update() {
		foreach (SSAction ac in waitngAdd)
			actions [ac.GetInstanceID ()] = ac;
		waitngAdd.Clear ();

		foreach (KeyValuePair <int, SSAction> kv in actions) {
			SSAction ac = kv.Value;
			if (ac.destoried)
				waitingDelete.Add (ac.GetInstanceID ());
			else if (ac.enable)
				ac.Update ();
		}

		foreach (int key in waitingDelete) {
			SSAction ac = actions [key];
			actions.Remove (key);
			DestroyObject (ac);
		}

		waitingDelete.Clear ();
	}

	public void RunAction (SSAction action, ISSActionCallback manager) {
		action.callback = manager;
		waitngAdd.Add (action);
		action.Start ();
	}

	public void SSActionEvent (SSAction source,
		SSActionEventType type,
		int intPar,
		string strPar,
		Object objPar) {}

	protected void Start() { }
}

public class EmiliaScenceActionManager : SSActionManager {
	private readonly static float defaultSpeed = 10;

	public void MoveBoat (BoatController boat) {
		CCMoveToAction action = CCMoveToAction.GetSSAction (boat.getDestination(), defaultSpeed, boat.boat);
		RunAction (action, this);
	}

	public void MoveCharacter (ICharacterController character, Vector3 destination) {
		CCJumpToAction action = CCJumpToAction.GetSSAction (destination, defaultSpeed, character.character);
		RunAction (action, this);
	}

	public void Update() {
		base.Update ();
	}
}