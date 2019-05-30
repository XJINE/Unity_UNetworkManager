using UnityEngine;
using UnityEngine.Networking;

public class SyncListSample : NetworkBehaviour
{
    public int value;

    SyncListInt syncList = new SyncListInt();

    void Start()
    {
        this.syncList.Callback += OnChanged;
        this.syncList.Add(value);
    }

    void Update()
    {
        if (base.isServer)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // NOTE:
                // "this.value" will keep the value,
                // and "this.syncList[0]" will update.

                this.syncList[0] += 1;
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("value / syncList = " + this.value + " / " + this.syncList[0]);
    }

    private void OnChanged(SyncListInt.Operation op, int index)
    {
        Debug.Log("OnChanged : " + op + " : " + this.value + " / " + this.syncList[index]);
    }
}