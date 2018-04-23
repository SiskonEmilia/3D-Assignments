using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using UnityEngine;

public interface ISceneController {
	void LoadResources();
}
// Interface of all scenecontroller

public enum GameState { ROUND_START, ROUND_FINISH, RUNNING, PAUSE, START}  

public interface IUserAction {  
	void GameOver();  
	GameState getGameState();  
	void setGameState(GameState gs);  
	int GetScore();  
	void hit(Vector3 pos);  
}
// Useraction interface

public class Director : System.Object {
	
	public ISceneController currentSceneController { get; set; }

	private static Director _instance;
	private Director() { 
		currentSceneController = FirstSceneControl.GetInstance ();
	}
	// Single Instance

	public static Director GetInstance () {
		if (_instance == null)
			_instance = new Director ();
		return _instance;
	}
}
// Director

/* Action Manage */

public enum SSActionEventType: int { started, completed }

public interface ISSActionCallback{
	void SSActionEvent (SSAction source,
		SSActionEventType type = SSActionEventType.completed,
		int intPar = 0,
		string strPar = null,
		Object objPar = null);
}

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

	public void Reset() {
		this.enable = false;
		this.destoried = false;
		this.gameobject = null;
		this.transform = null;
	}
}
// Base class of all actions

public class SSActionManager : MonoBehaviour{
	private Dictionary <int, SSAction> actions = new Dictionary <int, SSAction>();
	private List <SSAction> waitngAdd = new List<SSAction> ();

	protected void Update() {
		foreach (SSAction ac in waitngAdd)
			actions [ac.GetInstanceID ()] = ac;
		waitngAdd.Clear ();

		foreach (KeyValuePair <int, SSAction> kv in actions) {
			SSAction ac = kv.Value;

			if (ac.enable)
				ac.Update ();
		}
			
	}

	public void RunAction (GameObject gameobject, SSAction action, ISSActionCallback manager) {
		action.gameobject = gameobject;
		action.transform = gameobject.transform;
		action.callback = manager;
		waitngAdd.Add (action);
		action.Start ();
		action.enable = true;
		action.destoried = false;
		if (action is Emit)
			(action as Emit).isDone = false;
	}

	protected void Start() { }

	public void SSActionEvent (SSAction source,
		SSActionEventType type,
		int intPar,
		string strPar,
		Object objPar) {}
}
// Base class of all ActionManager

public class UFOData : MonoBehaviour {
	public Vector3 Size;
	public Color color;
	public float speed;
	public Vector3 direction;
}
// Data stucture of UFOs

