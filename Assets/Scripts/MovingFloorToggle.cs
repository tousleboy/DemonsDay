using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingFloorToggle : MonoBehaviour
{
    public GameObject anchor1;
    public GameObject anchor2;
    float left;
    float right;

    [System.NonSerialized]
    public bool active = false;

    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        active = false;
        player = GameObject.FindGameObjectWithTag("Player");
        left = anchor1.transform.position.x;
        right = anchor2.transform.position.x;
        if(left > right)
        {
            float tmp = left;
            left = right; right = tmp;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float x = player.transform.position.x;
        if(x >= left && x <= right && !active) active = true;
        if((x < left || x > right) && active) active = false;
    }
}
