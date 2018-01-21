using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour {

    //Components
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    //Objects
    [SerializeField] private GameObject aim_obj;
    [SerializeField] private GameObject circlet_obj;
    [SerializeField] private GameObject dash_obj;
    [SerializeField] private GameObject bullet_obj;
    [SerializeField] private GameObject dead_obj;
    [SerializeField] private GameObject hit_obj;
    [SerializeField] private GameObject health_obj;

    private GameObject aim;
    private GameObject circlet;
    private GameObject[] health_gui; 

    //Settings
    private bool canmove;
    private float spd;
    private int health;

    private bool dash;
    private float dash_spd;
    private int dash_direction;
    private int dash_press;
    private int dash_create;
    private int dash_time;

    private int shoot_time;

    private int invincible_time;

    //Keys
    private bool w_key;
    private bool s_key;
    private bool a_key;
    private bool d_key;

    private bool e_key;

    private bool clicked;

    //Set FrameRate
    void Awake() {
        Application.targetFrameRate = 60;
    }

    //Init Object
    void Start () {
		//Components
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        //Objects
        aim = Instantiate(aim_obj);
        circlet = Instantiate(circlet_obj);

        health_gui = new GameObject[3];
        for (int g = 0; g < 3; g++){
            health_gui[g] = Instantiate(health_obj, new Vector3(0f, 0f, -5000), transform.rotation);
            health_gui[g].GetComponent<HealthScript>().setNum(g);
        }

        //Settings
        canmove = true;
        spd = 2.5f;
        health = 2;

        dash = false;
        dash_spd = 0;
        dash_direction = 0;
        dash_press = 0;
        dash_create = 0;
        dash_time = 0;

        shoot_time = 0;

        invincible_time = 0;

        //Keys
        w_key = false;
        s_key = false;
        a_key = false;
        d_key = false;

        e_key = false;

        clicked = false;
	}
	
	//Key Checks & Animation
	void Update () {
		w_key = false;
        s_key = false;
        a_key = false;
        d_key = false;
        e_key = false;

        clicked = Input.GetMouseButton(0);
        if (shoot_time > 0){
            shoot_time--;
        }

        int facing = 0;
        string anim_name = "aCat_Idle";

        if (canmove){
            if (Input.GetKey(KeyCode.W)){
                w_key = true;
                anim_name = "aCat_Run";
            }
            else if (Input.GetKey(KeyCode.S)){
                s_key = true;
                anim_name = "aCat_Run";
            }

            if (Input.GetKey(KeyCode.D)){
                d_key = true;
                facing = 1;
                anim_name = "aCat_Run";
            }
            else if (Input.GetKey(KeyCode.A)){
                a_key = true;
                facing = -1;
                anim_name = "aCat_Run";
            }

            if (dash){
                anim_name = "aCat_Dash";
                dash_create--;
                if (dash_create <= 0){
                    GameObject dash_shadow = Instantiate(dash_obj, new Vector3(transform.position.x, transform.position.y, 1f), transform.rotation);
                    dash_shadow.GetComponent<SpriteRenderer>().flipX = sr.flipX;
                    dash_create = 4;
                }
            }

            if (dash_time == 0){
                if (!dash) {
                    if (dash_direction == 0){
                        if (Input.GetKey(KeyCode.W)){
                            dash_direction = -1;
                        }
                        else if (Input.GetKey(KeyCode.S)){
                            dash_direction = -2;
                        }
                        else if (Input.GetKey(KeyCode.A)){
                            dash_direction = -3;
                        }
                        else if (Input.GetKey(KeyCode.D)){
                            dash_direction = -4;
                        }

                        if (dash_direction != 0){
                            dash_press = 24;
                        }
                    }
                    else {
                        if (dash_direction == -1){
                            if (!Input.GetKey(KeyCode.W)){
                                dash_direction = 1;
                            }
                        }
                        else if (dash_direction == -2){
                            if (!Input.GetKey(KeyCode.S)){
                                dash_direction = 2;
                            }
                        }
                        else if (dash_direction == -3){
                            if (!Input.GetKey(KeyCode.A)){
                                dash_direction = 3;
                            }
                        }
                        else if (dash_direction == -4){
                            if (!Input.GetKey(KeyCode.D)){
                                dash_direction = 4;
                            }
                        }
                        else {
                            if (dash_direction == 1){
                                if (Input.GetKey(KeyCode.W)){
                                    dash = true;
                                }
                            }
                            else if (dash_direction == 2){
                                if (Input.GetKey(KeyCode.S)){
                                    dash = true;
                                }
                            }
                            else if (dash_direction == 3){
                                if (Input.GetKey(KeyCode.A)){
                                    dash = true;
                                }
                            }
                            else if (dash_direction == 4){
                                if (Input.GetKey(KeyCode.D)){
                                    dash = true;
                                }
                            }

                            if (dash){
                                dash_press = 0;
                                dash_time = 24;
                                dash_spd = 15f;
                            }
                        }

                        dash_press--;
                        if (dash_press == 0){
                            dash_direction = 0;
                        }
                    }
                }
            }
            else if (dash_time > 0){
                dash_time--;
                if (dash_time == 0){
                    dash = false;
                    dash_press = 0;
                    dash_direction = 0;
                    dash_time = -16;
                }
            }
            else if (dash_time < 0){
                dash_time++;
            }

            if (Input.GetKey(KeyCode.E)){
                e_key = true;
            }

            if (clicked){
                if (shoot_time == 0){
                    float temp_shoot_angle = Mathf.Atan2(aim.transform.position.y - transform.position.y, aim.transform.position.x - transform.position.x);
                    for (int b = 0; b < 6; b++){
                        GameObject temp_bullet = Instantiate(bullet_obj, circlet.transform.position, transform.rotation);
                        temp_bullet.GetComponent<BulletBehavior>().setDirection(Random.Range(17, 32), temp_shoot_angle + Random.Range(-0.35f, 0.35f));
                    }
                    shoot_time = 15;
                }
            }
        }

        //Aim Circlet
        float circlet_angle = Mathf.Atan2(aim.transform.position.y - transform.position.y, aim.transform.position.x - transform.position.x);
        circlet.transform.position = new Vector3(transform.position.x + (1.1f * Mathf.Cos(circlet_angle)), transform.position.y + (1.1f * Mathf.Sin(circlet_angle)), circlet.transform.position.z);

        //Animations
        anim.Play(anim_name);
        if (facing != 0){
            if (facing == 1){
                sr.flipX = false;
            }
            else {
                sr.flipX = true;
            }
        }

        //Health
        e_key = false; //Debug
        if (e_key){
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            for (int e = 0; e < enemies.Length; e++){
                Destroy(enemies[e].gameObject);
            }
        }
        float inv_alpha = 1;
        if (invincible_time > 0){
            invincible_time--;
            if (invincible_time % 8 < 5){
                inv_alpha = 0.4f;
            }
            else {
                inv_alpha = 0.8f;
            }
        }
        sr.color = new Color(1, 1, 1, inv_alpha);
        for (int g = 0; g < health_gui.Length; g++){
            if (health < g){
                if (health_gui[g] != null){
                    Destroy(health_gui[g].gameObject);
                }
            }
        }
        if (health < 0){
            GameObject corpse = Instantiate(dead_obj, transform.position, transform.rotation);
            corpse.GetComponent<SpriteRenderer>().flipX = sr.flipX;

            if (aim != null){
                Destroy(aim.gameObject);
            }
            if (circlet != null){
                Destroy(circlet.gameObject);
            }
            Destroy(gameObject);
        }
	}

    //Physics
    void FixedUpdate () {
        Vector2 v_spd = new Vector2(0f, 0f);

        if (!dash){
            if (w_key){
                v_spd.y = spd;
            }
            else if (s_key){
                v_spd.y = -spd;
            }

            if (a_key){
                v_spd.x = -spd;
            }
            else if (d_key){
                v_spd.x = spd;
            }
        }
        else {
            if (dash_direction == 1){
                v_spd.y = dash_spd;
            }
            else if (dash_direction == 2){
                v_spd.y = -dash_spd;
            }
            else if (dash_direction == 3){
                v_spd.x = -dash_spd;
            }
            else if (dash_direction == 4){
                v_spd.x = dash_spd;
            }
            dash_spd *= 0.95f;
        }

        rb.velocity = v_spd;
    }

    //Hit Collisions
    void OnTriggerEnter2D(Collider2D col){
        if (col.gameObject.tag == "EnemyBullet"){
            if (!dash){
                Destroy(col.gameObject);
                playerHit();
            }
        }
    }

    //Public Variable Methods
    public void playerHit(){
        if (invincible_time <= 0){
            health--;
            invincible_time = 80;
            for (int q = 0; q < 12; q++){
                Instantiate(hit_obj, transform.position, transform.rotation);
            }
        }
    }

    public bool Canmove {
        get {
            return canmove;
        }
    }

    public bool Dash {
        get {
            return dash;
        }
    }
}
