using System;
using UnityEngine;

// Bounds of player movement
[Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax, zMin, zMax;
}

// Enum with the list of players that can play one game
public enum Players
{
    Player1,
    Player2
}

public class PlayerMovement : MonoBehaviour
{
    // Player assigned
    public Players player;
    // Player movement speed
    public float speed;
    // Is the player moving up or down?
    public bool IsMovingUp { get; private set; }
    // The player Rigidbody component
    private Rigidbody rb;
    // Movement bounds
    public Boundary boundary;

    // Use this for initialization
    void Start()
    {
        // Gets the Rigidbody attached to this game object
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // We will use this number to move the player up or down
        float moveVertical;

        // Gets the vertical axis input depending on the player
        if (player == Players.Player1)
            moveVertical = Input.GetAxis("Vertical");
        else
            moveVertical = Input.GetAxis("Vertical2");

        // If moveVertical is greater than 0, the player is moving up.
        // Otherwise, the player will be moving down
        if (moveVertical > 0f)
            IsMovingUp = true;
        else
            IsMovingUp = false;

        // Creates the movement vector and sets the z value to moveVertical
        Vector3 movement = new Vector3(0f, 0f, moveVertical);
        // Calculates the movement vector depending on speed factor
        movement = this.transform.position + (movement * Time.deltaTime * speed);
        // Clamps the vector, so the player can not go outside of the screen
        movement = new Vector3(Mathf.Clamp(movement.x, boundary.xMin, boundary.xMax),
            Mathf.Clamp(movement.y, boundary.yMin, boundary.yMax),
            Mathf.Clamp(movement.z, boundary.zMin, boundary.zMax)
            );

        // Moves the player
        rb.MovePosition(movement);
    }
}
