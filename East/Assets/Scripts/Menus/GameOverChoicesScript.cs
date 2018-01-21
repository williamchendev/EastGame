using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverChoicesScript : MonoBehaviour {

    [SerializeField] private int type;
    [SerializeField] private GameObject trans_obj;

	//Settings
    private float alpha;
    private Collider2D col;
    private SpriteRenderer sr;

    private string scene;

	void Awake () {
		alpha = 0.35f;
        col = GetComponent<Collider2D>();
        sr = GetComponent<SpriteRenderer>();

        if (type == 0){
            scene = SceneManager.GetActiveScene().name;
        }
        else {
            scene = "Title";
        }

        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0.35f);
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
        alpha = Mathf.Clamp(alpha, 0.35f, 1);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);

        if (check_click){
            if (alpha > 0.8f){
                if (!credits_show){
                    GameObject cam = GameObject.FindGameObjectWithTag("MainCamera");
                    GameObject transition = Instantiate(trans_obj, new Vector3(cam.transform.position.x, cam.transform.position.y, -8f), transform.rotation);
                    transition.GetComponent<TransitionScript>().setSceneName(scene);
                }
            }
        }
	}
}
