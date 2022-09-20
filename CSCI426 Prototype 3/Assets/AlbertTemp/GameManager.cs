using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int countDownTime = 60;
    [SerializeField] private TextMeshProUGUI countDownTimeText;
    [SerializeField] private PlayerController[] players;
    [SerializeField] private BallFireMachine[] ballFireMachines;
    [SerializeField] private MMFeedbacks gameOverFeedbacks;
    private bool hasStarted;
    public bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        ballFireMachines = FindObjectsOfType<BallFireMachine>();
    }

    // Update is called once per frame
    void Update()
    {
        players = FindObjectsOfType<PlayerController>();
  

        if (players.Length == 2)
        {
            if (!hasStarted)
            {
                StartCoroutine(CountDownTimer());
                foreach (var ballFireMachine in ballFireMachines)
                {
                    ballFireMachine.enabled = true;
                }
                
                hasStarted = true;
            }
            
        }
        
        countDownTimeText.text = countDownTime.ToString();
        
        if (countDownTime == 0)
        {
            gameOver = true;
            gameOverFeedbacks.PlayFeedbacks();
            Debug.Log("game end");
            if (players[0].GetComponent<PlayerController>().hitCount <
                players[1].GetComponent<PlayerController>().hitCount)
            {
                players[1].GetComponent<playerSetUp>().ShowWinText();
                players[0].GetComponent<playerSetUp>().ShowLoseText();
            }
            if (players[0].GetComponent<PlayerController>().hitCount >
                players[1].GetComponent<PlayerController>().hitCount)
            {
                players[0].GetComponent<playerSetUp>().ShowWinText();
                players[1].GetComponent<playerSetUp>().ShowLoseText();
            }
        }
    }

    IEnumerator CountDownTimer()
    {
        yield return new WaitForSeconds(1f);
        if (countDownTime > 0)
        {
            countDownTime--;
            StartCoroutine(CountDownTimer());
        }
    }
}
