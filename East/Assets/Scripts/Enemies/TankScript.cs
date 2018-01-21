using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankScript : MonoBehaviour {

    //Objects
    [SerializeField] private GameObject bullet_obj;
    [SerializeField] private GameObject death_obj;
    [SerializeField] private GameObject hit_obj;

	//Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private PathScript ps;

    //Settings
    private int health;
    private bool dead;

    private bool canmove;
    private float spd;
    private float recalc_dis;
    private float move_distance;
    private float path_distance;

    private bool attack;
    private int charge_time;
    private int attack_time;
    private Vector2 attack_position;

    //Variables
    private Vector2[] path;
    private int path_num;
    private Vector2 velocity;

    //Init Event
	void Start () {
		//Components
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        ps = GetComponent<PathScript>();

        //Settings
        health = 240;
        dead = false;
        
        canmove = true;
        spd = 0.8f;
        recalc_dis = 2.4f;
        move_distance = 3.5f;
        path_distance = 0.25f;

        attack = false;
        charge_time = 60;
        attack_time = 120;
        attack_position = new Vector2(0f, 0f);

        //Variables
        path_num = 0;
        velocity = new Vector2(0f, 0f);
	}
	
    //Update Event
	void Update () {
        //Find Player
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //Set Visible only when on Camera
		if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().objectVisible(transform.position)){
            sr.enabled = true;
        }
        else {
            sr.enabled = false;
        }

		//Movement and Attack
        Vector2 vel = new Vector2(0f, 0f);
        if (player != null){
            if (canmove){
                Vector2 target_position = new Vector2(player.transform.position.x, player.transform.position.y);
                if (attack){
                    //Attacks
                    if (attack_time == 40){
                        Instantiate(bullet_obj, new Vector3(attack_position.x - 3f, attack_position.y, bullet_obj.transform.position.z), transform.rotation);
                        Instantiate(bullet_obj, new Vector3(attack_position.x + 3f, attack_position.y, bullet_obj.transform.position.z), transform.rotation);
                        Instantiate(bullet_obj, new Vector3(attack_position.x, attack_position.y - 3f, bullet_obj.transform.position.z), transform.rotation);
                        Instantiate(bullet_obj, new Vector3(attack_position.x, attack_position.y + 3f, bullet_obj.transform.position.z), transform.rotation);

                        Instantiate(bullet_obj, new Vector3(attack_position.x - 1.5f, attack_position.y - 1.5f, bullet_obj.transform.position.z), transform.rotation);
                        Instantiate(bullet_obj, new Vector3(attack_position.x + 1.5f, attack_position.y + 1.5f, bullet_obj.transform.position.z), transform.rotation);
                        Instantiate(bullet_obj, new Vector3(attack_position.x + 1.5f, attack_position.y - 1.5f, bullet_obj.transform.position.z), transform.rotation);
                        Instantiate(bullet_obj, new Vector3(attack_position.x - 1.5f, attack_position.y + 1.5f, bullet_obj.transform.position.z), transform.rotation);
                    }
                    else if (attack_time == 20){
                        Instantiate(bullet_obj, new Vector3(attack_position.x - 1.5f, attack_position.y, bullet_obj.transform.position.z), transform.rotation);
                        Instantiate(bullet_obj, new Vector3(attack_position.x + 1.5f, attack_position.y, bullet_obj.transform.position.z), transform.rotation);
                        Instantiate(bullet_obj, new Vector3(attack_position.x, attack_position.y - 1.5f, bullet_obj.transform.position.z), transform.rotation);
                        Instantiate(bullet_obj, new Vector3(attack_position.x, attack_position.y + 1.5f, bullet_obj.transform.position.z), transform.rotation);
                    }
                    else if (attack_time == 1){
                        Instantiate(bullet_obj, new Vector3(attack_position.x, attack_position.y, bullet_obj.transform.position.z), transform.rotation);
                    }

                    attack_time--;
                    if (attack_time <= 0){
                        attack = false;
                        charge_time = 180;
                    }
                }
                else if (Vector2.Distance(target_position, new Vector2(transform.position.x, transform.position.y)) > move_distance){
                    if (path == null){
                        path = ps.getPath(target_position, new Vector2(transform.position.x, transform.position.y));
                        path_num = path.Length - 2;
                    }
                    else {
                        if (Vector2.Distance(target_position, path[0]) > recalc_dis){
                            path = ps.getPath(target_position, new Vector2(transform.position.x, transform.position.y));
                            path_num = path.Length - 2;
                        }
                    }

                    if (path != null){
                        if (path_num >= 0){
                            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), path[path_num]) > path_distance){
                                if (Mathf.Abs(transform.position.x - path[path_num].x) > 0.1f){
                                    if (transform.position.x < path[path_num].x){
                                        vel.x = spd;
                                    }
                                    else {
                                        vel.x = -spd;
                                    }
                                }
                                if (Mathf.Abs(transform.position.y - path[path_num].y) > 0.1f){
                                    if (transform.position.y < path[path_num].y){
                                        vel.y = spd;
                                    }
                                    else {
                                        vel.y = -spd;
                                    }
                                }
                            }
                            else {
                                path_num--;
                            }
                        }
                    }
                }
                else {
                    charge_time--;
                    if (charge_time <= 0){
                        attack = true;
                        attack_time = 40;
                        attack_position = new Vector2(player.transform.position.x, player.transform.position.y - 0.25f);
                    }
                }
            }
        }
        velocity = vel;
	}

    //Physics
    void FixedUpdate (){
        rb.velocity = velocity;
    }

    //Hit Event
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == "PlayerBullet"){
            Vector3 closer_hit = new Vector3(Mathf.Lerp(col.gameObject.transform.position.x, transform.position.x, 0.5f), Mathf.Lerp(col.gameObject.transform.position.y, transform.position.y, 0.5f), col.gameObject.transform.position.z);
            Instantiate(hit_obj, closer_hit, transform.rotation);
            Destroy(col.gameObject);
            health--;
            if (health < 0){
                if (!dead){
                    float hit_angle = Mathf.Atan2(transform.position.y - col.gameObject.transform.position.y, transform.position.x - col.gameObject.transform.position.x);
                    GameObject corpse = Instantiate(death_obj, transform.position, transform.rotation);
                    corpse.GetComponent<SpriteRenderer>().flipX = sr.flipX;
                    corpse.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(hit_angle) * 500f, Mathf.Sin(hit_angle) * 500f));
                    Destroy(gameObject);
                    dead = true;
                }
            }
        }
    }
}
