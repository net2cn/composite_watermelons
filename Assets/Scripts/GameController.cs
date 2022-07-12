using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public GameObject[] Fruits;
    public GameObject FruitsParent;
    [Tooltip("The probability curve is reversed. The smaller id is more likely to be generated.")]
    public AnimationCurve GenerateProbability;
    public int MaxGenerateId = 3;

    public GameObject[] Juices;
    public AudioClip BoomAudio;
    public AudioClip JuiceAudio;
    public AudioClip KnockAudio;

    public Text ScoreLabel;

    public Sprite Flash;

    private int gameScore = 0;
    private Vector3 screenBounds;
    private Vector2 generatePoint;
    private GameObject currentFruit;

    private bool isOperatable = true;
    private bool gameOver = false;
    private bool isHeld = false;

    // Start is called before the first frame update
    void Start()
    {
        InitNewFruit();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (currentFruit && isOperatable)
            {
                var clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                clickedPosition.x = Mathf.Clamp(clickedPosition.x, -screenBounds.x + currentFruit.GetComponent<SpriteRenderer>().bounds.extents.x, screenBounds.x - currentFruit.GetComponent<SpriteRenderer>().bounds.extents.x);
                // Debug.Log(clickedPosition);
                currentFruit.transform.DOMoveX(clickedPosition.x, 0.3f);
            }
            else if (gameOver)
            {
                Debug.Log("Restart Game.");
                RestartGame();
            }
        }
        else if (Input.GetMouseButton(0))
        {
            var clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickedPosition.x = Mathf.Clamp(clickedPosition.x, -screenBounds.x + currentFruit.GetComponent<SpriteRenderer>().bounds.extents.x, screenBounds.x - currentFruit.GetComponent<SpriteRenderer>().bounds.extents.x);
            if (Mathf.Abs(clickedPosition.x - currentFruit.transform.position.x) < 0.1f || isHeld)
            {
                isHeld = true;
                clickedPosition.y = currentFruit.transform.position.y;
                clickedPosition.z = currentFruit.transform.position.z;
                currentFruit.transform.position = clickedPosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isHeld = false;
            var temp = currentFruit;
            currentFruit = null;
            temp.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            temp.GetComponent<CircleCollider2D>().enabled = true;
            InitNewFruit();
        }
    }

    public void InitNewFruit()
    {
        if (!currentFruit)
        {
            // Using the AnimationCurve as the probability curve.
            int id = Mathf.RoundToInt(Mathf.Clamp(1f - GenerateProbability.Evaluate(Random.value), 0.1f, 1f) * MaxGenerateId);
            Vector2 rUCorner = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));
            generatePoint = new Vector2(0, rUCorner.y * 0.8f);

            currentFruit = CreateFruitOnPos(generatePoint.x, generatePoint.y, id);
            currentFruit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            currentFruit.GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    public GameObject CreateFruitOnPos(float x, float y, int id = 0)
    {
        var fruit = CreateFruit(id);
        fruit.transform.position = new Vector3(x, y, 0);
        return fruit;
    }

    GameObject CreateFruit(int id)
    {
        var fruit = Instantiate(Fruits[id]);
        fruit.transform.SetParent(FruitsParent.transform);
        AddScore(id);
        return fruit;
    }

    public void CreateJuiceOnPos(float x, float y, int id = 0)
    {
        var juice = Instantiate(Juices[id]);
        juice.transform.SetParent(FruitsParent.transform);
        juice.GetComponent<Juice>().SetGeneratePosition(x, y);
        juice.GetComponent<Juice>().SetRadius(Fruits[id].GetComponent<SpriteRenderer>().bounds.extents.x);

        AudioSource.PlayClipAtPoint(JuiceAudio, new Vector3(0, 0));
        AudioSource.PlayClipAtPoint(BoomAudio, new Vector3(0, 0));
    }

    public void ShowWinningAnimation()
    {
        isOperatable = false;

        GameObject canvas = GameObject.Find("Canvas");
        GameObject winningParent = new GameObject("WinningParent");
        winningParent.transform.SetParent(canvas.transform);

        Sequence tween = DOTween.Sequence();

        SpriteRenderer blackScreen = new GameObject("BlackScreen").AddComponent<SpriteRenderer>();
        blackScreen.transform.SetParent(winningParent.transform);
        blackScreen.transform.localScale = screenBounds * 2;
        blackScreen.sprite = Sprite.Create(new Texture2D(100, 100), new Rect(0, 0, 100, 100), new Vector2(0.5f, 0.5f));
        blackScreen.sortingOrder = 10;
        blackScreen.color = new Color(0, 0, 0, 0);
        tween.Insert(0, blackScreen.DOColor(new Color(0, 0, 0, 0.8f), 0.1f));
        tween.Insert(0.8f, blackScreen.DOColor(new Color(0, 0, 0, 0), 0.4f));

        SpriteRenderer flash = new GameObject("Flash").AddComponent<SpriteRenderer>();
        flash.transform.SetParent(winningParent.transform);
        flash.transform.localScale = new Vector3((screenBounds.x < screenBounds.y ? screenBounds.x : screenBounds.y) * 0.8f, (screenBounds.x < screenBounds.y ? screenBounds.x : screenBounds.y) * 0.8f);
        flash.sprite = Flash;
        flash.sortingOrder = 11;
        flash.color = new Color(1f, 1f, 1f, 0.6f);
        tween.Insert(0, flash.DOColor(new Color(1, 1, 1, 1f), 0.1f));
        tween.Insert(0, flash.transform.DORotate(new Vector3(0, 0, -270f), 1.2f));
        tween.Insert(0, flash.transform.DOScale(screenBounds.x < screenBounds.y ? screenBounds.x : screenBounds.y, 0.2f));
        tween.Insert(0.8f, flash.transform.DOScale((screenBounds.x < screenBounds.y ? screenBounds.x : screenBounds.y) * 0.8f, 0.4f));
        tween.Insert(0.8f, flash.DOColor(new Color(1f, 1f, 1f, 0.8f), 0.4f));

        SpriteRenderer watermelon = new GameObject("Watermelon").AddComponent<SpriteRenderer>();
        watermelon.transform.SetParent(winningParent.transform);
        watermelon.transform.localScale = new Vector3(0.95f, 0.95f, 0.95f);
        watermelon.sprite = Fruits[Fruits.Length - 1].GetComponent<SpriteRenderer>().sprite;
        watermelon.sortingOrder = 12;
        tween.Insert(0, watermelon.transform.DOScale(1f, 0.2f));
        tween.Insert(0.8f, watermelon.transform.DOScale(0.95f, 0.4f));

        tween.Play();
        tween.OnComplete(() =>
        {
            isOperatable = true;
            Destroy(winningParent);
        });
    }

    public void ShowGameLost()
    {
        isOperatable = false;

        if (!GameObject.Find("LosingParent"))
        {
            GameObject canvas = GameObject.Find("Canvas");
            GameObject losingParent = new GameObject("LosingParent");

            losingParent.transform.SetParent(canvas.transform);

            Sequence tween = DOTween.Sequence();

            SpriteRenderer blackScreen = new GameObject("BlackScreen").AddComponent<SpriteRenderer>();
            blackScreen.transform.SetParent(losingParent.transform);
            blackScreen.transform.localScale = screenBounds * 2;
            blackScreen.sprite = Sprite.Create(new Texture2D(100, 100), new Rect(0, 0, 100, 100), new Vector2(0.5f, 0.5f));
            blackScreen.sortingOrder = 10;
            blackScreen.color = new Color(0, 0, 0, 0);
            tween.Insert(0, blackScreen.DOColor(new Color(0, 0, 0, 0.9f), 0.3f));

            Text gameOver = new GameObject("GameOver").AddComponent<Text>();
            gameOver.transform.SetParent(losingParent.transform);
            float widthScale = 1 / (screenBounds.x < screenBounds.y ? screenBounds.x * 4 : screenBounds.y * 2);
            gameOver.transform.localScale = new Vector3(widthScale, widthScale);
            gameOver.color = new Color(1f, 1f, 1f, 0f);
            gameOver.text = "Game Over!";
            gameOver.alignment = TextAnchor.MiddleCenter;
            gameOver.fontSize = 8;

            gameOver.font = ScoreLabel.font;
            tween.Insert(0.5f, gameOver.DOColor(new Color(1f, 1f, 1f, 1f), 0.3f));

            tween.Play();
            tween.OnComplete(() =>
            {
                this.gameOver = true;
            });
        }
    }

    void RestartGame()
    {
        gameOver = false;
        Destroy(GameObject.Find("LosingParent"));
        Destroy(currentFruit);

        foreach (Transform childTransform in FruitsParent.transform)
        {
            Destroy(childTransform.gameObject);
        }

        GameObject.Find("GameLoseEdge").GetComponent<GameLose>().ResetTriggered();

        gameScore = 0;
        ScoreLabel.text = gameScore.ToString();

        IEnumerator start()
        {
            yield return new WaitForEndOfFrame();
            InitNewFruit();
            isOperatable = true;
        }
        StartCoroutine(start());
    }

    void AddScore(int id)
    {
        gameScore += (id + 1) * 2;
        ScoreLabel.text = gameScore.ToString();
    }
}
