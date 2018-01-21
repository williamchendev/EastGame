using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitScript : MonoBehaviour {

	private float angle;
    private float spd;
    private float size;

	void Start () {
		angle = Random.Range(0, Mathf.PI * 2);
        spd = Random.Range(0.1f, 0.15f);
        size = 1;
	}
	
	void Update () {
        if (spd > 0.0005f){
            spd *= 0.88f;
            if (spd <= 0.0005f){
                spd = 0;
            }
        }
        transform.position = new Vector3(transform.position.x + (Mathf.Cos(angle) * spd), transform.position.y + (Mathf.Sin(angle) * spd), transform.position.z);

		size *= 0.96f;
        if (size < 0.1f){
            Destroy(gameObject);
        }
        else {
            transform.localScale = new Vector3(size, size, transform.localScale.z);
        }
	}
}
