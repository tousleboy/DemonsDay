using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloorScript : MonoBehaviour
{
    public float a = 10f;
    public float w = 5.0f;
    public MovingFloorToggle togle;
    float theta = 0;
    Vector3 original;

    public enum DIRECTION
    {
        holizontal,//horizontal
        vertical
    }

    public DIRECTION direction = DIRECTION.vertical;

    GameObject player;
    Rigidbody2D rbodyP;
    // Start is called before the first frame update
    void Start()
    {
        original = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(togle != null && !togle.active)
        {
            return;
        }
        theta = theta + w * Time.deltaTime;
        if(theta >= 360) theta = 0;

        if(direction == DIRECTION.vertical)
        {
            float y = original.y + a * Mathf.Sin(Mathf.Deg2Rad * theta);
            transform.position = new Vector2(transform.position.x, y);
        }
        else
        {
            float x = original.x + a * Mathf.Sin(Mathf.Deg2Rad * theta);
            transform.position = new Vector2(x, transform.position.y);
        }

        /*if(rbodyP != null && direction == DIRECTION.holizontal)
        {
            if(rbodyP.velocity.x != 0) player.transform.SetParent(null);
            else player.transform.SetParent(transform);
        }*/
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            //float x = collision.transform.localScale.x / transform.localScale.x;
            //float y = collision.transform.localScale.y / transform.localScale.y;
            //float z = collision.transform.localScale.z / transform.localScale.z;
            collision.transform.SetParent(transform);
            //collision.transform.localScale = new Vector3(x, y, z);
            if(collision.gameObject.tag == "Player")
            {
                player = collision.gameObject;
                rbodyP = player.GetComponent<Rigidbody2D>();
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            //float x = collision.transform.localScale.x * transform.localScale.x;
            //float y = collision.transform.localScale.y * transform.localScale.y;
            //float z = collision.transform.localScale.z * transform.localScale.z;
            collision.transform.SetParent(null);
            //collision.transform.localScale = new Vector3(x, y, z);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            //float x = collision.transform.localScale.x * transform.localScale.x;
            //float y = collision.transform.localScale.y * transform.localScale.y;
            //float z = collision.transform.localScale.z * transform.localScale.z;
            collision.transform.SetParent(null);
            //collision.transform.localScale = new Vector3(x, y, z);

            player = null;
            rbodyP = null;
        }
    }
}
