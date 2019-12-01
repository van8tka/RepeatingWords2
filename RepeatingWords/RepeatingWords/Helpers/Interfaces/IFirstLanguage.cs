using System;
using System.Collections.Generic;
using System.Text;

namespace RepeatingWords.Helpers.Interfaces
{
    public interface IFirstLanguage
    {
        /// <summary>
        /// get first language name(native or foreign) which will show first when begin to study 
        /// </summary>
        /// <returns>true - native, false - foreign</returns>
        bool GetFirstLanguage();
        /// <summary>
        /// change first language name(native or foreign) which will show first when begin to study 
        /// </summary>
        /// <returns>true - native current , false - foreign current</returns>
        bool ChangeFirstLanguage();
    }
}
