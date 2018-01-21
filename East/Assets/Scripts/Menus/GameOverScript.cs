using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour {

    //Objects
	[SerializeField] private GameObject restart_obj;
    [SerializeField] private GameObject title_obj;

    //Settings
    private bool jump;
    private bool still;
    private bool created;
    private float spd;

    //Init
	void Start () {
		jump = false;
        still = false;
        created = false;
        spd = 0.35f;
	}
	
	//Update Event
	void Update () {
        GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");

        float y_pos = cam.transform.position.y;
		if (!still){
            spd -= 0.01f;
            y_pos = transform.position.y + spd;
            if (jump){
                if (transform.position.y < cam.transform.position.y){
                    y_pos = cam.transform.position.y;
                    still = true;
                }
            }
            else {
                if (transform.position.y > cam.transform.position.y){
                    jump = true;
                }
            }
        }
        else {
            if (!created){
                created = true;
                Instantiate(restart_obj, new Vector3(cam.transform.position.x, cam.transform.position.y - 0.8f, -5f), transform.rotation);
                Instantiate(title_obj, new Vector3(cam.transform.position.x, cam.transform.position.y - 1.9f, -5f), transform.rotation);
            }
        }
        transform.position = new Vector3(cam.transform.position.x, y_pos, -4);
	}
}
