using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBClassLib.Common
{
    /// <summary>
    ///     SQLで使用する関数
    /// </summary>
    public enum Functions
    {
        /// <summary>最大値</summary>
        Max,
        /// <summary>最小値</summary>
        Min,
        /// <summary>合計</summary>
        Sum,
        /// <summary>レコードの個数</summary>
        Count
    }
}
