using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreRecorder : MonoBehaviour {
    private int _score = 0;

    public void Start()
    {
        MonsterController.playerLostEvent += Score;
    }

    public void Score(int score)
    {
        _score += score;
    }

    public int GetScore()
    {
        return _score;
    }

    public void reset()
    {
        _score = 0;
    }
}
