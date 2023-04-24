using Newtonsoft.Json;

namespace servicioWindows
{
    public static class Logger
    {
        private static object objetoBloqueo = new object();
        private static object objetoBloqueoJson = new object();
        public static void log(string strTramaRequest, string rutaArchivo)
        {
            try
            {

                lock (objetoBloqueo!)
                {
                    string fileName = rutaArchivo + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    using (var fs = File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (var writer = new StreamWriter(fs))
                        {
                            writer.WriteLine(DateTime.Now.ToString("HHmmss") + " " + strTramaRequest + " ");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }

        public static void log(String str_tipo, Object obj, string rutaArchivo)
        {
            try
            {

                lock (objetoBloqueoJson!)
                {
                    string fileName = rutaArchivo + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                    using (var fs = File.Open(fileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                    {
                        using (var writer = new StreamWriter(fs))
                        {
                            writer.WriteLine(DateTime.Now.ToString("HHmmss") + " " + str_tipo + JsonConvert.SerializeObject(obj) + " ");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }
        }
    }
}
