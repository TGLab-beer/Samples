using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainGameManager : MonoBehaviour
{
    public GameObject homeButton;

    public Ads adManager;

    public GameObject SorterSticker_first, SorterSticker_second, findSticker_first, findSticker_second, constructorSticker_first, constructorSticker_second;
    public GameObject bag, canvas;
    public bool isFirst = true;
    public Color bgDark;
    public Image darkImage;
    public SorterGame sorter;
    public FindGame findGame;
    public Bubbles bubble;
    public ConstructGameManager construct;
    public AudioClip clip1F, clip2F, clip3F;
    public AudioClip clip1S, clip2S, clip3S;
    public AudioClip greateClip, stickerGet, stickerFly;
    public AudioSource music;
    public GameObject finalMenu;
    public string levelKey, sceneName, firstPassKey, secondPassKey;
    public bool isFirstScene = false;
    public GameObject translateCanvas, translatePanel;
    public GameObject loadObject;
    public string[] firstStickersKeys, secondStickersKeys;
    public int levelNumber;
    public void Restart()
    {
        loadObject.GetComponent<Loader>().StartCoroutine(loadObject.GetComponent<Loader>().TranslateToNextLevel(sceneName));
    }
    public void Menu()
    {
        loadObject.GetComponent<Loader>().StartCoroutine(loadObject.GetComponent<Loader>().TranslateToNextLevel("Menu"));
    }
    public void SetStickers()
    {
        if (isFirst)
        {
            for(int i = 0; i < firstStickersKeys.Length; i++)
            {
                PlayerPrefs.SetInt(firstStickersKeys[i], 1);
            }
        } else
        {
            for (int i = 0; i < secondStickersKeys.Length; i++)
            {
                PlayerPrefs.SetInt(secondStickersKeys[i], 1);
            }
        }
    }
    private void Start()
    {
        isFirst = PlayerPrefs.GetInt(levelKey) == 0;
        int x = PlayerPrefs.GetInt(firstPassKey);
        if (isFirstScene && x == 0)
        {
            homeButton.SetActive(false);
        }
        sorter.enabled = true;
    }
    IEnumerator DarkImage(float setAlpha, float time)
    {
        float t = 0;
        Color clr = darkImage.color;
        clr.a = setAlpha;
        while (t < time)
        {
            yield return null;
            darkImage.color = Color.Lerp(darkImage.color, clr, t/time);
            t += Time.deltaTime;
            
            
        }
    }
    IEnumerator MoveObject(GameObject objectToMove, Vector3 newPosition, float time)
    {
        float t = 0;
        while (t < time)
        {
            yield return null;
            objectToMove.transform.position = Vector3.Lerp(objectToMove.transform.position, newPosition, t/time);
            t += Time.deltaTime;
            
        }
    }
    IEnumerator Scaler(GameObject objToRescale, Vector3 newScale, float time)
    {
        float t = 0;
        while (t < time)
        {
            yield return null;
            objToRescale.transform.localScale = Vector3.Lerp(objToRescale.transform.localScale, newScale, t/time);
            t += Time.deltaTime;
            Debug.Log(t);
            
        }
    }
    public void LowAudio()
    {
        StartCoroutine(MusicVolume(0.5f, 0.05f));
    }
    public void MaxAudio()
    {
        StartCoroutine(MusicVolume(0.5f, 0.3f));
    }
    IEnumerator MusicVolume(float time, float value)
    {
        float t = 0;
        while(t < time)
        {
            music.volume = Mathf.Lerp(music.volume, value, t / time);
            t += Time.deltaTime;
            yield return null;
        }
    }
    public IEnumerator FinishSorter()
    {
        LowAudio();
        GetComponent<AudioSource>().PlayOneShot(greateClip);
        yield return new WaitForSeconds(greateClip.length + 0.2f);
        float t = 0;
        if (isFirst && PlayerPrefs.GetInt(firstPassKey) == 0 || !isFirst && PlayerPrefs.GetInt(secondPassKey) == 0)
        {
            GameObject info = new GameObject();
            if (isFirst)
            {
                info = Instantiate(SorterSticker_first, new Vector3(0, -6, 0), Quaternion.identity);
            }
            else
            {
                info = Instantiate(SorterSticker_second, new Vector3(0, -6, 0), Quaternion.identity);
            }
            info.transform.SetParent(canvas.transform);
            info.transform.localScale = new Vector3(1, 1, 1);
            StartCoroutine(DarkImage(0.6f, 0.8f));
            GetComponent<AudioSource>().PlayOneShot(stickerGet);
            yield return StartCoroutine(MoveObject(info, Vector3.zero, 1));
            LowAudio();
            GetComponent<AudioSource>().PlayOneShot(isFirst ? clip1F : clip1S);
            yield return new WaitForSeconds(isFirst ? clip1F.length : clip1S.length + 0.2f);
            MaxAudio();
            StartCoroutine(DarkImage(0, 0.8f));
            StartCoroutine(Scaler(info, Vector3.zero, 1));
            StartCoroutine(Scaler(bag, new Vector3(1.2f, 1.2f, 1), 0.5f));
            GetComponent<AudioSource>().PlayOneShot(stickerFly);
            yield return StartCoroutine(MoveObject(info, bag.transform.position, 0.8f));
            StartCoroutine(Scaler(bag, new Vector3(1f, 1f, 1), 0.5f));
            DarkImage(1, 1);
        }
        sorter.enabled = false;
        findGame.enabled = true;
        yield break;
    }
    public IEnumerator FinishFindGame()
    {
        LowAudio();
        GetComponent<AudioSource>().PlayOneShot(greateClip);
        yield return new WaitForSeconds(greateClip.length + 0.2f);
        
        float t = 0;
        if (isFirst && PlayerPrefs.GetInt(firstPassKey) == 0 || !isFirst && PlayerPrefs.GetInt(secondPassKey) == 0)
        {
            GameObject info = new GameObject();
            if (isFirst)
            {
                info = Instantiate(findSticker_first, new Vector3(0, -6, 0), Quaternion.identity);
            }
            else
            {
                info = Instantiate(findSticker_second, new Vector3(0, -6, 0), Quaternion.identity);
            }
            info.transform.SetParent(canvas.transform);
            info.transform.localScale = new Vector3(1, 1, 1);
            StartCoroutine(DarkImage(0.6f, 0.8f));
            GetComponent<AudioSource>().PlayOneShot(stickerGet);
            yield return StartCoroutine(MoveObject(info, Vector3.zero, 1));
            LowAudio();
            GetComponent<AudioSource>().PlayOneShot(isFirst ? clip2F : clip2S);
            yield return new WaitForSeconds(isFirst ? clip2F.length : clip2S.length + 0.2f);
            MaxAudio();
            StartCoroutine(DarkImage(0, 0.8f));
            StartCoroutine(Scaler(info, Vector3.zero, 1));
            StartCoroutine(Scaler(bag, new Vector3(1.2f, 1.2f, 1), 0.5f));
            GetComponent<AudioSource>().PlayOneShot(stickerFly);
            yield return StartCoroutine(MoveObject(info, bag.transform.position, 0.8f));
            StartCoroutine(Scaler(bag, new Vector3(1f, 1f, 1), 0.5f));
            DarkImage(1, 1);
        }
        findGame.enabled = false;
        construct.enabled = true;
        yield break;
    }
    public IEnumerator FinishConstructGame()
    {
        LowAudio();
        GetComponent<AudioSource>().PlayOneShot(greateClip);
        yield return new WaitForSeconds(greateClip.length + 0.2f);

        float t = 0;
        if (isFirst && PlayerPrefs.GetInt(firstPassKey) == 0 || !isFirst && PlayerPrefs.GetInt(secondPassKey) == 0)
        {
            GameObject info = new GameObject();
            if (isFirst)
            {
                info = Instantiate(constructorSticker_first, new Vector3(0, -6, 0), Quaternion.identity);
            }
            else
            {
                info = Instantiate(constructorSticker_second, new Vector3(0, -6, 0), Quaternion.identity);
            }
            info.transform.SetParent(canvas.transform);
            info.transform.localScale = new Vector3(1, 1, 1);
            StartCoroutine(DarkImage(0.6f, 0.8f));
            GetComponent<AudioSource>().PlayOneShot(stickerGet);
            yield return StartCoroutine(MoveObject(info, Vector3.zero, 1));
            LowAudio();
            GetComponent<AudioSource>().PlayOneShot(isFirst ? clip3F : clip3S);
            yield return new WaitForSeconds(isFirst ? clip3F.length : clip3S.length + 0.2f);
            MaxAudio();
            StartCoroutine(DarkImage(0, 0.8f));
            StartCoroutine(Scaler(info, Vector3.zero, 1));
            StartCoroutine(Scaler(bag, new Vector3(1.2f, 1.2f, 1), 0.5f));
            GetComponent<AudioSource>().PlayOneShot(stickerFly);
            yield return StartCoroutine(MoveObject(info, bag.transform.position, 0.8f));
            StartCoroutine(Scaler(bag, new Vector3(1f, 1f, 1), 0.5f));
            DarkImage(1, 1);
        }
        construct.enabled = false;
        bubble.enabled = true;
        yield break;
    }

    public IEnumerator FinalGame()
    {
        SetStickers();
        bubble.enabled = false;
        PlayerPrefs.SetInt("LastLevel", levelNumber);
        int x = PlayerPrefs.GetInt(firstPassKey);
        if (PlayerPrefs.GetInt(levelKey) == 1)
        {
            PlayerPrefs.SetInt(levelKey, 0);
            PlayerPrefs.SetInt(secondPassKey, 1);
        }else
        {
            PlayerPrefs.SetInt(levelKey, 1);
            PlayerPrefs.SetInt(firstPassKey, 1);
        }
        if (isFirstScene && x == 0)
        {
            Menu();
        }
        else
        {
            adManager.ShowInterstitial();
            yield return StartCoroutine(MoveObject(finalMenu, Vector3.zero, 1));
        }
    }
}
