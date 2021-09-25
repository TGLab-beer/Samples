using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorterBag : MonoBehaviour
{
    public Sprite firstGameEmpty_sprite, firstGameFull_sprite, secondGameEmpty_sprite, secondGameFull_sprite;
    public SorterGame sorterManager;
    bool fisrtGame = true;
    public Number nmb;
    public Vector2 normalPos;
    public AudioClip fullBag;
    public enum Number
    {
        first,
        second,
        third
    }
    // Start is called before the first frame update
    void Start()
    {
        fisrtGame = Camera.main.GetComponent<MainGameManager>().isFirst;
        if (!fisrtGame)
        {
            GetComponent<SpriteRenderer>().sprite = secondGameEmpty_sprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<SorterObject>().started && collision.GetComponent<SorterObject>().nmb == nmb)
        {
            collision.GetComponent<SorterObject>().finish = true;
            sorterManager.DeleteObject(collision.gameObject, transform.position);
            if (fisrtGame)
            {
                if (GetComponent<SpriteRenderer>().sprite == firstGameEmpty_sprite)
                {
                    GetComponent<SpriteRenderer>().sprite = firstGameFull_sprite;
                    GetComponent<AudioSource>().PlayOneShot(fullBag);
                }
            }
            else
            {
                if (GetComponent<SpriteRenderer>().sprite == secondGameEmpty_sprite)
                {
                    GetComponent<SpriteRenderer>().sprite = secondGameFull_sprite;
                    GetComponent<AudioSource>().PlayOneShot(fullBag);
                }
            }
            sorterManager.PassAudio();
        }
    }
    public IEnumerator MoveObject(Vector2 newPosition, bool dis)
    {
        float t = 0;
        while (t < 1)
        {
            transform.position = Vector2.Lerp(transform.position, newPosition, t);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = newPosition;
        if (dis)
        {
            gameObject.SetActive(false);
        }
        yield break;
    }
}
