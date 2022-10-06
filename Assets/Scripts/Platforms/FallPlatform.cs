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
    [SerializeField] public float agroRange;
    private bool slamming, resetting;
	
	[Header("Time")]
	[SerializeField] float timerValue;
	[SerializeField] float waitFall;//Il valore numero del timer

    void Awake()
    {
        startPoint = thePlatform.position;
		timerValue = waitFall;

    }

    void Update()
    {
#region Targetizzazione
        //Se non sta attaccando
        Debug.DrawRay(transform.position, new Vector2(0, agroRange), Color.red);
        Debug.DrawRay(transform.position, new Vector2(agroRange, 0), Color.blue);
        float disToPlayer = Vector2.Distance(transform.position, PlayerMovement.instance.transform.position);
        if(disToPlayer < agroRange)
        {
            if (!slamming && !resetting)
        {
            StartCoroutine(UpdateTimer());
			//UpdateTimer();
        }
        }
       /* if (!slamming && !resetting)
        {
            if (Vector3.Distance(slammerPoint.position, PlayerMovement.instance.transform.position) < 3f)
            {
				UpdateTimer();
                
            }
        }

        */
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

private IEnumerator UpdateTimer()
{
    yield return new WaitForSeconds(timerValue);
    slamming = true;
    waitCounter = waitAfterSlam;
}
	
/*void UpdateTimer()
    {
        timerValue -= Time.deltaTime;
        //Indica quanto valore perde la variabile a frame
            if(timerValue < 0) //E il valore del timer è maggiore di 0
            {
                slamming = true;
                waitCounter = waitAfterSlam;
            }
           
    }*/
}
