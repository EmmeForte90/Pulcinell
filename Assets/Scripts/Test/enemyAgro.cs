using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyAgro : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] float agroRange;
    [SerializeField] float touchRange;
    [SerializeField] float moveSpeed;

//Punti per il raycast
    [SerializeField] Transform castPoint;
    [SerializeField] Transform touchPoint;

    bool isFacingLeft;
    bool isAgro = false;
    bool isSearching = false;



    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
       /* if(CanSeePlayer(agroRange))
        {
            isAgro = true;
            ChasePlayer();
        }
        else
        {
            if(isAgro)
            {
                if(!isSearching)
                {
                    isSearching = true;
                    Invoke("StopChasingPlayer", 3);
                }
            }
        }
*/

        if(TouchOtherEnemy(touchRange))
        {
            Boom();
                Debug.Log("hai toccato");

        }



      /* float disToPlayer = Vector2.Distance(transform.position, player.position);
       //Debug.Log("disToPlayer:" +disToPlayer); 
       if(disToPlayer < agroRange)
       {
        ChasePlayer();
       }
       else
       {
        StopChasingPlayer();
       }*/
    }

bool TouchOtherEnemy(float distance)
{
    bool val = false;
    float castDist = distance;
    if(isFacingLeft)
    {
        castDist = -distance;
    }
    Vector2 endPos = touchPoint.position + Vector3.right * castDist;
    RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Enemy"));
    if(hit.collider != null)
    {
        if(hit.collider.gameObject.CompareTag("Enemy"))
        {
            val = true;
        }
        else
        {
            val = false;
        }
        Debug.DrawLine(touchPoint.position, hit.point, Color.green);

    }else
    {
    Debug.DrawLine(touchPoint.position, endPos, Color.blue);

    }

    return val;

}



bool CanSeePlayer(float distance)
{
    bool val = false;
    float castDist = distance;
    if(isFacingLeft)
    {
        castDist = -distance;
    }
    Vector2 endPos = castPoint.position + Vector3.right * castDist;
    RaycastHit2D hit = Physics2D.Linecast(castPoint.position, endPos, 1 << LayerMask.NameToLayer("Player"));
    if(hit.collider != null)
    {
        if(hit.collider.gameObject.CompareTag("Player"))
        {
            val = true;
        }
        else
        {
            val = false;
        }
        Debug.DrawLine(castPoint.position, hit.point, Color.yellow);

    }else
    {
    Debug.DrawLine(castPoint.position, endPos, Color.red);

    }

    return val;

}

void  Boom()
{
    rb.velocity = new Vector2(moveSpeed, 1);
    transform.localScale = new Vector2(1,1);
    isFacingLeft = false;
}


void  ChasePlayer()
{
if(transform.position.x < player.position.x)
{
    //Left
    rb.velocity = new Vector2(moveSpeed, 0);
    transform.localScale = new Vector2(1,1);
    isFacingLeft = false;
}
else if(transform.position.x > player.position.x)
{
    //Right
    rb.velocity = new Vector2(-moveSpeed, 0);
    transform.localScale = new Vector2(-1,1);
    isFacingLeft = true;



}
}

void StopChasingPlayer()
{
    isAgro = false;
    isSearching = false;
rb.velocity = new Vector2(0, 0);
}

}
