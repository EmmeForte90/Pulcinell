using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torretta : DatabaseEnemy, IDamegable
{
   
    public int maxHP;
    


    [Header ("Fisica sparo")]
    public float distance;
    public float wakeRange;
    public float shootIntervallo;
    public float bulletSpeed;
    public float bulletTimer;

    //Bool
    public bool awake = false;
    public bool lookingRight = true;
    public bool isTorret;

    [Header("Riferimenti")]
    [SerializeField]
    protected Rigidbody2D theRB;
    [SerializeField]
    protected SpriteRenderer theSR;
    [SerializeField]
    public GameObject tor;
    public GameObject bullet;
    public Transform target;
    
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
        HP = maxHP;
        //HPBarra.SethmaxHP(maxHP);
    }

    // Update is called once per frame
    void Update()
    {

        //impostazione delle animazioni
        if (isTorret)
        {
            anim.SetBool("Awake", awake);
            anim.SetBool("LookingRight", lookingRight);
        }
        //avvia la call quando rileva il player
        RangeCheck();

        #region localizzazione bersaglio
        if (target.transform.position.x > transform.position.x)
        {
            lookingRight = true;
            if (!isTorret)
            {
                transform.localScale = new Vector2(-2, transform.localScale.y);
            }
        }

        if(target.transform.position.x < transform.position.x)
        {
            lookingRight = false;
            if(!isTorret)
            {
                transform.localScale = new Vector2(2, transform.localScale.y);
            }
        }
        #endregion

    }




    //il player viene rilevato, la torretta inizia a rilevarlo
    void RangeCheck()
    {
        distance = Vector3.Distance(transform.position, target.transform.position);


    if(distance < wakeRange)
        {
            awake = true;

        }
    if( distance > wakeRange)
        {
            awake = false;
        }



        }

    #region danno
    public new void Damage()
    {

        anim.SetTrigger("isHurt");

        hurtEnemy();

        HP--;
        //HPBarra.SethHP(HP);

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


    // call che richiama l'attacco
    public void Attack(bool attackingRight)
    {
        //tempo tra uno sparo e un altro
        bulletTimer += Time.deltaTime;

        if(bulletTimer >= shootIntervallo)
        {
            //direzione bullet
            Vector2 direction = target.transform.position - transform.position;
            direction.Normalize();

            //se non sta attaccando a destra
            if(!attackingRight)
            { GameObject bulletClone;
                bulletClone = Instantiate(bullet, shootPointLeft.transform.position, shootPointLeft.transform.rotation) as GameObject;
                bulletClone.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

                bulletTimer = 0;

            }
            // Se attacca a destra
            if (attackingRight)
            {
                GameObject bulletClone;
                bulletClone = Instantiate(bullet, shootPointRight.transform.position, shootPointRight.transform.rotation) as GameObject;
                bulletClone.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed;

                bulletTimer = 0;

            }
        }
    }

}



