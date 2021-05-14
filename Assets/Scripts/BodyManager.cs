using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyManager : MonoBehaviour
{
    public int damage = 0;
    public string collisionState;
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
        if(collision.gameObject.tag == "Attack")
        {
            Debug.Log("hit");
            AttackManager am = collision.gameObject.GetComponent<AttackManager>();
            collisionState = am.state;
            damage = am.val;

            if(am.knockBack)
            {
                Debug.Log(transform.parent.gameObject);
                am.KnockBack(transform.parent.gameObject);
            }
        }
    }
}
