using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
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

    private int gameScore = 0;
    private Vector3 screenBounds;
    private Vector2 generatePoint;
    private GameObject currentFruit;

    // Start is called before the first frame update
    void Start()
    {
        InitNewFruit();
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentFruit)
        {
            var clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            clickedPosition.x = Mathf.Clamp(clickedPosition.x, -screenBounds.x+currentFruit.GetComponent<SpriteRenderer>().bounds.extents.x, screenBounds.x-currentFruit.GetComponent<SpriteRenderer>().bounds.extents.x);
            Debug.Log(clickedPosition);
            var temp = currentFruit;
            currentFruit = null;
            temp.transform.DOMoveX(clickedPosition.x, 0.3f).OnComplete(() =>
            {
                temp.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                temp.GetComponent<CircleCollider2D>().enabled = true;
            });
        }
    }

    public void InitNewFruit()
    {
        if (!currentFruit)
        {
            // Using the AnimationCurve as the probability curve.
            int id = Mathf.RoundToInt(Mathf.Clamp(1f - GenerateProbability.Evaluate(Random.value), 0.1f, 1f) * MaxGenerateId);
            Vector2 rUCorner = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));
            generatePoint = new Vector2(0, rUCorner.y * 0.7f);

            currentFruit = CreateFruitOnPos(generatePoint.x, generatePoint.y, id);
            currentFruit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            currentFruit.GetComponent<CircleCollider2D>().enabled = false;
        }
    }

    public GameObject CreateFruitOnPos(float x, float y, int id=0)
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

    public void CreateJuiceOnPos(float x, float y, int id=0)
    {
        var juice = Instantiate(Juices[id]);
        juice.transform.SetParent(FruitsParent.transform);
        juice.GetComponent<Juice>().SetGeneratePosition(x, y);
        juice.GetComponent<Juice>().SetRadius(Fruits[id].GetComponent<SpriteRenderer>().bounds.extents.x);

        AudioSource.PlayClipAtPoint(JuiceAudio, new Vector3(0, 0));
        AudioSource.PlayClipAtPoint(BoomAudio, new Vector3(0, 0));
    }

    void AddScore(int id)
    {
        this.gameScore += (id+1) * 2;
        ScoreLabel.text = gameScore.ToString();
    }
}
