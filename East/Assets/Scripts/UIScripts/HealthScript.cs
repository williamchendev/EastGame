using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour {

	//Settings
    private int num;
	
	void FixedUpdate () {
		GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
        float x_pos = cam.transform.position.x - 6.5f;
        if (num == 1){
            x_pos += 1.25f;
        }
        else if (num == 2) {
            x_pos += 2.5f;
        }
        transform.position = new Vector3(x_pos, cam.transform.position.y + 3.5f, -4f);
	}

    public void setNum(int new_num){
        num = new_num;
    }
}
