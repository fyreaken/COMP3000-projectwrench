using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class FPSCamera : MonoBehaviour
{
    [SerializeField] public float xlookSpeed = 2.0f;
    [SerializeField] public float ylookSpeed = 2.0f;
    [SerializeField] public float lookXLimit = 45.0f;
 
    float rotationX = 0;
 
    [HideInInspector]
    public bool canMove = true;
 
    [SerializeField] private float cameraXOffset = 0.4f;
    [SerializeField] private float cameraYOffset = 0.4f;
    [SerializeField] private float cameraZOffset = 0.4f;
    private Camera playerCamera;

    //public Transform cameraTransform;
    //public float cameraDistance;
 
    private Alteruna.Avatar _avatar;
 
    void Start()
    {
        _avatar = GetComponent<Alteruna.Avatar>();
 
        if (!_avatar.IsMe)
            return;

        playerCamera = Camera.main;
        playerCamera.transform.position = new Vector3(transform.position.x + cameraXOffset, transform.position.y + cameraYOffset, transform.position.z + cameraZOffset);
        playerCamera.transform.SetParent(transform);
        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
 
    void Update()
    {
        if (!_avatar.IsMe)
            return;
 
        // Player and Camera rotation
        if (canMove && playerCamera != null)
        {
            rotationX += -Input.GetAxis("Mouse Y") * ylookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * xlookSpeed, 0);

            /*RaycastHit hit;
            if (Physics.Raycast(Vector3 origin, Vector3 direction, float maxDistance = Mathf.Infinity){
            get hit distance and adjust camera distance to hit distance
                playerCamera.transform.position = playerCamera.transform.forward * (cameraZOffset - hit.distance);
            }*/

            //origin = transform.position //player position
            //direction = (transform.position- playerCamera.transform.position).normalized // direction from player to camera
            //maxDistance = camera distance here //max distance to camera from player
        }

        /*Ray ray = new Ray(cameraTransform.position, -cameraTransform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, cameraDistance)){
            cameraTransform.localPosition = Vector3.back * hit.distance + new Vector3(0,1.54f,0);
        }
        else {
            cameraTransform.localPosition = Vector3.back * cameraDistance + new Vector3(0,1.54f,0);
        } */
    }
}