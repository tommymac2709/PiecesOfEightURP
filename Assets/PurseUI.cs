using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PurseUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI balanceField;

    Purse playerPurse = null;

    // Start is called before the first frame update
    void Start()
    {
        playerPurse = GameObject.FindGameObjectWithTag("Player").GetComponent<Purse>();

        if (playerPurse != null )
        {
            playerPurse.onChange += RefreshUI;
        }

        RefreshUI();
    }

    private void RefreshUI()
    {
        balanceField.text = $"${playerPurse.GetBalance():N2}";
    }
}
