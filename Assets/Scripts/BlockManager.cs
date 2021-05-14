using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public int val = 1;
    public bool blocked = false;

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
            Debug.Log("Blocked");
            blocked = true;
        }
    }

    public void ResetStatus()//親が必ず呼び出す
    {
        blocked = false;
    }
}
