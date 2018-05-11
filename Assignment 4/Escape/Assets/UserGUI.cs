using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserGUI : MonoBehaviour
{
    private bool isGameOver = false;
    public string scoreText;
    public string gameOverText;
    public ScoreRecorder sR;
    FirstController fc;

    void Start()
    {
        fc = GameDirector.getInstance().currentSceneController as FirstController;
        sR = Singleton<ScoreRecorder>.Instance;
        scoreText = "Score: 0";
        gameOverText = "Playing...";
    }

    void Update()
    {
        if (isGameOver)
        {//显示结束游戏
            gameOverText = "Game Over!";
            return;
        }
        else
        {
            gameOverText = "Playing...";
            isGameOver = fc.getGameOver();//检查游戏是否结束
            scoreText = "Score: " + sR.GetScore();//显示分数
            return;
        }
    }

    void OnGUI()
    {
        if (fc.isStart)
        {
            GUI.Label(new Rect(10, 10, 100, 30), gameOverText);
            GUI.Label(new Rect(10, 50, 100, 30), scoreText);
        }
        else
        {
            if (GUI.Button(new Rect(10, 10, 100, 30), "Start"))
            {
                fc.start();
            }
        }
    }
}
