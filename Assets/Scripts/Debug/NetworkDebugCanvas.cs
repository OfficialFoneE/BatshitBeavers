using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NetworkDebugCanvas : MonoBehaviour
{

    private static TextMeshProUGUI connectionStatusText;
    private static TextMeshProUGUI connectingStatusText;

    private void Awake()
    {
        var textMeshProUGUIs = GetComponentsInChildren<TextMeshProUGUI>();
        connectionStatusText = textMeshProUGUIs[0];
        connectingStatusText = textMeshProUGUIs[1];
    }

    public static void SetConnectionStatus(string status)
    {
        //connectionStatusText.text = "Connection Status : <color=green>" + status;
    }
    public static void SetConnectingStatus(string status)
    {
        //connectingStatusText.text = "Connecting Status : <color=green>" + status;
    }
}
