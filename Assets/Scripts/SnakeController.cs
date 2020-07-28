using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeController : MonoBehaviour
{
    private const int increment = 2;
    public Direction actualDirection = Direction.RIGHT;
    public float movementInterval;
    private float nextMovementTime;
    private Animator anim;
    public AudioSource finalSound;
    public GameObject gameOverMessage;
    public SpriteRenderer headSprite;
    public GameController gameController;

    public float soundInterval;
    private float nextSoundTime;

    public GameObject eatSound;
    public GameObject movementSound;


    public AudioClip[] eatSounds;

    public enum Direction
    {
        UP, DOWN, LEFT, RIGHT
    }

    public List<GameObject> body;
    public GameObject tail;

    public GameObject bodyPrefab;


    public Vector2[] GetPositions()
    {
        List<Vector2> positions = new List<Vector2>();

        positions.Add(transform.position);
        foreach(GameObject bodyPart in body)
        {
            positions.Add(bodyPart.transform.position);
        }

        positions.Add(tail.transform.position);

        return positions.ToArray();
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();  
    }

    private void FixedUpdate()
    {
        HandleUserInput();
        Move();
    }

    private void HandleUserInput()
    { 
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            if(actualDirection != Direction.LEFT) actualDirection = Direction.RIGHT;
            PlayMoveSound();
        }
        else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            if (actualDirection != Direction.RIGHT) actualDirection = Direction.LEFT;
            PlayMoveSound();
        }

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            if (actualDirection != Direction.DOWN) actualDirection = Direction.UP;
            PlayMoveSound();
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            if (actualDirection != Direction.UP) actualDirection = Direction.DOWN;
            PlayMoveSound();
        }
    }

    private void PlayMoveSound()
    {
        if(Time.time > nextSoundTime) {
            Instantiate(movementSound);
            nextSoundTime = Time.time + soundInterval;
        }
    }

    private void Move()
    {
        if(Time.time > nextMovementTime)
        {
            nextMovementTime = Time.time + movementInterval;

            Vector2 lastHeadPosition = transform.position;
            Quaternion lastHeadRotation = transform.rotation;

            switch (actualDirection)
            {
                case Direction.RIGHT:
                    {
                        transform.position = new Vector2(transform.position.x + increment, transform.position.y);
                        transform.rotation = Quaternion.Euler(0, 0, 0);
                        break;
                    }

                case Direction.LEFT:
                    {
                        
                        transform.position = new Vector2(transform.position.x - increment, transform.position.y);
                        transform.rotation = Quaternion.Euler(0, 0, 180);
                        break;
                    }

                case Direction.UP:
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y + increment);
                        transform.rotation = Quaternion.Euler(0, 0, 90);
                        break;
                    }

                case Direction.DOWN:
                    {
                        transform.position = new Vector2(transform.position.x, transform.position.y - increment);
                        transform.rotation = Quaternion.Euler(0, 0, 270);
                        break;
                    }
                default: break;
            }

            Vector2 lastBodyPosition = Vector2.zero;
            Quaternion lastBodyRotation = Quaternion.Euler(0, 0, 0); ;
            for (int i = 0; i < body.Count; i++)
            {
                if (i == 0)
                {
                    lastBodyPosition = body[i].transform.position;
                    lastBodyRotation = body[i].transform.rotation;
                    body[i].transform.position = lastHeadPosition;
                    body[i].transform.rotation = lastHeadRotation;
                }
                else
                {
                    lastHeadPosition = body[i].transform.position;
                    lastHeadRotation = body[i].transform.rotation;
                    body[i].transform.position = lastBodyPosition;
                    body[i].transform.rotation = lastBodyRotation;
                    lastBodyPosition = lastHeadPosition;
                    lastBodyRotation = lastHeadRotation;
                }
            }

            tail.transform.position = lastBodyPosition;
            tail.transform.rotation = lastBodyRotation;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            anim.SetTrigger("eat");

            Instantiate(eatSound);
            Destroy(collision.gameObject);

            gameController.addScore(1);
            gameController.generateFood();

            StartCoroutine(Grow()); 
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            GameOver();
        }

        if (collision.gameObject.CompareTag("Body"))
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        finalSound.Play();
        gameOverMessage.SetActive(true);
        headSprite.enabled = false;
        this.enabled = false;
        gameController.soundTrack.Stop();
    }

    private IEnumerator Grow()
    {
        GameObject newBody = Instantiate(bodyPrefab);
        body.Add(newBody);

        newBody.transform.position = tail.transform.position;
        newBody.transform.rotation = tail.transform.rotation;

        yield return null;
    }
}
