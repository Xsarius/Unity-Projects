/*
Script source: https://www.youtube.com/watch?v=rnqF6S7PfFA
Author: Game Dev Guide 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    ///////////////
    // Variables //
    ///////////////
    //
    // Summary:
    //     
    //
    // 
    //
    public Transform cameraTransform;
    //
    // Summary:
    //     
    //
    // 
    //
    private float lowerZoomLimit = 80f;
    //
    // Summary:
    //     
    //
    // 
    //
    private float upperZoomLimit = 500f;
    //
    // Summary:
    //     
    //
    // 
    //
    private float movementSpeed;
    //
    // Summary:
    //     
    //
    // 
    //
    public float movementTime;
    //
    // Summary:
    //     
    //
    // 
    //
    public float normalSpeed;
    //
    // Summary:
    //     
    //
    // 
    //
    public float fastSpeed;
    //
    // Summary:
    //     
    //
    // 
    //
    public float rotationAmmount;
    //
    // Summary:
    //     
    //
    // 
    //
    public Vector3 zoomAmmount;
    //
    // Summary:
    //     
    //
    // 
    //
    public Vector3 newZoom;
    //
    // Summary:
    //     
    //
    // 
    //
    Vector3 newPosition;
    //
    // Summary:
    //     
    //
    // 
    //
    Vector3 dragStartPosition;
    //
    // Summary:
    //     
    //
    // 
    //
    Vector3 dragCurrentPosition;
    //
    // Summary:
    //     
    //
    // 
    //
    Vector3 rotateStartPosition;
    //
    // Summary:
    //     
    //
    // 
    //
    Vector3 rotateCurrentPosition;
    //
    // Summary:
    //     
    //
    // 
    //
    Quaternion newRotation;

    /////////////
    // Methods //
    /////////////
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    void Start()
    {
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    void Update()
    {
        HandleMouseInput();
        HandleMovementInput();
    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastSpeed;
        }
        else
        {
            movementSpeed = normalSpeed;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationAmmount);
        }
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationAmmount);
        }

        if (Input.GetKey(KeyCode.R))
        {

            newZoom += zoomAmmount;
            if (newZoom.y < lowerZoomLimit && newZoom.z > -lowerZoomLimit)
            {
                newZoom.y = lowerZoomLimit;
                newZoom.z = -lowerZoomLimit;
            }
        }
        if (Input.GetKey(KeyCode.F))
        {
            newZoom -= zoomAmmount;
            if (newZoom.y > upperZoomLimit && newZoom.z < -upperZoomLimit)
            {
                newZoom.y = upperZoomLimit;
                newZoom.z = -upperZoomLimit;
            }
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * movementTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * movementTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * movementTime);

    }
    //
    // Summary:
    //     
    //
    // Parameters:
    //
    void HandleMouseInput()
    {
        if (Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmmount;
            if (newZoom.y > upperZoomLimit && newZoom.z < -upperZoomLimit)
            {
                newZoom.y = upperZoomLimit;
                newZoom.z = -upperZoomLimit;
            }
            else if (newZoom.y < lowerZoomLimit && newZoom.z > -lowerZoomLimit)
            {
                newZoom.y = lowerZoomLimit;
                newZoom.z = -lowerZoomLimit;
            }
        }
        ////////////////////////////////////////////////////
        // Moving the camera by "catch & drag" the screen //
        ////////////////////////////////////////////////////
        /*
                if (Input.GetMouseButtonDown(0))
                {
                    Plane plane = new Plane(Vector3.up, Vector3.zero);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    float entry;

                    if (plane.Raycast(ray, out entry))
                    {
                        dragStartPosition = ray.GetPoint(entry);
                    }
                }
                if (Input.GetMouseButton(0))
                {
                    Plane plane = new Plane(Vector3.up, Vector3.zero);

                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    float entry;

                    if (plane.Raycast(ray, out entry))
                    {
                        dragCurrentPosition = ray.GetPoint(entry);

                        newPosition = transform.position + dragStartPosition - dragCurrentPosition;
                    }
                }
        */

        if (Input.GetMouseButtonDown(1))
        {
            rotateStartPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1))
        {
            rotateCurrentPosition = Input.mousePosition;

            Vector3 difference = rotateStartPosition - rotateCurrentPosition;

            rotateStartPosition = rotateCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));

        }
    }

}


