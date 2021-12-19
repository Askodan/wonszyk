using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SystemMonit : MonoBehaviour
{
    public enum MessageType
    {
        Warning,
        Error,
        Info,
        Success
    }
    [System.Serializable]
    public class MessageData
    {
        public MessageType Type;
        public string Message;
    }
    // [SerializeField]
    // Dictionary<MessageType, Color> messageColorsDict = new Dictionary<MessageType, Color>();
    [System.Serializable]
    public class MessageTypeColor
    {
        public MessageType Type;
        public Color Color;
    }
    [SerializeField]
    List<MessageTypeColor> messageColors = new List<MessageTypeColor>();

    [SerializeField]
    Image background;
    [SerializeField]
    Text content;

    public void SetMessage(MessageType type, string text)
    {
        Color defaultColor = Color.white;
        for (int i = 0; i < messageColors.Count; i++)
        {
            if (messageColors[i].Type == type)
            {
                defaultColor = messageColors[i].Color;
            }
        }
        background.color = defaultColor;
        content.text = text;
        this.gameObject.SetActive(true);
    }
    public void SetMessage(MessageData Data)
    {
        SetMessage(Data.Type, Data.Message);
    }
}
