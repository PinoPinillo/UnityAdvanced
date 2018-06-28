using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    // Player 1 score
    public static int p1Score;
    // Player 2 score
    public static int p2Score;

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

        // Hides the gameOver text
        gameOverText.text = "";
        gameOverText.enabled = false;

        // Spawns a new ball
        NewBall();
    }

    // Update is called once per frame
    void Update()
    {
        // Updates the score texts
        p1ScoreText.text = p1Score.ToString();
        p2ScoreText.text = p2Score.ToString();

        // Calls the GameOver method when someone reaches the score to win
        if (p1Score >= scoreToWin || p2Score >= scoreToWin)
            GameOver();
    }

    /// <summary>
    /// Spawns a new ball if the game is not over.
    /// </summary>
    public void NewBall()
    {
        // If someone has reached the score to win, returns
        if (p1Score >= scoreToWin || p2Score >= scoreToWin)
            return;

        // Spawns a new ball
        Instantiate(ball, this.transform.position, Quaternion.identity);
    }

    /// <summary>
    /// This is called when one player reaches the score to win.
    /// </summary>
    private void GameOver()
    {
        // Change the game over text depending on the player who won
        if (p1Score > p2Score)
            gameOverText.text = "Player 1 win";
        else
            gameOverText.text = "Player 2 win";

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
