using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE: This class is being used also in AnimationStateController class
public class CharacterClass : MonoBehaviour
{
    //Variables:
    //----------------------------------------------------------------------------------------------------------------------------
    [SerializeField] float moveSpeed;
    private float originalMoveSpeed;
    [SerializeField] float twoKeysPressed; //Adjust the speed of the character when two keys are pressed at the same time

    private CharacterController ch;
    [SerializeField] private float smoothTime = 0.05f; //Used to make a more smooth rotation when moving into different directions
    private float currentVelocity;

    public Vector3 direction = new Vector3(0, 0, 0); //Movement direction
    //----------------------------------------------------------------------------------------------------------------------------

    void Start()
    {
        //Initialize variables
        originalMoveSpeed = moveSpeed; // Store the original moveSpeed
        ch = GetComponent<CharacterController>();
    }

    void Update()
    {
        MoveCharacter();


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
}
