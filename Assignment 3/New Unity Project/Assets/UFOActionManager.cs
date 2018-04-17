using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOActionManager : SSActionManager , ISSActionCallback {
	FirstSceneControl scenecontroller;
	public List<UFOFlyAction> Fly = new List<UFOFlyAction>();
	public int ufonumber = 0;

	private List<SSAction> working = new List<SSAction>();
	private List<SSAction> idel = new List<SSAction>();
	// Factory Mode

	

	public SSAction GetAction() {
		SSAction action = null;

		if (idel.Count > 0) {
			action = idel [0];
			idel.RemoveAt (0);
		} else {
			action = ScriptableObject.Instantiate<UFOFlyAction> (Fly[0]);
		}
		// Try to use actions in idel list, if failed, create one

		working.Add (action);
		return action;
	}

	public void FreeAction(SSAction action) {
		SSAction temp = null;

		foreach (SSAction item in working) {
			if (action.GetInstanceID () == item.GetInstanceID ()) {
				temp = item;
				break;
			}
		}
		// Look for the action

		if (temp != null) {
			// temp.Reset ();
			idel.Add (temp);
			working.Remove (temp);
		}
		// If action found, move it from working list to idel list, and reset all the properties
	}

	protected void Start() {
		scenecontroller = Director.GetInstance ().currentSceneController as FirstSceneControl;
		scenecontroller.actionManager = this;
		Fly.Add (UFOFlyAction.GetSSAction ());
	}

	public void SSActionEvent(SSAction source,  
		SSActionEventType events = SSActionEventType.completed,  
		int intParam = 0,  
		string strParam = null,  
		UnityEngine.Object objectParam = null)  
	{  
		if (source is UFOFlyAction)  
		{  
			UFOFactory uf = UFOFactory.GetInstance (); 
			uf.FreeUFO(source.gameobject);  
			FreeAction(source);  
		}  
	} 

	public void StartThrow(Queue<GameObject> diskQueue)  
	{  
		foreach (GameObject item in diskQueue)  
		{  
			RunAction(item, GetAction(), (ISSActionCallback)this);  
		}  
	}


}
