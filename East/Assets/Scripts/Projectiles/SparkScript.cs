using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SparkScript : MonoBehaviour {

	//Settings
    private SpriteRenderer sr;

    private float alpha;
    private float angle;
    private float spd;
    private float tilt_spd;

    //Init
	void Start () {
        sr = GetComponent<SpriteRenderer>();

        float range = 1.6f;

        alpha = 1;
        angle = Random.Range((Mathf.PI / 2) - range, (Mathf.PI / 2) + range);
		spd = 0.03f;
        tilt_spd = Random.Range(0.0001f, 0.0009f) * Mathf.Sign(angle - (Mathf.PI / 2));

        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 360f * (angle / (Mathf.PI * 2f)));
	}
	
	//Update Event
	void Update () {
        transform.position = new Vector3(transform.position.x + (Mathf.Cos(angle) * spd), transform.position.y + (Mathf.Sin(angle) * spd), transform.position.z);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 360f * (angle / (Mathf.PI * 2f)));
        angle += tilt_spd;
        spd *= 0.94f;

        alpha *= 0.92f;
        if (alpha > 0.1f){
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        }
        else {
            Destroy(gameObject);
        }
	}
}
