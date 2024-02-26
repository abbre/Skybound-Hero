using UnityEngine;

public class TutorialKeySpriteChange : MonoBehaviour
{
    public GameObject d;
    public Sprite blackD;
    private Sprite originalD;
    
    public GameObject w;
    public Sprite blackW;
    private Sprite originalW;
    
    public GameObject space;
    public Sprite blackSpace;
    private Sprite originalSpace;

    private void Start()
    {
        originalD = d.GetComponent<SpriteRenderer>().sprite;
        originalW = w.GetComponent<SpriteRenderer>().sprite;
        originalSpace = space.GetComponent<SpriteRenderer>().sprite;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            ChangeSprite(d, blackD);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            ChangeSprite(w, blackW);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ChangeSprite(space, blackSpace);
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            RestoreSprite(d, originalD);
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            RestoreSprite(w, originalW);
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            RestoreSprite(space, originalSpace);
        }
    }

    private void ChangeSprite(GameObject obj, Sprite newSprite)
    {
        obj.GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    private void RestoreSprite(GameObject obj, Sprite originalSprite)
    {
        obj.GetComponent<SpriteRenderer>().sprite = originalSprite;
    }
}
