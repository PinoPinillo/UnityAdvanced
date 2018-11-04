using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    // Ball base speed
    public float speed;
    // Ball acceleration factor
    public float acceleration;
    // Ball maximum speed
    public float maxSpeed;
    // The speed factor that depends on the acceleration
    private float speedFactor;
    // Z component of the direction
    private int zDirection;
    // X component of the direction
    private int xDirection;
    // Rigidbody attached to the ball
    private Rigidbody rb;
    // Has the ball collided with something?
    // We will use this to ensure that the ball does not collide with 
    // the same wall or player more than once at the same time
    private bool hasCollided;

    // Use this for initialization
    void Start()
    {
        // Sets hasCollided to false so the ball can collide with a wall or player
        hasCollided = false;
        // Gets the Rigidbody component attached
        rb = GetComponent<Rigidbody>();

        // Sets the speedFactor to 1, so the initial speed is the public float speed value
        speedFactor = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        // Calculates the ball speed
        float actualSpeed = speed * speedFactor;
        // Sets the new speed
        rb.velocity = transform.forward * actualSpeed;

        // If ball speed is less than max speed, increases the speed factor
        if (actualSpeed < maxSpeed)
            speedFactor += Time.deltaTime * acceleration;
    }

    void OnTriggerEnter(Collider other)
    {
        // Gets the goal component attached to the other object
        Goal goal = other.GetComponent<Goal>();

        // If the other object is a goal, scores and destroys the ball.
        // Otherwise, checks if the ball can collide with another object and 
        // the other object is a wall or a player
        if (goal != null)
        {
            goal.Score();
            Destroy(this.gameObject);
        }
        else if (!hasCollided)
        {
            // Sets hasCollided to true, so the ball can not collide with another wall or player
            hasCollided = true;
            // Plays the ball sound
            GetComponent<AudioSource>().Play();

            // If the other object is a wall, reflects the ball and returns
            if (other.CompareTag("Wall"))
            {
                CheckNewDirection();
                this.transform.forward = new Vector3(xDirection, 0f, -zDirection);
                return;
            }

            // Gets the PlayerMovement component attached to the other object
            PlayerMovement playerMov = other.gameObject.GetComponent<PlayerMovement>();
            // If the other object has a PlayerMovement component attached,
            // changes ball direction depending on the player movement
            if (playerMov != null)
            {
                CheckNewDirection();

                if (this.transform.forward.z <= 0f)
                {
                    if (playerMov.IsMovingUp)
                        this.transform.forward *= -1f;
                    else
                        this.transform.forward = new Vector3(-xDirection, 0f, zDirection);
                }
                else
                {
                    if (!playerMov.IsMovingUp)
                        this.transform.forward *= -1f;
                    else
                        this.transform.forward = new Vector3(-xDirection, 0f, zDirection);
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Sets hasCollided to false so the ball can collide with a wall or the other player
        hasCollided = false;
    }

    /// <summary>
    /// This method we ensure that the ball always will have its X and Z components to -1 or 1.
    /// </summary>
    void CheckNewDirection()
    {
        if (this.transform.forward.x >= 0f)
            xDirection = 1;
        else
            xDirection = -1;

        if (this.transform.forward.z >= 0f)
            zDirection = 1;
        else
            zDirection = -1;
    }

    public void SetStartDirection(int xDirection, int zDirection)
    {
        this.xDirection = xDirection;
        this.zDirection = zDirection;

        // Sets the initial direction vector
        Vector3 initialDirection = new Vector3(xDirection, 0f, zDirection);
        // Sets the forward vector of the ball as the initial direction
        this.transform.forward = initialDirection;
    }
}
