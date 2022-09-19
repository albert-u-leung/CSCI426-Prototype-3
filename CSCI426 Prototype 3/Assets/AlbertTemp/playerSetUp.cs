using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSetUp : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private PlayerController[] players;
    [SerializeField] private int playerIndex;
    [SerializeField] private Material[] playerMaterials;
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
            gameObject.GetComponent<Renderer>().material = playerMaterials[0];
            gameObject.name = "Player1";
        }
        else
        {
            gameObject.GetComponent<Renderer>().material = playerMaterials[1];
            gameObject.name = "Player2";
        }
    }
    
}
