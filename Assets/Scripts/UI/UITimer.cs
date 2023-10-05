using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITimer : MonoBehaviour
{

    public bool maxTimer;
    public float xpos1, xpos2, xpos3;
    HealthController playerHealth;

    private void Start()
    {
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth.GetMaxHealth() == 3)
        {
            transform.position = new Vector3(xpos1, transform.position.y, transform.position.z);
        }
        else if (playerHealth.GetMaxHealth() == 4)
        {
            transform.position = new Vector3(xpos2, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(xpos3, transform.position.y, transform.position.z);
        }

        float time;
        if (maxTimer)
        {
            if (PlayerPrefs.GetInt("HasBeatenGame") != 1)
            {
                GetComponent<Text>().text = "";
            }
            else
            {
                time = UIManager.Instance.GetMaxTime();
                int seconds = (int)time % 60;
                int minutes = (int)time / 60;
                time *= 1000;
                int milliseconds = (int)time % 1000;
                GetComponent<Text>().text = "Best Time: " + string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            }

        }
        else
        {

            if (PlayerPrefs.GetInt("HasBeatenGame") != 1)
            {
                GetComponent<Text>().text = "";
            }
            else
            {
                time = UIManager.Instance.GetTime();
                int seconds = ((int)time % 60);
                int minutes = ((int)time / 60);
                time *= 1000;
                int milliseconds = (int)time % 1000;
                GetComponent<Text>().text = "Time: " + string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
            }
        }
    }
}
