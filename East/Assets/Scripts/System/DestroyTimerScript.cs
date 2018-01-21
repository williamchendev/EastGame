using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimerScript : MonoBehaviour {

    //Timer
	[SerializeField] private float timer;

	//Update
	void Update () {
		timer--;
        if (timer < 0){
            Destroy(gameObject);
        }
	}

}
