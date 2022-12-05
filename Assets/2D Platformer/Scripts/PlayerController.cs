using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        public float slingshotForce;
        private float moveInput;


        private bool facingRight = false;
        [HideInInspector]
        public bool deathState = false;
        public bool winState = false;


        private bool isGrounded;
        public Transform groundCheck;
        private float coyoteTime = 0.1f;
        private float coyoteTimeCounter;

        private bool isFrozen = false;


        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;
        private State state;

        private MultiClockController lastCollidedClock;
        private GameObject lastCollidedFinish;

        void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        void Update()
        {
            switch (state)
            {
                case State.Moving:
                    if (Input.GetButton("Horizontal"))
                    {
                        moveInput = Input.GetAxis("Horizontal");
                        Vector3 direction = transform.right * moveInput;
                        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
                        animator.SetInteger("playerState", 1); // Turn on run animation
                    }
                    else
                    {
                        if (isGrounded) animator.SetInteger("playerState", 0); // Turn on idle animation
                    }
                    if (isGrounded)
                    {
                        coyoteTimeCounter = coyoteTime;
                    }
                    else
                    {
                        coyoteTimeCounter -= Time.deltaTime;
                    }
                    if (Input.GetKeyDown(KeyCode.Space) && coyoteTimeCounter > 0)
                    {

                        rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
                        animator.SetInteger("playerState", 2); // Turn on jump animation
                        // coyoteTimeCounter = 0;
                    }

                    if (facingRight == false && moveInput > 0)
                    {
                        Flip();
                    }
                    else if (facingRight == true && moveInput < 0)
                    {
                        Flip();
                    }
                    break;
                case State.Clock:
                    freezePlayer();
                    // rigidbody.gravityScale = 0;
                    // Move it to the center of the clock
                    transform.position = Vector3.MoveTowards(transform.position, lastCollidedClock.transform.position, movingSpeed * Time.deltaTime);
                    // Slingshot the player in their chosen direction if they press space
                    if (lastCollidedClock.getState() == MultiClockController.State.Shooting && lastCollidedClock.getShouldLaunch())
                    {
                        unfreezePlayer();
                        // rigidbody.gravityScale = 1;
                        // Debug.Log("Launching player");

                        rigidbody.AddForce(lastCollidedClock.GetArrowVector() * slingshotForce, ForceMode2D.Impulse);

                        lastCollidedClock.setState(MultiClockController.State.Inactive);
                        lastCollidedClock = null;
                        state = State.Moving;
                    }
                    if (lastCollidedClock == null)
                    {
                        unfreezePlayer();
                        state = State.Moving;
                    }
                    break;
                case State.Frozen:
                    GameObject finishChild = lastCollidedFinish.transform.GetChild(0).gameObject;
                    finishChild.SetActive(false);
                    rigidbody.velocity = Vector2.zero;
                    transform.position = Vector3.MoveTowards(transform.position, finishChild.transform.position, movingSpeed * Time.deltaTime);
                    // rigidbody.Sleep();
                    break;
            }
        }
        enum State
        {
            Moving,
            Clock,
            Frozen,
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            // Make sure that the colliders arent a clock or an arrow
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject.tag == "Ground")
                {
                    isGrounded = true;
                    return;
                }
            }
            isGrounded = false;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                deathState = true; // Say to GameManager that player is dead
            }
            else
            {
                deathState = false;
            }
        }
        public void freezePlayer()
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            rigidbody.velocity = Vector2.zero;
            isFrozen = true;
        }

        public void unfreezePlayer()
        {
            rigidbody.constraints = RigidbodyConstraints2D.None;
            rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            isFrozen = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Coin")
            {
                gameManager.coinsCounter += 1;
                Destroy(other.gameObject);
            }
            else if (other.gameObject.tag == "Clock")
            {

                lastCollidedClock = other.gameObject.GetComponent<MultiClockController>();
                if (lastCollidedClock.getState() != MultiClockController.State.Idle)
                {
                    Debug.Log("Clock is not idle");
                    lastCollidedClock = null;
                    return;
                }
                state = State.Clock;
                lastCollidedClock.setState(MultiClockController.State.PickingDirection);
            }
            if (other.gameObject.tag == "Finish")
            {
                winState = true;
                state = State.Frozen;
                lastCollidedFinish = other.gameObject;
            }
        }
    }
}
