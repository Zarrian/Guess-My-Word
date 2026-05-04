using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;


public class ManagerGameWords : MonoBehaviour
{
    public TextMeshProUGUI mainBoard;

    public List<GuessButtons> listButtons;

    //public NetworkVariable<NetworkObjectReference> goodAnswer;
    //public GameObject goodAnswer;

    public static ManagerGameWords instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        ManagerQuizGame.instance.OnNewRound += NewRound;
    }


    [Rpc(SendTo.Server)]
    public void NewRound()
    {
        print("test print si dans les deux cas se print ou po");
        mainBoard.GetComponent<SyncedText>().SetText(ManagerQuizGame.instance.WordDisplay.ToString());

        int randomButton = Random.Range(0, listButtons.Count);
        for (int i = 0; i < listButtons.Count; i++)
        {
            if (i != randomButton)
                listButtons[i].SetBadAnswer();
            else
            {
                listButtons[i].SetGoodAnswer();
                //goodAnswer.Value = new NetworkObjectReference(listButtons[i].GetComponent<NetworkObject>());
            }

        }
    }
}
