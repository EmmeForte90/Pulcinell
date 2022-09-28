using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemeySmall : DatabaseEnemy, IDamegable
{
    public new int HP { get; set; }



    [Header("Sprite e animazione")]
    [SerializeField]
    protected Rigidbody2D theRB;
    [SerializeField]
    protected SpriteRenderer theSR;

    [Header("Movimenti")]
    [SerializeField]
    public Transform leftPoint, rightPoint;
    [SerializeField]
    protected bool movingRight;
    [SerializeField]
    protected bool attack = false;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {


        leftPoint.parent = null;
        rightPoint.parent = null;

        movingRight = false;

        moveCount = moveTime;
    }







    // Update is called once per frame
    void Update()
    {
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();



        if (!hit || !attack)
        {

            #region movimenti nemici

            if (moveCount > 0)
            {
                moveCount -= Time.deltaTime;

                if (movingRight)
                {
                    theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);



                    if (transform.position.x > rightPoint.position.x)
                    {
                        movingRight = false;

                        transform.localScale = new Vector2(1, transform.localScale.y);
                    }
                }
                else
                {
                    theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);



                    if (transform.position.x < leftPoint.position.x)
                    {
                        movingRight = true;

                        transform.localScale = new Vector2(-1, transform.localScale.y);
                    }
                }

                if (moveCount <= 0)
                {
                    waitCount = Random.Range(waitTime * .75f, waitTime * 1.25f);
                }

                anim.SetBool("isMoving", true);
            }
            else if (waitCount > 0)
            {
                waitCount -= Time.deltaTime;
                theRB.velocity = new Vector2(0f, theRB.velocity.y);

                if (waitCount <= 0)
                {
                    moveCount = Random.Range(moveTime * .75f, waitTime * .75f);
                }
                anim.SetBool("isMoving", false);
            }
        }
        else if (hit || attack)
        {

        }
        #endregion















#if UNITY_EDITOR


        if (Input.GetKeyDown(KeyCode.H))
        {
            Damage();


        }
#endif


    }






    #region Attacco
    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.tag == "Player")
        {

            anim.SetTrigger("isAttacking");

            attack = true;
            //HitEnemy();
        }
    }// Cooldown dell'attacco
    /*IEnumerator HitEnemy()
    {
        Attacco.gameObject.SetActive(false);
        waitCount = 1;

        yield return new WaitForSeconds(0.5f);
        waitCount = 0;
        attack = false;
        Attacco.gameObject.SetActive(true);

    }*/
    #endregion




}
