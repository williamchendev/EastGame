using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	[SerializeField] private GameObject follow_obj;
    private float displace_in;

    void Start () {
        displace_in = 0;
    }

    void FixedUpdate () {
        if (follow_obj != null){
            Vector2 cam_position = new Vector2(transform.position.x, transform.position.y);

            float displace_distance = 1.5f;
            float x_displace = 0;
            float y_displace = 0;

            if (follow_obj.GetComponent<PlayerBehavior>().Canmove){
                Rigidbody2D rb = follow_obj.GetComponent<Rigidbody2D>();
                if (rb.velocity.x != 0 || rb.velocity.y != 0){
                    if (displace_in < 1){
                        displace_in += 0.05f;
                        if (displace_in > 1){
                            displace_in = 1;
                        }
                    }
                }
                else {
                    if (displace_in > 0){
                        displace_in -= 0.05f;
                        if (displace_in < 0){
                            displace_in = 0;
                        }
                    }
                }

                if (rb.velocity.x < 0){
                    x_displace = -displace_distance * displace_in;
                }
                else if (rb.velocity.x > 0){
                    x_displace = displace_distance * displace_in;
                }

                if (rb.velocity.y < 0){
                    y_displace = -displace_distance * displace_in;
                }
                else if (rb.velocity.y > 0){
                    y_displace = displace_distance * displace_in;
                }
            }

            Vector2 follow_position = new Vector2(follow_obj.transform.position.x + x_displace, follow_obj.transform.position.y + y_displace);
            Vector2 new_position =  cam_position + ((follow_position - cam_position) * 0.05f);
            transform.position = new Vector3(new_position.x, new_position.y, transform.position.z);
        }
    }

    public bool objectVisible(Vector3 position){
        bool is_visible = false;
        if (Mathf.Abs(transform.position.x - position.x) < 10){
            if (Mathf.Abs(transform.position.y - position.y) < 6){
                is_visible = true;
            }
        }
        return is_visible;
    }
}
