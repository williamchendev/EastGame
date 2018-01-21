using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour {

    //Settings
    [SerializeField] private string scene_name;

	//Variables
    private float alpha;
    private SpriteRenderer sr;

    //Init
	void Awake () {
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        if (scene_name != null){
		    alpha = 0;
        }
        else {
            alpha = 1;
        }
	}
	
	//Update Event
	void Update () {
        if (scene_name != null){
		    alpha += 0.035f;
            if (alpha > 1){
                SceneManager.LoadScene(scene_name, LoadSceneMode.Single);
            }
        }
        else {
            alpha -= 0.035f;
            if (alpha < 0){
                Destroy(gameObject);
            }
        }

        alpha = Mathf.Clamp(alpha, 0, 1);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
	}

    public void setSceneName(string new_scene){
        scene_name = new_scene;
    }
}
