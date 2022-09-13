using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemyGun : DatabaseEnemy, IDamegable

{
    //public int HP { get; set; }



    [Header("Sprite e animazione")]
    [SerializeField]
    public Transform Enemy;
    [SerializeField]
    protected Rigidbody2D theRB;
    [SerializeField]
    protected SpriteRenderer theSR;

    [Header("Shooting")]
    [SerializeField]
    public GameObject bullet;
    [SerializeField]
    public Transform firePoint;
    [SerializeField]
    public float timeBetweenShots;
    [SerializeField]
    public float shotCounter;
    [SerializeField]
    public float FUOCO;


    [Header("Movimenti")]
    [SerializeField]
    public Transform leftPoint, rightPoint;
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
        theRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        leftPoint.parent = null;
        rightPoint.parent = null;

        movingRight = true;

        moveCount = moveTime;
    }

    // Update is called once per frame
    void Update()
    {

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

                        transform.localScale = new Vector2(2, transform.localScale.y);
                    }
                }
                else
                {
                    theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);



                    if (transform.position.x < leftPoint.position.x)
                    {
                        movingRight = true;

                        transform.localScale = new Vector2(-2, transform.localScale.y);
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
          

        }
#endif


    }


    #region Attacco
    private void OnTriggerStay2D(Collider2D other)
    {

        waitaftershot();

        if (other.tag == "Player")
        {

            shotCounter -= Time.deltaTime;

            if (shotCounter <= 0)
            {
                shotCounter = timeBetweenShots;
                //Repeting shooter
                var newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
                anim.SetTrigger("isAttacking");


                newBullet.transform.localScale = Enemy.localScale;
            }

        }
    }

    IEnumerator waitaftershot()
    {
        moveCount -= Time.deltaTime;

        yield return new WaitForSeconds(1f);

       
        waitCount -= Time.deltaTime;


    }

    #endregion


    #region danno
    public new void Damage()
    {

        anim.SetTrigger("isHurt");

        hurtEnemy();

        HP--;
        HPBarra.SethHP(HP);



        if (HP <= 0)
        {
            Instantiate(DIE, enemy.transform.position, enemy.transform.rotation);


            Destroy(enemy.gameObject);

        }
    }
    IEnumerator hurtEnemy()
    {
        waitCount = 1;

        yield return new WaitForSeconds(0.5f);
        waitCount = 0;
        hit = false;

    }
    #endregion



}


