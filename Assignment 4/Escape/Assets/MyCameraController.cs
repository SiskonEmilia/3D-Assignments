using CameraController;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyCameraController : MonoBehaviour {
    Transform Character;
    public float smoothTime = 0.01f;
    private Vector3 AVelocity = Vector3.zero;
    Vector3 oldPosition, newPosition;
    // Use this for initialization
    void Start () {
        Character = (GameDirector.getInstance().currentSceneController as FirstController).player.transform;
        oldPosition = newPosition = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.SmoothDamp(transform.position, Character.position, ref AVelocity, smoothTime);
        newPosition = Input.mousePosition;

        if (Input.GetMouseButton((int)MouseButtonDown.MBD_RIGHT))
        {
            transform.Rotate(0, (newPosition - oldPosition).x, 0);
        }
        else
        {
            transform.rotation = Character.rotation;
        }
        oldPosition = newPosition;
    }
}
