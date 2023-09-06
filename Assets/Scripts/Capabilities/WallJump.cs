using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WallJump : MonoBehaviour
{

    public bool wallJumping { get; private set; }

    [Header("Wall Slide"), SerializeField, Range(0.1f, 5f)]
    private float wallSlideMaxSpeed = 2f;
    [Header("Wall Jump"), SerializeField]
    private Vector2 wallJumpClimb = new Vector2(4f, 12f);
    private Vector2 wallJumpBounce = new Vector2(10.7f, 10f);
    private Vector2 wallJumpLeap = new Vector2(14f, 12f);

    private CollisionData collisionData;
    private Rigidbody2D body;
    [SerializeField] private InputController input = null;

    private Vector2 velocity;
    private bool onWall, onGround, desiredJump;
    private float wallDirectionX;
    public bool unlocked = false;

    void Start()
    {
        collisionData = GetComponent<CollisionData>();
        body = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (onWall && !onGround)
        {
            desiredJump |= input.RetrieveJumpInput();
        }
    }

    private void FixedUpdate()
    {
        if (unlocked)
        {
            velocity = body.velocity;
            onWall = collisionData.onWall;
            onGround = collisionData.onGround;
            wallDirectionX = collisionData.contactNormal.x;

            if (onWall)
            {
                if (velocity.y < -wallSlideMaxSpeed)
                {
                    velocity.y = -wallSlideMaxSpeed;
                }
            }

            if (onWall && velocity.x == 0 || onGround)
            {
                wallJumping = false;
            }

            if (desiredJump)
            {
                if (-wallDirectionX == input.RetrieveMoveInput())
                {
                    velocity = new Vector2(wallJumpClimb.x * wallDirectionX, wallJumpClimb.y);
                    wallJumping = true;
                    desiredJump = false;
                }
                else if (input.RetrieveMoveInput() == 0)
                {
                    velocity = new Vector2(wallJumpBounce.x * wallDirectionX, wallJumpBounce.y);
                    wallJumping = true;
                    desiredJump = false;
                }
                else
                {
                    velocity = new Vector2(wallJumpLeap.x * wallDirectionX, wallJumpLeap.y);
                    wallJumping = true;
                    desiredJump = false;
                }
            }

            body.velocity = velocity;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collisionData.EvaluateCollision(collision);
        if (collisionData.onWall && !collisionData.onGround && wallJumping)
        {
            body.velocity = Vector2.zero;
        }
    }
}

