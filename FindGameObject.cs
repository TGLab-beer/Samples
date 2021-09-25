using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGameObject : MonoBehaviour
{
    public int number;
    public FindGame manager;
    public bool activeObj = false;

    private void OnMouseDown()
    {
        if (activeObj)
        {
            manager.StartCoroutine(manager.Find(number));
        }
    }
}
