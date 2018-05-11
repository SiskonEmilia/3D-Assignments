using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {

    private Vector3 NextPosition;
    private int speed;
    private CharacterController cc;
    private Animator anim;

    FirstController fc;

    public delegate void hitPlayer ();
    public delegate void playerLost (int score);
    public static event hitPlayer hitPlayerEvent;
    public static event playerLost playerLostEvent;

    // Use this for initialization
    void Start () {
        NextPosition = transform.position;
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        speed = (int)Random.Range(3, 11);
        fc = GameDirector.getInstance().currentSceneController as FirstController;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (fc.getGameOver())
            return;
        var diff = NextPosition - transform.position + Vector3.up * transform.position.y;
        while (diff.magnitude < 0.1)
        {
            GetNewPosition();
            diff = NextPosition - transform.position + Vector3.up * transform.position.y;
        }
        transform.LookAt(transform.position + diff);

        if (diff.magnitude > speed)
            cc.SimpleMove(diff / diff.magnitude * speed);
        else
            cc.SimpleMove(diff);
	}

    public void GetNewPosition()
    {
        NextPosition = new Vector3(transform.position.x + Random.Range(-50, 50), 0, transform.position.z + Random.Range(-50, 50));
    }

    void OnTriggerStay(Collider other)
    {
        if (fc.getGameOver())
            return;
        if (other.gameObject.tag == "Player")
        {
            NextPosition = new Vector3(other.gameObject.transform.position.x, 0, other.gameObject.transform.position.z);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (fc.getGameOver())
            return;
        if (hit == null)
            return;
        if (hit.gameObject.tag == "Player")
        {
            anim.SetTrigger("Attack");
            hitPlayerEvent();
        }
        else if (hit.gameObject.tag == "Trees")
        {
            GetNewPosition();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (fc.getGameOver())
            return;
        if (other.gameObject.tag == "Player")
        {
            playerLostEvent(speed);
            GetNewPosition();
        }
    }

}
