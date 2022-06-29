using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Juice : MonoBehaviour
{
    public Sprite Particle;
    public Sprite Explosion;
    public Sprite Slash;

    private GameObject parent;
    private Vector3 generatePosition;
    private float radius;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            var newObject = new GameObject();
            newObject.transform.SetParent(parent.transform);
            var newSprite = newObject.AddComponent<SpriteRenderer>();
            newSprite.sprite = Particle;

            newObject.transform.localScale = new Vector3(0.5f * Random.value + radius / 100f, 0.5f * Random.value + radius / 100f, 0.5f * Random.value + radius / 100f);
            var newScale = 0.5f * Random.value;


        }
    }

    public void SetParent(GameObject parent)
    {
        this.parent = parent;
    }

    public void SetGeneratePosititon(float x, float y)
    {
        generatePosition = new Vector3(x, y);
    }
}
