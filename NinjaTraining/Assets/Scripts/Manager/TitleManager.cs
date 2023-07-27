using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UniRx;

public class TitleManager : MonoBehaviour
{
    [Header("*Button")]
    [SerializeField] Button startButton;
    [SerializeField] Button endButton;

    private void Awake()
    {
        startButton
            .OnClickAsObservable()
            .Subscribe(x =>
            {
                SceneManager.LoadScene(1);
            });

        endButton
            .OnClickAsObservable()
            .Subscribe(x =>
            {
                Application.Quit();
            });
    }
}
