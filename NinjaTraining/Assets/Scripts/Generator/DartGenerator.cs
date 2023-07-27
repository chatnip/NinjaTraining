using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class DartGenerator : MonoBehaviour
{
    [Header("*Component")]
    [SerializeField] ObjectPooling ObjectPooling;
    [SerializeField] ChapterManager ChapterManager;

    [Header("*Player")]
    [SerializeField] GameObject player;
    [HideInInspector] public bool isSmokehSell = false;

    int pi;

    Coroutine easyPatternCoroutine;
    Coroutine normalPatternCoroutine;
    Coroutine hardPatternCoroutine;
    Coroutine dartVectorCoroutine;
    [HideInInspector] public Coroutine useSmokehSellCoroutine;

    public void DartCreate()
    {
        switch (ChapterManager.difficulty.Value)
        {
            case Difficulty.Easy:
                easyPatternCoroutine = StartCoroutine(EasyPattern());
                break;

            case Difficulty.Normal:
                easyPatternCoroutine = StartCoroutine(EasyPattern());
                normalPatternCoroutine = StartCoroutine(NormalPattern());
                break;

            case Difficulty.Hard:
                easyPatternCoroutine = StartCoroutine(EasyPattern());
                normalPatternCoroutine = StartCoroutine(NormalPattern());
                hardPatternCoroutine = StartCoroutine(HardPattern());
                break;

            default:
                break;
        }
    }

    private IEnumerator EasyPattern()
    {
        for (int i = 0; i < 200; i++)
        {
            int selectGenerator = Random.Range(0, 4);
            float dartPos = Random.Range(-10f, 10f);
            Dart dart = ObjectPooling.DartObjectPool();
            dartVectorCoroutine = StartCoroutine(DartVector(dart));
            switch (selectGenerator)
            {
                case 0:
                    dart.transform.position = new Vector3(dartPos, 5, 0);
                    break;
                case 1:
                    dart.transform.position = new Vector3(dartPos, -5, 0);
                    break;
                case 2:
                    dart.transform.position = new Vector3(5, dartPos, 0);
                    break;
                case 3:
                    dart.transform.position = new Vector3(-5, dartPos, 0);
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(1f);
        }
    }

    private IEnumerator NormalPattern()
    {
        yield return new WaitForSeconds(5f);
        for (int i = 0; i < 110; i++)
        {
            pi += 30;
            Dart dart = ObjectPooling.DartObjectPool();
            var rad = Mathf.Deg2Rad * pi;
            var x = Mathf.Sin(rad);
            var y = Mathf.Cos(rad);
            dart.transform.position = new Vector3(x * 5, y * 5, 0);

            if (pi > 360)
            {
                pi = 0;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    private IEnumerator HardPattern()
    {
        yield return new WaitForSeconds(10f);
        for (int i = 0; i < 100; i++)
        {
            int rand = Random.Range(0, 6);
            Dart dart = ObjectPooling.DartObjectPool();

            switch (rand)
            {
                case 0:
                    dart.transform.position = new Vector3(-5, 5, 0);
                    break;
                case 1:
                    dart.transform.position = new Vector3(0, 5, 0);
                    break;
                case 2:
                    dart.transform.position = new Vector3(5, 5, 0);
                    break;
                case 3:
                    dart.transform.position = new Vector3(-5, -5, 0);
                    break;
                case 4:
                    dart.transform.position = new Vector3(0, -5, 0);
                    break;
                case 5:
                    dart.transform.position = new Vector3(5, -5, 0);
                    break;
                default:
                    break;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator DartVector(Dart dart)
    {
        if (isSmokehSell == true)
        {
            dart.dartPos = new Vector3(0, 0, 0);
            yield return new WaitForSeconds(5f);
            dart.dartPos = player.transform.position;
        }
        else
        {
            Debug.Log("원래대로");
            dart.dartPos = player.transform.position;
        }
    }

    public IEnumerator UseSmokehSell()
    {
        isSmokehSell = true;
        yield return new WaitForSeconds(5f);
        isSmokehSell = false;
    }
}
