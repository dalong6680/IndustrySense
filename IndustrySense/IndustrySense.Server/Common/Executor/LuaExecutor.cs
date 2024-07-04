using NLua;

namespace IndustrySense.Server.Common.Executor
{
    public class LuaExecutor : IDisposable
    {
        private readonly Lua _lua;

        public LuaExecutor()
        {
            _lua = new Lua();
        }

        public object? Execute(string script)
        {
            try
            {
                return _lua.DoString(script);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lua execution error: {ex.Message}");
                return null;
            }
        }

        public void Dispose()
        {
            _lua.Dispose();
        }
    }
}
