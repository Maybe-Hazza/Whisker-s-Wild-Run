using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpVerticalSpeed = 5f;
    [SerializeField] float jumpHorizontalSpeed = -5f;
    [SerializeField] float climbSpeed = 5f;


    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    BoxCollider2D myBoxCollider;
    float gravityScaleAtStart;


    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBoxCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = myRigidbody.gravityScale;

    }

    void Update()
    {
        Run();
        FlipSprite();
        ClimbLadder();
    }

    void OnMove(InputValue value) 
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value) 
    {
        if (!myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {return;}

        if (value.isPressed) 
        {
            myRigidbody.velocity += new Vector2(jumpHorizontalSpeed, jumpVerticalSpeed);
        }
    }

    void Run() 
    { 
        Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
    }


    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
        }
    }

    void ClimbLadder()
    {
        if (!myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            myAnimator.SetBool("isClimbing", false);
            myRigidbody.gravityScale = gravityScaleAtStart;
            return;
        }

        Vector2 climbVelocity = new Vector2 (myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;
        myAnimator.SetBool("isClimbing", true);

        if (myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ladders")) && myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) 
        {
            myAnimator.SetBool("isClimbing", false);
        }
    }

}
