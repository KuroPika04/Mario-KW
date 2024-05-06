using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    // references
    private new Camera camera;
    private Rigidbody2D body;
    private float horizontal_input;
    private Vector2 velocity;

    [SerializeField] private float speed;
    [SerializeField] private float jump_power;
    [SerializeField] private Transform ground_check;
    [SerializeField] private LayerMask ground_layer;
    [SerializeField] private Transform wall_check;
    [SerializeField] private LayerMask wall_layer;
    public float maxJumpHeight = 5f;
    public float maxJumpTime = 1f;

    public float jumpForce => (2f * maxJumpHeight) / (maxJumpTime / 2f);
    public bool jumping { get; private set; }
    public bool running => Mathf.Abs(velocity.x) > 0.25f || Mathf.Abs(horizontal_input) > 0.25f;
    public bool sliding => (horizontal_input > 0f && velocity.x < 0f) || (horizontal_input < 0f && velocity.x > 0f);

    private float coyote_timer = 0.2f; // jump mechanic to make jump more realistic and smoother play
    private float coyote_counter;
    private float jumpbuffer_timer = 0.2f;
    private float jumpbuffer_counter;
    private bool wall_sliding;
    private float wallSlidingSpeed = 2f;
    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        camera = Camera.main;
    }
    void Update(){ 

        // Player Movement
        horizontal_input = Input.GetAxis("Horizontal");
        body.velocity = new Vector2(horizontal_input * speed, body.velocity.y);

        // Character Flip
        if (horizontal_input > 0.01)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontal_input < -0.01)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (grounded())
        {
            velocity.y = Mathf.Max(velocity.y, 0f);
            jumping = velocity.y > 0f;
            coyote_counter = coyote_timer;
        }
        else
        {
            coyote_counter -= Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpbuffer_counter = jumpbuffer_timer;
        }
        else
        {
            jumpbuffer_counter -= Time.deltaTime;
        }

        // Player Jump Input
        if (jumpbuffer_counter > 0f && coyote_counter > 0f)
        {
            Jump(); // Normal Jump
            jumpbuffer_counter = 0f;
        }
        if(Input.GetKeyUp(KeyCode.Space) && body.velocity.y > 0f)
        {
            ReleaseJump(); // Jump higher when hold space
            coyote_counter = 0f;
        }
    }
    /*private void FixedUpdate()
    {
        Vector2 position = body.position;
        position += velocity * Time.deltaTime;

        Vector2 leftEdge = camera.ScreenToWorldPoint(Vector2.zero);
        Vector2 rightEdge = camera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        position.x = Mathf.Clamp(position.x, leftEdge.x + 0.5f, rightEdge.x - 0.5f);

        body.MovePosition(position);
    }*/
    private void Jump()
    {
        body.velocity = new Vector2 (body.velocity.x, jump_power);
        jumping = true;
    }
    private void ReleaseJump()
    {
        body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.5f);
    }
    private bool grounded()
    {
        return Physics2D.OverlapCircle(ground_check.position, 0.2f, ground_layer);
    }
    private bool TouchWall()
    {
        return Physics2D.OverlapCircle(wall_check.position, 0.2f, wall_layer);
    }
    private void WallSlide()
    {
        if (TouchWall() && grounded() && horizontal_input != 0)
        {
            wall_sliding = true;
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            wall_sliding = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (transform.DotTest(collision.transform, Vector2.down))
            {
                velocity.y = jumpForce / 2f;
                jumping = true;
            }
        }
        else if (collision.gameObject.layer != LayerMask.NameToLayer("PowerUp"))
        {
            if (transform.DotTest(collision.transform, Vector2.up))
            {
                velocity.y = 0f;
            }
        }
    }
}
