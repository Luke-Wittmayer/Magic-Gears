using System.Collections;
using System.Collections.Generic;
using static storeLevel;
using UnityEngine;

//NOTE: This class is being used also in AnimationStateController class
public class CharacterClass : MonoBehaviour
{
    //Variables:
    //----------------------------------------------------------------------------------------------------------------------------
    public bool canMove; // Used to stop player from moving when talking to an NPC. Set to public so that the NPC class can set it to false and true later

    [SerializeField] float moveSpeed;
    private float originalMoveSpeed;
    [SerializeField] float twoKeysPressed; //Adjust the speed of the character when two keys are pressed at the same time

    private CharacterController ch;
    [SerializeField] private float smoothTime = 0.05f; //Used to make a more smooth rotation when moving into different directions
    private float currentVelocity;

    [SerializeField] private Transform cameraTransform;

    public Vector3 direction = new Vector3(0, 0, 0); //Movement direction. Made public for the conversations and animation classes

    [SerializeField] public int levelCompleted = 0;


    //----------------------------------------------------------------------------------------------------------------------------

    void Start()
    {
        //Initialize variables
        Cursor.lockState = CursorLockMode.Locked;
        originalMoveSpeed = moveSpeed; // Store the original moveSpeed
        canMove = true;
        ch = GetComponent<CharacterController>();
        if (levelCompleted <= storeLevel.level){
            levelCompleted = storeLevel.level;
        }
    }

    void Update()
    {
        if (canMove)
        {
            MoveCharacter();
        }


    }

    void MoveCharacter()
    {
        direction.x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        direction.z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        // Check for simultaneous key presses and adjust moveSpeed
        bool isMultiDirectionKeyPressed = (Input.GetKey("w") && (Input.GetKey("a") || Input.GetKey("d"))) ||
                                           (Input.GetKey("s") && (Input.GetKey("a") || Input.GetKey("d")));

        if (isMultiDirectionKeyPressed)
        {
            moveSpeed = originalMoveSpeed * twoKeysPressed; // Reduce moveSpeed when multiple keys are pressed
        }
        else
        {
            moveSpeed = originalMoveSpeed;
        }

        if (ch != null)
        {
            direction = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * direction;
        }
        else
        {
            ch = GetComponent<CharacterController>();
        }

        ch.Move(direction);

        RotateInMovement();
    }

    //If character moves to the left or right, rotate the character to that rotation
    void RotateInMovement()
    {
        if (direction.x == 0 && direction.z == 0) //No input, no need to turn
        {
            return;
        }
        else
        {
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentVelocity, smoothTime);
            transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
        }
    }

    private void OnApplicationFocus(bool focus)
    {
        if (focus)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
