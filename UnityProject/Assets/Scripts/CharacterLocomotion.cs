using System.Collections;
using System.Collections.Generic;
using Alteruna;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    Animator animator;
    Vector2 input;
    Rigidbody m_Rigidbody;

    public bool canMove = true; //canMove bool (determines if player can move in scene)

    public float MoveSpeed = 0; //player speed when not sprinting
    public float SprintSpeed = 0; //player speed when sprinting
    public float jumpForce = 0;

    public bool grounded;
    public float distToGround;

    private Alteruna.Avatar _avatar; //multiplayer api avatar component

    // Start is called before the first frame update
    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>(); //gets RigidbodySynchronizable component
        _avatar = GetComponent<Alteruna.Avatar>(); //gets AlterunaAvatar component
        animator = GetComponent<Animator>(); //gets Animator component
    }

    // Update is called once per frame
    void Update()
    {
        if (!_avatar.IsMe) //if Avatar is not owned by User
            return;             //do not run script

        if (Input.GetKeyUp(KeyCode.Escape)){ //if ESC button is pressed
            canMove = !canMove; //toggles canMove bool
        }

        if (canMove == true){ //enable Player movement and sprint
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");

            animator.SetFloat("InputX", input.x);
            animator.SetFloat("InputY", input.y);

            PlayerSprint();
            PlayerJump();
        }
    }

    void PlayerSprint()
    {
        if (!_avatar.IsMe) //if Avatar is not owned by User
            return;             //do not run script

        if (Input.GetKey(KeyCode.LeftShift)){ //if key pressed, player sprints
            animator.speed = SprintSpeed;     //increases animator speed
        }                                     // ^temp workaround
        else {
            animator.speed = MoveSpeed; //resets animator speed
        }
    }

    void PlayerJump()
    {
        if (!_avatar.IsMe) //if Avatar is not owned by User
            return;             //do not run script

        if (Input.GetKeyDown(KeyCode.Space) && GroundCheck()){ //if key pressed & player is grounded,
            m_Rigidbody.AddForce(jumpForce * Vector3.up, ForceMode.Impulse); //player jumps
        }       //jump mechanic is glitchy because rigidbody is not synchronized
    }

    public bool GroundCheck(){ //creates a raycast from players feet to check if player is touching the ground
        grounded = Physics.Raycast(transform.position, -Vector3.up, distToGround);
        return grounded; //returns true or false
    }       //this raycast can be buggy and not allow player jump sometimes
}
