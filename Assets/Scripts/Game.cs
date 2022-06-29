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

    public Text ScoreLabel;

    private int gameScore = 0;
    private Vector2 generatePoint;
    private GameObject currentFruit;
    private bool isOperatable = true;

    // Start is called before the first frame update
    void Start()
    {
        InitNewFruit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isOperatable)
        {
            isOperatable = false;
            var clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            currentFruit.transform.DOMoveX(clickedPosition.x, 0.3f).OnComplete(() =>
            {
                currentFruit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            });
        }
    }

    public void InitNewFruit()
    {
        // Using the AnimationCurve as the probability curve.
        int id = Mathf.RoundToInt(Mathf.Clamp(1f-GenerateProbability.Evaluate(Random.value), 0.1f, 1f)*MaxGenerateId);
        Vector2 rUCorner = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));
        generatePoint = new Vector2(0, rUCorner.y * 0.7f);

        currentFruit = CreateFruitOnPos(generatePoint.x, generatePoint.y, id);
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
        fruit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        AddScore(id);
        return fruit;
    }

    public void ShowJuice(float x, float y, int juiceId)
    {

    }

    public void SetOperatable(bool flag)
    {
        isOperatable = flag;
    }

    void AddScore(int id)
    {
        this.gameScore += (id+1) * 2;
        ScoreLabel.text = gameScore.ToString();
    }
}
