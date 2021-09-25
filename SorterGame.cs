using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class SorterGame : MonoBehaviour
{

    public List<GameObject> sortingObjects;
    public float posY;
    public GameObject[] objectsToSort;
    public GameObject[] bags;
    float timer = 0;
    int counter = 0;
    public AudioClip pass, miss, startText;
    public bool isFinished = false, started = false;
    void Start()
    {
       
    }
    private void OnEnable()
    {
        StartCoroutine(Starter());
    }
    IEnumerator Starter()
    {
        for(int i = 0; i < 3; i++)
        {
            bags[i].SetActive(true);
            bags[i].transform.position = bags[i].transform.position - new Vector3(0, 9, 0);
            bags[i].GetComponent<SorterBag>().StartCoroutine(bags[i].GetComponent<SorterBag>().MoveObject(bags[i].GetComponent<SorterBag>().normalPos, false));
        }
        Camera.main.GetComponent<MainGameManager>().LowAudio();
        GetComponent<AudioSource>().PlayOneShot(startText);
        yield return new WaitForSeconds(4);
        Camera.main.GetComponent<MainGameManager>().MaxAudio();
        counter = 5;
        started = true;
    }
    void AddNewObjects(int count)
    {
        GameObject[] newArray = new GameObject[count];
        for(int i = 0; i < count; i++)
        {
            newArray[i] = Instantiate(objectsToSort[Random.Range(0, objectsToSort.Length)], new Vector3(0, 7), Quaternion.identity);
        }
        AddObjects(newArray);
    }
    
    // Update is called once per frame
    void Update()
    {
        if(counter > 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                AddNewObjects(5);
                counter -= 1;
                timer = 5;
            }
        } else if(sortingObjects.Count == 0 && !isFinished && started)
        {
            for (int i = 0; i < 3; i++)
            {
                bags[i].GetComponent<SorterBag>().StartCoroutine(bags[i].GetComponent<SorterBag>().MoveObject(bags[i].transform.position + new Vector3(0, 8, 0), true));
            }
            GetComponent<MainGameManager>().StartCoroutine(GetComponent<MainGameManager>().FinishSorter());
            
            isFinished = true;
        }
    }

    #region ObjectsPanel
    public void DeleteObject(GameObject objToDelete, Vector2 bagPose)
    {
        sortingObjects.Remove(objToDelete);
        StartCoroutine(Deleter(objToDelete));
        StartCoroutine(MoveObject(bagPose, objToDelete, 0.6f));
        ReplaceObjects();
    }
    public void PassAudio()
    {
        GetComponent<AudioSource>().PlayOneShot(pass);
    }
    public void MissAudio()
    {
        GetComponent<AudioSource>().PlayOneShot(miss);
    }
    IEnumerator Deleter(GameObject objectForDestroy)
    {
        float t = 0;
        while(t < 1)
        {
            objectForDestroy.transform.localScale = Vector3.Lerp(objectForDestroy.transform.localScale, new Vector3(0, 0, 0), t);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(objectForDestroy);
        yield break;
    }
    public void ReplaceObjects()
    {
        //float start_X = -(sortingObjects.Count - 1) / 2 * 0.5f + 0.25f;
        float start_X = -(sortingObjects.Count -1) * 1.2f / 2;
        for (int i = 0; i < sortingObjects.Count; i++)
        {
            if(sortingObjects[i].GetComponent<SorterObject>().mover != null)
                sortingObjects[i].GetComponent<SorterObject>().StopCoroutine(sortingObjects[i].GetComponent<SorterObject>().mover);
            if (sortingObjects[i].GetComponent<SorterObject>().inHand == false)
                sortingObjects[i].GetComponent<SorterObject>().mover = sortingObjects[i].GetComponent<SorterObject>().StartCoroutine(sortingObjects[i].GetComponent<SorterObject>().MoveObject(new Vector2(start_X + i * 1.2f, posY), 0.8f));
            
            sortingObjects[i].GetComponent<SorterObject>().startPosition = new Vector2(start_X + i * 1.2f, posY);
        }
    }
    public void AddObjects(GameObject[] obj)
    {
        for(int i = 0; i < obj.Length; i++)
            sortingObjects.Add(obj[i]);
        ReplaceObjects();
    }
    IEnumerator MoveObject(Vector2 newPosition, GameObject obj, float time)
    {
        float t = 0;
        while(t < time)
        {
            yield return null;
            obj.transform.position = Vector2.Lerp(obj.transform.position, newPosition, t/time);
            t += Time.deltaTime;
            
        }
        obj.transform.position = newPosition;
        yield break;
    }
    #endregion
}
