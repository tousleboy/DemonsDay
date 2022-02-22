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
    BoxCollider2D pamcol;
    BoxCollider2D eamcol;

    bool wait = false;

    AttackManager.ATTACKTYPE nowat;
    AttackManager.ATTACKTYPE pastat;
    bool nowPamcol;
    bool pastPamcol;
    bool pulse = false;

    int punchP = 100;
    int kickP = 100;
    //int bodyP = 90;
    //int highP = 90;
    int stepP = 15;
    int duckP = 0;
    // Start is called before the first frame update
    void Start()
    {
        ec = GetComponent<EnemyController>();
        eamcol = transform.Find("AttackZone").gameObject.GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>();
        pam = player.transform.Find("AttackZone").gameObject.GetComponent<AttackManager>();
        pamcol = player.transform.Find("AttackZone").gameObject.GetComponent<BoxCollider2D>();

        nowat = pam.attackType;
        pastat = pam.attackType;
        nowPamcol = pamcol.enabled;
        pastPamcol = nowPamcol;
    }

    // Update is called once per frame
    void Update()
    {
        if(ec == null || pc == null || pam == null || wait)
        {
            return;
        }

        /*if(pc.attacking && !pc.parry && !pc.cut && ec.isPlayerNear && (ec.gap || ec.parry || ec.cut) && !pam.guardBreak && !ec.backStepping && !ec.damaged)
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

        if(ec.damaged && pam.guardBreak && pam.state == "high" && breakP == 0) breakP = 80;*/

        nowat = pam.attackType;
        nowPamcol = pamcol.enabled;
        if(nowat != pastat)
        {
            if(nowat != AttackManager.ATTACKTYPE.none)
            {
                pulse = true;
                Debug.Log("pulse");
            }
            pastat = nowat;
        }
        else if(pulse) pulse = false;


        if(ec.isPlayerNear && pulse && !ec.damaged && !eamcol.enabled)
        {
            if(Probability(duckP) && pam.attackType == AttackManager.ATTACKTYPE.kick)
            {
                Escape();
            }
            else if(Probability(stepP) && (pam.attackType == AttackManager.ATTACKTYPE.jab || pam.attackType == AttackManager.ATTACKTYPE.lowkick))
            {
                Escape();
            }
            
            else
            {
                if(pam.attackType == AttackManager.ATTACKTYPE.jab || pam.attackType == AttackManager.ATTACKTYPE.upper || pam.attackType == AttackManager.ATTACKTYPE.jodankick || pam.attackType == AttackManager.ATTACKTYPE.kick)
                {
                    if(Probability(punchP)) animator.SetTrigger("parry1");
                }
                else if(pam.attackType == AttackManager.ATTACKTYPE.straight)
                {
                    if(Probability(punchP)) animator.SetTrigger("parry2");
                }
                
                if(pam.attackType == AttackManager.ATTACKTYPE.lowkick || pam.attackType == AttackManager.ATTACKTYPE.middlekick || pam.attackType == AttackManager.ATTACKTYPE.chudankick)
                {
                    if(Probability(kickP)) animator.SetTrigger("cut");
                }
            }

            pulse = false;

            
        }


    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        int i = 15;
        int j = 30;
        if(collision.gameObject.tag == "Attack")
        {
            if(!ec.parry && !ec.cut)
            {
                punchP = Mathf.Min(punchP + 50, 100);
                kickP = Mathf.Min(kickP + 50, 100);
                if(pam.attackType == AttackManager.ATTACKTYPE.kick) duckP = 80;
            }
            else
            {
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

            if(pam.knockBack && ec.gap && !ec.damaged)
            {
                Invoke("Escape", 0.2f);
            }

            Debug.Log(punchP);
            Debug.Log(kickP);

            //PlayerController.concentration += 1;
        }
    }

    bool Probability(int p)
    {
        int n = Random.Range(1, 100);
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

    void Escape()
    {
        if(!ec.damaged) animator.SetTrigger("backstep");
    }
}
