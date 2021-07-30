using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExtraIntelligence : MonoBehaviour
{
    EnemyController ec;
    Animator animator;
    GameObject player;
    PlayerController pc;
    AttackManager pam;

    bool canDuck = false;
    // Start is called before the first frame update
    void Start()
    {
        ec = GetComponent<EnemyController>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>();
        pam = player.transform.Find("AttackZone").gameObject.GetComponent<AttackManager>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(ec == null || pc == null || pam == null)
        {
            return;
        }

        if(pc.attacking && !pc.parry && !pc.cut && ec.isPlayerNear &&ec.gap)
        {
            if(pam.state == "high" && !ec.parry) animator.SetTrigger("parry");
            if(pam.state == "low" && !ec.cut) animator.SetTrigger("cut");
        }
        if(pc.attacking && pam.guardBreak && pam.state == "high" && canDuck && !ec.damaged) animator.SetTrigger("duck");

        if(ec.damaged && pam.guardBreak && pam.state == "high" && !canDuck) canDuck = true;
    }
}
