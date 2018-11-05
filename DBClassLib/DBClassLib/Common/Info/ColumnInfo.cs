using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBClassLib.Common.Info
{
    /// <summary>
    ///     カラム情報
    /// </summary>
    public class ColumnInfo
    {
        /// <summary>
        ///     カラム名
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        ///     データベース型
        /// </summary>
        public DBDataType DBDataType { get; set; }

        /// <summary>
        ///     主キーかどうか
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        ///     NULL許可するかどうか
        /// </summary>
        public bool IsNullable { get; set; }
    }
}
