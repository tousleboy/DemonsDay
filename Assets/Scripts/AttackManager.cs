using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    public int val = 1;
    public int chain = 1;
    public string state = "all";

    public bool knockBack = false;
    public float hmkb = 0.0f; //how many knockback

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
