using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstSceneControl : MonoBehaviour, ISceneController, IUserAction {  


	public IActionManager actionManager { get; set; }  
	public IActionManager actionManager2 { get; set; }  
	public ScoreRecorder scoreRecorder { get; set; }  
	public UserGUI usergui;
	public Queue<GameObject> diskQueue = new Queue<GameObject>();  
	public int diskNumber;  
	private int currentRound = -1;  
	public int round = 3;  
	private float time = 0;  

	private GameState gameState = GameState.START;  

	private static FirstSceneControl _instance;
	public FirstSceneControl() {
		_instance = this;
	}

	public static FirstSceneControl GetInstance() {
		if (_instance == null)
			_instance = new FirstSceneControl ();
		return _instance;
	}

	void Awake () {  
		Director director = Director.GetInstance();  
		director.currentSceneController = this;  
		diskNumber = 10;  
		this.gameObject.AddComponent<ScoreRecorder>();  
		this.gameObject.AddComponent<UFOFactory>(); 
		this.gameObject.AddComponent<UFOActionManager> ();
		this.gameObject.AddComponent<PPActionManager> ();
		this.usergui = this.gameObject.AddComponent<UserGUI> () as UserGUI;
		director.currentSceneController.LoadResources(); 
	}  

	private void Update()  
	{  
		if (gameState == GameState.PAUSE || gameState == GameState.ROUND_FINISH)
			return;
		if (actionManager.getNum() <= 0 && gameState == GameState.RUNNING)  
		{  
			gameState = GameState.ROUND_FINISH;  

		}  
		else if (actionManager.getNum() <= 0 && gameState == GameState.ROUND_START)  
		{  
			currentRound = (currentRound + 1) % round;  
			NextRound();  
			actionManager.setNum (10);
			gameState = GameState.RUNNING;  
		}  
		else if (time > 2)  
		{  
			ThrowDisk();  
			time = 0;  
		}  
		else  
		{  
			time += Time.deltaTime;  
		}  


	}  

	private void NextRound()  
	{  
		UFOFactory df = UFOFactory.GetInstance (); 
		foreach (var disk in diskQueue) {
			df.FreeUFO (disk);
		}
		diskQueue.Clear ();
		actionManager.setNum(diskNumber);
		for (int i = 0; i < diskNumber; ++i) {
			diskQueue.Enqueue (df.GetUFO (currentRound));
		}
		ThrowDisk ();
		time = 0;
	}  

	void ThrowDisk()  
	{  
		foreach(var disk in diskQueue) {
			Vector3 position = new Vector3(0, 0, 0);  
			float y = Random.Range(0f, 4f);  
			position = new Vector3(-disk.GetComponent<UFOData>().direction.x * 7, y, 0);  
			disk.transform.position = position;  

			disk.SetActive(true);  
		}
		actionManager.StartThrow(diskQueue);
	}  

	public void LoadResources()  
	{  
		// UFOFactory uf = UFOFactory.GetInstance ();

		// DiskFactory df = Singleton<DiskFactory>.Instance;  
		// df.init(diskNumber);  
		// GameObject greensward = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/greensward"));  
	}  


	public void GameOver()  
	{  
		GUI.color = Color.red;  
		GUI.Label(new Rect(700, 300, 400, 400), "GAMEOVER");  

	}  

	public int GetScore()  
	{  
		return scoreRecorder.score;  
	}  

	public GameState getGameState()  
	{  
		return gameState;  
	}  

	public void setGameState(GameState gs)  
	{  
		gameState = gs;  
	}  

	public void hit(Vector3 pos)  
	{  
		Ray ray = Camera.main.ScreenPointToRay(pos);  

		RaycastHit[] hits;  
		hits = Physics.RaycastAll(ray);  
		for (int i = 0; i < hits.Length; i++)  
		{  
			RaycastHit hit = hits[i];  

			if (hit.collider.gameObject.GetComponent<UFOData>() != null)  
			{  
				scoreRecorder.Record(hit.collider.gameObject);  

				hit.collider.gameObject.transform.position = new Vector3(0, -5, 0);
				actionManager.setNum (actionManager.getNum () - 1);
			}  

		}  
	}  

	public void switchManager() {
		var temp = actionManager;
		actionManager = actionManager2;
		actionManager2 = temp;
	}
}  
