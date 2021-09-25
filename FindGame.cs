using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindGame : MonoBehaviour
{
    public AudioClip[] questions;
    public GameObject[] objectsToFind;
    int numberOfQuestion;
    public AudioClip miss, startGame;
    public AudioClip[] pass;

    private void OnEnable()
    {
        StartCoroutine(Starter());
    }
    public IEnumerator NextQuestion()
    {
        if(numberOfQuestion < questions.Length)
        {
            Camera.main.GetComponent<MainGameManager>().LowAudio();
            GetComponent<AudioSource>().PlayOneShot(questions[numberOfQuestion]);
            yield return new WaitForSeconds(questions[numberOfQuestion].length);
            Camera.main.GetComponent<MainGameManager>().MaxAudio();
            for (int i = 0; i < objectsToFind.Length; i++)
            {
                objectsToFind[i].GetComponent<FindGameObject>().activeObj = true;
            }
        }
        else
        {
            for (int i = 0; i < objectsToFind.Length; i++)
            {
                StartCoroutine(SetAlhpa(objectsToFind[i], 0, 1, true));
            }
            GetComponent<MainGameManager>().StartCoroutine(GetComponent<MainGameManager>().FinishFindGame());
        }
    }
    IEnumerator Starter()
    {
        for(int i = 0; i < objectsToFind.Length; i++)
        {
            objectsToFind[i].SetActive(true);
            StartCoroutine(SetAlhpa(objectsToFind[i], 1, 1, false));
        }
        Camera.main.GetComponent<MainGameManager>().LowAudio();
        GetComponent<AudioSource>().PlayOneShot(startGame);
        yield return new WaitForSeconds(startGame.length);
        StartCoroutine(NextQuestion());
    }
    
    IEnumerator SetAlhpa(GameObject obj, float setAlpha, float time, bool deactive)
    {
        float t = 0;
        Color clr = obj.GetComponent<SpriteRenderer>().color;
        clr.a = setAlpha;
        while (t < time)
        {
            yield return null;
            obj.GetComponent<SpriteRenderer>().color = Color.Lerp(obj.GetComponent<SpriteRenderer>().color, clr, t / time);
            t += Time.deltaTime;
        }
        if (deactive)
        {
            obj.SetActive(false);
        }
    }
    public IEnumerator Find(int number)
    {
        if(number == numberOfQuestion)
        {
            Camera.main.GetComponent<MainGameManager>().LowAudio();
            GetComponent<AudioSource>().PlayOneShot(pass[numberOfQuestion]);
            for (int i = 0; i < objectsToFind.Length; i++)
            {
                objectsToFind[i].GetComponent<FindGameObject>().activeObj = false;
            }
            objectsToFind[numberOfQuestion].GetComponent<Animator>().SetTrigger("StartAnim");
            yield return new WaitForSeconds(pass[numberOfQuestion].length);
            numberOfQuestion += 1;
            StartCoroutine(NextQuestion());
        } else
        {
            GetComponent<AudioSource>().PlayOneShot(miss);
        }
    }
}
