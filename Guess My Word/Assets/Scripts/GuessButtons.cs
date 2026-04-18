using System.Runtime.CompilerServices;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(NetworkObject))]
public class GuessButtons : MonoBehaviour
{

    public ReadCSV myCSV;

    public NetworkVariable<string> word;
    public bool isGoodAnswer = false;
    public SyncedText sync;


    public void SetGoodAnswer()
    {
        isGoodAnswer = true;
        sync.SetText(ManagerQuizGame.instance.WordToGuess.ToString());

        //word.Value = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }

    public void SetBadAnswer()
    {
        isGoodAnswer = false;

        string wrongWord = myCSV.ReadWord(Random.Range(0, ManagerQuizGame.instance.nbWord) , ManagerQuizGame.instance.langueToGuess);
        sync.SetText(wrongWord);

        //word.Value = transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text;
    }
}
