using UnityEngine;

public class CameraController : MonoBehaviour
{
    //This script is a current work in progress project where the player can use both keyboard and mouse commands to move the camera around a game space.
    //I use this camera for a tower defense game where the player can smoothly scroll down to zoom in and pitch the camera up simultaneously
    //the player can also yaw the camera (going to also pivot the camera around a center point in the future)
    // but if the player presses 'z' they can currently go back to the default position

    public float panSpeed = 30f;
    private float yawSpeed = 3f;
    public float panBoarderThickness = 10f;

    private bool doMovement = true;

    public float scrollSpeed = 5f;

    // camera translation limits variables
    private float cameraMinY = 10f;
    private float cameraMaxY = 80f;
    private float cameraMinX = 1.6f; // this is the center of the far left tile
    private float cameraMaxX = 76.6f; //this is the center of the far right tile
    private float cameraMinZ = -16.5f; // this is the center of bottom row
    private float cameraMaxZ = 63.5f; // I just hard picked a value here because of perspective camera angle (can be refined)

    //camera angular variables
    public float cameraPitch = 67f;
    public float cameraYaw = 0f;
    private float cameraMinPitch = 30f;
    private float cameraMaxPitch = 90f;
    private float pitchSensitivity = 1800f;






    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            doMovement = !doMovement; // flips the movement between true and false if escape is pressed

        if (!doMovement) // this will skip all of the camera panning if the movement is false
            return;

        //as long as W is held or you have mouse at top of the screen, move forward
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBoarderThickness) 
        {
            //objectScriptIsAttachedTo.Translate(3DCoordinates.forward * distanceItNeedsToMove * normailizedFrameRate, inRelationToTheWorld); 
            //so it moves North in relation to the world
            transform.Translate(Vector3.forward * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("s") || Input.mousePosition.y <= 0 + panBoarderThickness) //hold s to move back
        {
            transform.Translate(Vector3.back * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("a") || Input.mousePosition.x <= 0 + panBoarderThickness) // hold 'a' to move left
        {
            transform.Translate(Vector3.left * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBoarderThickness) // hold 'd' to move right
        {
            transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
        }

        if (Input.GetKey("q") ) // hold 'q' to yaw left
        {
            //transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
            cameraYaw += yawSpeed ;
        }

        if (Input.GetKey("e")) // hold 'e' to yaw right
        {
            //transform.Translate(Vector3.right * panSpeed * Time.deltaTime, Space.World);
            cameraYaw -= yawSpeed;
        }

        

        float scroll = Input.GetAxis("Mouse ScrollWheel"); //this retrives the rotation of the scroll wheel axis
        Debug.Log(Input.mousePosition.x);


        //Restricting and translating the camera values
        Vector3 pos = transform.position; //store camera position
        pos.y -= scroll * 1000 * scrollSpeed * Time.deltaTime; //compute the new location of camera
        pos.y = Mathf.Clamp(pos.y, cameraMinY, cameraMaxY); //This will restrict the vertical range of the camera
        pos.x = Mathf.Clamp(pos.x, cameraMinX, cameraMaxX); //restricts horizontal range
        pos.z = Mathf.Clamp(pos.z, cameraMinZ, cameraMaxZ); //restricts vertical range
                                                            //Note: the horizontal range doesn't fully line up with edge of tiles because the camera is at a perspective angle

        if (Input.GetKey("z")) // hold 'z' to reset camera
        {
            pos.x = 36.5f;
            pos.y = 78.1f;
            pos.z = -14.1f;

            cameraPitch = 67f;
            cameraYaw = 0f;
        }

        //This is will adjust the camera angles
        cameraPitch -= scroll * pitchSensitivity * scrollSpeed * Time.fixedDeltaTime; //used fixed delta time because it will move down at a more consistant pace than just Time.deltaTime

        cameraPitch = Mathf.Clamp(cameraPitch, cameraMinPitch, cameraMaxPitch);

        transform.position = pos; // commit the relocation of the camera 
        transform.eulerAngles = new Vector3(cameraPitch, cameraYaw, 0.0f); // commit the angular change of the camera
    }


}
