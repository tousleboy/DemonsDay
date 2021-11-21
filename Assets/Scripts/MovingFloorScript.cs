using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloorScript : MonoBehaviour
{
    public float a = 10f;
    public float w = 5.0f;
    float theta = 0;
    Vector3 original;

    public enum DIRECTION
    {
        holizontal,
        vertical
    }

    public DIRECTION direction = DIRECTION.vertical;
    // Start is called before the first frame update
    void Start()
    {
        original = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //float x = collision.transform.localScale.x / transform.localScale.x;
            //float y = collision.transform.localScale.y / transform.localScale.y;
            //float z = collision.transform.localScale.z / transform.localScale.z;
            collision.transform.SetParent(transform);
            //collision.transform.localScale = new Vector3(x, y, z);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
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
        if(collision.gameObject.tag == "Player")
        {
            //float x = collision.transform.localScale.x * transform.localScale.x;
            //float y = collision.transform.localScale.y * transform.localScale.y;
            //float z = collision.transform.localScale.z * transform.localScale.z;
            collision.transform.SetParent(null);
            //collision.transform.localScale = new Vector3(x, y, z);
        }
    }
}
