using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{
    public float minY = 0f;
    public LayerMask groundLayer;
    //bool onGround = false;
    Transform player;
    Rigidbody2D rbody;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y < minY)
        {
            gameObject.SetActive(false);
        }
        else
        {
            transform.localScale = new Vector3(Mathf.Sign(player.position.x - transform.position.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    void FixedUpdate()
    {
        /*onGround = Physics2D.Linecast(transform.position, transform.position -(transform.up * 0.1f), groundLayer);
        if(onGround) rbody.velocity = rbody.velocity / 2f;*/
    }

    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
}
