using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using kcp2k;

public class MultiplayerMenu : MonoBehaviour
{
    public NetworkManager _manager;
    public KcpTransport _transport;

    public InputField _hostingInputPort;

    public InputField _connectIP;
    public InputField _connectPort;

    public void StartHosting()
    {
        Debug.Log("Input port: " + _hostingInputPort.text);
        if (_hostingInputPort.text == "")
        {
            _hostingInputPort.text = "25296";
        }
        _transport.Port = ushort.Parse(_hostingInputPort.text);
        _manager.StartHost();
    }

    public void Connect()
    {
        if (_connectPort.text == "")
        {
            _connectPort.text = "25296";
        }
        _manager.networkAddress = _connectIP.text;
        _transport.Port = ushort.Parse(_connectPort.text);
        _manager.StartClient();
    }
}
