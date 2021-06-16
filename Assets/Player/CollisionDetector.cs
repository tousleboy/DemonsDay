using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public bool beingAttacked = false;
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
            Invoke("reset", 0.5f);
            Debug.Log("danger");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            beingAttacked = false;
            Debug.Log("danger");
        }
    }

    void reset()
    {
        beingAttacked = false;
    }
}
