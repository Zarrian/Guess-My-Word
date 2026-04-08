using System.Collections.Generic;
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
    public List<PlayerScripts> playerLists;
    public List<PlayerScripts> playerWaittingForValidation;

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
        if (playerWaittingForValidation.Contains(PlayerValid) == false)
        {
            playerWaittingForValidation.Add(PlayerValid);
        }

        //If all players has valid their answer...
        if (playerLists.Count == playerWaittingForValidation.Count)
        {
            ManagerQuizGame.instance.EndRound();
        }
    }

    public void NextRound()
    {
        playerWaittingForValidation.Clear();
    }

    public void EndRound()
    {

    }


}
