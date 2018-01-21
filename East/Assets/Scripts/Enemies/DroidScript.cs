using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroidScript : MonoBehaviour {

    //Objects
    [SerializeField] private GameObject bullet_obj;
    [SerializeField] private GameObject death_obj;
    [SerializeField] private GameObject hit_obj;

	//Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    //Settings
    private bool active;
    private bool canmove;
    private float move_spd;

    private float shoot_spd;
    private float shoot_radius;
    
    private bool dead;

    //Variables
    private int health;
    private Vector2 velocity;
    private int shoot_time;
    private int shoot_anim;

    private int move_timer;
    private float move_distance;
    private Vector2 start_position;
    private Vector2 target_position;

    //Init
	void Start () {
		//Components
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        //Settings
        active = true;
        canmove = true;
        move_spd = 0.8f;
        shoot_spd = 2.5f;
        shoot_radius = 6f;
        dead = false;

        //Variables
        health = 2;
        velocity = new Vector2(0f, 0f);
        shoot_time = 20;
        shoot_anim = -1;

        move_timer = 60;
        move_distance = 1.2f;
        start_position = new Vector2(transform.position.x, transform.position.y);
        target_position = start_position;
	}
	
	//Update
	void Update () {
        //Set Visible only when on Camera
		if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().objectVisible(transform.position)){
            sr.enabled = true;
            active = true;
        }
        else {
            sr.enabled = false;
            active = false;
        }

        //AI Behavior
        if (active){
            string anim_name = "aDroid_Idle";
            Vector2 vel = new Vector2(0f, 0f);
            if (canmove){
                GameObject player = GameObject.FindGameObjectWithTag("Player");

                //Moving
                if (Vector2.Distance(target_position, new Vector2(transform.position.x, transform.position.y)) < 0.35f){
                    move_timer--;
                    if (move_timer < 0){
                        float move_x = Random.Range(-1, 1);
                        float move_y = Random.Range(-1, 1);
                        move_timer = Random.Range(212, 312);
                        target_position = new Vector2(start_position.x + (move_x * move_distance), start_position.y + (move_y * move_distance));
                    }
                }
                else {
                    anim_name = "aDroid_Run";
                    if (Mathf.Abs(transform.position.x - target_position.x) > 0.1f){
                        if (transform.position.x < target_position.x){
                            vel.x = move_spd;
                            sr.flipX = false;
                        }
                        else if (transform.position.x > target_position.x){
                            vel.x = -move_spd;
                            sr.flipX = true;
                        }
                    }
                    if (Mathf.Abs(transform.position.y - target_position.y) > 0.1f){
                        if (transform.position.y < target_position.y){
                            vel.y = move_spd;
                        }
                        else if (transform.position.y > target_position.y){
                            vel.y = -move_spd;
                        }
                    }
                }

                //Shooting
                if (player != null){
                    Vector2 player_pos = new Vector2(player.transform.position.x, player.transform.position.y);
                    shoot_time--;
                    if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), player_pos) < shoot_radius){
		                if (shoot_time < 0){
                            float x_displace = 0;
                            if (player_pos.x < transform.position.x){
                                sr.flipX = true;
                                x_displace = -0.01f;
                            }
                            else if (player_pos.x > transform.position.x){
                                sr.flipX = false;
                                x_displace = 0.01f;
                            }
                            shoot_time = Random.Range(138, 252);
                            GameObject bullet = Instantiate(bullet_obj, new Vector3(transform.position.x + x_displace, transform.position.y, bullet_obj.transform.position.z), transform.rotation);
                            Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
                            float temp_shoot_angle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - (transform.position.x + x_displace));
                            bullet_rb.velocity = new Vector2(Mathf.Cos(temp_shoot_angle) * shoot_spd, Mathf.Sin(temp_shoot_angle) * shoot_spd);
                            anim.Play("aDroid_Shoot");
                            shoot_anim = 10;
                        }
                    }
                }
            }
            velocity = vel;

            //Animations
            if (shoot_anim < 0){
                if (anim_name != null){
                    anim.Play(anim_name);
                }
            }
            else {
                shoot_anim--;
            }
        }
	}

    //Velocity
    void FixedUpdate() {
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
