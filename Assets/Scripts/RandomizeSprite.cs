using UnityEngine;

public class RandomizeSprite : MonoBehaviour
{
    public Sprite[] PossibleSprites; // Liste mit allen möglichen Sprites

    void Start()
    {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (PossibleSprites.Length > 0)
        {
            renderer.sprite = PossibleSprites[Random.Range(0, PossibleSprites.Length)];
        }
    }
}
