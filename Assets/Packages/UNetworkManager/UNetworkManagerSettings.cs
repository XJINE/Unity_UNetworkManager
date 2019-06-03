using static UNetworkManager;

[System.Serializable]
public class UNetworkManagerSettings
{
    #region Field

    public string   networkAddress;
    public int      networkPort;
    public bool     autoStart;
    public UNetType autoStartType;

    #endregion Field

    #region Method

    public void Save(UNetworkManager uNetworkManager) 
    {
        this.networkAddress = uNetworkManager.networkAddress;
        this.networkPort    = uNetworkManager.networkPort;
        this.autoStart      = uNetworkManager.autoStart;
        this.autoStartType  = uNetworkManager.autoStartType;
    }

    public void Load(UNetworkManager uNetworkManager) 
    {
        bool active = uNetworkManager.isNetworkActive;

        if (active)
        {
            uNetworkManager.Stop();
            uNetworkManager.ClearStatusMessages();
        }

        uNetworkManager.networkAddress = this.networkAddress;
        uNetworkManager.networkPort    = this.networkPort;
        uNetworkManager.autoStart      = this.autoStart;
        uNetworkManager.autoStartType  = this.autoStartType;

        // NOTE:
        // It couldn't restart network in the same frame.
        // Because of the port conflict.
        // 
        //if (uNetworkManager.autoStart)
        //{
        //    uNetworkManager.Start(true);
        //}
    }

    #endregion Method
}