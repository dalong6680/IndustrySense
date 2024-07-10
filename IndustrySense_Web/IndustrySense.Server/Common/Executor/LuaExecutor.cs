using NLua;
using Org.BouncyCastle.Asn1;

namespace IndustrySense.Server.Common.Executor
{
    public class LuaExecutor : IDisposable
    {
        private readonly Lua _lua;

        public LuaExecutor()
        {
            _lua = new Lua();
            try
            {
                _lua.DoFile("./Lua/dkjson.lua");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading dkjson.lua: {ex.Message}");
            }
        }

        public object[] ExecuteScript(string script, string arg)
        {
            try
            {
                _lua["data"] = arg;
                var result = _lua.DoString(script);
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lua execution error: {ex.Message}");
                return null!;
            }
        }

        public void Dispose()
        {
            _lua.Dispose();
        }
    }
}
