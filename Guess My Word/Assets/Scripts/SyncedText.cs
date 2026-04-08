using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
public class SyncedText : NetworkBehaviour
{
    private TextMeshProUGUI textMesh;

    void Start()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private NetworkVariable<FixedString64Bytes> networkText = new NetworkVariable<FixedString64Bytes>(
        "",
        NetworkVariableReadPermission.Everyone,
        NetworkVariableWritePermission.Server
    );

    public override void OnNetworkSpawn()
    {
        // S'abonner aux changements
        networkText.OnValueChanged += OnTextChanged;

        // Afficher la valeur initiale
        textMesh.text = networkText.Value.ToString();
    }

    public override void OnNetworkDespawn()
    {
        networkText.OnValueChanged -= OnTextChanged;
    }

    private void OnTextChanged(FixedString64Bytes oldValue, FixedString64Bytes newValue)
    {
        textMesh.text = newValue.ToString();
    }

    // Appelle ça depuis le serveur pour changer le texte
    public void SetText(string newText)
    {
        if (IsServer)
        {
            networkText.Value = new FixedString64Bytes(newText);
        }
    }
}
