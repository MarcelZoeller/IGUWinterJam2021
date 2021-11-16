using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    
    [SerializeField] Rigidbody rb;
    [SerializeField] Animator animator;

    [Header("Settings")]
    [SerializeField] float movementSpeed = 6;
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] float fallSpeed = 2f;

    Vector3 moveDirection;
    Vector3 gravity;

    void Update()
    {
        Move((Vector3)GameManager.InputManager.movementInput);
    }

    private void FixedUpdate()
    {
        //gravity += Vector3.up * Physics.gravity.y * fallSpeed * Time.fixedDeltaTime;
        rb.velocity = (moveDirection * movementSpeed) + gravity;

        //if (senses.IsGrounded() && rb.velocity.y <= 0)
        //{
        //    gravity = Vector3.zero;
        //    //TODO Set to floor
        //}
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
