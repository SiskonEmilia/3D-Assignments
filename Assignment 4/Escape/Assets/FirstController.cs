using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstController : MonoBehaviour, SceneController
{
    public GameObject player;
    public GameObject mycamera; 
    public List<GameObject> monsters;
    public MyFactory mF;
    private bool isGameOver = false;
    public bool isStart = false;

    void Awake()
    {
        mycamera = (GameObject)Resources.Load("Prefabs/CameraContainer");
        player = (GameObject)Resources.Load("Prefabs/Character");
        GameDirector director = GameDirector.getInstance();
        director.currentSceneController = this;
    }

    void Start()
    {
        mF = Singleton<MyFactory>.Instance;//获得工厂单例
        monsters = mF.getMonsters();//从工厂获得所有的怪物
        MonsterController.hitPlayerEvent += gameOver;//订阅怪物撞击玩家的事件
        player = Instantiate(player);
        player.transform.position = new Vector3(-53, 1.1F, 60);
        mycamera = Instantiate(mycamera);
    }

    public bool getGameOver()
    {
        return isGameOver;
    }

    public void gameOver()
    {
        player.GetComponent<Animator>().SetTrigger("Lose");
        this.isGameOver = true;
    }

    public void start()
    {
        Singleton<ScoreRecorder>.Instance.reset();
        mF.Reput();
        isStart = true;
    }

    public void LoadResources()
    {
        //
    }
}