using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public int val = 1;
    public int chain = 1;
    public string state = "all";

    public bool knockBack = false;
    public bool guardBreak = false;
    public float hmkb = 0.0f; //how many knockback

    public enum ATTACKTYPE{
        none,
        jab,
        straight,
        upper,
        lowkick,
        middlekick,
        chudankick,
        jodankick,
        kick
    }

    public ATTACKTYPE attackType = ATTACKTYPE.none;

    public EffectScript efs;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
            if(pc != null)
            {
                if((state == "high" && pc.parry || state == "low" && pc.cut) && !guardBreak) efs.ParryEffect();
                else efs.AttackEffect();
            }
            else efs.AttackEffect();
        }
        else if(collision.gameObject.tag == "Enemy")
        {
            EnemyController ec = collision.gameObject.GetComponent<EnemyController>();
            if(ec != null)
            {
                if((state == "high" && ec.parry || state == "low" && ec.cut) && !guardBreak) efs.ParryEffect();
                else efs.AttackEffect();
            }
            else efs.AttackEffect();
        }
        //else efs.AttackEffect();
    }

    public void KnockBack(GameObject target)
    {
        Vector3 targetPos = target.transform.position;
        Rigidbody2D rbody = target.GetComponent<Rigidbody2D>();
        
        Vector2 direction = Direction(targetPos);
        Debug.Log(direction);
        rbody.velocity = new Vector2(0.0f, 0.0f);
        rbody.AddForce(direction, ForceMode2D.Impulse);
    }

    Vector2 Direction(Vector3 targetPos)
    {
        Vector3 OwnPos = transform.parent.transform.position;
        if(OwnPos.x - targetPos.x > 0)
        {
            Vector2 direction = new Vector2(hmkb * -1, 0.0f);
            return direction;
        }
        else
        {
            Vector2 direction = new Vector2(hmkb, 0.0f);
            return direction;
        }
    }
}
