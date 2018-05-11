using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyFactory : MonoBehaviour {
    public GameObject monster;
    private List<GameObject> _monsters;
    private int min_x = -80;
    private int max_x = 80;
    private int min_z = -80;
    private int max_z = 80;

    public void Awake()
    {
        monster = (GameObject)Resources.Load("Prefabs/Monster");
    }

    public List<GameObject> getMonsters()
    {
        List<GameObject> Monsters = new List<GameObject> ();
        for (int i = 0; i < 9; ++i)
        {
            GameObject newMonster = Instantiate<GameObject>(monster);

            newMonster.transform.position = new Vector3(500, 2, 500); // 流放
            Monsters.Add(newMonster);
        }
        _monsters = Monsters;
        return Monsters;
    }

    public void Reput()
    {
        foreach (var amonster in _monsters)
        {
            amonster.transform.position = new Vector3(Random.Range(min_x, max_x), 2, Random.Range(min_z, max_z));
            monster.GetComponent<MonsterController>().GetNewPosition();
        }
    }
}
