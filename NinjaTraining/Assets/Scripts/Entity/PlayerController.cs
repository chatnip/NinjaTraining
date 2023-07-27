using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using UniRx.Triggers;
using DG.Tweening;

public class PlayerController : MonoBehaviour
{
    [Header("*Move")]
    [SerializeField] float moveSpeed;
    [SerializeField] Transform point;

    [HideInInspector] public float itemSpeed = 0;
    [HideInInspector] public bool isBluePotion;

    [HideInInspector] public Coroutine playerSizeCoroutine;
    [HideInInspector] public Coroutine useShoesCoroutine;

    public ReactiveProperty<GameObject> shield = new ReactiveProperty<GameObject>();

    Vector2 moveValue;

    private void Awake()
    {
        point.parent = null;
        shield.Value.SetActive(false);

        this.UpdateAsObservable()
            .Select(_ => transform.position)
            .Subscribe(x =>
            {
                playerMove();
            });

        shield
            .Where(_ => _.activeSelf == true)
            .Delay(TimeSpan.FromSeconds(5f))
            .Subscribe(x =>
            {
                Debug.Log("ÁßÁö");
                x.SetActive(false);
            });
    }

    public void Input(InputAction.CallbackContext context)
    {
        moveValue = context.ReadValue<Vector2>();
    }

    public void playerMove()
    {
        // transform.Translate(moveValue * (moveSpeed + itemSpeed) * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, point.position, (moveSpeed + itemSpeed) * Time.deltaTime);
        if(Vector3.Distance(transform.position, point.position) <= 0.10f)
        {
            if (Mathf.Abs(moveValue.x) == 1)
            {
                point.position += new Vector3(moveValue.x * 0.2f, 0, 0f);
            }       
            else if(Mathf.Abs(moveValue.y) == 1)
            {
                point.position += new Vector3(0, moveValue.y * 0.2f, 0f);
            }
            else if (Mathf.Abs(Mathf.Round(moveValue.x)) == 1 && Mathf.Abs(Mathf.Round(moveValue.y)) == 1)
            {
                point.position += new Vector3(Mathf.Round(moveValue.x) * 0.2f, Mathf.Round(moveValue.y) * 0.2f, 0f);
            }
        }
    }

    public IEnumerator PlayerSize()
    {
        if (isBluePotion == true)
        {
            transform.DOScale(0.25f, 5f);
            transform.DOScale(1f, 5f).SetDelay(10f);
            yield return null;
        }
        else
        {
            transform.DOScale(2f, 5f);
            transform.DOScale(1f, 5f).SetDelay(10f);
            yield return null;
        }
    }

    public IEnumerator UseShoes()
    {
        itemSpeed = 2;
        yield return new WaitForSeconds(5f);
        itemSpeed = 0;
        Debug.Log(itemSpeed);
    }
}
