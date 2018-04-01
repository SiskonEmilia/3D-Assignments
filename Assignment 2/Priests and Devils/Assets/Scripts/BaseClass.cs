using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Director : System.Object {

	private static Director _director;
	public SceneController currentSceneController { get; set; }

	public static Director getInstance() {
		if (_director == null)
			_director = new Director ();
		return _director;
	}

	private Director() {}

}

public interface SceneController {
	void loadResources ();
}

public interface UserAction {
	void moveBoat ();
	void characterIsClicked (ICharacterController characterController);
	void restart ();
}

public class ClickGUI : MonoBehaviour {
	private UserAction action;
	private ICharacterController characterController;

	public void setController(ICharacterController characterCtrl) {
		characterController = characterCtrl;
	}

	void Start() {
		action = Director.getInstance ().currentSceneController as UserAction;
	}

	void OnMouseDown() {
		if (gameObject.name == "boat") {
			action.moveBoat ();
		} else {
			action.characterIsClicked (characterController);
		}
	}
}

public class Moveable: MonoBehaviour {
	readonly float speed = 20;

	enum movement {waiting, moving};
	movement present;
	Vector3 dest;

	void Update() {
		if (present == movement.moving) {
			transform.position = Vector3.MoveTowards (transform.position, dest, speed * Time.deltaTime);
		}
		if (transform.position == dest)
			present = movement.waiting;
	}

	public void setDest (Vector3 dest) {
		this.dest = dest;
		present = movement.moving;
	}

	public void reset() {
		present = movement.waiting;
	}
}

public class CoastController {
	readonly GameObject coast;
	readonly Vector3 from_pos = new Vector3(7,0,0);
	readonly Vector3 to_pos = new Vector3(-7,-0,0);
	readonly Vector3[] positions;
	readonly int to_or_from;	// to->-1, from->1

	// change frequently
	ICharacterController[] passengerPlaner;

	public CoastController(string _to_or_from) {
		positions = new Vector3[] {new Vector3(3.5F,1.5F,0), new Vector3(4.5F,1.5F,0), new Vector3(5.5F,1.5F,0), 
			new Vector3(6.5F,1.5F,0), new Vector3(7.5F,1.5F,0), new Vector3(8.5F,1.5F,0)};

		passengerPlaner = new ICharacterController[6];

		if (_to_or_from == "from") {
			coast = Object.Instantiate (Resources.Load ("Perfabs/Coast", typeof(GameObject)), from_pos, Quaternion.identity, null) as GameObject;
			coast.name = "from";
			to_or_from = 1;
		} else {
			coast = Object.Instantiate (Resources.Load ("Perfabs/Coast", typeof(GameObject)), to_pos, Quaternion.identity, null) as GameObject;
			coast.name = "to";
			to_or_from = -1;
		}
	}

	public int getEmptyIndex() {
		for (int i = 0; i < passengerPlaner.Length; i++) {
			if (passengerPlaner [i] == null) {
				return i;
			}
		}
		return -1;
	}

	public Vector3 getEmptyPosition() {
		Vector3 pos = positions [getEmptyIndex ()];
		pos.x *= to_or_from;
		return pos;
	}

	public void getOnCoast(ICharacterController characterCtrl) {
		int index = getEmptyIndex ();
		passengerPlaner [index] = characterCtrl;
	}

	public ICharacterController getOffCoast(string passenger_name) {	// 0->priest, 1->devil
		for (int i = 0; i < passengerPlaner.Length; i++) {
			if (passengerPlaner [i] != null && passengerPlaner [i].getName () == passenger_name) {
				ICharacterController charactorCtrl = passengerPlaner [i];
				passengerPlaner [i] = null;
				return charactorCtrl;
			}
		}
		Debug.Log ("can\'t find passenger on coast: " + passenger_name);
		return null;
	}

	public int get_to_or_from() {
		return to_or_from;
	}

	public int[] getCharacterNum() {
		int[] count = {0, 0};
		for (int i = 0; i < passengerPlaner.Length; i++) {
			if (passengerPlaner [i] == null)
				continue;
			if (passengerPlaner [i].getType () == 0) {	// 0->priest, 1->devil
				count[0]++;
			} else {
				count[1]++;
			}
		}
		return count;
	}

	public void reset() {
		passengerPlaner = new ICharacterController[6];
	}
}

public class ICharacterController {
	enum CharacterType {priest, devil};

	private readonly GameObject character;
	private readonly Moveable moveable;
	private readonly ClickGUI clickGUI;
	private readonly CharacterType characterType;

	private bool isOnBoat;
	CoastController coastController;

	public ICharacterController(string type) {
		if (type == "priest") {
			character = Object.Instantiate (Resources.Load ("Perfabs/Priest", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
			characterType = CharacterType.priest;
		}
		else {
			character = Object.Instantiate (Resources.Load ("Perfabs/Devil", typeof(GameObject)), Vector3.zero, Quaternion.identity, null) as GameObject;
			characterType = CharacterType.devil;
		}

		moveable = character.AddComponent (typeof(Moveable)) as Moveable;

		clickGUI = character.AddComponent (typeof(ClickGUI)) as ClickGUI;
		clickGUI.setController (this);
	}

	public void setName (string name) {
		character.name = name;
	}

	public void setPosition (Vector3 position) {
		character.transform.position = position;
	}

	public void moveTo(Vector3 dest) {
		moveable.setDest (dest);
	}

	public int getType() {
		return (characterType == CharacterType.priest) ? 0 : 1;
	}

	public string getName () {
		return character.name;
	}

	public bool IsOnBoat () {
		return isOnBoat;
	}

	public void getOnBoat(BoatController boatCtrl) {
		coastController = null;
		character.transform.parent = boatCtrl.getGameobj().transform;
		isOnBoat = true;
	}

	public CoastController getCoastController() {
		return coastController;
	}

	public void getOnCoast(CoastController coast) {
		coastController = coast;
		character.transform.parent = null;
		isOnBoat = false;
	}

	public void reset() {
		moveable.reset();
		coastController = (Director.getInstance ().currentSceneController as FirstController).fromCoast;
		getOnCoast (coastController);
		setPosition (coastController.getEmptyPosition ());
		coastController.getOnCoast(this);
	}
}

public class BoatController {
	readonly GameObject boat;
	readonly Moveable moveableScript;
	readonly Vector3 fromPosition = new Vector3 (1.5F, 0.65F, 0);
	readonly Vector3 toPosition = new Vector3 (-1.5F, 0.65F, 0);
	readonly Vector3[] from_positions;
	readonly Vector3[] to_positions;

	// change frequently
	int to_or_from; // to->-1; from->1, indentify & compute the position
	ICharacterController[] passenger = new ICharacterController[2];

	public BoatController() {
		to_or_from = 1;

		from_positions = new Vector3[] { new Vector3 (0.75F, 1.2F, 0), new Vector3 (2.25F, 1.2F, 0) };
		to_positions = new Vector3[] { new Vector3 (-2.25F, 1.2F, 0), new Vector3 (-0.75F, 1.2F, 0) };

		boat = Object.Instantiate (Resources.Load ("Perfabs/Boat", typeof(GameObject)), fromPosition, Quaternion.identity, null) as GameObject;
		boat.name = "boat";

		moveableScript = boat.AddComponent (typeof(Moveable)) as Moveable;
		boat.AddComponent (typeof(ClickGUI));
	}


	public void Move() {
		if (to_or_from == -1) {
			moveableScript.setDest(fromPosition);
			to_or_from = 1;
		} else {
			moveableScript.setDest(toPosition);
			to_or_from = -1;
		}
	}

	public int getEmptyIndex() {
		for (int i = 0; i < passenger.Length; i++) {
			if (passenger [i] == null) {
				return i;
			}
		}
		return -1;
	}

	public bool isEmpty() {
		for (int i = 0; i < passenger.Length; i++) {
			if (passenger [i] != null) {
				return false;
			}
		}
		return true;
	}

	public Vector3 getEmptyPosition() {
		Vector3 pos;
		int emptyIndex = getEmptyIndex ();
		if (to_or_from == -1) {
			pos = to_positions[emptyIndex];
		} else {
			pos = from_positions[emptyIndex];
		}
		return pos;
	}

	public void GetOnBoat(ICharacterController characterCtrl) {
		int index = getEmptyIndex ();
		passenger [index] = characterCtrl;
	}

	public ICharacterController GetOffBoat(string passenger_name) {
		for (int i = 0; i < passenger.Length; i++) {
			if (passenger [i] != null && passenger [i].getName () == passenger_name) {
				ICharacterController charactorCtrl = passenger [i];
				passenger [i] = null;
				return charactorCtrl;
			}
		}
		Debug.Log ("Cant find passenger in boat: " + passenger_name);
		return null;
	}

	public GameObject getGameobj() {
		return boat;
	}

	public int get_to_or_from() { // to->-1; from->1
		return to_or_from;
	}

	public int[] getCharacterNum() {
		int[] count = {0, 0};
		for (int i = 0; i < passenger.Length; i++) {
			if (passenger [i] == null)
				continue;
			if (passenger [i].getType () == 0) {	// 0->priest, 1->devil
				count[0]++;
			} else {
				count[1]++;
			}
		}
		return count;
	}

	public void reset() {
		moveableScript.reset ();
		if (to_or_from == -1) {
			Move ();
		}
		passenger = new ICharacterController[2];
	}
}