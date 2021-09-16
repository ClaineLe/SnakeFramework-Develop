namespace com.snake.framework
{
    namespace runtime
    {
        public interface IManager
        {
            string mName { get; }
            float mBootUpProgress { get; }
            void Initialization();

        }
    }
}