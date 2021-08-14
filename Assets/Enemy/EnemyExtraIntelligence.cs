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

    bool wait = false;

    int punchP = 70;
    int kickP = 90;
    //int bodyP = 90;
    //int highP = 90;
    int breakP = 0;
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
        if(ec == null || pc == null || pam == null || wait)
        {
            return;
        }

        if(pc.attacking && !pc.parry && !pc.cut && ec.isPlayerNear && (ec.gap || ec.parry || ec.cut) && !pam.guardBreak && !ec.backStepping && !ec.damaged)
        {
            if(!ec.parry && !ec.cut && Probability(50))
            {
                ec.goBackStep = true;
            }
            else if(pam.state == "high" && !ec.parry && Probability(punchP))
            {
                animator.SetTrigger("parry");
            }
            else if(pam.state == "low" && !ec.cut && Probability(kickP))
            {
                animator.SetTrigger("cut");
            }
        }
        if(pc.attacking && pam.guardBreak && pam.state == "high" && !ec.damaged && Probability(breakP)) animator.SetTrigger("duck");

        if(ec.damaged && pam.guardBreak && pam.state == "high" && breakP == 0) breakP = 80;


    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        int i = 10;
        int j = 10;
        if(collision.gameObject.tag == "Attack")
        {
            if(!ec.parry && !ec.cut)
            {
                punchP = Mathf.Min(punchP + 100, 100);
                kickP = Mathf.Min(kickP + 100, 100);
            }
            if(ec.parry)
            {
                punchP = Mathf.Min(punchP + i, 100);
                kickP = Mathf.Max(kickP - j, 0);
            }
            if(ec.cut)
            {
                punchP = Mathf.Max(punchP - j, 0);
                kickP = Mathf.Min(kickP + i, 100);
            }
        }
    }

    bool Probability(int p)
    {
        int n = Random.Range(1, 100);
        Debug.Log(n);
        if(p >= n) return true;
        else
        {
            wait = true;
            Invoke("Restart", 0.3f);
            return false;
        }
    }

    void Restart()
    {
        wait = false;
    }
}
