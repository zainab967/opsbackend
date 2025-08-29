using dotenv.net;

namespace CMS.API
{
    public static class EnvLoader
    {
        public static void LoadEnv()
        {
            DotEnv.Load();
        }
    }
}
