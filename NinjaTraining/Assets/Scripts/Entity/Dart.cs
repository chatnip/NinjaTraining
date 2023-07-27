using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class Dart : MonoBehaviour
{
    [Header("*Component")]
    [SerializeField] ObjectPooling ObjectPooling;
    [SerializeField] GameManager GameManager;
    [SerializeField] DartGenerator DartGenerator;
    [SerializeField] PlayerController PlayerController;

    [Header("*etc")]
    [SerializeField] GameObject player;
    [SerializeField] Dart dart;
    [SerializeField] Rigidbody2D rigid;
    [SerializeField] public float dartSpeed;

    Vector3 newPos;
    [HideInInspector] public Vector3 dartPos;
    Coroutine dartMoveCoroutine;

    private void Awake()
    {
        GameManager.endGame
            .Where(_ => _ == true)
            .Subscribe(x =>
            {
                StopCoroutine(dartMoveCoroutine);
                ObjectPooling.DartObjectPick(dart);
            });
    }

    private void OnEnable()
    {
        this.UpdateAsObservable()
            .Select(_ => gameObject.activeSelf)
            .DistinctUntilChanged()
            .Subscribe(x =>
            {
                newPos = (dartPos - transform.position);
                newPos.Normalize();
                StartCoroutine(DartDelete());
            });

        dartMoveCoroutine = StartCoroutine(DartMove());
    }

    IEnumerator DartMove()
    {
        transform.position += newPos * dartSpeed * Time.deltaTime;
        yield return new WaitForSeconds(0.001f);
        dartMoveCoroutine = StartCoroutine(DartMove());
    }

    IEnumerator DartDelete()
    {
        yield return new WaitForSeconds(5f);
        ObjectPooling.DartObjectPick(dart);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if(PlayerController.shield.Value.activeSelf == false)
            {
                GameManager.currentHp.Value -= 1;
            }
            else if(PlayerController.shield.Value.activeSelf == true)
            {
                PlayerController.shield.Value.SetActive(false);
            }
            ObjectPooling.DartObjectPick(dart);
        }
    }
}
