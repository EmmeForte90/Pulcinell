using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnBulletChas : MonoBehaviour
{
    [SerializeField] float moveSpeed = 7f;
	private Rigidbody2D rb;
	PlayerMovement target;
	Vector2 moveDirection;
	[SerializeField] GameObject Explode;
	

	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		target = GameObject.FindObjectOfType<PlayerMovement>();
		moveDirection = (target.transform.position - transform.position).normalized * moveSpeed;
		rb.velocity = new Vector2(moveDirection.x, moveDirection.y);
	}
	
	private void OnCollisionEnter2D(Collision2D collision){
		Destroy(gameObject);

	}

	void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.gameObject.tag == "Player")
        //Se il proiettile tocca il nemico
        {            
            Instantiate(Explode, transform.position, transform.rotation);
            //CameraShake.Shake(0.10f, 0.50f);
            PlayerMovement.instance.Hurt();
			Destroy(gameObject);


        }

		if(other.gameObject.tag == "ground")
        //Se il proiettile tocca il nemico
        {            
            Instantiate(Explode, transform.position, transform.rotation);
            //CameraShake.Shake(0.10f, 0.50f);
            Destroy(gameObject);
            //Viene distrutto
        }
	}
}
