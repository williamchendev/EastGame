using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScript : MonoBehaviour {

    [SerializeField] private int type;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject change_scene;

	//Settings
    private float alpha;
    private Collider2D col;
    private SpriteRenderer sr;

	void Awake () {
		alpha = 0.35f;
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();
	}
	
	//Update Event
	void Update () {
        bool check_click = Input.GetMouseButtonDown(0);
        bool credits_show = false;
        GameObject credit_obj = GameObject.FindGameObjectWithTag("Credits");
        if (credit_obj != null){
            credits_show = true;
        }

        Vector2 v2 = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        if (col.bounds.Contains(new Vector3(v2.x, v2.y, transform.position.z))){
            alpha += 0.035f;
        }
        else {
            alpha -= 0.035f;
        }

        if (type == 1){
            alpha = 0;
        }
        alpha = Mathf.Clamp(alpha, 0.35f, 1);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
        
        if (check_click){
            if (alpha > 0.8f){
                if (!credits_show){
                    if (type == 0){
                        Instantiate(change_scene, new Vector3(0f, 0f, -5f), transform.rotation);
                    }
                    else if (type == 1){

                    }
                    else if (type == 2){
                        int scale = Screen.width / 480;

                        int width = 480;
                        int height = 270;
                        bool full = false;

                        if (scale == 1){
                            width = 960;
                            height = 540;
                        }
                        else if (scale == 2){
                            width = 1920;
                            height = 1080;
                            full = true;
                        }

                        Screen.SetResolution(width, height, full, 60);
                    }
                    else if (type == 3){
                        Instantiate(credits, new Vector3(0f, 0f, -5f), transform.rotation);
                    }
                    else if (type == 4){
                        Application.Quit();
                    }
                }
            }
        }
    }

}
