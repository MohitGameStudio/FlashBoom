using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // Speed of movement
    [SerializeField] private float gridSize = 1f;  // Size of each grid unit for tracking

    private Vector3 moveDirection = Vector3.zero; // Current movement direction
    private Vector3 currentGridCell; // The grid cell the player is currently in
    private Rigidbody rb; 
    private Animator animator;
    //public Vector2Int GridSize { get { return new Vector2Int(gridWidth, gridHeight); } }
    //public float CellSize { get { return gridSize; } }
    void Start()
    {
        // Get the Rigidbody component for physics calculations
        rb = GetComponent<Rigidbody>();

        // Ensure the Rigidbody is kinematic to prevent unwanted forces
        rb.isKinematic = false; // Set to true if you're not using physics forces

        // Get the Animator component
        animator = GetComponent<Animator>();

    }

    void FixedUpdate()
    {
        // Handle input and update movement direction
        HandleInput();

        // Apply movement using Rigidbody for physics-based movement
        if (moveDirection != Vector3.zero)
        {
            MovePlayer();
        }

        // Track and update the current grid cell in each FixedUpdate
        TrackGridCell();

        // Update the Animator with the player's speed (movement)
        UpdateAnimation();
    }

    void HandleInput()
    {
        // Handle player movement input
        if (Input.GetKey(KeyCode.W)) moveDirection = Vector3.forward;
        else if (Input.GetKey(KeyCode.S)) moveDirection = Vector3.back;
        else if (Input.GetKey(KeyCode.A)) moveDirection = Vector3.left;
        else if (Input.GetKey(KeyCode.D)) moveDirection = Vector3.right;
        else moveDirection = Vector3.zero;
    }

    void MovePlayer()
    {
        Vector3 targetPosition = transform.position + moveDirection * moveSpeed * Time.deltaTime;
        rb.MovePosition(targetPosition);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);  // Adjust rotation speed
        }
    }


    void TrackGridCell()
    {
        // Calculate the current grid cell the player is in
        float gridX = Mathf.Floor(transform.position.x / gridSize);
        float gridZ = Mathf.Floor(transform.position.z / gridSize);

        currentGridCell = new Vector3(gridX * gridSize, 10, gridZ * gridSize);

        // Debugging: Display the current grid cell
        Debug.Log("Current Grid Cell: " + currentGridCell);
    }

    void UpdateAnimation()
    {
        // Set the Speed parameter in Animator based on movement
        float speed = moveDirection.magnitude; // This will give us the magnitude of the moveDirection vector (0 if not moving)
        animator.SetFloat("Speed", speed); // Update the Animator's Speed parameter
    }
    //void OnDrawGizmos()
    //{
    //    // Draw the grid for visualization (in Scene View)
    //    Gizmos.color = Color.green;

    //    for (int x = -10; x <= 10; x++)
    //    {
    //        for (int z = -10; z <= 10; z++)
    //        {
    //            Vector3 gridPoint = new Vector3(x * gridSize, 10, z * gridSize);
    //            Gizmos.DrawWireCube(gridPoint, new Vector3(gridSize, 0.1f, gridSize));
    //        }
    //    }

    //    //// Highlight the player's current grid cell in the Scene View
    //    //Gizmos.color = Color.red;
    //    //Gizmos.DrawWireCube(currentGridCell, new Vector3(gridSize, 0.1f, gridSize));
    //}
    //void OnDrawGizmosSelected()
    //{
    //    // Highlight the player's current grid cell in the Scene View
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireCube(currentGridCell + Vector3.up * 0.5f, new Vector3(gridSize, 0.1f, gridSize));  // Adjust the height slightly
    //}

}
