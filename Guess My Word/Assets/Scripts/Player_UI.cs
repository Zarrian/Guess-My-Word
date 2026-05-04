using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class Player_UI : MonoBehaviour
{
    public PlayerScripts player;

    private void Start()
    {
        print("player");
        player.OnRoundStart += RoundStart;
        player.OnRoundEnd += RoundEnd;
        player.OnGoodAnswer += GoodAnswer;
        player.OnWrongAnswer += WrongAnswer;
    }


    [Rpc(SendTo.Server)]
    public void MovePlayerUIRpc(GuessButtons AnswerSelectedButton)
    {
        if (AnswerSelectedButton != null)
        {
            //Test de just déplacer les positions
            transform.position = AnswerSelectedButton.transform.position;
        }
    }

    public void RoundStart()
    {
        transform.position = Vector3.one * 1000;
    }

    public void RoundEnd()
    {

    }

    public void GoodAnswer()
    {
        GetComponent<Image>().color = Color.green;
    }

    public void WrongAnswer()
    {
        GetComponent<Image>().color = Color.red;
    }


}
