using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour {

	//Objects
    [SerializeField] private GameObject hit_obj;
    [SerializeField] private GameObject slash_obj;
    [SerializeField] private GameObject dead_obj;

    //Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator anim;

    //Settings
    private bool canmove;
    private int health;
    private bool dead;

    private bool can_attack;
    private bool attack;
    private int attack_timer;
    private float attack_radius;

    private int dash_timer;
    private float dash_spd;
    private float dash_angle;
    private float hit_radius;

    //Variables
    private float alpha;
    private Vector2 velocity;


    //Init
	void Start () {
		//Components
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        //Settings
        canmove = true;
        health = 18;
        dead = false;

        can_attack = false;
        attack = false;
        attack_timer = 60;
        attack_radius = 2.8f;

        dash_timer = 48;
        dash_spd = 12f;
        dash_angle = 0;
        
        hit_radius = 0.35f;

        //Variables
        alpha = 0;
        velocity = new Vector2(0f, 0f);

        //Collisions
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null){
            Physics2D.IgnoreCollision(player.GetComponent<Collider2D>(), GetComponent<Collider2D>(), true);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Enemy"), LayerMask.NameToLayer("Enemy"), true);
        }
	}
	
	//Update
	void Update () {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //Movement & Attack
        string anim_name = null;
        Vector2 vel = new Vector2(0f, 0f);
        if (player != null){
	        if (canmove){
                if (attack){
                    if (dash_timer < 0){
                        vel = new Vector2(Mathf.Cos(dash_angle) * dash_spd, Mathf.Sin(dash_angle) * dash_spd);
                        if (dash_spd > 0.1f){
                            dash_spd *= 0.963f;
                            if (dash_spd <= 0.1f){
                                dash_spd = 0;
                            }
                            if (can_attack){
                                if (Vector2.Distance(new Vector2(player.transform.position.x, player.transform.position.y), new Vector2(transform.position.x, transform.position.y)) < hit_radius){
                                    if (!player.GetComponent<PlayerBehavior>().Dash){
                                        player.GetComponent<PlayerBehavior>().playerHit();
                                        can_attack = false;
                                        Instantiate(slash_obj, new Vector3(transform.position.x, transform.position.y, slash_obj.transform.position.z), transform.rotation);
                                        anim_name = "aGhost_End";
                                    }
                                }
                            }
                        }
                        if (dash_spd < 1.2f) {
                            alpha *= 0.95f;
                            if (alpha < 0.5f){
                                alpha = 0;
                                attack = false;
                                attack_timer = Random.Range(314, 518);
                            }
                            can_attack = false;
                            anim_name = "aGhost_End";
                        }
                    }
                    else {
                        dash_timer--;
                        if (alpha < 1){
                            alpha += 0.035f;
                            if (alpha > 1){
                                alpha = 1;
                            }
                        }
                        if (dash_timer < 0){
                            dash_angle = Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x);
                            dash_spd = 12f;
                            can_attack = true;
                            anim_name = "aGhost_Dash";
                        }
                    }
                }
                else {
                    alpha = 0;
                    attack_timer--;
                    if (attack_timer < 0){
                        attack = true;
                        float point_angle = Random.Range(0, 2 * Mathf.PI);
                        float point_x = player.transform.position.x + (Mathf.Cos(point_angle) * attack_radius);
                        float point_y = player.transform.position.y + (Mathf.Sin(point_angle) * attack_radius);
                        transform.position = new Vector3(point_x, point_y, transform.position.z);
                        dash_timer = 48;
                        anim_name = "aGhost_Idle";
                    }
                }
            }
        }
        else {
            alpha *= 0.99f;
            if (alpha < 0.25f){
                alpha = 0;
            }
        }
        velocity = vel;

        //Animations
        if (anim_name != null){
            anim.Play(anim_name);
        }
        if (velocity.x < 0){
            sr.flipX = true;
        }
        else if (velocity.x > 0){
            sr.flipX = false;
        }
        

        //Visible
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, alpha);
	}

    //Physics
    void FixedUpdate() {
        rb.velocity = velocity;
    }

    //Hit Enemy
    void OnTriggerEnter2D(Collider2D col){
        if (alpha > 0.15f){
            if (col.gameObject.tag == "PlayerBullet"){
                Vector3 closer_hit = new Vector3(Mathf.Lerp(col.gameObject.transform.position.x, transform.position.x, 0.5f), Mathf.Lerp(col.gameObject.transform.position.y, transform.position.y, 0.5f), col.gameObject.transform.position.z);
                Instantiate(hit_obj, closer_hit, transform.rotation);
                Destroy(col.gameObject);
                health--;
                if (health < 0){
                    if (!dead){
                        float hit_angle = Mathf.Atan2(transform.position.y - col.gameObject.transform.position.y, transform.position.x - col.gameObject.transform.position.x);
                        GameObject corpse = Instantiate(dead_obj, transform.position, transform.rotation);
                        corpse.GetComponent<SpriteRenderer>().flipX = sr.flipX;
                        corpse.GetComponent<Rigidbody2D>().AddForce(new Vector2(Mathf.Cos(hit_angle) * 500f, Mathf.Sin(hit_angle) * 500f));
                        Destroy(gameObject);
                        dead = true;
                    }
                }
            }
        }
    }
}
