using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rbody;
    public float runSpeed = 3.0f;
    float axisH = 0.0f;

    public float jump = 9.0f;
    public LayerMask groundLayer;
    bool goJump = false;
    public bool onGround = false;
    public int howManyJump = 2;
    int jumpCount = 0;
    bool jumpStop = false;

    public float backStep = 1.0f;
    bool goBackStep;
    public bool backStepping;

    Animator animator;
    public string stopAnime = "PlayerIdle";
    public string runAnime = "PlayerRun";
    public string jumpAnime = "PlayerJump";
    public string jumpUpAnime = "PlayerJumpUp";
    public string[] comboAnimes;
    public string[] kickAnimes;
    public string[] parryAnimes;
    public string[] cutAnimes;
    //public string jabAnime = "PlayerJab";
    public string blockAnime = "PlayerBlock";
    public string elbowBlockAnime = "PlayerElbowBlock";
    public string duckingAnime = "PlayerDuck";
    public string pearingAnime = "PlayerLeftPearing";
    public string bodyJabAnime = "PlayerBodyJab";
    public string kneeKickAnime = "PlayerKneeKick";
    public string damagedAnime = "PlayerDamaged";
    //public string straightAnime = "PlayerStraight";
    //public string kickAnime = "PlayerKick";
    string nowAnime;
    string oldAnime;
    string actionAnime;

    bool goAttack = false;
    public bool attacking = false;//アニメーション内で制御
    //bool delay = false;
    string attackTrigger;
    //bool goBlock = false;
    public bool blocking = false;
    public bool parry = false;
    public bool cut = false;
    //bool goDuck = false;
    public bool ducking = false;
    public bool combo = false;//アニメーション内で制御
    public bool comboFinisher = false;//アニメーション内で制御
    public int combocount = 1;
    int maxcombo;
    public float comboInterval = 0.5f;
    //float pushTimes = 0.0f;
    //public bool high = false;
    //public bool low = false;
    AttackManager amgr;
    CollisionDetector cd;

    public static int life;
    public static int maxLife = 6;
    public int damage;
    public int diffence = 1;
    public bool damaged = false;
    string collisionState;

    public static string gameState;
    public static int score = 0;

    public static int concentration = 0;
    GameObject conEffect;

    AudioSource soundPlayer;
    public AudioClip punch;
    public AudioClip punchHit;
    public AudioClip heavyHit;
    public AudioClip guardHit;
    public AudioClip moneySound;
    public AudioClip eat;
    public AudioClip slow;
    public AudioClip quitSlow;
    public AudioClip concentrationSound;

    public static string messages = ""; //recieve message from talk event. default should be "not recieved" 
    public string texts = "";


    GameObject frontFoot;
    GameObject backFoot;
    GameObject backP;
    bool back = false;

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;
        amgr = transform.Find("AttackZone").gameObject.GetComponent<AttackManager>();
        cd = transform.Find("CollisionDetector").gameObject.GetComponent<CollisionDetector>();
        maxcombo = comboAnimes.Length;
        life = maxLife;
        concentration = 0;
        gameState = "playing";
        soundPlayer = GetComponent<AudioSource>();
        //StartCoroutine("Deconcentration");
        conEffect = transform.Find("ConcentrationEffect").gameObject;

        frontFoot = transform.Find("FrontFoot").gameObject;
        backFoot = transform.Find("BackFoot").gameObject;
        backP = transform.Find("Back").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(texts != "")
        {
            messages = texts;
            texts = "";
        }

        if(gameState != "playing")
        {
            return;
        }

        //Debug.Log(GameManager.battleScore);

        axisH = Input.GetAxisRaw("Horizontal");
        //Debug.Log("texts" + texts +  " messages" + messages);
        if(concentration >= ConcentrateGaugeManager.maxCon)
        {
            if(!conEffect.activeSelf)
            {
                conEffect.SetActive(true);
                soundPlayer.PlayOneShot(concentrationSound);
            }
        }
        else if(conEffect.activeSelf) conEffect.SetActive(false);
        //passedTimes += Time.deltaTime;

        /*if(damage > 0)
        {
            if(collisionState == "high" && blocking)
            {
                damage -= diffence;
            }

            life -= damage;

            if(damage > 0)
            {
                Damage();
            }

            damage = 0;
        }*/
        if(!backStepping && !attacking && !damaged)
        {
            if(cd.nextToEnemy && axisH * transform.localScale.x < 0 && back)
            {
                goBackStep = true;
            }
            else if(axisH > 0.0f)
            {
                transform.localScale = new Vector2(1, 1);
            }
            else if(axisH < 0.0f)
            {
                transform.localScale = new Vector2(-1, 1);
            }

            if(Input.GetButtonDown("Jump") && onGround)
            {
                Jump();
            }
            if(Input.GetButtonUp("Jump") && !onGround && rbody.velocity.y > 0)
            {
                jumpStop = true;
            }
        }

        if(Input.GetKeyDown("j") || Input.GetButtonDown("Fire1"))
        {
            //StartCoroutine("PushJ");
            Attack("punch");
        }
        if(Input.GetKeyDown("k") || Input.GetButtonDown("Fire2"))
        {
            Attack("kick");
        }

        /*if(GetKeyDown("j"))
        {
            if(onGround && !damaged)
            {
                AttackManager am = attackZone.GetComponent<AttackManager>();

                if(IsInvoking("ComboReset"))
                {
                    CancelInvoke("ComboReset");
                }
                Invoke("ComboReset", comboInterval);
                blocking = false;

                /*if(blocking)
                {
                    am.state = "knee";
                    Action(kneeKickAnime, "attack");
                    blocking = false;
                }
                else if(ducking)
                {
                    am.state = "low";
                    Action(bodyJabAnime, "attack");
                }
                else if(!comboFinisher)
                {
                    am.state = "high";
                    actionAnime = comboAnimes[combocount - 1];
                    combocount = Mathf.Min(combocount + 1, maxcombo);
                    //passedTimes = 0.0f;
                }
                else
                {
                    ComboReset();
                }
            }
        }
        /*if(combocount > comboAnimes.Length || passedTimes > comboInterval)
        {
            combocount = 1;
        }*/

        /*if(Input.GetMouseButton(1) || Input.GetAxisRaw("Vertical") > 0)
        {
            if(!damaged)
            {
                blocking = true;
            }
            //Action(blockAnime, "block");
        }
        else if(Input.GetMouseButtonUp(1) || Input.GetAxisRaw("Vertical") <= 0 /*|| !Input.GetKey("j") || damaged)
        {
            blocking = false;
        }*/
        
        if(Input.GetAxisRaw("Vertical") < 0 && !damaged && !attacking)
        {
            if(!goAttack)
            {
                //Action(duckingAnime, "ducking");
                blocking = false;
                ducking = true;
            }
            if(goBackStep) goBackStep = false;
            //passedTimes = 0.0f;
        }
        else
        {
            ducking = false;
        }

        if(life <= 0)
        {
            PunchTriggerOff();
            KickTriggerOff();
            Die();
        }
    }

    void FixedUpdate()
    {
        if(gameState != "playing")
        {
            return;
        }

        Vector3 frontFootPos = frontFoot.transform.position;
        Vector3 backFootPos = backFoot.transform.position;
        Vector3 backPos = backP.transform.position;
        bool center = Physics2D.Linecast(transform.position, transform.position -(transform.up * 0.1f), groundLayer);
        bool frontf = Physics2D.Linecast(frontFootPos, frontFootPos -(transform.up * 0.1f), groundLayer);
        bool backf = Physics2D.Linecast(backFootPos, backFootPos -(transform.up * 0.1f), groundLayer);
        back = Physics2D.Linecast(backPos, backPos -(transform.up * 0.1f), groundLayer);
        //Debug.Log("center" + center + "vector" + transform.position);
        //Debug.Log("front" + front + "vector" + frontFootPos);
        //Debug.Log("back" + back + "vector" + backFootPos);
        onGround = center || frontf || backf; 
        //onGround = Physics2D.Linecast(transform.position, transform.position -(transform.up * 0.1f), groundLayer);

        if(damaged)
        {
            return;
        }

        if(attacking || blocking || ducking || backStepping)
        {
            axisH = 0;
            goJump = false;
            goBackStep = false;
        }

        if((onGround || axisH != 0) && !backStepping)//走り
        {
            rbody.velocity = new Vector2(axisH * runSpeed, rbody.velocity.y);
        }

        /*if(goBackStep)
        {
            Vector2 backStepPw = new Vector2(backStep * transform.localScale.x * -1, 0);
            rbody.AddForce(backStepPw, ForceMode2D.Impulse);
        }*/

        if((goJump && (jumpCount <= howManyJump)) || (onGround && goJump))//多段ジャンプ
        {
            Vector2 jumpPw = new Vector2(0, jump);
            rbody.AddForce(jumpPw, ForceMode2D.Impulse);
            goJump = false;
        }
        else if(onGround)
        {
            jumpCount = 0;
        }

        if(jumpStop)
        {
            //rbody.velocity = new Vector2(rbody.velocity.x, 0.0f);
            StartCoroutine("JumpStop");
            jumpStop = false;
        }

        if(!attacking)
        {
            if(onGround)
            {
                animator.SetBool("onground", true);
                animator.SetBool("duck", ducking);
                if(goAttack)
                {
                    //nowAnime = actionAnime;
                    animator.SetTrigger(attackTrigger);
                    goAttack = false;
                }
                else if(goBackStep)
                {
                    animator.SetTrigger("backstep");
                    goBackStep = false;
                }
                /*else if(blocking && ducking)
                {
                    nowAnime = elbowBlockAnime;
                }*/
                /*else if(blocking)
                {
                    nowAnime = blockAnime;
                }
                else if(ducking)
                {
                    nowAnime = duckingAnime;
                }*/
                else if(axisH == 0)
                {
                    //nowAnime = stopAnime;
                    animator.SetBool("move", false);
                }
                else
                {
                    //nowAnime = runAnime;
                    animator.SetBool("move", true);
                }
            }
            else
            {
                goAttack = false;
                animator.SetBool("onground", false);
                if(rbody.velocity.y > 0)
                {
                    animator.SetTrigger("jumpup");
                    //nowAnime = jumpUpAnime;
                }
                else
                {
                    animator.SetTrigger("jumpdown");
                    //nowAnime = jumpAnime;
                }
            }
        }
        else if(goAttack)
        {
            animator.SetTrigger(attackTrigger);
            //nowAnime = actionAnime;
            goAttack = false;
        }
        /*if(nowAnime != oldAnime)
        {
            oldAnime = nowAnime;
            animator.Play(nowAnime);
        }*/
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            Debug.Log("hit");
            AttackManager am = collision.gameObject.GetComponent<AttackManager>();
            //cd.beingAttacked = false;
            collisionState = am.state;
            damage = am.val;
            bool blockSuccess = false;

            if((collisionState == "high" && parry || collisionState == "low" && cut) && !am.guardBreak)
            {
                damage = 0;
                blockSuccess = true;
                soundPlayer.PlayOneShot(guardHit);
            }

            life -= damage;

            if((damage > 0 || am.knockBack) && !blockSuccess)
            {
                Damage();
                if(damage > 0)
                {
                    GetComponent<Renderer>().material.color = Color.red;
                    Invoke("ColorReset", 0.1f);
                }
            }
            if(am.knockBack && !blockSuccess)
            {
                am.KnockBack(gameObject);
            }

            damage = 0;
        }
        if(collision.gameObject.tag == "Dead")
        {
            life = 0;
            Damage();
        }
        if(collision.gameObject.tag == "Item")
        {
            //Debug.Log(collision.gameObject.GetComponent<ItemData>().val + "yen get");
            score = collision.gameObject.GetComponent<ItemData>().val;
            soundPlayer.PlayOneShot(moneySound);
            Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "Heal")
        {
           life = Mathf.Min(life + collision.gameObject.GetComponent<ItemData>().val, maxLife);
           soundPlayer.PlayOneShot(eat);
           Destroy(collision.gameObject);
        }
        if(collision.gameObject.tag == "Goal")
        {
            Wait();
            float delay = 1.0f;
            Invoke("Goal", delay);
        }
    }

    void Jump()
    {
        goJump = true;
        jumpCount += 1;
    }

    void ComboReset()
    {
        combocount = 1;
    }

    void ColorReset()
    {
        GetComponent<Renderer>().material.color = Color.white;
    }

    IEnumerator JumpStop()
    {
        while(rbody.velocity.y >= 0)
        {
            rbody.velocity = new Vector2(rbody.velocity.x, rbody.velocity.y - 60.0f * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator Deconcentration()
    {
        int deconSpeed = 1;
        while(gameState == "playing")
        {
            if(!cd.nextToEnemy && concentration != 0)
            {
                concentration = Mathf.Max(concentration - deconSpeed, 0);
                yield return new WaitForSeconds(1.0f);
            }
            else yield return null;
        }
    }

    /*IEnumerator PushJ()
    {
        float longPush = 0.001f;
        float passed = 0.0f;
        while(Input.GetKey("j") && passed <= longPush)
        {
            passed += Time.deltaTime * Time.deltaTime * Time.deltaTime * 30;
            Debug.Log(passed);
            yield return null;
        }
        if(passed < longPush) Attack();
        else Block();
    }*/

    /*void Action(string action, string mode)
    {
        actionAnime = action;
        if(mode == "attack")
        {
            goAttack = true;
        }
        if(mode == "block")
        {
            goBlock = true;
        }
        if(mode == "ducking")
        {
            goDuck = true;
        }
    }*/

    void Attack(string mode)
    {
        if(onGround && !damaged)
        {
            /*if(blocking)
            {
                am.state = "knee";
                Action(kneeKickAnime, "attack");
                blocking = false;
            }
            else if(ducking)
            {
                am.state = "low";
                Action(bodyJabAnime, "attack");
            }
            else */if(!comboFinisher)
            {
                goAttack = true;
                if(cd.beingAttacked)
                {
                    if(mode == "punch")
                    {
                        //Action(parryAnimes[(combocount - 1) % 2], "attack");
                        attackTrigger = "parry";
                        animator.SetInteger("chain", cd.chain);
                    }
                    else if(mode == "kick" && concentration < ConcentrateGaugeManager.maxCon)
                    {
                        //Action(cutAnimes[(combocount - 1) % 1] , "attack");
                        attackTrigger = "cut";
                    }

                    /*if(concentration >= ConcentrateGaugeManager.maxCon)
                    {
                        animator.SetBool("counter", true);
                        SlowMotion();
                    }
                    else animator.SetBool("counter", false);*/

                    cd.beingAttacked = false;
                }
                else
                {
                    /*if(IsInvoking("DelayAttack"))
                    {
                        CancelInvoke("DelayAttack");
                    }
                    if(delay)
                    {
                        PunchTriggerOff();
                        KickTriggerOff();
                        mode = "delay" + mode;
                        delay = false;
                        if(IsInvoking("DelayTimeEnd")) CancelInvoke("DelayTimeEnd");
                    }*/

                    if(mode == "kick" && concentration >= ConcentrateGaugeManager.maxCon)
                    {
                        attackTrigger = "con1";
                        concentration = concentration / 4;
                    }
                    /*else if((mode == "kick" && Input.GetAxisRaw("Vertical") > 0) ||(mode == "punch" && Input.GetAxisRaw("Vertical") <0))
                    {
                        attackTrigger = "delay" + mode;
                    }*/
                    /*if(mode == "punch")
                    {
                        Action(comboAnimes[combocount - 1], "attack");
                    }
                    else if(mode == "kick")
                    {
                        Action(kickAnimes[combocount - 1], "attack");
                    }*/
                    else attackTrigger = mode;
                }
                combocount = Mathf.Min(combocount + 1, maxcombo);
                //passedTimes = 0.0f;
            }
            else
            {
                ComboReset();
            }
        }
    }

    void Block()
    {
        blocking = true;
    }

    void Damage()
    {
        damaged = true;
        soundPlayer.PlayOneShot(punchHit);
        oldAnime = damagedAnime;
        GameManager.battleScore = Mathf.Max(GameManager.battleScore - 1, 0);
        concentration = Mathf.Max(concentration - 20, 0);
        //animator.Play(damagedAnime);
        animator.SetTrigger("damaged");

        amgr.attackType = AttackManager.ATTACKTYPE.none;
        QuitSlowMotion();
    }

    public void Wait()
    {
        gameState = "waiting";
        //animator.Play(stopAnime);
        animator.SetBool("wait", true);
        rbody.velocity = new Vector2(0, rbody.velocity.y);
    }
    public void StopWait()
    {
        gameState = "playing";
        animator.SetBool("wait", false);
    }

    void Die()
    {
        rbody.velocity = new Vector2(0, 0);
        rbody.simulated = false;
        GetComponent<BoxCollider2D>().enabled = false;
        animator.SetTrigger("die");

        GameManager.retry += 1;
        GameManager.battleScore = Mathf.Max(GameManager.battleScore - 3, 0);
        gameState = "gameover";
    }

    void Goal()
    {
        gameState = "gameclear";
        //animator.Play(stopAnime);
        rbody.velocity = new Vector2(0, rbody.velocity.y);
    }

    void Teleport(Vector3 pos)
    {
        gameObject.transform.position = pos;
    }

    void StopMove()
    {
        rbody.velocity = new Vector2(0, rbody.velocity.y);
    }

    void SlowMotion()
    {
        soundPlayer.PlayOneShot(slow);
        Time.timeScale = 0.5f;
    }

    void QuitSlowMotion()
    {
        //soundPlayer.PlayOneShot(quitSlow);
        Time.timeScale = 1f;
    }

    /*public void DelayAttack()
    {
        delay = true;
        if(IsInvoking("DelayTimeEnd")) CancelInvoke("DelayTimeEnd");
        Invoke("DelayTimeEnd", 0.5f);
    }

    void DelayTimeEnd()
    {
        delay = false;
    }*/
    void BackStep()
    {
        //concentration = Mathf.Max(concentration - 1, 0);
        rbody.velocity = new Vector2(0f, rbody.velocity.y);
        Vector2 backStepPw = new Vector2(backStep * transform.localScale.x * -1, 0);
        rbody.AddForce(backStepPw, ForceMode2D.Impulse);
    }
    public void SetHigh()
    {
        amgr.state = "high";
    }
    public void SetLow()
    {
        amgr.state = "low";
    }
    public void SetGuardBreak1()
    {
        amgr.state = "guardbreak1";
    }
    public void SetAttackType(AttackManager.ATTACKTYPE at)
    {
        amgr.attackType = at;
    }
    public void PlaySound()
    {
        soundPlayer.PlayOneShot(punch);
    }
    public void PunchTriggerOff()
    {
        animator.ResetTrigger("punch");
    }
    public void KickTriggerOff()
    {
        animator.ResetTrigger("kick");
    }
}
