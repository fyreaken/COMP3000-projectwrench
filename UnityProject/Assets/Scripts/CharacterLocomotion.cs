using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotion : MonoBehaviour
{
    Animator animator;
    Vector2 input;
    Rigidbody m_Rigidbody;

    public bool canMove = true; //canMove bool (determines if player can move in scene)

    public float MoveSpeed = 0; //player speed when not sprinting
    public float SprintSpeed = 0; //player speed when sprinting

    private Alteruna.Avatar _avatar; //multiplayer api avatar component

    // Start is called before the first frame update
    void Start()
    {
        _avatar = GetComponent<Alteruna.Avatar>(); //gets Alteruna avatar component

        animator = GetComponent<Animator>(); //gets Animator component

        if (!_avatar.IsMe) //if the Player is not owned by the User, return, don't run remaining script
            return;

        m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_avatar.IsMe) //if the Player is not owned by the User, return, don't run remaining script
            return;

        if (Input.GetKeyUp(KeyCode.Escape)){ //if ESC button is pressed
            canMove = !canMove; //enables/disables canMove bool
        }

        if (canMove == true){ //if canMove = true, enable Player movement and sprint
            input.x = Input.GetAxis("Horizontal");
            input.y = Input.GetAxis("Vertical");

            animator.SetFloat("InputX", input.x);
            animator.SetFloat("InputY", input.y);

            PlayerSprint();
        }
    }

    void PlayerSprint()
    {
        if (!_avatar.IsMe) //if the Player is not owned by the User, return, don't run remaining script
            return;
        
        if (Input.GetKey(KeyCode.LeftShift)) { //speeds up animator (makes player faster) when L-SHIFT is pressed
            animator.speed = SprintSpeed;       // ^^ temp workaround
        }
        else {
            animator.speed = MoveSpeed;
        }
    }
}
