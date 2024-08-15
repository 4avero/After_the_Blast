using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public float followSpeed = 2f; // Speed at which the player follows the mouse (lower value = slower)
    private bool isFollowing = false; // Indicates if the player is currently following the mouse
    private PlayC playerControllerScript;
    public bool Jactive = false;
    public bool pressed = false;
    private void Start()
    {
        playerControllerScript = GameObject.FindWithTag("Player").GetComponent<PlayC>();
    }   
    public void OnPlayerReleased()
    {
        
        if (playerControllerScript.isOnGround && playerControllerScript.GameOver == false)
        {
            playerControllerScript.JumpForce();
            Jactive = true;
            
        }
    }
    public void Press()
    {
        pressed = false;
        //Debug.Log(pressed);
    }
    void Update()
    {
        Jactive = false;
        // Check if the left mouse button is being pressed
        if (Input.GetMouseButtonDown(0))
        {
            pressed = true;
            //Debug.Log(pressed);
            Invoke("Press", 1);
            // Cast a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
       
            if (Physics.Raycast(ray, out hit))
            {
                // Check if the raycast hit the player
                isFollowing = true;

            }
        }

        // Check if the left mouse button is being held down
        if (Input.GetMouseButton(0) && isFollowing)
        {
            // Cast a ray from the camera to the mouse position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, float.MaxValue, 1 << LayerMask.NameToLayer("Ground")) && playerControllerScript.GameOver == false)
            {
                // Move the player towards the point where the raycast hits, but with controlled speed
                Vector3 targetPosition = hit.point;

                // Block vertical movement by setting the y position to the current y position
                targetPosition.x = transform.position.x;

                // Interpolate the player's position towards the target position
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

                // Update the player's position
                transform.position = new Vector3(smoothedPosition.x, transform.position.y, smoothedPosition.z);
            }
        }

        // Check if the left mouse button is released
        if (Input.GetMouseButtonUp(0))
        {
            if (isFollowing)
            {
                pressed = false;
                Debug.Log("relesed");
                OnPlayerReleased(); // Call the method when the mouse button is released
                isFollowing = false;
            }
        }
    }
}