﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slammer : MonoBehaviour
{
    public Transform theSlammer, slammerTarget, corda, cordatarget;
    private Vector3 startPoint;

    public float slamSpeed, waitAfterSlam, resetSpeed, cordaresetspeed;
    private float waitCounter;
    private bool slamming, resetting;

    // Start is called before the first frame update
    void Start()
    {
        startPoint = theSlammer.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Se non sta attaccando
        if (!slamming && !resetting)
        {
            if (Vector3.Distance(slammerTarget.position, PlayerController.instance.transform.position) < 2f)
            {
                slamming = true;
                waitCounter = waitAfterSlam;
            }
        }
        //Se sta attaccando
        if (slamming)
        {
            theSlammer.position = Vector3.MoveTowards(theSlammer.position, slammerTarget.position, slamSpeed * Time.deltaTime);
            corda.position = Vector3.MoveTowards(corda.position, cordatarget.position, slamSpeed * Time.deltaTime);


            //Quando colpisce l'area designata
            if (theSlammer.position == slammerTarget.position)
            {
                CameraShake.Shake(0.10f, 0.50f);

                waitCounter -= Time.deltaTime;
                if (waitCounter <= 0)
                {
                    slamming = false;
                    resetting = true;
                }

            }
        }
        //Modalità di ripristino
        if (resetting)
        {
            theSlammer.position = Vector3.MoveTowards(theSlammer.position, startPoint, resetSpeed * Time.deltaTime);
            corda.position = Vector3.MoveTowards(corda.position, startPoint, cordaresetspeed * Time.deltaTime);

            if (theSlammer.position == startPoint)
            {
                resetting = false;
            }
        }
    }
}
