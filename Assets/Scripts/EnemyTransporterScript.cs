using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTransporterScript : MonoBehaviour
{
    public GameObject pair;
    EnemyTransporterScript ets;
    [System.NonSerialized]
    public bool active;
    [System.NonSerialized]
    public bool enemyOut = true;
    // Start is called before the first frame update
    void Start()
    {
        ets = pair.GetComponent<EnemyTransporterScript>();
        active = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(ets.enemyOut && enemyOut) active = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy" && active)
        {
            Rigidbody2D rbody = collision.gameObject.GetComponent<Rigidbody2D>();
            rbody.velocity = new Vector2(0, rbody.velocity.y);
            ets.active = false;
            enemyOut = false;
            collision.gameObject.transform.position = pair.transform.position;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            enemyOut = true;
        }
    }
}
