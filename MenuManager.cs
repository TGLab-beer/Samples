using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Purchasing;
using System;

public class MenuManager : MonoBehaviour
{
    public AdReward reward;

    public int activeLvl, levelsCount;
    public string[] lvlName;
    public Loader ld;
    public Image[] bgs;
    public Sprite[] playButtons;
    public GameObject buttonBook, buttonPlay, closeButton, btRight, btLeft;
    public StickersManager[] stManagers;
    public bool inAlbum = false;
    public AudioSource music;
    public AudioClip stickerSoundClip;

    public GameObject lockedButton;
    public GameObject[] buyWindows;
    public bool[] lockedLevels = { false, false, false };

    public AudioClip welcome, play, stickers, book, placeSticker, good, press, more;
    public GameObject[] startGameObjects;
    public GameObject[] stickersLight;
    public GameObject startDark;
    public GameObject stickersDark;
    public GameObject bookDark;
    public GameObject videoAdIcon;
    

    public void Start()
    {
        activeLvl = PlayerPrefs.GetInt("LastLevel");
        if(activeLvl != 0)
        {
            ChangeLevelInBook(activeLvl);
        }
        if(PlayerPrefs.GetInt("FirstGame") == 0)
        {
            if(PlayerPrefs.GetInt("Level1FirstPass") == 0)
            {
                StartCoroutine(FirstStart());

            } else if(PlayerPrefs.GetInt("Training") == 0)
            {
                StartCoroutine(Training());
            }
        }
        
    }

    IEnumerator FirstStart()
    {
        StartCoroutine(ChangeAlpha(startDark.GetComponent<Image>(), 0.5f, 0.7f));
        for (int i = 0; i < startGameObjects.Length; i++)
        {
            if (startGameObjects[i].GetComponent<Button>() != null)
            {
                startGameObjects[i].GetComponent<Button>().interactable = false;
            }
            else if (startGameObjects[i].GetComponent<BoxCollider2D>() != null)
            {
                startGameObjects[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        buttonBook.GetComponent<Button>().interactable = false;
        GetComponent<AudioSource>().PlayOneShot(welcome);
        yield return new WaitForSeconds(welcome.length);
        GetComponent<AudioSource>().PlayOneShot(play);
        yield return new WaitForSeconds(play.length);
    }
    IEnumerator Training()
    {
        for(int i = 0; i < startGameObjects.Length; i++)
        {
            if (startGameObjects[i].GetComponent<Button>() != null)
            {
                startGameObjects[i].GetComponent<Button>().interactable = false;
            } else if(startGameObjects[i].GetComponent<BoxCollider2D>() != null)
            {
                startGameObjects[i].GetComponent<BoxCollider2D>().enabled = false;
            }
        }
        StartCoroutine(ChangeAlpha(stickersDark.GetComponent<Image>(), 0.5f, 0.7f));
        GetComponent<AudioSource>().PlayOneShot(stickers);
        yield return new WaitForSeconds(stickers.length);
        StartCoroutine(ChangeAlpha(stickersDark.GetComponent<Image>(), 0.5f, 0f));
        StartCoroutine(ChangeAlpha(bookDark.GetComponent<Image>(), 0.5f, 0.7f));
        GetComponent<AudioSource>().PlayOneShot(book);
        yield return new WaitForSeconds(book.length);
        
    }
    public IEnumerator ContinueTraining1()
    {
        StartCoroutine(ChangeAlpha(bookDark.GetComponent<Image>(), 0.5f, 0f));
        closeButton.GetComponent<Button>().interactable = false;
        GetComponent<AudioSource>().PlayOneShot(placeSticker);
        yield return new WaitForSeconds(placeSticker.length);
    }
        public IEnumerator ContinueTraining()
    {
        GetComponent<AudioSource>().PlayOneShot(good);
        yield return new WaitForSeconds(good.length);
        GetComponent<AudioSource>().PlayOneShot(press);
        yield return new WaitForSeconds(press.length);
        GetComponent<AudioSource>().PlayOneShot(more);
        yield return new WaitForSeconds(more.length);
        closeButton.GetComponent<Button>().interactable = true;
        PlayerPrefs.SetInt("Training", 1);
        PlayerPrefs.SetInt("FirstGame", 1);
        closeButton.GetComponent<Button>().interactable = true;
        for (int i = 0; i < startGameObjects.Length; i++)
        {
            if (startGameObjects[i].GetComponent<Button>() != null)
            {
                startGameObjects[i].GetComponent<Button>().interactable = true;
            }
            else if (startGameObjects[i].GetComponent<BoxCollider2D>() != null)
            {
                startGameObjects[i].GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
    public void StickerSound()
    {
        GetComponent<AudioSource>().PlayOneShot(stickerSoundClip);
    }
    IEnumerator ChangeAlpha(Image obj, float time, float alpha)
    {
        float t = 0;
        Color clr = obj.color;
        clr.a = alpha;
        while (t < time)
        {
            obj.color = Color.Lerp(obj.color, clr, t / time);
            t += Time.deltaTime;
            yield return null;
        }
        obj.color = clr;

    }
    public void BuyLevel(int level)
    {
        lockedLevels[level] = true;
        ChangeLevelInBook(activeLvl);
    }
    public void StartGame()
    {
        if (activeLvl == 1)
            reward.ShowRewardedAd();
        else
        ld.StartCoroutine(ld.TranslateToNextLevel(lvlName[activeLvl]));
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
        while (t < time)
        {
            music.volume = Mathf.Lerp(music.volume, value, t / time);
            t += Time.deltaTime;
            yield return null;
        }
    }
    public void OpenStickers()
    {
        inAlbum = true;
        stManagers[activeLvl].gameObject.SetActive(true);
        stManagers[activeLvl].Activate();
        StartCoroutine(ScalerToZero(Vector3.zero, 0.6f, btRight));
        StartCoroutine(ScalerToZero(Vector3.zero, 0.6f, btLeft));
        StartCoroutine(ScalerToZero(Vector3.zero, 0.6f, buttonBook));
        StartCoroutine(ScalerToZero(Vector3.zero, 0.6f, buttonPlay));
        StartCoroutine(ScalerToOne(Vector3.one, 0.6f, closeButton));
    }
    public void CloseStickers()
    {
        inAlbum = false;
        stManagers[activeLvl].Deactivate();
        StartCoroutine(ScalerToOne(Vector3.one, 0.6f, buttonBook));
        StartCoroutine(ScalerToOne(Vector3.one, 0.6f, buttonPlay));
        StartCoroutine(ScalerToOne(Vector3.one, 0.6f, btRight));
        StartCoroutine(ScalerToOne(Vector3.one, 0.6f, btLeft));
        StartCoroutine(ScalerToZero(Vector3.zero, 0.6f, closeButton));
    }
    void ChangeLevelInBook(int number)
    {
        lockedButton.SetActive(false);
        videoAdIcon.SetActive(false);
        for (int i = 0; i < bgs.Length; i++)
        {
            bgs[i].gameObject.SetActive(false);
        }
        bgs[number].gameObject.SetActive(true);
        if(activeLvl == 1)
        {
            videoAdIcon.SetActive(true);
        }
        buttonPlay.GetComponent<Image>().sprite = playButtons[number];
    }
    public void NextLvl()
    {
        if (inAlbum == false)
        {
            activeLvl += 1;
            if (activeLvl > levelsCount)
                activeLvl = 0;
            ChangeLevelInBook(activeLvl);
        }
    }
    public void PreviousLvl()
    {
        if (inAlbum == false)
        {
            activeLvl -= 1;
            if (activeLvl < 0)
            {
                activeLvl = levelsCount;
            }
            ChangeLevelInBook(activeLvl);
        }
    }
    IEnumerator ScalerToOne(Vector3 newScale, float time, GameObject obj)
    {
        float t = 0;
        while(t < time)
        {
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, newScale, t / time);
            t += Time.deltaTime;
            yield return null;
        }
        obj.transform.localScale = newScale;
        if (obj.GetComponent<Button>() != null)
        {
            obj.GetComponent<Button>().interactable = true;
        }
    }
    IEnumerator ScalerToZero(Vector3 newScale, float time, GameObject obj)
    {
        if (obj.GetComponent<Button>() != null)
        {
            obj.GetComponent<Button>().interactable = false;
        }
        float t = 0;
        while (t < time)
        {
            obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, newScale, t / time);
            t += Time.deltaTime;
            yield return null;
        }
        obj.transform.localScale = newScale;
        
    }
}
