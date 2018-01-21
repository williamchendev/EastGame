using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadScript : MonoBehaviour {

	[SerializeField] private GameObject gameover_obj;

    //Settings
    private int timer;
    private bool created;

	void Start () {
		timer = 120;
        created = false;
	}
	
	//Update Event
	void Update () {
		if (timer > 0){
            timer--;
        }
        else if (!created){
            created = true;
            GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
            Instantiate(gameover_obj, new Vector3(cam.transform.position.x, cam.transform.position.y - 4.5f, -4f), transform.rotation);
        }
	}
}
