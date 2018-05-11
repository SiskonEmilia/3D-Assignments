using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    CharacterController cc;
    Animator anim;
    FirstController fc;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        fc = GameDirector.getInstance().currentSceneController as FirstController;
    }

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (fc.getGameOver())
            return;
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if (v > 0.1)
        {
            anim.SetBool("WalkingBack", false);
            anim.SetBool("Run", true);
            transform.Rotate(0, h * 4, 0);
            cc.SimpleMove(transform.forward * 10 * v);
        }
        else if (v < -0.1)
        {
            anim.SetBool("WalkingBack", true);
            anim.SetBool("Run", false);
            transform.Rotate(0, h * 4, 0);
            cc.SimpleMove(transform.forward * v * 3);
        }
        else
        {
            transform.Rotate(0, h * 4, 0);
            anim.SetBool("Run", false);
            anim.SetBool("WalkingBack", false);
        }
    }
}
