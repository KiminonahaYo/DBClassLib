using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBClassLib.Common.Info
{
    /// <summary>
    ///     テーブル情報クラス
    /// </summary>
    public class TableInfo
    {
        /// <summary>
        ///     カラム情報のコレクション
        /// </summary>
        public ColumnsInfo Columns { get; set; } = new ColumnsInfo();

        /// <summary>
        ///     テーブル名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        ///     スキーマ名
        /// </summary>
        public string Schema { get; set; }
    }
}
