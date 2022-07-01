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
        GetComponent<CircleCollider2D>().radius = this.GetComponent<SpriteRenderer>().bounds.extents.x;
        GetComponent<Rigidbody2D>().mass = Mathf.Pow(GetComponent<SpriteRenderer>().bounds.extents.x, 2) * Mathf.PI*5;
        //Debug.Log(GetComponent<SpriteRenderer>().bounds.extents.x);
        maxId = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>().Fruits.Length;
    }

    private void FixedUpdate()
    {
        if (transform.position.y < -Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)).y*2)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>().InitNewFruit();
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Nah, too noisy. I decide not to play this effect sound.
        // AudioSource.PlayClipAtPoint(GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>().KnockAudio, new Vector3(0,0));
        if (isFirstCollison)
        {
            GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>().InitNewFruit();
            isFirstCollison = false;
        }

        Fruit collided = collision.gameObject.GetComponent<Fruit>();
        if (collided)
        {
            if (!isProcessing && collided.Id == Id && Id<maxId-1)
            {
                collided.isProcessing = true;
                
                GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                collided.GetComponent<Rigidbody2D>().bodyType=RigidbodyType2D.Static;
                GetComponent<CircleCollider2D>().enabled = false;
                collided.GetComponent<CircleCollider2D>().enabled = false;
                
                collided.transform.DOMove(transform.position, 0.3f).OnComplete(() =>
                {
                    var newFruit = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>().CreateFruitOnPos(collided.transform.position.x, collided.transform.position.y, Id + 1);
                    GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>().CreateJuiceOnPos(collided.transform.position.x, collided.transform.position.y , Id);
                    
                    newFruit.GetComponent<Fruit>().isFirstCollison = false;
                    var originalScale = newFruit.transform.localScale;
                    newFruit.transform.localScale = new Vector3(originalScale.x*0.5f, originalScale.y * 0.5f);
                    newFruit.transform.DOScale(originalScale, 0.2f);
                    DOTween.To(()=>newFruit.GetComponent<CircleCollider2D>().radius, x=>newFruit.GetComponent<CircleCollider2D>().radius=x, newFruit.GetComponent<CircleCollider2D>().radius, 0.2f);
                    Destroy(collided.gameObject);
                    Destroy(this.gameObject);
                });
            }
        }
    }
}
