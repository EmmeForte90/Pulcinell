using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public GameObject explosion;

    public float speed;

    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlaySFX(2);
        rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Calcolo posizione
        //rb.velocity = transform.right * speed;
        transform.position += new Vector3(speed * transform.localScale.x * Time.deltaTime, 0f, 0f);


    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        AudioManager.instance.PlaySFX(1);

        //Se colpisce il nemico
        if (other.tag == "Enemy")
        {
            //Il nemico subisce danno
            Debug.Log("Hit enemey");
            IDamegable hit = other.GetComponent<IDamegable>();
            hit.Damage();
            //DatabaseEnemy.instance.Damage();
            CameraShake.Shake(0.10f, 0.50f);

            Instantiate(explosion, transform.position, transform.rotation);

            Destroy(this.gameObject);

        }

        //Se colpisce un boss

        if (other.tag == "Boss")
        {
          
            Instantiate(explosion, transform.position, transform.rotation);

            Destroy(this.gameObject);

        }

        //Se colpisce lo scenario

        if (other.tag == "Ground")
        {

            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);

        }




    }
}
