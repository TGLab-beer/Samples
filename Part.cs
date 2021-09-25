using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Part : MonoBehaviour
{
    public Vector2 _needPosition, _startPosition;
    public bool complete;
    bool drag;
    Vector2 offset;
    public ConstructGameManager manager;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseUp()
    {
        if (!complete && drag)
        {
            if (Math.Abs(transform.position.x - _needPosition.x) < 0.5f && Math.Abs(transform.position.y - _needPosition.y) < 0.5f)
            {
                StartCoroutine(MoveToPosition(_needPosition, 0.8f));
                complete = true;
                manager.CheckWin();
            }
            else
            {
                manager.Mistake();
                StartCoroutine(MoveToPosition(_startPosition, 0.8f));
            }
            drag = false;
        }
    }
    private void OnMouseDrag()
    {
        if (!complete)
        {
            Vector2 pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //transform.position = Vector2.MoveTowards(transform.position, pos + offset, Time.deltaTime * 10);
            transform.position = pos + offset;
        }
    }
    private void OnMouseDown()
    {
        if (!complete)
        {
            drag = true;
            Vector2 pos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            offset = (Vector2)transform.position - pos;
        }
    }
    public IEnumerator MoveToPosition(Vector2 newPos, float time)
    {
        float t = 0;
        while(t < time)
        {
            transform.position = Vector2.Lerp(transform.position, newPos, t/time);
            t += Time.deltaTime;
            yield return null;
        }
        transform.position = newPos;
    }
}
