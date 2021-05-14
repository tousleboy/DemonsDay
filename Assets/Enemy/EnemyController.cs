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

    Animator animator;
    public string stopAnime = "RichmenIdle";
    public string runAnime = "RichmenRun";
    public string comboAnime = "RichmenCombo1";
    public string damagedAnime = "RichmenDamaged";
    public string deadAnime = "RichmenDead";
    string nowAnime;
    string oldAnime;

    public int enemyLife = 8;
    int damage = 0;
    string collisionState;
    public bool damaged = false;
    public bool moving = false;
    bool dead = false;

    GameObject attackZone;
    public bool high = false;
    public bool low = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerPos = player.transform.position;
        animator = GetComponent<Animator>();
        nowAnime = stopAnime;
        oldAnime = stopAnime;
        attackZone = transform.Find("AttackZone").gameObject;
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

        if(moving || dead)
        {
            return;
        }
        
        float xPosition = transform.position.x;
        float xPlayerPosition = player.transform.position.x;
        
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
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        PlayerController pc = player.GetComponent<PlayerController>();
        onGround = Physics2D.Linecast(transform.position, transform.position -(transform.up * 0.1f), groundLayer);

        if(damaged || dead)
        {
            return;
        }

        if(pc.onGround)
        {
            playerPos = player.transform.position;
        }

        if(CheckLength(playerPos, range) && !attacking && onGround)
        {
            if(CheckLength(playerPos, maai))
            {
                rbody.velocity = new Vector2(0.0f, rbody.velocity.y);
                goAttack = true;
                moving = false;
            }
            else
            {
                rbody.velocity = new Vector2(speed * transform.localScale.x, rbody.velocity.y);
                moving = true;
            }
        }

        if(attacking)
        {
            AttackManager am = attackZone.GetComponent<AttackManager>();
            if(high)
            {
                am.state = "high";
            }
            else if(low)
            {
                am.state = "low";
            }
        }

        if(!attacking && !damaged)
        {
            if(goAttack)
            {
                nowAnime = comboAnime;
                goAttack = false;
            }
            else
            {
                if(moving)
                {
                    nowAnime = runAnime;
                }
                else
                {
                    nowAnime = stopAnime;
                }
            }
        }
        if(nowAnime != oldAnime)
        {
            animator.Play(nowAnime);
            oldAnime = nowAnime;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            Debug.Log("hit" + enemyLife);
            AttackManager am = collision.gameObject.GetComponent<AttackManager>();
            collisionState = am.state;
            damage = am.val;

            enemyLife -= damage;
            damage = 0;
            if(gap)
            {
                Damaged();
            }
            GetComponent<Renderer>().material.color = Color.red;
            Invoke("ColorReset", 0.1f);

            if(am.knockBack)
            {
                am.KnockBack(gameObject);
            }
        }
    }

    void Damaged()
    {
        animator.Play(damagedAnime, 0, 0);
        oldAnime = damagedAnime;
    }

    void Dead()
    {
        dead = true;
        GameObject shield = transform.Find("shield").gameObject;
        Destroy(shield);
        GetComponent<CapsuleCollider2D>().enabled = false;
        animator.Play(deadAnime);
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
}
