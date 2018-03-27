using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {

	private int gameStatue;
	private string[,] chessString;
	private string result;
	private char chess;
	private GUIStyle centerText16;
	private int counter;

	void init() {
		gameStatue = new int ();
		chessString = new string[3,3];
		result = null;
		chess = new char ();
		centerText16 = new GUIStyle () {
			fontSize = 20,
			alignment = TextAnchor.MiddleCenter
		};
		counter = new int();
	}

	void reset() {
		gameStatue = 0; // 0: Stopped 1: Gaming 2: End
		chess = 'X';
		counter = 0;

		for (int i = 0; i < 3; ++i) {
			for (int t = 0; t < 3; ++t) {
				chessString [i, t] = null;
			}
		} // null: not occupied, "X"/"O": Player's chess
	}

	void judge() {
		if (chessString [0, 0] != null) {
			if (chessString [0, 0] == chessString [0, 1] && chessString [0, 1] == chessString [0, 2]) {
				gameStatue = 2;
			} else if (chessString [0, 0] == chessString [1, 1] && chessString [1, 1] == chessString [2, 2]) {
				gameStatue = 2;
			} else if (chessString [0, 0] == chessString [0, 1] && chessString [0, 1] == chessString [0, 2]) {
				gameStatue = 2;
			}
		} if (chessString [2, 2] != null) {
			if (chessString [2, 2] == chessString [2, 1] && chessString [2, 1] == chessString [2, 0]) {
				gameStatue = 2;
			} else if (chessString [2, 2] == chessString [1, 2] && chessString [1, 2] == chessString [0, 2]) {
				gameStatue = 2;
			}
		} if (chessString [0, 2] != null) {
			if (chessString [0, 2] == chessString [1, 1] && chessString [1, 1] == chessString [2, 0]) {
				gameStatue = 2;
			}
		} if (chessString [0, 1] != null) {
			if (chessString [0, 1] == chessString [1, 1] && chessString [1, 1] == chessString [2, 1]) {
				gameStatue = 2;
			}
		} if (chessString [1, 0] != null) {
			if (chessString [1, 0] == chessString [1, 1] && chessString [1, 1] == chessString [1, 2]) {
				gameStatue = 2;
			}
		}
		if (gameStatue == 2) {
			result = (chess == 'X') ? "Player2 Wins!" : "Player1 Wins!";
		} else if (counter == 9) {
			gameStatue = 2;
			result = "Stalemate!";
		}
	}

	// Use this for initialization
	void Start () {
		init ();
		reset ();
	}

	void OnGUI() {
		
		GUI.BeginGroup (new Rect (Screen.width / 2 - 150, Screen.height / 2 - 200, 300, 400));

		for (int i = 0; i < 3; ++i)
			for (int t = 0; t < 3; ++t) {
				if (GUI.Button (new Rect (t * 100, i * 100 + 50, 100, 100), chessString [i, t])) {
					if (gameStatue == 1) {
						if (chessString [i, t] == null) {
							++counter;
							chessString [i, t] = "" + chess;
							chess = (chess == 'X') ? 'O' : 'X';
							judge ();
		
						}
					}
				}
			}
		
		switch (gameStatue) {
		case 0:
			GUI.Label (new Rect (50, 0, 200, 50), "TicTacToe!", centerText16);
			break;
		case 1:
			GUI.Label (new Rect (50, 0, 200, 50), (chess == 'X') ? "Player1's Turn" : "Player2's Turn", centerText16);
			break;
		case 2:
			GUI.Label (new Rect (50, 0, 200, 50), result, centerText16);
			break;
		}
				
		if (GUI.Button (new Rect (0, 350, 300, 50), "RESTART!", centerText16)) {
			reset ();
			gameStatue = 1;
		}

		GUI.EndGroup ();
	}

	// Update is called once per frame
	void Update () {
		// As there's no entity, this part remains empty
	}
}
