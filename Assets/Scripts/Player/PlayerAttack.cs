using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header ("Attacco, frequenza e drop")]
    public Transform[] points;
    public EnemyController bossCont;
    public Animator animator;

    public Transform attackPoint;
    public LayerMask enemyLayers;
    public GameObject deathEffect;

    public GameObject collectible;
    [Range(0, 100)] public float chanceToDrop;

    public float attackRange = 0.5f;
    

    public float attackRate = 2f;
    float nextAttackTime = 0f;

    void Update()
    {

        if (Time.time >= nextAttackTime)
        {

            if (Input.GetButtonDown("Fire1"))
            {

                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
                

                

            }


        }





    }
  


    
    void Attack()
    {

        //animation
        animator.SetTrigger("Attack");

        //Detect enemy

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        //damage

        foreach (Collider2D enemy in hitEnemies)
        {

                    Debug.Log("Hit Enemy");

            

            

                   


                    float dropSelect = Random.Range(0, 100f);

                    if (dropSelect <= chanceToDrop)
                    {
                        Instantiate(collectible, enemy.transform.position, enemy.transform.rotation);
                    }

                    AudioManager.instance.PlaySFX(3);
                }
            }

    


    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();


    }

    // Update is called once per frame

}