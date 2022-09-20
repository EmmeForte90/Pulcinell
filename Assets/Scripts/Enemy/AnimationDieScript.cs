using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDieScript : MonoBehaviour
{
    Rigidbody2D rb;
    public Animator anim;
    public bool isPlayer;
    public float speed;
    public float Delay;
    public float destroyTime;
    bool isDead;
    public GameObject PDIE;

    // Start is called before the first frame update
    void Start()
    {
        //AudioManager.instance.PlaySFX(2);
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.SetTrigger("isDead");


    }

    // Update is called once per frame
    void Update()
    {

        if (isPlayer)
        {
            PlayerDie();
        }
        else
        {
            trackMovement();
        }


    }

    void PlayerDie()
    {
        Vector2 direction = rb.velocity;
        //anim.SetBool("isDead", isDead);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }


    void trackMovement()
    {
        Vector2 direction = rb.velocity;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        StartCoroutine(Destroy());
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

    }

IEnumerator Destroy()
{
    yield return new WaitForSeconds(destroyTime);
    Destroy(gameObject);

}
    //IEnumerator Scaletime()
    //{
      //  PDIE.transform.localScale += new Vector3(1, 1, 0) * Delay * Time.deltaTime;
        //CameraShake.Shake(0.2f, 1f);
        //yield return new WaitForSeconds(0.2f);
        //PDIE.transform.localScale = new Vector3(13, 10, 0);
        //yield return new WaitForSeconds(1);

        //yield return null;

    //}


}
