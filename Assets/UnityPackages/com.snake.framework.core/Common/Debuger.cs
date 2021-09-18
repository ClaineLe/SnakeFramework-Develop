namespace com.snake.framework
{
    /// <summary>
    /// �����־
    /// </summary>
    public class Debuger
    {
        /// <summary>
        /// �����־��Ϣ
        /// </summary>
        /// <param name="message"></param>
        static public void Info(object message)
        {
            UnityEngine.Debug.Log(message);
        }

        /// <summary>
        /// �����־��Ϣ������ʽ��
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        static public void InfoFormat(string message, params object[] args)
        {
            Info(Utility.Text.Format(message, args));
        }

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        /// <param name="message"></param>
        static public void Log(object message)
        {
            UnityEngine.Debug.Log(message);
        }
        /// <summary>
        /// ���������Ϣ������ʽ��
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        static public void LogFormat(string message, params object[] args)
        {
            Log(Utility.Text.Format(message, args));
        }

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        /// <param name="message"></param>
        static public void Warn(object message)
        {
            UnityEngine.Debug.LogWarning(message);

        }

        /// <summary>
        /// ���������Ϣ������ʽ��
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        static public void WarnFormat(string message, params object[] args)
        {
            Warn(Utility.Text.Format(message, args));
        }

        /// <summary>
        /// ���������Ϣ
        /// </summary>
        /// <param name="message"></param>
        static public void Error(object message)
        {
            UnityEngine.Debug.LogError(message);
        }

        /// <summary>
        /// ���������Ϣ������ʽ��
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        static public void ErrorFormat(string message, params object[] args)
        {
            Error(Utility.Text.Format(message, args));
        }
    }
}
