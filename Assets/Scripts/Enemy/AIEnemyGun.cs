using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemyGun : DatabaseEnemy, IDamegable

{
    //public int HP { get; set; }



    [Header("Sprite e animazione")]
    [SerializeField]
    public Transform Enemy;

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
        RB = GetComponent<Rigidbody2D>();
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
                    RB.velocity = new Vector2(moveSpeed, RB.velocity.y);



                    if (transform.position.x > rightPoint.position.x)
                    {
                        movingRight = false;

                        transform.localScale = new Vector2(1, transform.localScale.y);
                    }
                }
                else
                {
                    RB.velocity = new Vector2(-moveSpeed, RB.velocity.y);



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
                RB.velocity = new Vector2(0f, RB.velocity.y);

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

    #region Danno
 
private void OnTriggerEnter2D(Collider2D other)
    {
        /*if (other.tag == "Player")
        {

            //anim.SetTrigger("isAttack");
            //attack = true;
            //AudioManager.instance.PlaySFX(10);
            //HitEnemy();
        }*/
        if(other.tag == "Bullet")
        {
            StartCoroutine(HitEnemy());
            //Quando il nemico collide con il bullet parte la corutine
        }

    }
    

    // Cooldown dell'attacco
IEnumerator HitEnemy()
    {
        //Attacco.gameObject.SetActive(false);
        moveCount = 0;
        //Il nemico si ferma 
        anim.SetTrigger("isHurt");
        RB.AddForce(transform.up * bounceForce);
        yield return new WaitForSeconds(0.5f);
        //Si ferma per mezzo secondo
        moveCount = 1;
        //Poi riparte
        attack = false;
        //Attacco.gameObject.SetActive(true);

    }

    #endregion

}


