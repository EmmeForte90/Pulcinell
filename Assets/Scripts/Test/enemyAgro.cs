using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAgro : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float agroRange;
    [SerializeField] float moveSpeed;

    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       float disToPlayer = Vector2.Distance(transform.position, player.position);
       //Debug.Log("disToPlayer:" +disToPlayer); 
       if(disToPlayer < agroRange)
       {
        ChasePlayer();
       }
       else
       {
        StopChasingPlayer();
       }
    }

void  ChasePlayer()
{
if(transform.position.x < player.position.x)
{
    //Left
    rb.velocity = new Vector2(moveSpeed, 0);
}
else if(transform.position.x > player.position.x)
{
    //Right
    rb.velocity = new Vector2(-moveSpeed, 0);

}
}

void StopChasingPlayer()
{
rb.velocity = new Vector2(0, 0);
}

}
