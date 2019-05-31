using static UNetworkManager;

[System.Serializable]
public class UNetworkManagerSettings
{
    // NOTE:
    // This class is mainly used for Save & Load settings.

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
        }

        uNetworkManager.networkAddress = this.networkAddress;
        uNetworkManager.networkPort    = this.networkPort;
        uNetworkManager.autoStart      = this.autoStart;
        uNetworkManager.autoStartType  = this.autoStartType;
    }

    #endregion Method
}