using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOFactory : MonoBehaviour {

	public GameObject ufoperfab;

	private List<UFOData> working = new List<UFOData> ();
	private List<UFOData> idel = new List<UFOData> ();

	private static UFOFactory _instance;
	private UFOFactory () { }

	public static UFOFactory GetInstance() {
		if (_instance == null)
			_instance = new UFOFactory ();
		return _instance;
	}

	private void Awake() {
		ufoperfab = GameObject.Instantiate<GameObject> (Resources.Load<GameObject> ("UFO"), Vector3.zero, Quaternion.identity);
		ufoperfab.SetActive (false);
	}

	public GameObject GetUFO (int round) {
		/* Get the UFO from idel list, if not enough, create one */
		float RanX;

		GameObject newUFO = null;
		if (idel.Count > 0) {
			newUFO = idel [0].gameObject;
			idel.RemoveAt (0);
		} else {
			newUFO = GameObject.Instantiate<GameObject> (Resources.Load<GameObject> ("UFO"), Vector3.zero, Quaternion.identity);
			newUFO.AddComponent<UFOData>();
		}

		/* Rule design: Different round with diefferent color range */

		int start = 0;
		if (round == 1) start = 100;
		else if (round == 2) start = 250;
		int selectedcolor = Random.Range (start, round * 499);

		if (selectedcolor > 500)
			selectedcolor = 2;
		else if (selectedcolor > 300)
			selectedcolor = 1;
		else
			selectedcolor = 0;

		switch (selectedcolor) {
		case 0:
			newUFO.GetComponent<UFOData> ().color = Color.yellow;
			newUFO.GetComponent<UFOData> ().speed = 4.0f;
			RanX = Random.Range (-1f, 1f) < 0 ? -1 : 1;
			newUFO.GetComponent<UFOData> ().direction = new Vector3 (RanX, 1, 0);
			newUFO.GetComponent<Renderer> ().material.color = Color.yellow;
			break;
		case 1:
			newUFO.GetComponent<UFOData> ().color = Color.red;
			newUFO.GetComponent<UFOData> ().speed = 8.0f;
			RanX = Random.Range (-1f, 1f) < 0 ? -1 : 1;
			newUFO.GetComponent<UFOData> ().direction = new Vector3 (RanX, 1, 0);
			newUFO.GetComponent<Renderer> ().material.color = Color.red;
			break;
		case 2:
			newUFO.GetComponent<UFOData> ().color = Color.black;
			newUFO.GetComponent<UFOData> ().speed = 12.0f;
			RanX = Random.Range (-1f, 1f) < 0 ? -1 : 1;
			newUFO.GetComponent<UFOData> ().direction = new Vector3 (RanX, 1, 0);
			newUFO.GetComponent<Renderer> ().material.color = Color.black;
			break;
		default:
			Debug.Log ("You cannot be here");
			break;
		}

		working.Add (newUFO.GetComponent<UFOData>());
		newUFO.SetActive (true);
		newUFO.name = newUFO.GetInstanceID ().ToString ();

		return newUFO;
	}

	public void FreeUFO(GameObject ufo) {
		UFOData temp = null;
		foreach (UFOData item in working) {
			if (item.gameObject.GetInstanceID () == ufo.GetInstanceID ()) {
				temp = item;
				break;
			}
		}

		if (temp != null) {
			temp.gameObject.SetActive (false);
			working.Remove (temp);
			idel.Add (temp);
		}
	}
}
