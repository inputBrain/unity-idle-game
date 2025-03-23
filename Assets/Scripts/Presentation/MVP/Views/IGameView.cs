namespace Presentation.MVP.Views
{
    public interface IGameView
    {
        /// <summary>
        /// Метод для вывода сообщения о событии.
        /// </summary>
        /// <param name="message">Сообщение события</param>
        void LogEvent(string message);
    }
}