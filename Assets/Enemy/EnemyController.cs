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

    Animator animator;
    public string stopAnime = "RichmenIdle";
    public string runAnime = "RichmenRun";
    public string comboAnime = "onetwo gap exit";
    public string[] nextComboAnimes;
    string[] comboStack;
    int stackPointer = 0;
    int stackLen;
    public string damagedAnime = "RichmenDamaged";
    public string deadAnime = "RichmenDead";
    string nowAnime;
    string oldAnime;

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
    int diffence = 1;
    bool dead = false;

    GameObject attackZone;
    AttackManager am;
    BoxCollider2D abc;
    public bool high = false;
    public bool low = false;
    public bool knee = false;
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
        nowAnime = stopAnime;
        oldAnime = stopAnime;
        attackZone = transform.Find("AttackZone").gameObject;
        if(attackZone != null)
        {
            am = attackZone.GetComponent<AttackManager>();
            abc = attackZone.GetComponent<BoxCollider2D>();
        }
        soundPlayer = GetComponent<AudioSource>();

        comboStack = comboAnime.Split(' ');
        stackLen = comboStack.Length;

        rbody = GetComponent<Rigidbody2D>();
        pc = player.GetComponent<PlayerController>();

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
        
        if(xPosition - xPlayerPosition > 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if(xPosition - xPlayerPosition < 0)
        {
            transform.localScale = new Vector2(1, 1);
        }

        if(enemyLife <= 0)
        {
            Dead();
        }
    }

    void FixedUpdate()
    {
        onGround = Physics2D.Linecast(transform.position, transform.position -(transform.up * 0.1f), groundLayer);
        isPlayerNear = CheckLength(playerPos, maai);

        if(damaged || dead)
        {
            return;
        }
        if(PlayerController.gameState != "playing")
        {
            rbody.velocity = new Vector2(0.0f, rbody.velocity.y);
            moving = false;
            animator.SetBool("move", false);
            return;
        }

        if(goAttack)
        {
            attacking = true;
            Combo();
            goAttack = false;
        }
        else if(CheckLength(playerPos, range) && onGround && !attacking)
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
        if(collision.gameObject.tag == "Attack")
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
    }

    void Damaged()
    {
        soundPlayer.PlayOneShot(punchHit);
        attacking = false;
        stackPointer = 0;
        //animator.Play(damagedAnime, 0, 0);
        animator.SetTrigger("damage");
        oldAnime = damagedAnime;
    }

    void Dead()
    {
        dead = true;
        /*GameObject shield = transform.Find("shield").gameObject;
        Destroy(shield);*/
        GetComponent<CapsuleCollider2D>().enabled = false;
        //animator.Play(deadAnime);
        animator.SetTrigger("die");
        PlayerController.concentration = Mathf.Max(PlayerController.concentration - 5, 0);
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

    public void Combo()
    {
        if(!isPlayerNear && comboStack[stackPointer] == "gap")
        {
            stackPointer = 0;
            animator.SetTrigger("exit");
            attacking = false;
            return;
        }
        animator.SetTrigger(comboStack[stackPointer]);
        if(comboStack[stackPointer] == "exit")
        {
            attacking = false;
            Debug.Log("exit");
        }
        stackPointer = (stackPointer + 1) % stackLen;
    }
}
