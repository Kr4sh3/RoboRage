using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Obsolete]
public class HealthBar : MonoBehaviour
{

    public SpriteRenderer ArmorBarBackgroundRenderer;

    public void UpdateHealth(int hp, int maxHP)
    {
        for(int i = transform.childCount - 1; i > 0; i--)
        {
            if(maxHP < i + 1)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
            else
            {
                transform.GetChild(i).gameObject.SetActive(true);
                if (hp > i)
                {
                    transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = GameManager.Instance.AssetManager.ArmorPip;
                }
                else
                {
                    transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = GameManager.Instance.AssetManager.BrokenArmorPip;
                }
            }
        }
        if(maxHP == 3)
        {
            ArmorBarBackgroundRenderer.sprite = GameManager.Instance.AssetManager.ArmorBarBackground[0];
        }
        else if(maxHP == 4)
        {
            ArmorBarBackgroundRenderer.sprite = GameManager.Instance.AssetManager.ArmorBarBackground[1];
        }
        else if (maxHP == 5)
        {
            ArmorBarBackgroundRenderer.sprite = GameManager.Instance.AssetManager.ArmorBarBackground[2];
        }
    }
}
