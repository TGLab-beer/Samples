using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ConstructGameManager : MonoBehaviour
{
    public AudioClip levelComplete, mistake, part, startAudio;
    public Vector2 firstPos, secondPos;
    public Part[] parts, partsSecond;
    public Vector3 scaleVectorFirst = new Vector3(0.7f, 0.7f, 0.7f), scaleVectorSecond = new Vector3(0.7f, 0.7f, 0.7f); 
    public GameObject completeImage, completeImageSecond, shadow, shadowSecond;
    bool isFirst;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        isFirst = GetComponent<MainGameManager>().isFirst;
        StartCoroutine(Starter());
    }
    IEnumerator Starter()
    {
        if (isFirst)
        {
            for (int i = 0; i < 4; i++)
            {
                parts[i].gameObject.SetActive(true);
                StartCoroutine(ChangeAlpha(1, parts[i].GetComponent<SpriteRenderer>()));
            }
            StartCoroutine(ChangeAlpha(0.8f, shadow.GetComponent<SpriteRenderer>()));
        } else
        {
            for (int i = 0; i < 4; i++)
            {
                partsSecond[i].gameObject.SetActive(true);
                StartCoroutine(ChangeAlpha(1, partsSecond[i].GetComponent<SpriteRenderer>()));
            }
            StartCoroutine(ChangeAlpha(0.8f, shadowSecond.GetComponent<SpriteRenderer>()));
        }
        GetComponent<MainGameManager>().LowAudio();
        GetComponent<AudioSource>().PlayOneShot(startAudio);
        yield return new WaitForSeconds(startAudio.length);
        GetComponent<MainGameManager>().MaxAudio();
        if (isFirst)
        {
            for (int i = 0; i < 4; i++)
            {
                parts[i]._needPosition = parts[i].gameObject.transform.position;
                parts[i]._startPosition = i == 0 ? firstPos : i == 1 ? secondPos : i == 2 ? -firstPos : -secondPos;
                parts[i].StartCoroutine(parts[i].MoveToPosition(parts[i]._startPosition, 1f));
            }
        } else
        {
            for (int i = 0; i < 4; i++)
            {
                partsSecond[i]._needPosition = partsSecond[i].gameObject.transform.position;
                partsSecond[i]._startPosition = i == 0 ? firstPos : i == 1 ? secondPos : i == 2 ? -firstPos : -secondPos;
                partsSecond[i].StartCoroutine(partsSecond[i].MoveToPosition(partsSecond[i]._startPosition, 1f));
            }
        }
    }
    public void Mistake()
    {
        GetComponent<AudioSource>().PlayOneShot(mistake);
    }
    public void CheckWin()
    {
        GetComponent<AudioSource>().PlayOneShot(part);
        if (isFirst)
        {
            for (int i = 0; i < 4; i++)
            {
                if (parts[i].complete == false)
                {
                    return;
                }
            }
        } else
        {
            for (int i = 0; i < 4; i++)
            {
                if (partsSecond[i].complete == false)
                {
                    return;
                }
            }
        }
        StartCoroutine(Win());
    }
    IEnumerator Win()
    {
        GetComponent<AudioSource>().PlayOneShot(levelComplete);
        if (isFirst)
        {
            for (int i = 0; i < 4; i++)
            {
                StartCoroutine(ChangeAlpha(0, parts[i].GetComponent<SpriteRenderer>()));
            }
            StartCoroutine(ChangeAlpha(0, shadow.GetComponent<SpriteRenderer>()));
            yield return StartCoroutine(ChangeAlpha(1, completeImage.GetComponent<SpriteRenderer>()));
            yield return StartCoroutine(Scaler(completeImage, scaleVectorFirst));
            yield return StartCoroutine(ChangeAlpha(0, completeImage.GetComponent<SpriteRenderer>()));
        } else
        {
            for (int i = 0; i < 4; i++)
            {
                StartCoroutine(ChangeAlpha(0, partsSecond[i].GetComponent<SpriteRenderer>()));
            }
            StartCoroutine(ChangeAlpha(0, shadowSecond.GetComponent<SpriteRenderer>()));
            yield return StartCoroutine(ChangeAlpha(1, completeImageSecond.GetComponent<SpriteRenderer>()));
            yield return StartCoroutine(Scaler(completeImageSecond, scaleVectorSecond));
            yield return StartCoroutine(ChangeAlpha(0, completeImageSecond.GetComponent<SpriteRenderer>()));
        }
        if (isFirst)
        {
            for (int i = 0; i < 4; i++)
            {
                parts[i].gameObject.SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                partsSecond[i].gameObject.SetActive(false);
            }
        }
        GetComponent<MainGameManager>().StartCoroutine(GetComponent<MainGameManager>().FinishConstructGame());
    }
    IEnumerator Scaler(GameObject objectToRescale, Vector3 newScale)
    {
        float t = 0;
        while (t < 1)
        {
            objectToRescale.transform.localScale = Vector3.Lerp(objectToRescale.transform.localScale, newScale, t);
            t += Time.deltaTime / 3;
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator ChangeAlpha(float val, SpriteRenderer spr)
    {
        float t = 0;
        Color clr = spr.color;
        clr.a = val;
        while(t < 1)
        {
            spr.color = Color.Lerp(spr.color, clr, t);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        spr.color = clr;
    }
}
