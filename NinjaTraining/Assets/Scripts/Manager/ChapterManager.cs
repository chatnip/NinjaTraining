using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class ChapterManager : MonoBehaviour
{
    [Header("*Component")]
    [SerializeField] GameManager GameManager;

    [Header("*Button")]
    [SerializeField] public List<Button> ChapterSelectButtons = new List<Button>();

    [HideInInspector] public ReactiveProperty<Difficulty> difficulty = new ReactiveProperty<Difficulty>();

    private void Awake()
    {
        foreach (Button Chapter in ChapterSelectButtons)
        {
            Chapter
                .OnClickAsObservable()
                .Select(ChapterNum => Chapter.transform.GetSiblingIndex())
                .Subscribe(ChapterNum =>
                {
                    switch(ChapterNum)
                    {
                        case 0:
                            difficulty.Value = Difficulty.Easy;
                            break;
                        case 1:
                            difficulty.Value = Difficulty.Normal;
                            break;
                        case 2:
                            difficulty.Value = Difficulty.Hard;
                            break;
                    }
                    GameManager.GameSetting();
                });
        }
    }
}

[System.Serializable]
public enum Difficulty
{
    Easy,
    Normal,
    Hard
}
