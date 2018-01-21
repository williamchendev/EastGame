using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAttack : MonoBehaviour {

    //Objects
    [SerializeField] private GameObject spark_obj;
    [SerializeField] private GameObject hit_obj;

    //Settings
    private SpriteRenderer sr;
    private Collider2D col;

    private float alpha;
    private float spd;
    
    private bool destroy;

	//Init
	void Awake () {
		//Settings
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0f);
        col = GetComponent<Collider2D>();

        alpha = 0f;
        spd = 0.013f;

        destroy = false;
	}
	
	//Update
	void Update () {
		alpha += spd;

        alpha = Mathf.Clamp(alpha, 0, 1);
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha * alpha);

        if (destroy){
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null){
                if (col.bounds.Contains(new Vector3(player.transform.position.x, player.transform.position.y - 0.5f, transform.position.z))){
                    if (!player.GetComponent<PlayerBehavior>().Dash){
                        player.GetComponent<PlayerBehavior>().playerHit();
                    }
                }
            }

            for (int i = 0; i < 5; i++){
                //Instantiate(spark_obj, new Vector3(transform.position.x + Random.Range(-0.25f, 0.25f), transform.position.y + Random.Range(-0.25f, 0.25f), spark_obj.transform.position.z), transform.rotation);
                Instantiate(hit_obj, new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(-0.5f, 0.5f), hit_obj.transform.position.z), transform.rotation);
            }
            Destroy(gameObject);
        }
        else if (alpha == 1){
            destroy = true;
        }
	}

    //Hit Player
    void OnTriggerStay2D(Collider2D col){
        if (alpha > 0.9f){
            if (col.gameObject.tag == "Player"){
                col.gameObject.GetComponent<PlayerBehavior>().playerHit();
                destroy = true;
            }
        }
    }
}
