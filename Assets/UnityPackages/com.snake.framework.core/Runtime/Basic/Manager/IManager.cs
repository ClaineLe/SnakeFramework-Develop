namespace com.snake.framework
{
    namespace runtime
    {
        public interface IManager
        {
            string mName { get; }
            float GetInitProgress();
            float GetPreloadProgress();
            void Initialization();
            void Preload();
        }
    }
}