using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScript : MonoBehaviour {

    [SerializeField] private GameObject follow_obj;

	void Awake(){
        GetComponent<SpriteRenderer>().enabled = true;
    }

    void FixedUpdate(){
        transform.position = new Vector3(follow_obj.transform.position.x, follow_obj.transform.position.y, 500);
    }
}
