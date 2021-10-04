using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameObject player;
    Vector2 playerPos;
    public float range = 8.0f;
    public float maai = 1.2f;
    public float speed = 10.0f;
    bool onGround = false;
    public LayerMask groundLayer;
    public bool attacking = false;
    public bool gap = false;
    bool goAttack = false;
    public bool isPlayerNear = false;
    bool restrainting = false;
    bool scaleFix = false;

    public bool backStepping = false;
    public bool goBackStep = false;
    float backStep = 6.0f;

    Animator animator;
    //public string stopAnime = "RichmenIdle";
    //public string runAnime = "RichmenRun";
    public string style = "swarmer";
    public string comboAnime = "onetwo gap exit";
    public string[] nextComboAnimes;
    public string restraintAnime = "";
    int comboChangeInterval;
    int comboChangeNum = 0;
    string[] comboStack;
    string[] restraintStack;
    int stackPointer = 0;
    int stackLen;
    int rStackPointer = 0;
    int rStackLen;
    //public string damagedAnime = "RichmenDamaged";
    //public string deadAnime = "RichmenDead";
    //string nowAnime;
    //string oldAnime;

    int enemyLife;
    public int maxLife = 8;
    int damage = 0;
    string collisionState;
    public bool damaged = false;
    public bool moving = false;
    //public bool blocking = false;
    public bool parry = false;
    public bool cut = false;
    public bool ducking = false;
    public bool dramaticWhenDie = false;
    int diffence = 1;
    bool dead = false;

    GameObject attackZone;
    AttackManager am;
    BoxCollider2D abc;
    //public bool high = false;
    //public bool low = false;
    //public bool knee = false;
    //bool isCalledFirst = true;

    Rigidbody2D rbody;
    PlayerController pc;

    AudioSource soundPlayer;
    public AudioClip punch;
    public AudioClip punchHit;
    public AudioClip heavyHit;
    public AudioClip guardHit;
    public AudioClip oh;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform.position;
        enemyLife = maxLife;
        animator = GetComponent<Animator>();
        //nowAnime = stopAnime;
        //oldAnime = stopAnime;
        attackZone = transform.Find("AttackZone").gameObject;
        if(attackZone != null)
        {
            am = attackZone.GetComponent<AttackManager>();
            abc = attackZone.GetComponent<BoxCollider2D>();
        }
        soundPlayer = GetComponent<AudioSource>();

        comboStack = comboAnime.Split(' ');
        stackLen = comboStack.Length;

        if(style == "slugger")
        {
            restraintStack = restraintAnime.Split(' ');
            rStackLen = restraintStack.Length;
        }

        rbody = GetComponent<Rigidbody2D>();
        pc = player.GetComponent<PlayerController>();

        comboChangeInterval = maxLife / (nextComboAnimes.Length + 1);

        Debug.Log(stackLen + "stacks");
    }

    // Update is called once per frame
    void Update()
    {
        /*if(damage > 0)
        {
            enemyLife -= damage;
            damage = 0;
            if(gap)
            {
                Damaged();
            }
            GetComponent<Renderer>().material.color = Color.red;
            Invoke("ColorReset", 0.1f);
        }*/

        if(pc.onGround)
        {
            playerPos = player.transform.position;
        }

        /*if(moving || dead)
        {
            return;
        }*/
        
        float xPosition = transform.position.x;
        float xPlayerPosition = playerPos.x;
        if(!scaleFix)
        {
            if(xPosition - xPlayerPosition > 0)
            {
                transform.localScale = new Vector2(-1, 1);
            }
            else if(xPosition - xPlayerPosition < 0)
            {
                transform.localScale = new Vector2(1, 1);
            }
        }

        if(enemyLife <= 0 && !dead)
        {
            Dead();
        }
    }

    void FixedUpdate()
    {
        onGround = Physics2D.Linecast(transform.position, transform.position -(transform.up * 0.1f), groundLayer);
        isPlayerNear = CheckLength(playerPos, maai);

        animator.SetBool("jump", !onGround);

        if(damaged || dead || backStepping)
        {
            return;
        }

        if(onGround && attacking) rbody.velocity = new Vector2(0.0f, rbody.velocity.y);

        if(PlayerController.gameState != "playing")
        {
            rbody.velocity = new Vector2(0.0f, rbody.velocity.y);
            moving = false;
            animator.SetBool("move", false);
            if(style == "slugger")
            {
                StopRestraint();
            }
            return;
        }

        if(goBackStep)
        {
            Vector2 backStepPw = new Vector2(backStep * transform.localScale.x * -1, 0);
            rbody.AddForce(backStepPw, ForceMode2D.Impulse);
            animator.SetTrigger("backstep");
            goBackStep = false;
            return;
        }

        if(style == "swarmer")
        {
            if(goAttack)
            {
                attacking = true;
                Combo();
                goAttack = false;
            }
            else if(CheckLength(playerPos, range) && onGround && !attacking && !backStepping)
            {
                if(isPlayerNear)
                {
                    rbody.velocity = new Vector2(0.0f, rbody.velocity.y);
                    goAttack = true;
                    moving = false;
                    animator.SetBool("move", false);
                }
                else
                {
                    rbody.velocity = new Vector2(speed * transform.localScale.x, rbody.velocity.y);
                    moving = true;
                    animator.SetBool("move", true);
                }
            }
        }
        else if(style == "slugger")
        {
            if(goAttack)
            {
                attacking = true;
                Combo();
                goAttack = false;
            }
            if(attacking)
            {
                return;
            }
            if(moving)
            {
                rbody.velocity = new Vector2(speed * transform.localScale.x, rbody.velocity.y);
            }

            if(!isPlayerNear)//restraint
            {
                if(!restrainting) Restraint();
            }
            else
            {
                StopRestraint();

                if(moving)//tackle
                {
                    rbody.velocity = new Vector2(0.0f, rbody.velocity.y);
                    moving = false;
                    animator.SetBool("move", false);
                    animator.SetTrigger("charge");
                }
                else//kick or upper
                {
                    rbody.velocity = new Vector2(0.0f, rbody.velocity.y);
                    goAttack = true;
                    moving = false;
                    animator.SetBool("move", false);
                }
            }
        }
        
        /*if(attacking)
        {
            if(high)
            {
                am.state = "high";
            }
            else if(low)
            {
                am.state = "low";
            }
            else if(knee)
            {
                am.state = "knee";
            }
            if(abc.enabled == true)
            {
                if(isCalledFirst == true)
                {
                    soundPlayer.PlayOneShot(punch);
                    isCalledFirst = false;
                }
            }
            else
            {
                isCalledFirst = true;
            }
        }*/

        //if(!attacking && !damaged)
        //{
            /*if(goAttack)
            {
                Combo();
                goAttack = false;
            }
            /*else
            {
                if(moving)
                {
                    //nowAnime = runAnime;
                    animator.SetTrigger("move");
                }
                else
                {
                    nowAnime = stopAnime;
                }
            }
        //}
        /*if(nowAnime != oldAnime)
        {
            animator.Play(nowAnime);
            oldAnime = nowAnime;
        }

        /*if(attacking && CheckLength(player.transform.position, maai))
        {
            if(blocking && pc.ducking)
            {
                animator.SetTrigger("KneeKick");
                blocking = false;
            }
        }*/
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack" && !backStepping)
        {
            Debug.Log("hit" + enemyLife);
            AttackManager am = collision.gameObject.GetComponent<AttackManager>();
            collisionState = am.state;
            damage = am.val;
            bool blockSuccess = false;

            if(!ducking && collisionState == "knee")
            {
                return;
            }
            if((collisionState == "high" && parry || collisionState == "low" && cut) && !am.guardBreak)
            {
                damage -= diffence;
                soundPlayer.PlayOneShot(guardHit);
                blockSuccess = true;
            }
            
            if(damage > 0 && !blockSuccess)
            {
                enemyLife -= damage;
                damage = 0;
                if(am.guardBreak) soundPlayer.PlayOneShot(heavyHit);
                if(gap || (am.knockBack && damaged) || am.guardBreak)
                {
                    Damaged();
                }
                else soundPlayer.PlayOneShot(punchHit);
                GetComponent<Renderer>().material.color = Color.red;
                Invoke("ColorReset", 0.1f);
            }

            if(am.knockBack && !blockSuccess)
            {
                am.KnockBack(gameObject);
            }
        }
        if(collision.gameObject.tag == "Dead")
        {
            enemyLife = 0;
            Damaged();
        }
    }

    IEnumerator SlowMotion()
    {
        Time.timeScale = 0.2f;
        yield return new WaitForSeconds(0.2f);
        Time.timeScale = 1.0f;
    }

    void Damaged()
    {
        soundPlayer.PlayOneShot(punchHit);
        attacking = false;
        goBackStep = false;
        stackPointer = 0;
        string[] warnings = new string[] {"様子が変わった!警戒!", "前となんか違う！", "動きが変わった！"};
        //animator.Play(damagedAnime, 0, 0);
        animator.SetTrigger("damage");
        //oldAnime = damagedAnime;
        if(comboChangeInterval != maxLife && comboChangeNum < nextComboAnimes.Length)
        {
            if(enemyLife <= maxLife - comboChangeInterval * (comboChangeNum + 1))
            {
                comboStack = nextComboAnimes[comboChangeNum].Split(' ');
                stackLen = comboStack.Length;
                PlayerController.messages = warnings[comboChangeNum % warnings.Length];
                comboChangeNum += 1;
            }
        }
    }

    void Dead()
    {
        dead = true;
        if(dramaticWhenDie) StartCoroutine("SlowMotion");
        /*GameObject shield = transform.Find("shield").gameObject;
        Destroy(shield);*/
        GetComponent<CapsuleCollider2D>().enabled = false;
        //animator.Play(deadAnime);
        animator.SetTrigger("die");
        PlayerController.concentration = Mathf.Max(PlayerController.concentration - 10, 0);
        GameManager.battleScore = Mathf.Min(GameManager.battleScore + 5, 100);
        GameManager.defeats += 1;
        Destroy(gameObject, 1.0f);
    }

    void ColorReset()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    bool CheckLength(Vector2 targetPos, float length)
    {
        float d = Vector2.Distance(transform.position, targetPos);
        return d < length && d != 0;
    }

    public void SetHigh()
    {
        am.state = "high";
    }
    public void SetLow()
    {
        am.state = "low";
    }
    public void SetGuardBreak()
    {
        am.state = "guardbreak";
    }
    public void PlaySound()
    {
        soundPlayer.PlayOneShot(punch);
    }

    public void Voice()
    {
        soundPlayer.PlayOneShot(oh);
    }

    public void BackStep()
    {
        Vector2 backStepPw = new Vector2(backStep * transform.localScale.x * -1, 0);
        rbody.AddForce(backStepPw, ForceMode2D.Impulse);
    }

    public void Combo()
    {
        string trigger;
        if(!isPlayerNear && (comboStack[stackPointer] == "gap"))
        {
            stackPointer = -1;
            trigger = "exit";
            attacking = false;
        }
        else if(comboStack[Mathf.Max(0, stackPointer - 1)] == "backstep")
        {
            trigger = "exit";
            Debug.Log("backstep");
            attacking = false;
        }
        else
        {
            trigger = comboStack[stackPointer];
            if(trigger == "exit")
            {
                attacking = false;
                Debug.Log("exit");
            }
        }

        animator.SetTrigger(trigger);
        stackPointer = (stackPointer + 1) % stackLen;
    }

    void SetAttacking()
    {
        attacking = true;
    }

    void ResetAttacking()
    {
        attacking = false;
    }

    void ScaleFix()
    {
        scaleFix = true;
    }

    void ScaleFree()
    {
        scaleFix = false;
    }

    public void Restraint()
    {
        string trigger;
        trigger = restraintStack[rStackPointer];
        restrainting = true;

        if(trigger == "idle")
        {
            rbody.velocity = new Vector2(0.0f, rbody.velocity.y);
            moving = false;
            animator.SetBool("move", false);
        }
        else if(trigger == "run")
        {
            moving = true;
            animator.SetBool("move", true);
        }

        Invoke("Restraint", 0.5f);
        rStackPointer = (rStackPointer + 1) % rStackLen;
    }

    void StopRestraint()
    {
        if(IsInvoking("Restraint"))
        {
            CancelInvoke("Restraint");
        }
        rStackPointer = 0;
        restrainting = false;
    }
}
