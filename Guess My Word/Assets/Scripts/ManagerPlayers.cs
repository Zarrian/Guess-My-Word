using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class ManagerPlayers : MonoBehaviour
{

    public static ManagerPlayers instance;

    private void Start()
    {
        ManagerQuizGame.instance.OnNewRound += NextRound;
        ManagerQuizGame.instance.OnEndRound += EndRound;
    }

    [Header("Players")]
    public NetworkVariable<List<PlayerScripts>> playerLists;
    public NetworkVariable<List<PlayerScripts>> playerWaittingForValidation;


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }


    public void PlayerValidAnswer(PlayerScripts PlayerValid)
    {
        //Comfirm one of the players valid his answer
        if (playerWaittingForValidation.Value.Contains(PlayerValid) == false)
        {
            playerWaittingForValidation.Value.Add(PlayerValid);
        }

        //If all players has valid their answer...
        if (playerLists.Value.Count == playerWaittingForValidation.Value.Count)
        {
            ManagerQuizGame.instance.EndRound();
        }
    }


    public void NextRound()
    {
        playerWaittingForValidation.Value.Clear();
    }

    public void EndRound()
    {

    }


}
