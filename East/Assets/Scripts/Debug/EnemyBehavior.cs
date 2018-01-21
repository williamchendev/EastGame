using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour {

    //Objects
    [SerializeField] private GameObject bullet_obj;
    [SerializeField] private GameObject hit_obj;
    [SerializeField] private GameObject dead_obj;

    //Components
    private PathScript pathing;
    private Rigidbody2D rb;

    //Settings
    private float spd;
    private int health;
    private bool dead;

    private float shoot_radius;
    private float shoot_spd;

    //Variables
    private Vector2 velocity;

    private int path_point;
    private Vector2 path_move;
    private Vector2[] path;

    private float shoot_time;

	// Use this for initialization
	void Start () {
        //Components
        pathing = GetComponent<PathScript>();
        rb = GetComponent<Rigidbody2D>();

        //Settings
        spd = 0.5f;
        health = 2;
        dead = false;

        shoot_radius = 3.5f;
        shoot_spd = 2f;

        //Variables
        velocity = new Vector2(0f, 0f);
        path_point = 0;

		shoot_time = 0;
	}
	
	// Update is called once per frame
	void Update () {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        bool can_shoot = false;

        if (GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>().objectVisible(transform.position)){
            GetComponent<SpriteRenderer>().enabled = true;
        }
        else {
            GetComponent<SpriteRenderer>().enabled = false;
        }

        //Moving towards the Player
        Vector2 vel = new Vector2(0, 0);
        if (player != null){
            Vector2 player_pos = new Vector2(player.transform.position.x, player.transform.position.y);
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.y), player_pos) > shoot_radius){
                if (path == null){
                    path_point = 0;
                    path = pathing.getPath(new Vector2(transform.position.x, transform.position.y), player_pos);
                    path_move = player_pos;
                }
                else {
                    if (path_point < path.Length){
                        Vector2 target = path[path_point];
                        if (transform.position.x < target.x){
                            vel.x = spd;
                        }
                        else if (transform.position.x > target.x){
                            vel.x = -spd;
                        }
                        if (transform.position.y < target.y){
                            vel.y = spd;
                        }
                        else if (transform.position.y > target.y){
                            vel.y = -spd;
                        }

                        if (Vector2.Distance(target, new Vector2(transform.position.x, transform.position.y)) < 1){
                            path_point++;
                        }
                    }
                    else {
                        path = null;
                    }

                    if (Vector2.Distance(player_pos, path_move) > 1.5f){
                        path = null;
                    }
                }
            }
            else {
                can_shoot = true;
            }
        }
        velocity = vel;

        //Attacking the Player
        if (can_shoot){
            shoot_time--;
		    if (shoot_time < 0){
                shoot_time = Random.Range(138, 252);
                GameObject bullet = Instantiate(bullet_obj, transform.position, transform.rotation);
                Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
                float temp_shoot_angle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x);
                bullet_rb.velocity = new Vector2(Mathf.Cos(temp_shoot_angle) * shoot_spd, Mathf.Sin(temp_shoot_angle) * shoot_spd);
            }
        }
	}

    void FixedUpdate() {
        rb.velocity = velocity;
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
                    Destroy(gameObject);
                    dead = true;
                }
            }
        }
    }

}
