using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Item : MonoBehaviour
{
    [Header("*Component")]
    [SerializeField] GameManager GameManager;
    [SerializeField] DartGenerator DartGenerator;
    [SerializeField] ItemGenerator ItemGenerator;
    [SerializeField] PlayerController PlayerController;
    [SerializeField] ObjectPooling ObjectPooling;

    [Header("*Item")]
    [SerializeField] Item item;
    [SerializeField] public SpriteRenderer itemSprite;

    [HideInInspector] public Effect effect;
    Coroutine itemEffectCoroutine;

    private void Awake()
    {
        GameManager.endGame
            .Where(_ => _ == true)
            .Subscribe(x =>
            {
                ObjectPooling.ItemObjectPick(item);
            });
    }

    private void OnDisable()
    {
        switch (effect)
        {
            case Effect.SmokehSell:
                DartGenerator.StopCoroutine(DartGenerator.useSmokehSellCoroutine);
                break;
            case Effect.BluePotion:
                PlayerController.StopCoroutine(PlayerController.playerSizeCoroutine);
                break;
            case Effect.RedPotion:
                PlayerController.StopCoroutine(PlayerController.playerSizeCoroutine);
                break;
            case Effect.Shoes:
                PlayerController.StopCoroutine(PlayerController.useShoesCoroutine);
                break;
            default:
                break;
        }
    }

    private IEnumerator ItemEffect()
    {
        switch(effect)
        {
            case Effect.SmokehSell:
                Debug.Log("SmokehSell");
                DartGenerator.useSmokehSellCoroutine = DartGenerator.StartCoroutine(DartGenerator.UseSmokehSell());
                break;
            case Effect.BluePotion:
                Debug.Log("BluePotion");
                PlayerController.isBluePotion = true;
                PlayerController.playerSizeCoroutine = PlayerController.StartCoroutine(PlayerController.PlayerSize());
                break;
            case Effect.RedPotion:
                Debug.Log("RedPotion");
                PlayerController.isBluePotion = false;
                PlayerController.playerSizeCoroutine = PlayerController.StartCoroutine(PlayerController.PlayerSize());
                break;
            case Effect.Shield:
                Debug.Log("Shield");
                PlayerController.shield.Value.SetActive(true);
                break;
            case Effect.Heart:
                Debug.Log("Heart");
                ++GameManager.currentHp.Value;
                break;
            case Effect.Shoes:
                Debug.Log("Shoes");
                PlayerController.useShoesCoroutine = PlayerController.StartCoroutine(PlayerController.UseShoes());
                break;
        }
        yield return null;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            itemEffectCoroutine = StartCoroutine(ItemEffect());
            ObjectPooling.ItemObjectPick(item);
        }
    }
}