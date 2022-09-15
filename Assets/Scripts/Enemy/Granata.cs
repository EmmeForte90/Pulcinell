using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class Granata : MonoBehaviour
{

    public GameObject explosion;
    Rigidbody2D rb;
    public GameObject granata;

    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.PlaySFX(2);
        rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

        trackMovement();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            PlayerHealthController.instance.Dealdamage();
        }

               AudioManager.instance.PlaySFX(3);


        if (other.tag == "Ground")
        {
            Destroy(gameObject);

            Instantiate(explosion, transform.position, transform.rotation);
        }


        


    }

    void trackMovement()
    {
        Vector2 direction = rb.velocity;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

}
