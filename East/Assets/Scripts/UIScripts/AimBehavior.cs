using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimBehavior : MonoBehaviour {

	//Settings
    private bool clicked;

    //Init Event
	void Start () {
		clicked = false;
	}
	
	//Update Event
	void Update () {
        clicked = Input.GetMouseButton(0);

		Vector3 v3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(v3.x, v3.y, transform.position.z);

        if (clicked){
            transform.Rotate(new Vector3(0f, 0f, 2.5f));
        }
        else {
            transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
        }
	}
}
