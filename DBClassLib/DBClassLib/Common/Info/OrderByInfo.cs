using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBClassLib.Common.Info
{
    /// <summary>
    ///     OrderBy情報クラス
    /// </summary>
    /// <typeparam name="T">カラム名の型（string または 列挙体）</typeparam>
    public class OrderByInfo<T>
    {
        /// <summary>
        ///     カラム名
        /// </summary>
        public T Column { get; set; }

        /// <summary>
        ///     ソート方向
        /// </summary>
        public OrderByAscDesc AscDesc { get; set; }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public OrderByInfo()
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="column">カラム名</param>
        /// <param name="ascdesc">ソート方向</param>
        public OrderByInfo(T column, OrderByAscDesc ascdesc)
        {
            this.Column = column;
            this.AscDesc = ascdesc;
        }
    }
}
