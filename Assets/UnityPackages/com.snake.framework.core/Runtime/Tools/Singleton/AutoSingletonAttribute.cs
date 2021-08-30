namespace com.snake.framework
{
    namespace runtime.tool
    {
        public class AutoSingletonAttribute : System.Attribute
        {
            public bool bAutoCreate;

            public AutoSingletonAttribute(bool bCreate)
            {
                this.bAutoCreate = bCreate;
            }
        }
    }
}
