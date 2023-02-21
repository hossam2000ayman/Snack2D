using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveSnake : MonoBehaviour
{
    public float Speed = 0.2f;
    public float Step = 1.2f;
    Vector3 lastPos;
    public List<Transform> Tail = new List<Transform>();
    public List<GameObject> Food = new List<GameObject>();
    public GameObject GameOver_Panel;

    public TextMeshProUGUI txtScore1, txtScore2;
    int score1, score2;
    enum Direction
    {
        up, down, left, right
    }

    public GameObject SnakeTailPrefap;
    Direction direction;

    void Start()
    {
        txtScore1 = GameObject.Find("score").GetComponent<TextMeshProUGUI>();
        txtScore2 = GameObject.Find("result").GetComponent<TextMeshProUGUI>();
        Instantiate(Food[Random.Range(0, Food.Count)], RandomSpawnPostion(), Quaternion.identity);
        InvokeRepeating("Move", Speed, Speed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            direction = Direction.up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            direction = Direction.down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            direction = Direction.left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            direction = Direction.right;
        }
    }

    private void Move()
    {
        lastPos = transform.position;
        var nextPos = Vector3.zero;
        switch (direction)
        {
            case Direction.up:
                nextPos = Vector3.up;
                break;
            case Direction.down:
                nextPos = Vector3.down;
                break;
            case Direction.left:
                nextPos = Vector3.left;
                break;
            case Direction.right:
                nextPos = Vector3.right;
                break;
        }
        nextPos *= Step;
        transform.position += nextPos;
        MoveTail();
    }

    private void MoveTail()
    {
        for (int i = 0; i < Tail.Count; i++)
        {
            var tempPos = Tail[i].position;
            Tail[i].position = lastPos;
            lastPos = tempPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("enemy"))
        {
            CancelInvoke();
            StartCoroutine(Reload(3f));
            GameOver_Panel.SetActive(true);
        }
        else if (collision.CompareTag("food"))
        {
            Tail.Add(Instantiate(SnakeTailPrefap, Tail.Last().position, Quaternion.identity).transform);
            score1 += 50;
            score2++;
            txtScore1.text = score1.ToString();
            txtScore2.text = score2.ToString();
            Destroy(collision.gameObject);
            Instantiate(Food[Random.Range(0, Food.Count)], RandomSpawnPostion(), Quaternion.identity);
        }
    }

    private IEnumerator Reload(float secounds)
    {
        yield return new WaitForSeconds(secounds);
        SceneManager.LoadScene(0);
    }

    Vector2 RandomSpawnPostion()
    {
        float buffer = 50f;  // adjust this based on the size of your objects
        Vector3 worldMin = Camera.main.ScreenToWorldPoint(new Vector2(buffer, buffer));
        Vector3 worldMax = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width - buffer, Screen.height - buffer));
        return new Vector2(
           Random.Range(worldMin.x, worldMax.x),
           Random.Range(worldMin.y, worldMax.y));
    }

    public bool CheckBound(GameObject first, GameObject second)
    {
        var firstBounds = first.GetComponent<SpriteRenderer>().bounds;
        var SecondBounds = second.GetComponent<SpriteRenderer>().bounds;
        return firstBounds.Intersects(SecondBounds);
    }
}
