using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Header("Parametri")]

    //Qui va la camera
    public Transform Cam;
    public Transform farBackground, middleBackground;
    public bool stopFollow;
    private Vector2 lastPos;



    // Start is called before the first frame update
    void Start()
    {
        //lastXPos = transform.position.x;
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {


        if (!stopFollow)
        {
            transform.position = new Vector3(Cam.position.x, Cam.position.y, transform.position.z);


            Vector2 amountToMove = new Vector2(transform.position.x - lastPos.x, transform.position.y - lastPos.y);

//            farBackground.position = farBackground.position + new Vector3(amountToMove.x, amountToMove.y, 0f);
            middleBackground.position += new Vector3(amountToMove.x, amountToMove.y, 0f) * .5f;


            lastPos = transform.position;
        }
    }
}


