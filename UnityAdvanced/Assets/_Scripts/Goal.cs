using UnityEngine;

public class Goal : MonoBehaviour
{
    // Player who owns this goal
    public Players player;
    // The GameController
    private GameController gameController;

    // Use this for initialization
    void Start()
    {
        // Finds the GameController in the scene
        gameController = FindObjectOfType<GameController>();
    }

    /// <summary>
    /// This is called when one player scores. 
    /// </summary>
    public void Score()
    {
        // Increases the score depending on the player who owns this goal
        if (player == Players.Player1)
            GameController.p2Score++;
        else
            GameController.p1Score++;

        // Spawns a new ball in 2 seconds
        gameController.Invoke("NewBall", 2f);
    }
}
