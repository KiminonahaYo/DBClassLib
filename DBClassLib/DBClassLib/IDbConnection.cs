using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBClassLib
{
    /// <summary>
    ///     コネクション用インターフェイス
    /// </summary>
    public interface IDbConnection : IDisposable
    {
        /// <summary>
        ///     接続文字列
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        ///     接続が開かれているかどうか
        /// </summary>
        bool IsOpen { get; }

        /// <summary>
        ///     コネクションを開く
        /// </summary>
        void Open();

        /// <summary>
        ///     コネクションを閉じる
        /// </summary>
        void Close();
    }
}
