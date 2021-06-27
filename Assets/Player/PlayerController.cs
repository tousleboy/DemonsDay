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
    int maxLife;
    public int damage;
    public int diffence = 1;
    public bool damaged = false;
    string collisionState;

    public static string gameState;
    public static int score = 0;

    AudioSource soundPlayer;
    public AudioClip punch;
    public AudioClip punchHit;
    public AudioClip guardHit;
    public AudioClip moneySound;

    public static string messages = "not recieved"; //recieve message from talk event. default should be "not recieved" 
    public string texts = "";

    // Start is called before the first frame update
    void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;
        amgr = transform.Find("AttackZone").gameObject.GetComponent<AttackManager>();
        cd = transform.Find("CollisionDetector").gameObject.GetComponent<CollisionDetector>();
        life = 3;
        maxcombo = comboAnimes.Length;
        maxLife = life;
        gameState = "playing";
        soundPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState != "playing")
        {
            return;
        }

        axisH = Input.GetAxisRaw("Horizontal");
        //Debug.Log("texts" + texts +  " messages" + messages);
        if(texts != "")
        {
            messages = texts;
            texts = "";
        }
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
        if(!backStepping)
        {
            if(cd.nextToEnemy && axisH * transform.localScale.x < 0)
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

            if(Input.GetButtonDown("Jump"))
            {
                Jump();
            }
            if(Input.GetButtonUp("Jump") && !onGround && rbody.velocity.y > 0)
            {
                jumpStop = true;
            }
        }

        if(Input.GetKeyDown("j"))
        {
            //StartCoroutine("PushJ");
            Attack("punch");
        }
        if(Input.GetKeyDown("k"))
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

        if(Input.GetMouseButton(1) || Input.GetAxisRaw("Vertical") > 0)
        {
            if(!damaged)
            {
                blocking = true;
            }
            //Action(blockAnime, "block");
        }
        else if(Input.GetMouseButtonUp(1) || Input.GetAxisRaw("Vertical") <= 0 /*|| !Input.GetKey("j") */|| damaged)
        {
            blocking = false;
        }
        
        if(Input.GetAxisRaw("Vertical") < 0 && !damaged)
        {
            if(!goAttack)
            {
                //Action(duckingAnime, "ducking");
                blocking = false;
                ducking = true;
            }
            //passedTimes = 0.0f;
        }
        else
        {
            ducking = false;
        }

        if(life <= 0)
        {
            gameState = "gameover";
            Die();
        }
    }

    void FixedUpdate()
    {
        if(gameState != "playing")
        {
            return;
        }

        onGround = Physics2D.Linecast(transform.position, transform.position -(transform.up * 0.1f), groundLayer);

        if(damaged)
        {
            return;
        }

        if(attacking || blocking || ducking || backStepping)
        {
            axisH = 0;
            goJump = false;
        }

        if((onGround || axisH != 0) && !backStepping)//走り
        {
            rbody.velocity = new Vector2(axisH * runSpeed, rbody.velocity.y);
        }

        if(goBackStep)
        {
            Vector2 backStepPw = new Vector2(backStep * transform.localScale.x * -1, 0);
            rbody.AddForce(backStepPw, ForceMode2D.Impulse);
        }

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
            cd.beingAttacked = false;
            collisionState = am.state;
            damage = am.val;

            if(collisionState == "high" && (blocking || parry) || collisionState == "low" && (blocking || cut))
            {
                damage -= diffence;
                soundPlayer.PlayOneShot(guardHit);
            }

            life -= damage;

            if((damage > 0 || am.knockBack) && !blocking)
            {
                Damage();
                if(damage > 0)
                {
                    GetComponent<Renderer>().material.color = Color.red;
                    Invoke("ColorReset", 0.1f);
                }
                if(am.knockBack)
                {
                    am.KnockBack(gameObject);
                }
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
            rbody.velocity = new Vector2(rbody.velocity.x, rbody.velocity.y - 1.0f);
            yield return null;
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
            else */if(!comboFinisher)
            {
                goAttack = true;
                if(cd.beingAttacked)
                {
                    if(mode == "punch")
                    {
                        //Action(parryAnimes[(combocount - 1) % 2], "attack");
                        attackTrigger = "parry";
                    }
                    else if(mode == "kick")
                    {
                        //Action(cutAnimes[(combocount - 1) % 1] , "attack");
                        attackTrigger = "cut";
                    }

                    cd.beingAttacked = false;
                }
                else
                {
                    /*if(mode == "punch")
                    {
                        Action(comboAnimes[combocount - 1], "attack");
                    }
                    else if(mode == "kick")
                    {
                        Action(kickAnimes[combocount - 1], "attack");
                    }*/
                    attackTrigger = mode;
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
        //animator.Play(damagedAnime);
        animator.SetTrigger("damaged");
    }

    void Wait()
    {
        gameState = "waiting";
        //animator.Play(stopAnime);
        animator.SetBool("wait", true);
        rbody.velocity = new Vector2(0, rbody.velocity.y);
    }

    void Die()
    {
        rbody.velocity = new Vector2(0, 0);
        rbody.simulated = false;
        GetComponent<CapsuleCollider2D>().enabled = false;
        animator.SetTrigger("die");

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

    public void SetHigh()
    {
        amgr.state = "high";
    }
    public void SetLow()
    {
        amgr.state = "low";
    }
    public void SetGuardBreak()
    {
        amgr.state = "guardbreak";
    }
    public void PlaySound()
    {
        soundPlayer.PlayOneShot(punch);
    }
    public void TriggerOff()
    {
        animator.ResetTrigger("punch");
        animator.ResetTrigger("kick");
    }
}
