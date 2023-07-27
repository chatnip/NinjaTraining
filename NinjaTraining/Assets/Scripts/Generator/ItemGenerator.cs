using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [Header("*Component")]
    [SerializeField] ObjectPooling ObjectPooling;

    private Item RandomItem()
    {
        Item item = ObjectPooling.ItemObjectPool();
        int randomNum = Random.Range(0, 6);
        item.effect = (Effect)4;
        switch (item.effect)
        {
            case Effect.SmokehSell:
                item.itemSprite.color = Color.gray;
                break;
            case Effect.BluePotion:
                item.itemSprite.color = Color.blue;
                break;
            case Effect.RedPotion:
                item.itemSprite.color = Color.red;
                break;
            case Effect.Shield:
                item.itemSprite.color = Color.cyan;
                break;
            case Effect.Heart:
                item.itemSprite.color = Color.magenta;
                break;
            case Effect.Shoes:
                item.itemSprite.color = Color.yellow;
                break;
            default:
                break;
        }
        return item;
    }
    public IEnumerator ItemCreate()
    {
        for(int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(13f);
            float x = Random.Range(-7f, 7f);
            float y = Random.Range(-3f, 3f);
            RandomItem().transform.position = new Vector3(x, y, 0);
        }
    }
}

[System.Serializable]
public enum Effect
{
    SmokehSell,
    BluePotion,
    RedPotion,
    Shield,
    Heart,
    Shoes
}
