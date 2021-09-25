using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleObject : MonoBehaviour
{
    public Bubbles manager;
    public GameObject ps;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        manager.Explosion();
        Instantiate(ps, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
