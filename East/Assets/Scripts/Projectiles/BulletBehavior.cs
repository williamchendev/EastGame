
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {

    //Components
    private Rigidbody2D rb;

	//Settings
    private float spd;
    private float angle;

    //Init
    void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    //Update
    void Update (){
        rb.velocity = new Vector2(spd * Mathf.Cos(angle), spd * Mathf.Sin(angle));
        spd *= 0.85f;
        if (Mathf.Abs(spd) < 1){
            transform.localScale -= new Vector3(0.05f, 0f, 0f);
        }
        if (Mathf.Abs(spd) < 0.1){
            Destroy(this.gameObject);
        }
    }

    public void setDirection(float spd, float angle){
        this.spd = spd;
        this.angle = angle;
        transform.eulerAngles = new Vector3(0f, 0f, (angle / (2 * Mathf.PI)) * 360);
    }

}
