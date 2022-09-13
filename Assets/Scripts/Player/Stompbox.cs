using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stompbox : MonoBehaviour
{
    
    public Transform attackPoint;
    public float attackrange;
    public GameObject collectible;
    public LayerMask enemyLayers;

    [Range(0, 100)] public float chanceToDrop;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D Enemy)
    {
        if (Enemy.tag == "Player" )//&& PlayerController.instance.m_Rigidbody2D.position.y > transform.position.y)
        {
            Debug.Log("We hit");
            AIEnemyDefault.instance.Damage();

            float dropSelect = Random.Range(0, 100f);

            if (dropSelect <= chanceToDrop)
            {
                Instantiate(collectible, Enemy.transform.position, Enemy.transform.rotation);
            }
            //PlayerController.instance.Bounce();
            AudioManager.instance.PlaySFX(3);
        }

       


           


    }
}
