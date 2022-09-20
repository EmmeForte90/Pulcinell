using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
	Rigidbody2D rb;
	[SerializeField]
	public float Delay;
	[SerializeField]
	public float Explose;
	[SerializeField]
	public Transform pointClass;


	// Use this for initialization
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
	}


	//Quando il player collide con l'oggetto
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player")
		{
			//Avvia un timer stabilito con una variabile nel database delle piattaforme per mantenere il conteggio
			Platformdatabase.Instance.StartCoroutine("SpawnPlatform", new Vector2(transform.position.x, transform.position.y));
			
			//La piattaforma cade
			Invoke("DropPlatform", Delay);
			Destroy(gameObject, Explose);
		}
	}

	void DropPlatform()
	{
		rb.velocity = new Vector2(transform.position.x, pointClass.transform.position.y);
	}
}
