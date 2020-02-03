using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer tileSprite;
    [SerializeField] private bool isFill;
    [SerializeField] private Sprite texture;

    public bool IsFill
    {
        get
        {
            return isFill;
        }
        set
        {
            if (value)
            {
                tileSprite.sprite = texture;
            }
            else
            {
                tileSprite.sprite = null;
            }
            isFill = value;
        }
    }

    private void Update()
    {
        if (IsFill)
        {
            tileSprite.sprite = texture;
        }
        else
        {
            tileSprite.sprite = null;
        }
    }

}
