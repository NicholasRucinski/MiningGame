using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Move : NetworkBehaviour
{
    [SerializeField] private InputController input;
    [SerializeField, Range(0f, 100f)] private float maxSpeed = 4f;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration = 35f;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration = 20f;
    [SerializeField, Range(0f, 100f)] private float wallStickTime = 0.25f;

    private Vector2 direction;
    private Vector2 desiredVelocity;
    private Vector2 velocity;

    private Rigidbody2D body;
    private CollisionData collisionData;
    private WallJump wallJump;

    private float maxSpeedChange;
    private float acceleration;
    private float wallStickCounter;

    private bool onGround;
    private bool facingRight = true;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        collisionData = GetComponent<CollisionData>();
        wallJump = GetComponent<WallJump>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner)
        {
            return;
        }
        direction.x = input.RetrieveMoveInput();
        desiredVelocity = new Vector2(direction.x, 0f) * Mathf.Max(maxSpeed - collisionData.GetFriction(), 0f);
    }

    private void FixedUpdate()
    {
        if (!IsOwner)
        {
            return;
        }
        onGround = collisionData.GetOnGround();
        velocity = body.velocity;

        acceleration = onGround ? maxAcceleration : maxAirAcceleration;
        maxSpeedChange = acceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        if (velocity.x > 0 && !facingRight)
        {
            flipCharacter();
        } else if (velocity.x < 0 && facingRight)
        {
            flipCharacter();
        }

        body.velocity = velocity;
    }

    private void flipCharacter()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
