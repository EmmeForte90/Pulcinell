using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyMeleeController : DatabaseEnemy,IDamegable
{
    public int maxHP;

    [Header("Fisica sparo")]
    public float distance;
    public float wakeRange;
    public float shootIntervallo;
    public float bulletSpeed;
    public float bulletTimer;
    private bool isAttack;
    //Bool
    public bool awake = false;
    public bool lookingRight = true;

    [Header("Riferimenti")]
    [SerializeField]
    protected Rigidbody2D theRB;
    [SerializeField]
    protected SpriteRenderer theSR;
    [SerializeField]
    public GameObject tor;
    public GameObject bullet;
    public Transform target;
    public float distanceToAttackPlayer, chaseSpeed;
    public Transform[] points;
    public new float moveSpeed;
    private int currentPoint;

    private Vector3 attackTarget;

    public float waitAfterAttack;
    private float attackCounter;


    [SerializeField]
    public Transform shootPointLeft;
    [SerializeField]
    public Transform shootPointRight;





    private void Awake()
    {
        anim = gameObject.GetComponent<Animator>();

    }
    // Start is called before the first frame update
    void Start()
    {
        //Inizializzazione
        for (int i = 0; i < points.Length; i++)
        {
            points[i].parent = null;
        }
        HP = maxHP;
        HPBarra.SethmaxHP(maxHP);
    }

    // Update is called once per frame
    void Update()
    {

        // Guarda verso il player
        if (target.transform.position.x > transform.position.x)
        {
            lookingRight = true;

            transform.localScale = new Vector2(-2, transform.localScale.y);

        }

        if (target.transform.position.x < transform.position.x)
        {
            lookingRight = false;

            transform.localScale = new Vector2(2, transform.localScale.y);

        }

        //Calcolo della distanza se attaccare il player o continuare il suo percorso senza meta
        /*if (Vector3.Distance(transform.position  PlayerController.instance.transform.position) > distanceToAttackPlayer)
        {
            isAttack = false;

            attackTarget = Vector3.zero;

            transform.position = Vector3.MoveTowards(transform.position, points[currentPoint].position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, points[currentPoint].position) < .05f)
            {
                currentPoint++;

                if (currentPoint >= points.Length)
                {
                    currentPoint = 0;
                }
            }

            

           


        }
        else
        {
            //SCAPPA, COGLIONE, VUOLE ATTACCARTI!

            if (attackTarget == Vector3.zero)
            {
                attackTarget = //PlayerController.instance.transform.position;
                isAttack = true;

            }

            transform.position = Vector3.MoveTowards(transform.position, attackTarget, chaseSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, attackTarget) <= .1f)
            {

                attackCounter = waitAfterAttack;
                attackTarget = Vector3.zero;
                isAttack = false;
            }
        }

        UpdateAnimator();
        */
    }
    private void UpdateAnimator()
    {
      
        anim.SetBool("isAttacking", isAttack);
        

    }


    #region danno
    public new void Damage()
            {

                anim.SetTrigger("isHurt");

                hurtEnemy();

                HP--;
                HPBarra.SethHP(HP);



                if (HP <= 0)
                {
                    Instantiate(DIE, tor.transform.position, tor.transform.rotation);


                    Destroy(tor.gameObject);

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

    



