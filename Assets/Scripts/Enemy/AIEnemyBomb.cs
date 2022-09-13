using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemyBomb : DatabaseEnemy, IDamegable

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
    public bool isFacingleft;

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



            if (isFacingleft)
            {






                transform.localScale = new Vector2(2, transform.localScale.y);

            }
            if (!isFacingleft)
            {

                transform.localScale = new Vector2(-2, transform.localScale.y);
            }




            #endregion


#if UNITY_EDITOR


            if (Input.GetKeyDown(KeyCode.H))
            {



            }
#endif


        }

    }
        #region Attacco
        private void OnTriggerStay2D(Collider2D other)
        {

            if (other.tag == "Player")
            {

                anim.SetTrigger("isAttacking");
                shotCounter -= Time.deltaTime;

                if (shotCounter <= 0)
                {
                    shotCounter = timeBetweenShots;
                    //Repeting shooter
                    var newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);

                    if (isFacingleft)
                    {
                        newBullet.GetComponent<Rigidbody2D>().AddForce(-transform.right * FUOCO);
                    }
                    if (!isFacingleft)
                    {
                        newBullet.GetComponent<Rigidbody2D>().AddForce(transform.right * FUOCO);

                    }
                }

            }
        }

        #endregion






     
}
