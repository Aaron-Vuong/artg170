using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation = null;

    float xRotation;
    float yRotation;
    
    public GameMenu gameMenu;
    private bool canUpdate = true;

    private void Update()
    {
        if (!canUpdate) {return;}
        // If we don't have a child camera, yoink one.
        if (transform.parent.gameObject.name != "---- Player ----")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (player != null) { 
                transform.SetParent(player.transform);
                transform.localPosition = new Vector3(0, 2, 0);
                orientation = player.transform;
            }
            else {
                // Don't move the camera if we aren't on a Player.
                return;
            }
        }
        //Debug.Log($"ORIENTATION {orientation}");

        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        

    }

    public void lockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void unlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    
}
