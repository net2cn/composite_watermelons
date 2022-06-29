using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour
{
    public int Id;

    private bool isFirstCollison = true;
    private bool isProcessing = false;
    private int maxId = 0;

    void Start()
    {
        //this.GetComponent<CircleCollider2D>().radius = this.GetComponent<SpriteRenderer>().bounds.size.x / 2f;
        maxId = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>().Fruits.Length;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isFirstCollison)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>().InitNewFruit();
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>().SetOperatable(true);
            isFirstCollison = false;
        }

        Fruit collided = collision.gameObject.GetComponent<Fruit>();
        if (collided)
        {
            Debug.Log("Fruits collide");
            if (!isProcessing && collided.Id == Id && Id<maxId)
            {
                collided.isProcessing = true;
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                collided.GetComponent<Rigidbody2D>().bodyType=RigidbodyType2D.Static;
                transform.DOMove(collided.transform.position, 0.3f).OnComplete(() =>
                {
                    var newFruit = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>().CreateFruitOnPos(collided.transform.position.x, collided.transform.position.y, Id+1);
                    newFruit.GetComponent<Fruit>().isFirstCollison = false;
                    var originalScale = newFruit.transform.localScale;
                    newFruit.transform.localScale = new Vector3(originalScale.x*0.5f, originalScale.y * 0.5f);
                    newFruit.transform.DOScale(originalScale, 0.2f);
                    newFruit.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                    Destroy(collided.gameObject);
                    Destroy(this.gameObject);
                });
            }
        }
    }
}
