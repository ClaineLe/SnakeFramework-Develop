using XLua;

namespace com.snake.framework 
{
    namespace custom 
    {
        public sealed class LuaImplement
        {
            private LuaEnv _luaEnv;

            public LuaImplement() 
            {
                this._luaEnv = new LuaEnv();
            }

            public void Release() 
            {
                if (this._luaEnv != null)
                {
                    this._luaEnv.Dispose();
                    this._luaEnv = null;
                }
            }
        }
    }
}