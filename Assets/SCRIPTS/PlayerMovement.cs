using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   public CharacterController controller;
 
    public float speed = 12f;
    public float walkingGravity = -9.81f * 2;
    public float gravity;


    public float jumpHeight = 2f;
 
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
 
    Vector3 velocity;
 
    public bool isGrounded;
    public bool isUnderwater;

    private Vector3 lastPositon = new Vector3(0f, 0f, 0f);

    private bool isMoving;

    //Swimming
    public bool isSwimming;
    public float swimmingGravity = -0.5f;


    // Update is called once per frame
    void Update()
    {
        //if (!DialogSystem.Instance.dialogUIActive &&
        //    !StorageManager.Instance.storageUIOpen &&
        //    !CampfireUIManager.Instance.isUiOpen)
        //{
        //    Movement();
        //}

        if (MovementManager.Instance.canMove)
        {
            Movement();
        }
    }

    public void Movement()
    {
        if (isSwimming)
        {
            if (isUnderwater)
            {
                gravity = swimmingGravity;
            }
            else
            {
                velocity.y = 0f;
            }
            
        }
        else
        {
            gravity = walkingGravity;
        }


        //checking if we hit the ground to reset our falling velocity, otherwise we will fall faster the next time
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //right is the red Axis, foward is the blue axis
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        //check if the player is on the ground so he can jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            //the equation for jumping
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (lastPositon != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;

            SoundManager.Instance.PlaySound(SoundManager.Instance.grassWalkSound);
        }
        else
        {
            isMoving = false;
            SoundManager.Instance.grassWalkSound.Stop();
        }
        lastPositon = gameObject.transform.position;
    }
}
