using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Usa le API di unity di base
using UnityEngine.InputSystem;

public class LSPlayer : MonoBehaviour
{
    [SerializeField] public MapPoint currentPoint;
    [SerializeField] public float moveSpeed = 10f;
    private bool levelLoading;
    [SerializeField] public LSManager theManager;
    bool stopInput = false;
    //Blocca i movimenti
    [SerializeField] public GameObject PauseMenu;

    
#region Pausa
public void OnPause(InputValue value)
//Funzione pausa
{
    if (value.isPressed && !stopInput)
    {
        stopInput = true;
        PauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0f;
    }
    else if(value.isPressed && stopInput)
    {
        stopInput = false;
        Time.timeScale = 1;
        PauseMenu.gameObject.SetActive(false);
    }
}
#endregion


    void Update()
    {
        if(!stopInput)
        {
        transform.position = Vector3.MoveTowards(transform.position, currentPoint.transform.position, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, currentPoint.transform.position) < .1f && !levelLoading)
        {

            if (Input.GetAxisRaw("Horizontal") > .5f)
            {
                if (currentPoint.right != null)
                {
                    SetNextPoint(currentPoint.right);
                    AudioManager.instance.PlaySFX(1);
                }
            }

            if (Input.GetAxisRaw("Horizontal") < -.5f)
            {
                if (currentPoint.left != null)
                {
                    SetNextPoint(currentPoint.left);
                    AudioManager.instance.PlaySFX(1);

                }
            }

            if (Input.GetAxisRaw("Vertical") > .5f)
            {
                if (currentPoint.up != null)
                {
                    SetNextPoint(currentPoint.up);
                    AudioManager.instance.PlaySFX(1);

                }
            }

            if (Input.GetAxisRaw("Vertical") < -.5f)
            {
                if (currentPoint.down != null)
                {
                    SetNextPoint(currentPoint.down);
                    AudioManager.instance.PlaySFX(1);
                }
            }

            if(currentPoint.isLevel && currentPoint.levelToLoad != "" && !currentPoint.isLocked)
            {
                LSUIController.instance.ShowInfo(currentPoint);

                if(Input.GetButtonDown("Jump"))
                {
                    levelLoading = true;
                    AudioManager.instance.PlaySFX(2);
                    theManager.LoadLevel();
                }
            }
        }

        }
    }

    public void SetNextPoint(MapPoint nextPoint)
    {
        currentPoint = nextPoint;
        //LSUIController.instance.HideInfo();

        //AudioManager.instance.PlaySFX(5);
    }
}
