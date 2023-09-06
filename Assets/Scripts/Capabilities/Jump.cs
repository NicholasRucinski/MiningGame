using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Jump : NetworkBehaviour
{

    [SerializeField] private InputController input = null;
    [SerializeField, Range(0f, 10f)] private float jumpHeight = 3f;
    [SerializeField, Range(0, 5)] private int maxAirJumps = 0;
    [SerializeField, Range(0f, 5f)] private float downwardMovementMultiplier = 3f;
    [SerializeField, Range(0f, 5f)] private float upwardMovementMultiplier = 1.7f;
    [SerializeField, Range(0f, 0.3f)] private float coyoteTime = 0.2f;
    [SerializeField, Range(0f, 0.3f)] private float jumpBufferTime = 0.2f;

    [SerializeField, Range(0f, 10f)] private float dashDistance = 3f;
    [SerializeField, Range(0, 5)] private int maxDashes = 0;

    private Rigidbody2D body;
    private CollisionData ground;
    private Vector2 velocity;

    private int jumpPhase;
    private float coyoteCounter;
    private float defaultGravityScale;
    private float jumpBufferCounter;

    private bool desiredJump;
    public bool onGround;
    private bool isJumping;

    private Vector2 direction;
    private int dashPhase;
    private bool desiredDash;


    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<CollisionData>();

        defaultGravityScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        desiredJump |= input.RetrieveJumpInput();
        desiredDash |= input.RetrieveDashInput();
        direction.x = input.RetrieveMoveInput();

    }

    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        onGround = ground.GetOnGround();
        velocity = body.velocity;

        if (onGround && body.velocity.y == 0)
        {
            jumpPhase = 0;
            coyoteCounter = coyoteTime;
            isJumping = false;
            dashPhase = 0;
        } else {
            coyoteCounter -= Time.deltaTime;
        }

        if (desiredJump) {
            desiredJump = false;
            jumpBufferCounter = jumpBufferTime;
        } else if (!desiredJump && jumpBufferCounter > 0) {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (desiredDash)
        {
            desiredDash = false;
            dashAction();
        }

        if (jumpBufferCounter > 0) {
            JumpAction();
        }

        if (input.RetrieveHoldInput() && body.velocity.y > 0)
        {
            body.gravityScale = upwardMovementMultiplier;
        }
        else if (!input.RetrieveHoldInput() || body.velocity.y < 0)
        {
            body.gravityScale = downwardMovementMultiplier;
        }
        else if (body.velocity.y == 0)
        {
            body.gravityScale = defaultGravityScale;
        }
        
        body.velocity = velocity;


    }

    private void JumpAction()
    {
        if (coyoteCounter > 0f || (jumpPhase < maxAirJumps && isJumping)) {

            if (isJumping) {
                jumpPhase += 1;
            }

            jumpBufferCounter = 0;
            coyoteCounter = 0;
            float jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight);
            isJumping = true;
            if (velocity.y > 0f) {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0);
            }
            velocity.y += jumpSpeed;
        }
    }

    private void dashAction()
    {
        if (dashPhase < maxDashes)
        {
            dashPhase += 1;
            float dashSpeed;
            if (direction.x <= 0)
            {
                dashSpeed = -2f * dashDistance;
            }
            else
            {
                dashSpeed = 2f * direction.x * dashDistance;
            }
            velocity.x += dashSpeed;
        }
    }

}


