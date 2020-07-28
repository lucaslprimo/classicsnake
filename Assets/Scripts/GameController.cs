using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Text scoreText;
    private int score = 0;

    public GameObject food;

    private Vector2[] boardPositions = new Vector2[255];

    private const int X_POS_OFFSET = -16;
    private const int Y_POS_OFFSET = -14;

    public SnakeController snake;

    public GameObject TutorialMenu;
    public GameObject UICanvas;

    private static bool firstPlay = true;
    public AudioSource soundTrack;

    private void Awake()
    {
        soundTrack = GetComponent<AudioSource>();
    }

    void Start()
    { 
        int c = 0;
        for(int i = 0; i <= 32; i+=2)
        {
            for(int j=0; j <= 28; j+=2)
            {
                boardPositions[c].x = i;
                boardPositions[c].y = j;

                c++;
            }
        }

        if (!firstPlay)
        {
            UICanvas.SetActive(true);
            TutorialMenu.SetActive(false);
            snake.enabled = true;
        }
        else
        {
            soundTrack.Play();
        }
    }

    void Update()
    {
        if (firstPlay)
        {
            if (Input.GetKey(KeyCode.Mouse0) 
                || Input.GetKeyDown(KeyCode.DownArrow)
                || Input.GetKeyDown(KeyCode.LeftArrow)
                || Input.GetKeyDown(KeyCode.RightArrow)
                || Input.GetKeyDown(KeyCode.UpArrow)
                || Input.GetKeyDown(KeyCode.Space)
                || Input.GetKeyDown(KeyCode.Return))
            {
                UICanvas.SetActive(true);
                TutorialMenu.SetActive(false);
                snake.enabled = true;
                firstPlay = false;
            }

           
        }
    }

    public void addScore(int points)
    {
        score += points;
        scoreText.text = score.ToString();
    }

    public void generateFood()
    {
        int randomPosition;
        bool invalidPosition = true;
        do
        {
            randomPosition = Random.Range(0, 255);
            foreach (Vector2 snakePosition in snake.GetPositions())
            {
                if (snakePosition == boardPositions[randomPosition])
                {
                    invalidPosition = true;
                    break;
                }
                else
                {
                    invalidPosition = false;
                }
            }
        } while (invalidPosition);


        Instantiate(food, new Vector3(boardPositions[randomPosition].x + X_POS_OFFSET, boardPositions[randomPosition].y + Y_POS_OFFSET, 0), new Quaternion());
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
