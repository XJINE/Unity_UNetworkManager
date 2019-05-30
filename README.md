# Unity_UNetworkManager

![](https://github.com/XJINE/Unity_UNetworkManager/blob/master/screenshot.png)

This is default "NetworkManager" extension.

- Auto Connection.
    - When failed to connect or when disconnected, try again.
- Event Logging.
    - Start
    - Connect
    - Disconnect
- Lots of Event.

And, this includes ``csc.rsp`` file to ignore UNET alert in UnityEditor.

## EventHandler

| Name                         | Called when              | Called In   |
| ---------------------------- | ------------------------ | ----------- | 
| StartServerEventHandler      | Start as Server          | Server side |
| StopServerEventHandler       | Stop Server              | Server side |
| ServerConnectEventHandler    | Client connect           | Server side |
| ServerDisconnectEventHandler | Client disconnect        | Server side |
| ServerErrorEventHandler      | Error occurred in Server | Server side |
| StartHostEventHandler        | Start as Host            | Server side |
| StopHostEventHandler         | Stop Host                | Server side |
| StartClientEventHandler      | Start as Client          | Client side |
| StopClientEventHandler       | Stop Client              | Client side |
| ClientConnectEventHandler    | Connect to Server        | Client side |
| ClientDiscconectEventHandler | Disconnect from Server   | Client side |
| ClientErrorEventHandler      | Error occurred in Client | Client side |

## Import to Your Project

You can import this asset from UnityPackage.

- [UNetworkManager.unitypackage](https://github.com/XJINE/Unity_UNetworkManager/blob/master/UNetworkManager.unitypackage)