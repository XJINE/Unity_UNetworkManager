using UnityEngine;

public class UNetworkManagerSample : MonoBehaviour
{
    #region Field

    public string networkAddress = "127.0.0.1";
    public int    networkPort    = 5555;

    #endregion Field

    #region Method

    protected virtual void Start()
    {
        UNetworkManager.singleton.networkAddress = this.networkAddress;
        UNetworkManager.singleton.networkPort    = this.networkPort;
    }

    protected virtual void Update ()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            UNetworkManager.singleton.autoStart = !UNetworkManager.singleton.autoStart;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            UNetworkManager.singleton.StartServerSafe();
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            UNetworkManager.singleton.StartHostSafe();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            UNetworkManager.singleton.StartClientSafe();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            UNetworkManager.singleton.Stop();
        }
    }

    protected virtual void OnGUI()
    {
        GUILayout.Label("___Key Control___");
        GUILayout.Label("[A] Toggle Auto Start : " + (UNetworkManager.singleton.autoStart ? "ON" : "OFF"));
        GUILayout.Label(UNetworkManager.singleton.AutoStartIntervalTick.ToString());
        GUILayout.Label("[S] Start As Server");
        GUILayout.Label("[H] Start As Host");
        GUILayout.Label("[C] Start As Client");
        GUILayout.Label("[D] Stop");

        GUILayout.Label("___Status Log___");

        //GUILayout.BeginArea(new Rect(100, 100, 300, 300));

        foreach (UNetworkManager.StatusMessage statusMessage in UNetworkManager.singleton.StatusMessages)
        {
            GUILayout.Label(statusMessage.time.ToLongTimeString() + " - "+ statusMessage.message);
        }

        //GUILayout.EndArea();
    }

    public virtual void SampleEventHandler()
    {
        UNetworkManager.singleton.AddStatusMessage("EventHandle Message.");
    }

    #endregion Method
}