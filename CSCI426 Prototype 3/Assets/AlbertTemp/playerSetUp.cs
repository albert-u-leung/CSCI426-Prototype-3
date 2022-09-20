using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class playerSetUp : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerController[] players;
    [SerializeField] private int playerIndex;
    [SerializeField] private Material[] playerMaterials;
    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private TextMeshProUGUI playerCountText;
    void Start()
    {
        players = GameObject.FindObjectsOfType<PlayerController>();
        if (players.Length == 1)
        {
            playerIndex = 1;
        }
        else if (players.Length ==2)
        {
            playerIndex = 2;
        }

        if (playerIndex == 1)
        {
            meshRenderer.material = playerMaterials[0];
            gameObject.name = "Player1";
            playerCountText.text = "P1";
        }
        else
        {
            meshRenderer.material = playerMaterials[1];
            gameObject.name = "Player2";
            playerCountText.text = "P2";
        }
        

    }
    public void ShowWinText()
    {
        playerCountText.text = "I win :D";
    }

    public void ShowLoseText()
    {
        playerCountText.text = "S**T :(";
    }
    
}
