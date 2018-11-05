using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DBClassLib
{
    /// <summary>
    ///     トランザクション用インターフェイス
    /// </summary>
    public interface IDbTransaction : IDbConnection, IDisposable
    {
        /// <summary>
        ///     このクラスのリソースを破棄するときにコネクションのリソースも破棄するかどうか
        /// </summary>
        bool IsDisposeConnection { get; set; }

        /// <summary>
        ///     トランザクションを開始する
        /// </summary>
        void BeginTrans();

        /// <summary>
        ///     トランザクションを開始する
        /// </summary>
        /// <param name="level">分離レベル</param>
        void BeginTrans(IsolationLevel level);

        /// <summary>
        ///     トランザクションをコミットする
        /// </summary>
        void CommitTrans();

        /// <summary>
        ///     トランザクションをロールバックする
        /// </summary>
        void RollbackTrans();
    }
}
