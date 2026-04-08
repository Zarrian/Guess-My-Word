using System.Collections.Generic;
using UnityEngine;

public class ReadCSV : MonoBehaviour
{
    public TextAsset myCSV;  

    /// <summary>
    /// Read My word in my CSV
    /// </summary>
    public string ReadWord(int idLine, int idLangage)
    {
        if (myCSV == null)
        {
            Debug.LogError("Forgot CSV !");
            return null;
        }

        string[] lignes = myCSV.text.Split('\n');

        // ==================== CHANGE ICI CE QUE TU VEUX LIRE ====================
        int numeroLigne = idLine;     // ← Change ce numéro pour choisir la ligne
        int numeroColonne = idLangage;   // ← 0 = id, 1 = français (B), 2 = anglais (C), 3 = espagnol (D)
        // =========================================================================

        if (numeroLigne >= lignes.Length)
        {
            //Debug.Log("La ligne " + numeroLigne + " n'existe pas dans le fichier !");
            return null;
        }

        string ligneChoisie = lignes[numeroLigne];
        string[] cellules = ligneChoisie.Split(',');



        if (numeroColonne < cellules.Length)
        {
            string resultat = cellules[numeroColonne].Trim();
            //Debug.Log("Colonne " + numeroColonne + " - Ligne " + numeroLigne + " = " + resultat);
            return resultat;
        }
        else
        {
            //Debug.Log("La colonne " + numeroColonne + " n'existe pas !");
            return null;
        }
    }
}