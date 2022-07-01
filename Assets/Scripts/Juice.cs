using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juice : MonoBehaviour
{
    public Sprite Particle;
    public Sprite Explosion;
    public Sprite Splash;

    private float radius;
    private Vector3 generatePosition = new Vector3(0, 0,1);

    // Start is called before the first frame update
    void Start()
    {
        // Particles explosion
        for (int i = 0; i < 10; i++)
        {
            var newObject = new GameObject("Particles");
            newObject.transform.SetParent(transform);
            newObject.transform.position = generatePosition;

            var t = 2f * Random.value + radius;
            newObject.transform.localScale = new Vector3(t,t,t);

            var newSprite = newObject.AddComponent<SpriteRenderer>();
            newSprite.sprite = Particle;
            newSprite.sortingOrder = 3; // Render on top of the fruits

            var p = 1f * Random.value;  // time
            var a = 359 * Random.value; // outwards angle
            var j = 2 * Random.value + radius / 2;  // length
            var l = new Vector3(Mathf.Sin(a * Mathf.PI/180) * j, Mathf.Cos(a * Mathf.PI/180) * j);  // cartesian vector from polar coordinates

            Sequence tween = DOTween.Sequence();
            tween.Insert(0,newObject.transform.DOMove(newObject.transform.position+l, p));
            tween.Insert(0,newObject.transform.DOScale(.5f, p + .5f));
            tween.Insert(0,newObject.transform.DOLocalRotate(new Vector3(0, 0, Random.Range(-360f, 360f)), p + .5f));
            var newColor = newObject.GetComponent<SpriteRenderer>().color;
            newColor.a = 0;
            tween.Insert(0.2f ,newObject.GetComponent<SpriteRenderer>().DOColor(newColor,p+.3f));
            tween.Play();
            tween.OnComplete(() =>
            {
                Destroy(newObject);
            });
        }

        // Bubbles
        for (int i = 0; i < 20; i++)
        {
            var newObject = new GameObject("Bubbles");
            newObject.transform.SetParent(transform);
            newObject.transform.position = generatePosition;

            var t = 1f * Random.value + radius;
            newObject.transform.localScale = new Vector3(t, t, t);

            var newSprite = newObject.AddComponent<SpriteRenderer>();
            newSprite.sprite = Explosion;
            newSprite.sortingOrder = 2; // Render on top of the fruits

            var p = 1f * Random.value;  // time
            var a = 359 * Random.value; // outwards angle
            var j = 2 * Random.value + radius / 2;  // length
            var l = new Vector3(Mathf.Sin(a * Mathf.PI / 180) * j, Mathf.Cos(a * Mathf.PI / 180) * j);  // cartesian vector from polar coordinates

            Sequence tween = DOTween.Sequence();
            tween.Insert(0, newObject.transform.DOMove(newObject.transform.position + l, p));
            tween.Insert(0, newObject.transform.DOScale(.2f, p + .5f));
            var newColor = newObject.GetComponent<SpriteRenderer>().color;
            newColor.a = 0;
            tween.Insert(0.2f, newObject.GetComponent<SpriteRenderer>().DOColor(newColor, p + .3f));
            tween.Play();
            tween.OnComplete(() =>
            {
                Destroy(newObject);
            });
        }

        // Juices
        var newJuice = new GameObject("Splash");
        newJuice.transform.SetParent(transform);
        newJuice.transform.position = generatePosition;

        var newJucieSprite = newJuice.AddComponent<SpriteRenderer>();
        newJucieSprite.sprite = Splash;
        newJucieSprite.sortingOrder = 1; // Render on top of the fruits

        newJuice.transform.localEulerAngles=new Vector3(0, 0, Random.value*359);
        newJuice.transform.localScale = new Vector3(0, 0, 0);

        Sequence juiceTween = DOTween.Sequence();
        juiceTween.Insert(0, newJuice.transform.DOScale(.6f,0.3f));
        var newJuiceColor = newJuice.GetComponent<SpriteRenderer>().color;
        newJuiceColor.a = 0;
        juiceTween.Insert(.2f, newJuice.GetComponent<SpriteRenderer>().DOColor(newJuiceColor, 1f));
        juiceTween.Play();
        juiceTween.OnComplete(() =>
        {
            Destroy(newJuice);
        });

        Destroy(this);
    }

    public void SetRadius(float radius)
    {
        this.radius = radius*2;
    }

    public void SetGeneratePosition(float x, float y)
    {
        generatePosition.x = x;
        generatePosition.y = y;
    }
}
