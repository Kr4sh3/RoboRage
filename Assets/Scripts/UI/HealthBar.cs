using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                    transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = AssetManager.Instance.ArmorPip;
                }
                else
                {
                    transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = AssetManager.Instance.BrokenArmorPip;
                }
            }
        }
        if(maxHP == 3)
        {
            ArmorBarBackgroundRenderer.sprite = AssetManager.Instance.ArmorBarBackground[0];
        }
        else if(maxHP == 4)
        {
            ArmorBarBackgroundRenderer.sprite = AssetManager.Instance.ArmorBarBackground[1];
        }
        else if (maxHP == 5)
        {
            ArmorBarBackgroundRenderer.sprite = AssetManager.Instance.ArmorBarBackground[2];
        }
    }
}
