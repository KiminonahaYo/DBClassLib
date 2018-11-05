using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBClassLib.Common
{
    /// <summary>
    ///     データベース型
    /// </summary>
    public enum DBDataType
    {
        /// <summary>通常の型</summary>
        Normal,
        /// <summary>日付型</summary>
        Date,
        /// <summary>NCHAR型</summary>
        NChar,
        /// <summary>NVARCHAR型</summary>
        NVarchar,
    }
}
