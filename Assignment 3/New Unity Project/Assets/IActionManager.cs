using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActionManager  
{
	void StartThrow (Queue<GameObject> diskQueue);
	int getNum ();
	void setNum (int num);
}  
