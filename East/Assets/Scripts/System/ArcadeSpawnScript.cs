using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcadeSpawnScript : MonoBehaviour {

    //Objects
    [SerializeField] private GameObject grid;

    //Enemies
    [SerializeField] private GameObject droid;
    [SerializeField] private GameObject ghost;
    [SerializeField] private GameObject turret;
    [SerializeField] private GameObject tank;

    //Settings
    private bool wave_break;
    private bool start_break;
    private Vector3[] spawn_array;
    private int spawn_limit;
    private int wave_num;

    private float droid_chance;
    private float ghost_chance;
    private float turret_chance;
    private float tank_chance;

    //Variables
    private float timer;

    //Init
	void Start () {
        //Settings
        wave_break = true;
        start_break = true;
        
        GameObject[] spawn_objs = GameObject.FindGameObjectsWithTag("Spawner");
        spawn_array = new Vector3[spawn_objs.Length];
        for (int i = 0; i < spawn_objs.Length; i++){
            spawn_array[i] = new Vector3(spawn_objs[i].transform.position.x, spawn_objs[i].transform.position.y, 50f);
        }
        
        spawn_limit = 60;
        wave_num = 0;

        //Variables
		timer = 180;

        droid_chance = 100f;
        ghost_chance = 0f;
        turret_chance = 0f;
        tank_chance = 0f;
	}
	
	//Update
	void Update () {
        int num = GameObject.FindGameObjectsWithTag("Enemy").Length;
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (!wave_break){
            if (player != null){
                if (num < spawn_limit){
                    wave_break = true;
                    if (wave_num < 32){
                        wave_num++;
                    }
                    timer = Random.Range(340, 480);
                    int spawn_num = Random.Range(12, 16);
                    for (int i = 0; i < spawn_num; i++){
                        int spawn_chance = Random.Range(0, 100);
                        Vector3 spawn_position = spawn_array[Random.Range(0, spawn_array.Length - 1)];

                        if (spawn_chance < 5){
                            //Spawn Tank
                            GameObject spawn_tank = Instantiate(tank, spawn_position, transform.rotation);
                            spawn_tank.GetComponent<PathScript>().Grid = grid.GetComponent<Grid>();
                        }
                        else if (spawn_chance < 15){
                            //Spawn Turret
                            Instantiate(turret, spawn_position, transform.rotation);
                        }
                        else if (spawn_chance < 25){
                            //Spawn Ghost
                            Instantiate(ghost, spawn_position, transform.rotation);
                        }
                        else {
                            Instantiate(droid, spawn_position, transform.rotation);
                        }
                    }
                }
            }
        }
        else {
            timer--;
            if (timer < 0){
                wave_break = false;
                start_break = false;
            }
            else if (timer > 30){
                if (!start_break){
                    if (num < 12){
                        timer = 30;
                    }
                }
            }
        }

        droid_chance = 100f - ((wave_num / 32f) * 2);
        ghost_chance = (((wave_num / 32f) * 2) / 35f) * 20;
        turret_chance = (((wave_num / 32f) * 2) / 35f) * 10;
        tank_chance = ((wave_num / 32f) / 35f) * 5;
	}
}
