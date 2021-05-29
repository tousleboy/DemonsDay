using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    GameObject npc;
    Animator npcAnimator;
    // Start is called before the first frame update
    void Start()
    {
        npc = transform.Find("Npc").gameObject;
        npcAnimator = npc.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            npcAnimator.SetTrigger("Goal");
            //PlayerController pc = collision.gameObject.GetComponent<PlayerController>();

        }
    }
}
