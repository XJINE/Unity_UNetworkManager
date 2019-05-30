using UnityEngine;
using UnityEngine.Networking;

public class SendReceiveMessageSample : NetworkBehaviour
{
    #region Class

    public class SampleMessageType
    {
        public const short SampleMessage = MsgType.Highest + 1;
    }

    public class SampleMessage : MessageBase
    {
        public int value;
    }

    #endregion Class

    #region Field

    public int value;

    #endregion Field

    #region Method

    void Awake()
    {
        UNetworkManager.singleton.startClientEvent.AddListener(() =>
        {
            UNetworkManager.singleton.client.RegisterHandler
            (SampleMessageType.SampleMessage, ReceiveMessage);
        });
    }

    [ServerCallback]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            this.value += 1;
            SendMessage();
        }
    }

    void OnGUI()
    {
        GUILayout.Label("VALUE : " + value);
    }

    protected void ReceiveMessage(NetworkMessage networkMessage)
    {
        SampleMessage message = networkMessage.ReadMessage<SampleMessage>();

        this.value = message.value;

        Debug.Log("RECEIVE : " + message.value);
    }

    public void SendMessage()
    {
        SampleMessage message = new SampleMessage()
        {
            value = this.value
        };

        NetworkServer.SendToAll(SampleMessageType.SampleMessage, message);
    }

    #endregion Method
}