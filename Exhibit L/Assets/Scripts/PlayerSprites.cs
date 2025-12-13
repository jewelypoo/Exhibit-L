using System.Collections;
using UnityEngine;

public class PlayerSprites : MonoBehaviour
{
    public PlayerController controller;
    private Renderer rend;
    public float spriteTime;
    public bool flipSprite;
    public bool canFlip;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canFlip = true;
        flipSprite = false;
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (controller.currentSpeed >= 1f)
        {
            if (canFlip)
            {
                canFlip = false;
                StartCoroutine(SpriteSwap());
            }
        }
        else
        {
            rend.material.mainTextureOffset = new Vector2(0f, 0f);
            //print("static sprite");
        }

    }

    public IEnumerator SpriteSwap()
    {
        flipSprite = !flipSprite;

        if (flipSprite)
        {
            rend.material.mainTextureOffset = new Vector2(.25f, 0f);
            //print("sprite 1");
        }
        else
        {
            rend.material.mainTextureOffset = new Vector2(.5f, 0f);
            //print("sprite 2");
        }
        yield return new WaitForSeconds(spriteTime);
        canFlip = true;
    }
}
