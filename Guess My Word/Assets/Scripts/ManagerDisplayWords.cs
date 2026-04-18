using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ManagerDisplayWords : MonoBehaviour
{
    public TextMeshProUGUI mainBoard;

    public List<GuessButtons> listButtons;

    public static ManagerDisplayWords instance;

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


    public void NewRound()
    {
        mainBoard.GetComponent<SyncedText>().SetText(ManagerQuizGame.instance.WordDisplay.ToString());


        int randomButton = Random.Range(0, listButtons.Count);
        for (int i = 0; i < listButtons.Count; i++)
        {
            if (i != randomButton)
                listButtons[i].SetBadAnswer();
            else
                listButtons[i].SetGoodAnswer();

        }
    }
}
