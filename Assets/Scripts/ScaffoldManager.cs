using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaffoldManager : MonoBehaviour
{
    bool isPlayerOn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButton("Jump") && Input.GetAxisRaw("Vertical") < 0 && isPlayerOn)
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
            gameObject.layer = 0;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rbody = collision.gameObject.GetComponent<Rigidbody2D>();
        if(rbody == null)
        {
            return;
        }
        if(rbody.velocity.y <= 0 )//&& transform.position.y <= collision.gameObject.transform.position.y)
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
            gameObject.layer = 8;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            isPlayerOn = true;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GetComponent<BoxCollider2D>().isTrigger = true;
            isPlayerOn = false;
            gameObject.layer = 0;
        }
    }
}
