using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningManScript : MonoBehaviour
{
    public float range = 10.0f;
    public float speed = 2.0f;
    float howfar = 0.0f;
    bool activated = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(activated && howfar <= range)
        {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            howfar += speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" && !activated) activated = true;
    }
}
