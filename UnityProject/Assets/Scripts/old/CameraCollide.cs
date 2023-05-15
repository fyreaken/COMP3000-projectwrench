using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollide : MonoBehaviour
{
    //public Camera playerCamera;

    public Transform cameraTransform;
    public float cameraDistance;

    //[SerializeField] private float cameraXOffset = 0.3656f;
    //[SerializeField] private float cameraYOffset = 1.54f;
    //[SerializeField] private float cameraZOffset = -2.31f;

    /*private void OnTriggerEnter(Collider collider){
        //Debug.Log("Trigger");
        if (collider.gameObject.name == "Plane")
        {
            playerCamera.transform.position += playerCamera.transform.forward * 0.1f;
            //Debug.Log(playerCamera.transform.position);
        }
    }*/

    void Update(){
        Ray ray = new Ray(cameraTransform.position, -cameraTransform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, cameraDistance)){
            cameraTransform.localPosition = Vector3.back * hit.distance + new Vector3(0,1.54f,-2.31f);
        }
        else {
            cameraTransform.localPosition = Vector3.back * cameraDistance + new Vector3(0,1.54f,-2.31f);
        }
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
    }
}
