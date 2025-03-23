using TMPro;
using UnityEngine;

namespace Presentation.MVP.Views
{
    public class GameView : MonoBehaviour, IGameView
    {
        [Header("UI Элементы")]
        public TMP_Text eventLogText;

        public void LogEvent(string message)
        {
            eventLogText.text += message + "\n";
            Debug.Log(message);
        }
    }
}