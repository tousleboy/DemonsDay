using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaffoldManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Rigidbody2D rbody = collision.gameObject.GetComponent<Rigidbody2D>();
        if(rbody == null)
        {
            return;
        }
        if(rbody.velocity.y <= 0)
        {
            GetComponent<BoxCollider2D>().isTrigger = false;
            gameObject.layer = 8;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        GetComponent<BoxCollider2D>().isTrigger = true;
        gameObject.layer = 0;
    }
}
