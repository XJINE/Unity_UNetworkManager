using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class UNetworkManager : NetworkManager
{
    #region Enum

    public enum UNetType
    {
        Server = 0,
        Host   = 1,
        Client = 2,
        None   = 3,
    }

    #endregion Enum

    #region Field

    public     bool autoStart;
    public UNetType autoStartType;
    public    float autoStartIntervalTimeSec = 15;
    protected float previousAutoStartTimeSec = 0;

    #region Status 

    public struct StatusMessage
    {
        public DateTime time;
        public string   message;
    }

    public int statusMessagesCount = 10;

    public List<StatusMessage> StatusMessages
    {
        get;
        protected set; 
    }

    #endregion Status

    #region Event

    [Serializable]
    public class NetworkConnectionEvent : UnityEvent<NetworkConnection> { }

    public UnityEvent             startServerEvent;
    public UnityEvent             stopServerEvent;
    public NetworkConnectionEvent serverConnectEvent;
    public NetworkConnectionEvent serverDisconnectEvent;
    public NetworkConnectionEvent serverErrorEvent;

    public UnityEvent             startHostEvent;
    public UnityEvent             stopHostEvent;

    public UnityEvent             startClientEvent;
    public UnityEvent             stopClientEvent;
    public NetworkConnectionEvent clientConnectEvent;
    public NetworkConnectionEvent clientDisconnectEvent;
    public NetworkConnectionEvent clientErrorEvent;

    #endregion Event

    #endregion Field

    #region Property

    public static new UNetworkManager singleton
    {
        get { return (UNetworkManager)NetworkManager.singleton; }
    }

    public UNetType NetworkType { get; protected set; }

    public bool IsConnectedClient { get; protected set; }

    public float AutoStartIntervalTick
    { get { return this.autoStartIntervalTimeSec
                 - (Time.timeSinceLevelLoad - this.previousAutoStartTimeSec); } }

    #endregion Propery

    #region Method

    protected virtual void OnEnable()
    {
        // NOTE:
        // It should not to initialize something in Awake.
        // Because based NetworkManager use Awake to initialize singleton logic.
        // Awake is not able to override.

        this.NetworkType = UNetType.None;

        this.StatusMessages = new List<StatusMessage>();

        AddStatusMessage("…");
    }

    protected virtual void Start()
    {
        AutoStart(true);
    }

    protected virtual void Update()
    {
        AutoStart();
    }


    public void AddStatusMessage(string statusMessage)
    {
        AddStatusMessage(new StatusMessage()
        {
            time    = DateTime.Now,
            message = statusMessage
        });
    }

    public void AddStatusMessage(StatusMessage statusMessage)
    {
        this.StatusMessages.Insert(0, statusMessage);
        TrimStatusMessages();
    }

    protected void TrimStatusMessages()
    {
        int count = this.StatusMessages.Count;

        while (count > this.statusMessagesCount)
        {
            this.StatusMessages.RemoveAt(count - 1);

            count = count - 1;
        }
    }

    public void ClearStatusMessages()
    {
        this.StatusMessages.Clear();
    }


    protected virtual void AutoStart(bool ignoreInterval = false)
    {
        if (!this.autoStart || this.NetworkType != UNetType.None)
        {
            this.previousAutoStartTimeSec = Time.timeSinceLevelLoad;
            return;
        }

        if (!ignoreInterval)
        {
            float elapsedTime = Time.timeSinceLevelLoad - this.previousAutoStartTimeSec;

            if (this.autoStartIntervalTimeSec > elapsedTime)
            {
                return;
            }
        }

        this.previousAutoStartTimeSec = Time.timeSinceLevelLoad;

        AddStatusMessage("Auto start as " + this.autoStartType.ToString() + ".");

        switch (this.autoStartType)
        {
            case UNetType.Server: { base.StartServer(); break; }
            case UNetType.Host:   { base.StartHost();   break; }
            case UNetType.Client: { base.StartClient(); break; }
        }
    }

    public virtual void StartServerSafe()
    {
        if (this.NetworkType != UNetType.None)
        {
            AddStatusMessage("Faild to start server.");
            return;
        }

        base.StartServer();
    }

    public virtual void StartHostSafe()
    {
        if (this.NetworkType != UNetType.None)
        {
            AddStatusMessage("Faild to start host.");
            return;
        }

        base.StartHost();
    }

    public virtual void StartClientSafe()
    {
        if (this.NetworkType != UNetType.None)
        {
            AddStatusMessage("Faild to start client.");
            return;
        }

        base.StartClient();
    }

    public virtual void Stop()
    {
        switch (this.NetworkType)
        {
            case UNetType.Server: { base.StopServer(); break; }
            case UNetType.Host:   { base.StopHost();   break; }
            case UNetType.Client: { base.StopClient(); break; }
            case UNetType.None:
            {
                AddStatusMessage("Failed to Stop : Nothing is Started.");
                break;
            }
        }
    }


    // NOTE:
    // When start as Host, OnStartServer() and OnStartClient()
    // are also called after OnStartHost() called.

    public override void OnStartServer()
    {
        if (this.NetworkType != UNetType.Host)
        {
            AddStatusMessage("Server start.");
            this.NetworkType = UNetType.Server;
        }

        base.OnStartServer();

        this.startServerEvent.Invoke();
    }

    public override void OnStopServer()
    {
        AddStatusMessage("Server stop.");

        this.NetworkType = UNetType.None;

        base.OnStopServer();

        this.stopServerEvent.Invoke();
    }

    public override void OnServerConnect(NetworkConnection networkConnection)
    {
        AddStatusMessage("Client connected. : " + networkConnection.address);

        base.OnServerConnect(networkConnection);

        this.serverConnectEvent.Invoke(networkConnection);
    }

    public override void OnServerDisconnect(NetworkConnection networkConnection)
    {
        AddStatusMessage("Client disconnected. : " + networkConnection.address);

        base.OnServerDisconnect(networkConnection);

        this.serverDisconnectEvent.Invoke(networkConnection);
    }

    public override void OnServerError(NetworkConnection networkConnection, int errorCode)
    {
        AddStatusMessage("Server error. : " + (NetworkError)errorCode);

        base.OnServerError(networkConnection, errorCode);

        this.serverErrorEvent.Invoke(networkConnection);
    }

    public override void OnStartHost()
    {
        AddStatusMessage("Host start.");

        this.NetworkType = UNetType.Host;

        base.OnStartHost();

        this.startHostEvent.Invoke();
    }

    public override void OnStopHost()
    {
        AddStatusMessage("Host stop.");

        this.NetworkType = UNetType.None;

        base.OnStopHost();

        this.stopHostEvent.Invoke();
    }

    public override void OnStartClient(NetworkClient networkClient)
    {
        if (this.NetworkType != UNetType.Host)
        {
            AddStatusMessage("Client start.");
            this.NetworkType = UNetType.Client;
        }

        base.OnStartClient(networkClient);

        this.startClientEvent.Invoke();
    }

    public override void OnStopClient()
    {
        AddStatusMessage("Client stop.");

        this.NetworkType = UNetType.None;

        base.OnStopClient();

        this.stopClientEvent.Invoke();
    }

    public override void OnClientConnect(NetworkConnection networkConnection)
    {
        AddStatusMessage("Connected to server. : " + networkConnection.address);

        this.IsConnectedClient = true;

        base.OnClientConnect(networkConnection);

        this.clientConnectEvent.Invoke(networkConnection);
    }

    public override void OnClientDisconnect(NetworkConnection networkConnection)
    {
        if (this.IsConnectedClient)
        {
            AddStatusMessage("Disconnected from server. : " + networkConnection.address);
        }
        else
        {
            AddStatusMessage("Faild to connect server. : " + networkConnection.address);
        }

        this.IsConnectedClient = false;

        base.OnClientDisconnect(networkConnection);

        this.clientDisconnectEvent.Invoke(networkConnection);
    }

    public override void OnClientError(NetworkConnection networkConnection, int errorCode)
    {
        AddStatusMessage("Client error. : " + (NetworkError)errorCode);

        base.OnClientError(networkConnection, errorCode);

        this.clientErrorEvent.Invoke(networkConnection);
    }

    #endregion Method
}