using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMove : MonoBehaviour
{
    Rigidbody2D body;
    float hDirection;
    [SerializeField]
    float moveSpeed = 5f, jumpForce = 10f, dashForce = 10f;
    [SerializeField]
    LayerMask layerMask;
    bool isGround = false, canDoubleJump = false;

    bool isDash = false;
    float lastDashTime;
    [SerializeField] float dashTime = 5f; 
    Vector2 dashDir;
    Vector2 wallJumpDir;

    bool canWallJumpLeft = false, canWallJumpRight = false, isWallJumping = false;


    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        lastDashTime = Time.time;
    }

    

    private void FixedUpdate()
    {
    ApplyMovement();
    CheckGround();
    checkWallJump();
    }


    void ApplyMovement()
    {
        body.velocity = new Vector2(hDirection * moveSpeed, body.velocity.y);
        
        if(isDash)
        {
            body.velocity = Vector2.zero;
            body.AddForce(dashDir.normalized * dashForce, ForceMode2D.Impulse);
        
        if(Time.time > lastDashTime + dashTime)
        {
        isDash = false;
        body.velocity = Vector2.zero;
        }
        }
        if(isWallJumping)
        {
            body.velocity = wallJumpDir * jumpForce; //jumpForce;
        }
        
    }

void checkWallJump()
{
     if(Physics2D.Raycast(transform.position, Vector2.left, 1f, layerMask))
        {
            canWallJumpLeft = true;
            canWallJumpRight = false;
            isWallJumping = false;

        }
        else
        {
            canWallJumpLeft = false;

        }
        
        if(Physics2D.Raycast(transform.position, Vector2.right, 1f, layerMask))
        {
            canWallJumpLeft = false;
            canWallJumpRight = true;
            isWallJumping = false;
        }
        else
        {
            canWallJumpRight = false;
        }
}

    public void Dash()
    {
        isDash = true;
        lastDashTime = Time.time;
        /*Vector3 rawMouse = Input.mousePosition;
        Vector2 mousePosition = GetComponent<Camera>().main.screenToWorldPoint(rawMouse);
        dashDir = mousePosition - body.position;*/
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
        else if(canWallJumpLeft)
        {
            isWallJumping = true;
            wallJumpDir = new Vector2(1, 1);
        
        }
        else if(canWallJumpRight)
        {
            isWallJumping = true;
            wallJumpDir = new Vector2(-1, 1);
        
        }
    }



    public void GetMovementDirection(float inputDir)
    {
        hDirection = inputDir;
    }
}
