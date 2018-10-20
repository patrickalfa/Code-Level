using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum GAME_STATE
{
    START,
    INTRO,
    GAME,
    HELP
}

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Version[] _versions;
    public Label _labelVersion;
    public GAME_STATE currentState;

    [Header("Screens")]
    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject introScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject helpScreen;
    [SerializeField] private GameObject[] helpPages;

    private GAME_STATE lateState;
    private int currentVersion;
    private int currentHelpPage;

    /////////////////////////////////////////////////////////

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        lateState = currentState;
        currentVersion = 0;
    }

    private void Update()
    {
        OnStateUpdate();

        if (currentState != lateState)
            OnStateChanged();

        lateState = currentState;
    }

    /////////////////////////////////////////////////////////

    public void ScreenShake(float time, float strength, int vibrato)
    {
        Camera.main.DOShakePosition(time, strength, vibrato).SetUpdate(true);
    }

    public void FreezeFrame(float time)
    {
        Time.timeScale = 0f;

        transform.DOMove(Vector3.zero, time).SetUpdate(true).OnComplete(() =>
        {
            Time.timeScale = 1f;
        });
    }

    public void NextVersion()
    {
        _versions[currentVersion].gameObject.SetActive(false);

        Commander.instance.ClearTiles();

        currentVersion++;
        if (currentVersion < _versions.Length)
        {
            if (currentVersion == 4)
                SoundManager.SetChannelVolume("Arpeggio", 1f);
            else if (currentVersion == 7)
                SoundManager.SetChannelVolume("Lead", 1f);
            else if (currentVersion == 9)
                SoundManager.SetChannelVolume("Bass2", .75f);
            else if (currentVersion == _versions.Length - 1)
                SoundManager.PlaySound("win");


            _versions[currentVersion].gameObject.SetActive(true);
            _labelVersion.text = _versions[currentVersion].name;
        }
        else
            Debug.Break(); // DEBUG
    }

    private void OnStateUpdate()
    {
        switch (currentState)
        {
            case GAME_STATE.START:
                if (Input.GetKeyUp(KeyCode.Return))
                    currentState = GAME_STATE.INTRO;
                break;
            case GAME_STATE.INTRO:
                if (Input.GetKeyUp(KeyCode.Return))
                {
                    Writer writer = FindObjectOfType<Writer>();
                    if (writer.finished)
                        currentState = GAME_STATE.GAME;
                    else
                        writer.Skip();
                }
                break;
            case GAME_STATE.GAME:
                if (Input.GetKeyUp(KeyCode.F1))
                    currentState = GAME_STATE.HELP;
                break;
            case GAME_STATE.HELP:
                if (Input.GetKeyUp(KeyCode.Return))
                    ChangeHelpPage();
                break;
        }
    }

    private void OnStateChanged()
    {
        GameManager.instance.ScreenShake(.1f, 1f, 50);

        switch (currentState)
        {
            case GAME_STATE.START:
                startScreen.SetActive(true);
                introScreen.SetActive(false);
                gameScreen.SetActive(false);
                helpScreen.SetActive(false);
                break;
            case GAME_STATE.INTRO:
                startScreen.SetActive(false);
                introScreen.SetActive(true);
                gameScreen.SetActive(false);
                helpScreen.SetActive(false);
                break;
            case GAME_STATE.GAME:
                startScreen.SetActive(false);
                introScreen.SetActive(false);
                gameScreen.SetActive(true);
                helpScreen.SetActive(false);
                SoundManager.SetChannelVolume("Snare", 1f);
                break;
            case GAME_STATE.HELP:
                startScreen.SetActive(false);
                introScreen.SetActive(false);
                gameScreen.SetActive(false);
                helpScreen.SetActive(true);
                SoundManager.SetChannelVolume("Snare", 0f);
                break;
        }

        SoundManager.PlaySound("button");
    }

    private void ChangeHelpPage()
    {
        if (currentHelpPage == helpPages.Length - 1)
        {
            currentHelpPage = 0;
            currentState = GAME_STATE.GAME;
        }
        else
        {
            currentHelpPage++;
            GameManager.instance.ScreenShake(.1f, 1f, 50);
            SoundManager.PlaySound("button");
        }

        for (int i = 0; i < helpPages.Length; i++)
            helpPages[i].SetActive(i == currentHelpPage);
    }
}