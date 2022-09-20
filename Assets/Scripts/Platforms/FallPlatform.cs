using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
	[Header("GameObject e Target")]
	public Transform thePlatform, slammerTarget, slammerPoint, slammerReset; 
    private Vector3 startPoint;
    public float slamSpeed, waitAfterSlam, resetSpeed;
    private float waitCounter;
    private bool slamming, resetting;
	
	[Header("Time")]
	[SerializeField] float timerValue;
	[SerializeField] float waitFall;//Il valore numero del timer

    void Start()
    {
        startPoint = thePlatform.position;
		timerValue = waitFall;
    }

    void Update()
    {
#region Targetizzazione
        //Se non sta attaccando
        if (!slamming && !resetting)
        {
            if (Vector3.Distance(slammerPoint.position, PlayerMovement.instance.transform.position) < 2f)
            {
				UpdateTimer();
                
            }
        }
        //Se sta attaccando
        if (slamming)
        {
            thePlatform.position = Vector3.MoveTowards(thePlatform.position, slammerTarget.position, slamSpeed * Time.deltaTime);
            //corda.position = Vector3.MoveTowards(corda.position, cordatarget.position, slamSpeed * Time.deltaTime);


            //Quando colpisce l'area designata
            if (thePlatform.position == slammerTarget.position)
            {
               // CameraShake.Shake(0.10f, 0.50f);

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
            thePlatform.position = Vector3.MoveTowards(thePlatform.position, slammerReset.position, resetSpeed * Time.deltaTime);
            //corda.position = Vector3.MoveTowards(corda.position, startPoint, cordaresetspeed * Time.deltaTime);

            if (thePlatform.position == slammerReset.position)
            {
                resetting = false;
				timerValue = waitFall;

            }
        }
    }
	#endregion
	
void UpdateTimer()
    {
        timerValue -= Time.deltaTime;
        //Indica quanto valore perde la variabile a frame
            if(timerValue < 0) //E il valore del timer è maggiore di 0
            {
                slamming = true;
                waitCounter = waitAfterSlam;
            }
           
    }
}
