using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

public class GameManager : MonoBehaviour
{
    [Header("*Component")]
    [SerializeField] ChapterManager ChapterManager;
    [SerializeField] DartGenerator DartGenerator;
    [SerializeField] ItemGenerator ItemGenerator;

    [Header("*UI")]
    [SerializeField] Text playTimeText;
    [SerializeField] List<Image> hartImageList = new List<Image>();
    [SerializeField] int maxHp;
    [SerializeField] Image backgroundImage;
    [SerializeField] Button backButton;

    [Header("*ResultUI")]
    [SerializeField] GameObject resultWindow;
    [SerializeField] Text resultText;
    [SerializeField] Text resultTimeText;
    [SerializeField] Button stageSelectButton;
    [SerializeField] Button retryButton;

    [Header("*etc")]
    [SerializeField] GameObject LobbyScreen;
    [SerializeField] GameObject GameScreen;
    [SerializeField] GameObject player;
    [HideInInspector] public IntReactiveProperty currentHp = new IntReactiveProperty();
    [HideInInspector] public BoolReactiveProperty endGame = new BoolReactiveProperty();

    public float currentTime;
    Result result;

    Coroutine TimeCoroutine;

    private void Awake()
    {
        LobbyScreen.SetActive(true);
        GameScreen.SetActive(false);

        backButton
            .OnClickAsObservable()
            .Subscribe(x =>
            {
                SceneManager.LoadScene(0);
            });

        currentHp
            .Subscribe(hp =>
            {
                if (hp >= 3)
                {
                    currentHp.Value = 3;
                }
                for (int i = 0; i < hartImageList.Count; i++)
                {
                    if(i < hp)
                    {
                        hartImageList[i].color = Color.red;
                    }
                    else
                    {
                        hartImageList[i].color = Color.gray;

                    }
                }   
                if (GameScreen.activeSelf == true)
                {
                    if (hp <= 0)
                    {
                        result = Result.Lose;
                        endGame.Value = true;
                        GameResult();
                    }
                }
            });

        ChapterManager.difficulty
            .Subscribe(x =>
            {
                switch (x)
                {
                    case Difficulty.Easy:
                        backgroundImage.color = new Color(132 / 255f, 205 / 255f, 194 / 255f, 255 / 255f);
                        break;
                    case Difficulty.Normal:
                        backgroundImage.color = new Color(254 / 255f, 242 / 255f, 216 / 255f, 255 / 255f);
                        break;
                    case Difficulty.Hard:
                        backgroundImage.color = new Color(255 / 255f, 186 / 255f, 160 / 255f, 255 / 255f);
                        break;
                    default:
                        break;
                }
            });

        stageSelectButton
            .OnClickAsObservable()
            .Subscribe(x =>
            {
                LobbyScreen.SetActive(true);
                GameScreen.SetActive(false);
            });

        retryButton
            .OnClickAsObservable()
            .Subscribe(x =>
            {
                GameSetting();
            });
    }

    public void GameSetting()
    {
        player.transform.position = new Vector3(0, 0, 0);
        player.transform.localScale = new Vector3(1, 1, 1);

        endGame.Value = false;
        LobbyScreen.SetActive(false);
        GameScreen.SetActive(true);

        currentTime = 0;
        TimeCoroutine = StartCoroutine(StartTimer());

        currentHp.Value = maxHp;
        resultWindow.SetActive(false);

        DartGenerator.DartCreate();
        StartCoroutine(ItemGenerator.ItemCreate());
    }

    private void GameResult()
    {
        if (result == Result.Lose)
        {
            resultText.text = "GameOver";
        }
        if (result == Result.Win)
        {
            resultText.text = "Win!";
        }
        StopFunction();
        ResultWindowSetting();
    }

    private void ResultWindowSetting()
    {
        resultWindow.SetActive(true);
        StopCoroutine(TimeCoroutine);
        resultTimeText.text = playTimeText.text;
    }

    private void StopFunction()
    {
        DartGenerator.StopAllCoroutines();
        ItemGenerator.StopAllCoroutines();
        StopAllCoroutines();
        // 풀에 돌려보내기

    }

    private IEnumerator StartTimer()
    {
        if (currentTime >= 60f)
        {
            result = Result.Win;
            endGame.Value = true;
            GameResult();
        }
        yield return new WaitForSeconds(0.01f);
        currentTime += 0.01f;
        playTimeText.text = "Time " + currentTime.ToString("F2");
        TimeCoroutine = StartCoroutine(StartTimer());
    }
}

[System.Serializable]
public enum Result
{
    Win,
    Lose
}

// 점점 실제 시간보다 느려짐.
/*
        Observable
            .Interval(TimeSpan.FromMilliseconds(10))
            .TakeWhile(x => x < 6000)
            .Subscribe(time =>
            {
                currentTime.Value += 0.01f;
            }).AddTo(this);

        yield return null;
*/

// win 상태일 때 StopCoroutine이 작동하지 않음.
/*
currentTime
    .Subscribe(time =>
    {
        playTimeText.text = "Time " + time.ToString("F2");
    });

currentTime
    .Where(_ => _ >= 5f)
    .Subscribe(time =>
    {
        if (currentHp.Value > 0)
        {
            result = Result.Win;
            GameResult();
        }
    });
*/