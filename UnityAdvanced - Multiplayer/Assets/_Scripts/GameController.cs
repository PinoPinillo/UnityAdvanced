using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class GameController : NetworkBehaviour
{
    // Player 1 score
    [SyncVar(hook = "OnChangeP1Score")]
    public int p1Score;
    // Player 2 score
    [SyncVar(hook = "OnChangeP2Score")]
    public int p2Score;

    public static int playersCount;

    // Text that shows the player 1 score
    public Text p1ScoreText;
    // Text that shows the player 2 score
    public Text p2ScoreText;
    // Text that shows the player who won
    public Text gameOverText;
    // Score needed to win the game
    public int scoreToWin;

    // The ball game object
    public GameObject ball;

    // Use this for initialization
    void Start()
    {
        // Resets the scores
        p1Score = 0;
        p2Score = 0;

        playersCount = 0;

        // Hides the gameOver text
        gameOverText.text = "";
        gameOverText.enabled = false;
    }

    public void Score(int player)
    {
        if (!isServer)
            return;

        // Increases the score
        if (player == 1)
            p1Score++;
        else if (player == 2)
            p2Score++;
    }

    void OnChangeP1Score(int score)
    {
        // Updates the score texts
        p1ScoreText.text = string.Format("{0:00}", score);
        // Calls the GameOver method when someone reaches the score to win
        if (score >= scoreToWin)
            GameOver(1);
    }

    void OnChangeP2Score(int score)
    {
        // Updates the score texts
        p2ScoreText.text = string.Format("{0:00}", score);
        // Calls the GameOver method when someone reaches the score to win
        if (score >= scoreToWin)
            GameOver(2);
    }

    /// <summary>
    /// Spawns a new ball if the game is not over.
    /// </summary>
    [Command]
    public void CmdNewBall()
    {
        if (!isServer)
            return;

        // If someone has reached the score to win, returns
        if (p1Score >= scoreToWin || p2Score >= scoreToWin)
            return;

        // Gets a random start direction for the ball on the plane XZ
        int zDirection = Random.Range(-1, 2);
        if (zDirection == 0)
        {
            zDirection = 1;
        }

        int xDirection = Random.Range(-1, 2);
        if (xDirection == 0)
        {
            xDirection = 1;
        }

        // Spawns a new ball and sets its starting direction
        GameObject ballInstantiated = Instantiate(ball, this.transform.position, Quaternion.identity);
        ballInstantiated.GetComponent<BallBehaviour>().SetStartDirection(xDirection, zDirection);
        NetworkServer.Spawn(ballInstantiated);
    }

    /// <summary>
    /// This is called when one player reaches the score to win.
    /// </summary>
    private void GameOver(int winner)
    {
        // Change the game over text depending on the player who won
        if (winner == 1)
            gameOverText.text = "Player 1 win";
        else
            gameOverText.text = "Player 2 win";

        gameOverText.enabled = true;

        // Quits the game in 2 seconds 
        Invoke("Exit", 2f);
    }

    /// <summary>
    /// Closes the game.
    /// </summary>
    void Exit()
    {
        Application.Quit();
    }
}
