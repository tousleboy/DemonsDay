using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public bool beingAttacked = false;
    public int chain = 1;
    public bool nextToEnemy = false;
    //PlayerController pc;
    // Start is called before the first frame update
    void Start()
    {
       //pc = GetComponent<PlayerController>(); 
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            beingAttacked = true;
            chain = collision.gameObject.GetComponent<AttackManager>().chain;
            //Invoke("reset", 0.5f);
            //Debug.Log("danger");
        }
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            nextToEnemy = true;
            Debug.Log("enemyin");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            beingAttacked = false;
            Debug.Log("dangergone");
        }
        if(collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Boss")
        {
            nextToEnemy = false;
        }
    }

    void reset()
    {
        beingAttacked = false;
    }
}
