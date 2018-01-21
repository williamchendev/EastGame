using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashBehavior : MonoBehaviour {

	private float alpha;
    private SpriteRenderer sr;

    void Start() {
        sr = GetComponent<SpriteRenderer>();
        alpha = 0.8f;
    }

    void Update() {
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        alpha *= 0.95f;
        if (alpha <= 0.01f){
            Destroy(gameObject);
        }
    }
}
