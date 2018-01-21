using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehavior : MonoBehaviour {

	//Objects
    [SerializeField] private GameObject dead_obj;
    [SerializeField] private GameObject hit_obj;

    [SerializeField] private GameObject laser_obj;
    [SerializeField] private GameObject laseraim_obj;
    [SerializeField] private GameObject laserradius_obj;
    [SerializeField] private GameObject spark_obj;

    private GameObject aim;
    private GameObject aim_rad;
    private GameObject laser;

    //Components
    private SpriteRenderer sr;

    //Settings
    private float spd;
    private int health;
    private bool dead;

    private float charge_spd;
    private float charge_shoot_spd;
    private float charge_radius;
    private float charge_y_displace;

    //Variables
    private Vector2 charge_position;
    private float charge_angle;
    private float charge_direction;
    private float shot_length;
    private int charge_attack;
    private int charge;

    //Init
	void Start () {
        //Objects
        aim = Instantiate(laseraim_obj, transform.position, transform.rotation);
        aim_rad = Instantiate(laserradius_obj, transform.position, transform.rotation);
        laser = null;

        aim.GetComponent<SpriteRenderer>().color = new Color(aim.GetComponent<SpriteRenderer>().color.r, aim.GetComponent<SpriteRenderer>().color.g, aim.GetComponent<SpriteRenderer>().color.b, 0.4f);
        aim_rad.GetComponent<SpriteRenderer>().color = new Color(aim_rad.GetComponent<SpriteRenderer>().color.r, aim_rad.GetComponent<SpriteRenderer>().color.g, aim_rad.GetComponent<SpriteRenderer>().color.b, 0.8f);

        aim.GetComponent<SpriteRenderer>().enabled = false;
        aim_rad.GetComponent<SpriteRenderer>().enabled = false;

		//Components
        sr = GetComponent<SpriteRenderer>();

        //Settings
        spd = 0.005f; //Move Speed
        health = 150;
        dead = false;

        charge_spd = 0.007f; //Speed the aim radius travels
        charge_shoot_spd = 0.05f;  //Speed of the actual projectile
        charge_radius = 6f; //How far away the player can be before the shot can be charged
        charge_y_displace = 0.5f; //The displacement of the Charge radius so that it aligns with the player's feet

        //Variables
        charge_position = new Vector2(0f, 0f); //Position of the aimed shot
        charge_angle = 0;  //Angle the shot is aimed
        charge_direction = 0;  //Direction the the shot will sweep
        shot_length = 0.42f; //Length of the shot
        charge_attack = 60; //Timer until the shot is charged
        charge = 120; //Timer until the shot is released
	}
	
	//Update Event
	void Update () {
		GameObject player = GameObject.FindGameObjectWithTag("Player");

        //Set Visible if on Camera
        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().objectVisible(transform.position)){
            sr.enabled = true;
        }
        else {
            sr.enabled = false;
        }

        //Behavior
        Vector2 vel = new Vector2(0f, 0f);
        if (player != null){
            bool move = false;
            if (Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y), new Vector2(transform.position.x, transform.position.y)) < charge_radius){
                if (charge_attack > 0){
                    charge_attack--;
                    if (charge_attack <= 0){
                        charge_position = new Vector2(player.transform.position.x, player.transform.position.y - charge_y_displace);
                        charge = 120;
                        aim.GetComponent<SpriteRenderer>().enabled = true;
                        aim_rad.GetComponent<SpriteRenderer>().enabled = true;
                        aim_rad.transform.position = charge_position;
                        aim.transform.position = new Vector3((transform.position.x + charge_position.x) / 2, (transform.position.y + charge_position.y) / 2, aim.transform.position.z);
                        aim.transform.eulerAngles = new Vector3(0f, 0f, 360 * (Mathf.Atan2(charge_position.y - transform.position.y, charge_position.x - transform.position.x) / (2f * Mathf.PI)));
                        aim.transform.localScale = new Vector3(Vector2.Distance(new Vector2(transform.position.x, transform.position.y), charge_position), aim.transform.localScale.y, aim.transform.localScale.z);
                        aim.GetComponent<SpriteRenderer>().color = new Color(aim.GetComponent<SpriteRenderer>().color.r, aim.GetComponent<SpriteRenderer>().color.g, aim.GetComponent<SpriteRenderer>().color.b, 0.4f);
                    }
                }
                else {
                    if (charge > 0){
                        charge--;
                        charge_position = new Vector2(charge_position.x + ((player.transform.position.x - charge_position.x) * charge_spd), charge_position.y + (((player.transform.position.y - charge_y_displace) - charge_position.y) * charge_spd));
                        
                        aim_rad.transform.position = charge_position;
                        aim.transform.position = new Vector3((transform.position.x + charge_position.x) / 2, (transform.position.y + charge_position.y) / 2, aim.transform.position.z);
                        aim.transform.eulerAngles = new Vector3(0f, 0f, 360 * (Mathf.Atan2(charge_position.y - transform.position.y, charge_position.x - transform.position.x) / (2f * Mathf.PI)));
                        aim.transform.localScale = new Vector3(Vector2.Distance(new Vector2(transform.position.x, transform.position.y), charge_position), aim.transform.localScale.y, aim.transform.localScale.z);
                        aim.GetComponent<SpriteRenderer>().color = new Color(aim.GetComponent<SpriteRenderer>().color.r, aim.GetComponent<SpriteRenderer>().color.g, aim.GetComponent<SpriteRenderer>().color.b, 0.4f + Mathf.Clamp((0.4f - (0.4f * (charge / 60f))), 0, 0.4f));
                        
                        if (charge <= 0){
                            float player_angle = (Mathf.Atan2((player.transform.position.y - charge_y_displace) - transform.position.y, player.transform.position.x - transform.position.x) / Mathf.PI) * 180f;
                            charge_angle = (Mathf.Atan2(charge_position.y - transform.position.y, charge_position.x - transform.position.x) / Mathf.PI) * 180f;
                            
                            if (player.transform.position.x < transform.position.x){
                                if (player_angle < 0){
                                    player_angle = 360 + player_angle;
                                }
                                if (charge_angle < 0){
                                    charge_angle = 360 + charge_angle;
                                }
                            }

                            charge_direction = 0;
                            if (charge_angle <= 90 && player_angle > 270){
                                charge_direction = 1;
                            }
                            else if (charge_angle >= 270 && player_angle < 90){
                                charge_direction = -1;
                            }
                            else if (Mathf.Abs(charge_angle - player_angle) > 0.05f){
                                charge_direction = Mathf.Sign(player_angle - charge_angle);
                            }

                            aim.GetComponent<SpriteRenderer>().enabled = false;
                            aim_rad.GetComponent<SpriteRenderer>().enabled = false;
                            laser = Instantiate(laser_obj, transform.position, transform.rotation);
                            laser.transform.position = aim.transform.position;
                            laser.transform.eulerAngles = aim.transform.eulerAngles;
                            laser.transform.localScale = new Vector3(Vector2.Distance(new Vector2(transform.position.x, transform.position.y), charge_position), laser.transform.localScale.y, laser.transform.localScale.z);
                            shot_length = Mathf.Clamp(Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y), new Vector2(transform.position.x, transform.position.y)), 0.5f, charge_radius) + 1f;
                            charge_angle = charge_angle * Mathf.Deg2Rad;
                        }
                    }
                    else {
                        charge_angle += charge_direction * charge_shoot_spd;
                        charge_position = new Vector2(transform.position.x + (Mathf.Cos(charge_angle) * shot_length), transform.position.y + (Mathf.Sin(charge_angle) * shot_length));
                        if (laser != null){
                            laser.transform.position = new Vector3((transform.position.x + charge_position.x) / 2, (transform.position.y + charge_position.y) / 2, aim.transform.position.z);
                            laser.transform.eulerAngles = new Vector3(0f, 0f, 360 * (Mathf.Atan2(charge_position.y - transform.position.y, charge_position.x - transform.position.x) / (2f * Mathf.PI)));
                            laser.transform.localScale = new Vector3(Vector2.Distance(new Vector2(transform.position.x, transform.position.y), charge_position), laser.transform.localScale.y * 0.97f, laser.transform.localScale.z);
                            Instantiate(spark_obj, new Vector3(charge_position.x, charge_position.y, spark_obj.transform.position.z), transform.rotation);
                        }
                        else {
                            charge_attack = 120;
                        }
                    }
                }
            }
            else {
                charge_attack = 30;
                if (laser != null){
                    Destroy(laser.gameObject);
                }
                aim.GetComponent<SpriteRenderer>().enabled = false;
                aim_rad.GetComponent<SpriteRenderer>().enabled = false;
                move = true;
            }

            //Set Movement
            if (move){
                if (Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y), new Vector2(transform.position.x, transform.position.y)) > 3f){
                    float direction = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x);
                    vel.x = Mathf.Cos(direction) * spd;
                    vel.y = Mathf.Sin(direction) * spd;
                }
            }
        }
        
        //Movement
        transform.position = new Vector3(transform.position.x + vel.x, transform.position.y + vel.y, transform.position.z);
	}

    //Hit Enemy
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == "PlayerBullet"){
            Vector3 closer_hit = new Vector3(Mathf.Lerp(col.gameObject.transform.position.x, transform.position.x, 0.5f), Mathf.Lerp(col.gameObject.transform.position.y, transform.position.y, 0.5f), col.gameObject.transform.position.z);
            Instantiate(hit_obj, closer_hit, transform.rotation);
            Destroy(col.gameObject);
            health--;
            if (health < 0){
                if (!dead){
                    float hit_angle = Mathf.Atan2(transform.position.y - col.gameObject.transform.position.y, transform.position.x - col.gameObject.transform.position.x);
                    GameObject corpse = Instantiate(dead_obj, transform.position, transform.rotation);
                    corpse.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(hit_angle) * 500f, Mathf.Sin(hit_angle) * 500f));
                    Destroy(aim.gameObject);
                    Destroy(aim_rad.gameObject);
                    if (laser != null){
                        Destroy(laser.gameObject);
                    }
                    Destroy(gameObject);
                    dead = true;
                }
            }
        }
    }
}
