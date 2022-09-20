using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDownTimer : MonoBehaviour
{
    public float timeStart = 60;
    public TextMeshProUGUI timerUI;

    // Start is called before the first frame update
    void Start()
    {
        timerUI.text = timeStart.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        timeStart -= Time.deltaTime;
        timerUI.text = Mathf.Round(timeStart).ToString();
        if (timeStart <= 0)
        {
            timeStart = 0;
        }
    }
}
