using System;
using UnityEngine;
using UnityEngine.Networking;

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

public class PlayerMovement : NetworkBehaviour
{
    // Player movement speed
    public float speed;
    // Is the player moving up or down?
    public bool IsMovingUp { get; private set; }
    // The player Rigidbody component
    private Rigidbody rb;
    // Movement bounds
    private Boundary boundary;
    // Bounds for each player
    public Boundary p1Boundary;
    public Boundary p2Boundary;

    // Use this for initialization
    void Start()
    {
        // Gets the Rigidbody attached to this game object
        rb = GetComponent<Rigidbody>();

        // Gets the player data depending on playersCount
        if (GameController.playersCount < 1)
        {
            boundary = p1Boundary;
        }
        else
        {
            boundary = p2Boundary;
            // Spawns the ball when both players are connected
            FindObjectOfType<GameController>().Invoke("CmdNewBall", 2f);
        }
        // Increases the players count
        GameController.playersCount++;
    }

    void FixedUpdate()
    {
        // If this is not the local player, we can not move it
        if (!isLocalPlayer)
            return;

        // Gets the vertical axis input
        float moveVertical = Input.GetAxis("Vertical");

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
