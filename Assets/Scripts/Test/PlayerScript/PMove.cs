using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMove : MonoBehaviour
{
    Rigidbody2D body;
    float hDirection;
    [SerializeField]
    float moveSpeed = 5f, jumpForce = 10f;
    [SerializeField]
    LayerMask layerMask;
    bool isGround = false, canDoubleJump = false;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
    CheckGround();
    ApplyMovement();
    }


    void ApplyMovement()
    {
        body.velocity = new Vector2(hDirection * moveSpeed, body.velocity.y);
    }

    void CheckGround()
    {
        if(Physics2D.Raycast(transform.position, Vector2.down, 1f, layerMask))
        {
            isGround = true;
            canDoubleJump = true;
        }
        else
        {
            isGround = false;

        }
    }

    public void Jump()
    {
        if(isGround)
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        } else if(canDoubleJump)
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            canDoubleJump = false;
        }
    }



    public void GetMovementDirection(float inputDir)
    {
        hDirection = inputDir;
    }
}
