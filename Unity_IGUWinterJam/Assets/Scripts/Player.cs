using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] LayerMask groundLayer;
    
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator animator;

    [Header("Settings")]
    [SerializeField] float movementSpeed = 6;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] float fallSpeed = 2f;

    Vector3 moveDirection;
    Vector3 gravity;

    public bool isGrounded;
    float slopeAmount;

    Vector3 CombinedRaycast;
    Vector3 raycastFloorPos;
    Vector3 floorNormal;
    public float slopeLimit = 45f;
    private Vector3 floorMovement;
    private float floorOffsetY;

    void Update()
    {
        Move((Vector3)GameManager.InputManager.movementInput);
        isGrounded = IsGrounded();


        CheckForInteraction();

    }

    private void FixedUpdate()
    {
        gravity += Vector3.up * Physics.gravity.y * fallSpeed * Time.fixedDeltaTime;
        rb.velocity = (moveDirection * movementSpeed) + gravity;

        if (IsGrounded() && rb.velocity.y <= 0)
        {
            gravity = Vector3.zero;
            //TODO Set to floor

            rb.MovePosition(GetGrounded());
        }

        // find the Y position via raycasts
        floorMovement = new Vector3(rb.position.x, FindFloor().y + floorOffsetY, rb.position.z);

        // only stick to floor when grounded
        if (floorMovement != rb.position && IsGrounded() && rb.velocity.y <= 0)
        {
            // move the rigidbody to the floor
            rb.MovePosition(floorMovement);
            gravity.y = 0;
        }


    }



    void CheckForInteraction()
    {
        if (GameManager.InputManager.interact)
        {
            GameManager.InputManager.interact = false;
            Debug.Log("interact");
        }
    }



    Vector3 FindFloor()
    {
        // width of raycasts around the centre of your character
        float raycastWidth = 0.25f;
        // check floor on 5 raycasts   , get the average when not Vector3.zero  
        int floorAverage = 1;

        CombinedRaycast = FloorRaycasts(0, 0, 1.6f);
        floorAverage += (getFloorAverage(raycastWidth, 0) + getFloorAverage(-raycastWidth, 0) + getFloorAverage(0, raycastWidth) + getFloorAverage(0, -raycastWidth));

        return CombinedRaycast / floorAverage;
    }

    int getFloorAverage(float offsetx, float offsetz)
    {

        if (FloorRaycasts(offsetx, offsetz, 1.6f) != Vector3.zero)
        {
            CombinedRaycast += FloorRaycasts(offsetx, offsetz, 1.6f);
            return 1;
        }
        else { return 0; }
    }

    Vector3 FloorRaycasts(float offsetx, float offsetz, float raycastLength)
    {
        RaycastHit hit;
        // move raycast
        raycastFloorPos = transform.TransformPoint(0 + offsetx, 0 + 0.5f, 0 + offsetz);

        Debug.DrawRay(raycastFloorPos, Vector3.down, Color.magenta);
        if (Physics.Raycast(raycastFloorPos, -Vector3.up, out hit, raycastLength, groundLayer))
        {
            floorNormal = hit.normal;

            if (Vector3.Angle(floorNormal, Vector3.up) < slopeLimit)
            {
                return hit.point;
            }
            else return Vector3.zero;
        }
        else return Vector3.zero;
    }

    bool IsGrounded()
    {
        if (FloorRaycasts(0, 0, 0.6f) != Vector3.zero)
        {
            slopeAmount = Vector3.Dot(transform.forward, floorNormal);
            return true;
        }
        else
        {
            return false;
        }
    }

    //bool IsGrounded() 
    //{
    //    return Physics.Raycast(transform.position + Vector3.up*0.5f, Vector3.down, 1.55f);
    //}

    Vector3 GetGrounded()
    {
        Physics.Raycast(transform.position + Vector3.up * 0.5f, Vector3.down, out var hit, 1.55f);
        return hit.point;
    }

    public void Move(Vector3 inputDirection)
    {
        // base movement on camera
        Vector3 correctedVertical = inputDirection.x * Camera.main.transform.right;
        Vector3 correctedHorizontal = inputDirection.y * Camera.main.transform.forward;

        Vector3 combinedInput = correctedHorizontal + correctedVertical;
        // normalize so diagonal movement isnt twice as fast, clear the Y so your character doesnt try to
        // walk into the floor/ sky when your camera isn't level
        moveDirection = new Vector3((combinedInput).normalized.x, 0, (combinedInput).normalized.z);

        // make sure the input doesnt go negative or above 1;
        float inputMagnitude = Mathf.Abs(inputDirection.x) + Mathf.Abs(inputDirection.y);
        var inputAmount = Mathf.Clamp01(inputMagnitude);


            // rotate player to movement direction
            if (moveDirection != Vector3.zero)
            {
                Quaternion rot = Quaternion.LookRotation(moveDirection);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, rot, Time.fixedDeltaTime * inputAmount * rotateSpeed);
                transform.rotation = targetRotation;
            }
        
        // handle animation blendtree for walking
        //anim.SetFloat("moveSpeed", inputAmount, 0.2f, Time.deltaTime);
    }
}
