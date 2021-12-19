using UnityEngine;
using System.Collections;

public enum MessageClass
{
    Unknown,
    PlayerIndex,
}

public delegate IEnumerator OnPlayerIndex(PlayerIndexData Data);

public class MessageAnalyzer
{
    // delegates
    public OnPlayerIndex OnPlayerIndex;

    public MessageAnalyzer()
    {

    }

    public void WorkWithMessage(int MessageID, byte[] Data)
    {
        switch ((MessageClass)MessageID)
        {
            case MessageClass.PlayerIndex:
                var PIData = new PlayerIndexData(Data);
                UnityMainThreadDispatcher.Instance().Enqueue(OnPlayerIndex(PIData));
                break;
            case MessageClass.Unknown:
                Debug.LogError("Unknown Message class! " + MessageID);
                break;
            default:
                Debug.LogError("No such message! " + MessageID);
                break;
        }
    }


}