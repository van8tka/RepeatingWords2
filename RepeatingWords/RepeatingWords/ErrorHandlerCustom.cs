using System;
using System.Diagnostics;

namespace RepeatingWords
{
    public class ErrorHandlerCustom
    {
        public static void getErrorMessage(Exception er, string propertiesMessage = null)
        {
#if (DEBUG)
            Debug.WriteLine("[---CUSTOM HANDLE ERROR---]" + propertiesMessage!=null?propertiesMessage:" " + er.Message + er.StackTrace);
           
#endif
        }
    }
}
