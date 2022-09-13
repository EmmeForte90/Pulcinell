using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public Transform[] points;

    public float moveSpeed;

    public int currentPoint;

    public Transform platform;


    // Start is called before the first frame update
    void Start()
    {
        


    }

    // Update is called once per frame
    void Update()
    {
        //La piattaforma si muove nei punti e nella velocità stabiliti
        platform.position = Vector3.MoveTowards(platform.position, points[currentPoint].position, moveSpeed * Time.deltaTime);

        //Conteggio dei puntidi locazione
        if(Vector3.Distance(platform.position, points[currentPoint].position) <.05f)
        {
            currentPoint++;

            if(currentPoint >= points.Length)
            {
                currentPoint = 0;

            }

        }
    }
}
