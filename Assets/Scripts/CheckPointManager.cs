using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    GameObject paint;
    public static bool active = false;
    // Start is called before the first frame update
    void Start()
    {
        paint = transform.Find("paint").gameObject;
        paint.SetActive(active);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(Input.GetAxisRaw("Vertical") > 0 && !active)
            {
                paint.SetActive(true);
                active = true;
            }
        }
    }
}
