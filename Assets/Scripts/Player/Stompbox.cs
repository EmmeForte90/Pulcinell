using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stompbox : MonoBehaviour
{
    public GameObject stompBox;
    //public Transform attackPoint;
    //public float attackrange;
    //public GameObject collectible;
    //public LayerMask enemyLayers;

    //[Range(0, 100)] public float chanceToDrop;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy" )//&& PlayerController.instance.m_Rigidbody2D.position.y > transform.position.y)
        {
           // Debug.Log("We hit");
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage();
            FindObjectOfType<DatabaseEnemy>().Hurt();           
            //PlayerMovement.instance.OnAir();
            PlayerMovement.instance.BumpEnemy();
            /*float dropSelect = Random.Range(0, 100f);

            if (dropSelect <= chanceToDrop)
            {
                Instantiate(collectible, Enemy.transform.position, Enemy.transform.rotation);
            }
            //PlayerController.instance.Bounce();
            AudioManager.instance.PlaySFX(3);*/
        }

        if (other.gameObject.tag == "Ground" )//&& PlayerController.instance.m_Rigidbody2D.position.y > transform.position.y)
        {
           // Debug.Log("Sei a terra");
           // PlayerMovement.instance.OnGround();
        }
    }
}
