using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LesserEnemyController : MonoBehaviour
{
    public float jumpInterval;
    float passedTimes = 0.0f;
    public float jumpX = 0.0f;
    public float jumpY = 0.0f;
    bool onGround;

    public LayerMask groundLayer;

    Rigidbody2D rbody;
    Renderer R;

    Animator animator;
    public string stopAnime = "LesserEnemyStop";
    public string jumpAnime = "LesserEnemyJump";
    public string deadAnime = "LesserEnemyBeated";

    public int enemyLife = 1;
    bool dead;
    // Start is called before the first frame update
    void Start()
    {
       rbody = GetComponent<Rigidbody2D>();
       animator = GetComponent<Animator>();
       R = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyLife <= 0 && !dead)
        {
            Dead();
        }
    }

    void FixedUpdate()
    {
        if(dead || !R.isVisible)
        {
            return;
        }
        onGround = Physics2D.Linecast(transform.position, transform.position -(transform.up * 0.01f), groundLayer);

        if(onGround)
        {
            if(passedTimes == 0)
            {
                rbody.velocity = new Vector2(0.0f, 0.0f);
            }
            passedTimes += Time.deltaTime;
            animator.Play(stopAnime);
        }
        else
        {
            animator.Play(jumpAnime);
        }

        if(passedTimes >= jumpInterval)
        {
            passedTimes = 0;
            Vector2 v2 = new Vector2(jumpX, jumpY);
            rbody.AddForce(v2, ForceMode2D.Impulse);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Attack")
        {
            AttackManager am = collision.gameObject.GetComponent<AttackManager>();
            enemyLife -= am.val;
            am.KnockBack(gameObject);
            Debug.Log("enemylife" + enemyLife);
        }
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Crash");
            Vector2 v2 = new Vector2(5.0f, 0.0f);
            rbody.AddForce(v2, ForceMode2D.Impulse);
        }
    }

    void Dead()
    {
        //Debug.Log("enemy beated");
        dead = true;
        GetComponent<CapsuleCollider2D>().enabled = false;
        GameObject attackZone = transform.Find("AttackZone").gameObject;
        Destroy(attackZone);
        //rbody.velocity = new Vector2(0, 0);
        animator.Play(deadAnime);
        Destroy(gameObject, 1.0f);
    }
}
