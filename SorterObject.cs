using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorterObject : MonoBehaviour
{
    public Vector2 startPosition;
    public bool inHand = false, finish = false, started = false;
    public SorterBag.Number nmb;
    public Coroutine mover;
    public Sprite secondSprite;
    private void OnMouseDrag()
    {
        if(inHand)
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseDown()
    {
        inHand = true;
        if(mover != null)
        {
            StopCoroutine(mover);
        }
        if (started == false)
        {
            started = true;
        }
    }
    private void Start()
    {
        if(Camera.main.GetComponent<MainGameManager>().isFirst == false)
        {
            GetComponent<SpriteRenderer>().sprite = secondSprite;
        }
    }
    private void OnMouseUp()
    {
        inHand = false;
        if (!finish)
        {
            if(mover != null)
            {
                StopCoroutine(mover);
            }
            mover = StartCoroutine(MoveObject(startPosition, 0.8f));
            Camera.main.GetComponent<SorterGame>().MissAudio();
        }
    }
    public IEnumerator MoveObject(Vector2 newPosition, float time)
    {
        float t = 0;
        while (t < time)
        {
            
            transform.position = Vector2.Lerp(transform.position, newPosition, t/time);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = newPosition;
        if(started == false)
        {
            started = true;
        }
    }
}
