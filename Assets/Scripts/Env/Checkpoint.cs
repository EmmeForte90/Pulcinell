using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    public SpriteRenderer theSR;

    public Sprite cpOn, cpOff;

    private Animator anim;

    private bool checkOn;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.CompareTag("Player"))
        {
            //Disattiva i checkpoint precedenti
                CheckpointController.instance.DeactivateCheckpoints();
                theSR.sprite = cpOn;
            checkOn = true;
            anim.SetBool("checkOn", checkOn);

            //E setta quello toccato come attuale
            CheckpointController.instance.SetSpawnPoint(transform.position);


        }

    }


    public void ResetCheckpoint()
    {
        
        theSR.sprite = cpOff;
        checkOn = false;
        anim.SetBool("checkOn", checkOn);


    }

}
