using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CharacterAiming : MonoBehaviour
{
    public float turnSpeed = 15f; //player turn speed
    Camera mainCamera;

    public bool canMove = true; //canMove bool (determines if player can move in scene)

    [SerializeField] public GameObject UICanvas; //player ui object
    //[SerializeField] public GameObject Username;

    [SerializeField] private CinemachineFreeLook freelookcamera; //freelookcamera object
    [SerializeField] Transform target;

    private Alteruna.Avatar _avatar; //multiplayer api avatar component

    void Start()
    {
        _avatar = GetComponent<Alteruna.Avatar>(); //gets Alteruna avatar component

        if (!_avatar.IsMe) //if the Player is not owned by the User, return, don't run remaining script
            return;

        freelookcamera = CinemachineFreeLook.FindObjectOfType<CinemachineFreeLook>(); //gets freelookcamera object in Scene
        freelookcamera.LookAt = target.transform; //assigns a gameObject to the camera's LookAt field
        freelookcamera.Follow = this.gameObject.transform; //assigns the Player object to the camera's Follow field (camera will follow this object once the field set)

        mainCamera = Camera.main; //puts the Camera object into mainCamera variable
        Cursor.visible = false; //hides cursor
        Cursor.lockState = CursorLockMode.Locked; //locks cursor to application
    }

    void Update() {
        if (!_avatar.IsMe) //if the Player is not owned by the User, return, don't run remaining script
            return;

        UICanvas.SetActive(true); //enables Player UI (if user owns the Player object)
        //Username.SetActive(false);

        if (Input.GetKeyDown(KeyCode.Escape)){ //if ESC button is pressed
            canMove = !canMove; //enables/disables canMove bool
        }

        if (canMove == false){ //if canMove = false, player cannot move camera on X and Y axis
            freelookcamera.m_YAxis.m_MaxSpeed = 0; //^^ this is used when the player activates the pause menu with ESC
            freelookcamera.m_XAxis.m_MaxSpeed = 0;
        } else if (canMove == true){ //player can move camera on X and Y axis
            freelookcamera.m_YAxis.m_MaxSpeed = 2;
            freelookcamera.m_XAxis.m_MaxSpeed = 300;
        }
    }

    void FixedUpdate()
    {
        if (!_avatar.IsMe) //if the Player is not owned by the User, return, don't run remaining script
            return;
            
        if (canMove == true){ //make sure that Player moves with its player Camera
            float yawCamera = mainCamera.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), turnSpeed * Time.fixedDeltaTime);
        }
    }
}
