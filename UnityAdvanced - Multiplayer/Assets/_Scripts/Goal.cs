using UnityEngine;

public class Goal : MonoBehaviour
{
    // Player who owns this goal
    public Players player;
    // The GameController
    public GameController gameController;

    /// <summary>
    /// This is called when one player scores. 
    /// </summary>
    public void Score()
    {
        // Increases the score depending on the player who owns this goal
        if (player == Players.Player1)
            gameController.Score(2);
        else
            gameController.Score(1);

        // Spawns a new ball in 2 seconds
        gameController.Invoke("CmdNewBall", 2f);
    }
}
