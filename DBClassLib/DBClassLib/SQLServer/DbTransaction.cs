using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DBClassLib.SQLServer
{
    /// <summary>
    ///     SQL Server トランザクションクラス
    /// </summary>
    public class DbTransaction : DbConnection, IDbTransaction
    {
        /// <summary>
        ///     このクラスのリソースを破棄するときにコネクションのリソースも破棄するかどうか
        /// </summary>
        public bool IsDisposeConnection { get; set; } = false;

        /// <summary>
        ///     SQL Server トランザクション
        /// </summary>
        internal SqlTransaction Transaction { get; set; } = null;

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DbTransaction()
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="con">コピーするコネクション</param>
        public DbTransaction(DbConnection con)
        {
            this.CopyConnection(con);
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="trans">コピーするトランザクション</param>
        public DbTransaction(DbTransaction trans)
        {
            this.CopyTransaction(trans);
        }

        /// <summary>
        ///     トランザクションを開始する。
        /// </summary>
        public void BeginTrans()
        {
            if (this.Transaction != null) return;

            try
            {
                this.Transaction = this.Connection.BeginTransaction();
            }
            catch (Exception ex)
            {
                this.Transaction = null;
                throw new DBClassLibException("トランザクションの開始に失敗しました。", ex);
            }
        }

        /// <summary>
        ///     トランザクションを開始する
        /// </summary>
        /// <param name="level">分離レベル</param>
        public void BeginTrans(IsolationLevel level)
        {
            if (this.Transaction != null) return;

            try
            {
                this.Transaction = this.Connection.BeginTransaction(level);
            }
            catch (Exception ex)
            {
                this.Transaction = null;
                throw new DBClassLibException("トランザクションの開始に失敗しました。", ex);
            }
        }

        /// <summary>
        ///     トランザクションをコミットする。
        /// </summary>
        public void CommitTrans()
        {
            if (this.Transaction == null) return;

            try
            {
                this.Transaction.Commit();
                this.Transaction = null;
            }
            catch (Exception ex)
            {
                throw new DBClassLibException("トランザクションのコミットに失敗しました。", ex);
            }
        }

        /// <summary>
        ///     トランザクションをロールバックする。
        /// </summary>
        public void RollbackTrans()
        {
            if (this.Transaction == null) return;

            try
            {
                this.Transaction.Rollback();
                this.Transaction = null;
            }
            catch (Exception ex)
            {
                throw new DBClassLibException("トランザクションのロールバックに失敗しました。", ex);
            }
        }

        /// <summary>
        ///     コネクションをコピーする
        /// </summary>
        /// <param name="con">コネクション</param>
        internal override void CopyConnection(DbConnection con)
        {
            if (con is DbTransaction trans)
            {
                this.CopyTransaction(trans);
            }
            else
            {
                base.CopyConnection(con);
            }
        }

        /// <summary>
        ///     トランザクションをコピーする
        /// </summary>
        /// <param name="trans">トランザクション</param>
        internal void CopyTransaction(DbTransaction trans)
        {
            trans.Copy(this);
        }

        /// <summary>
        ///     トランザクションのコピーを行う。
        /// </summary>
        /// <param name="to"></param>
        internal override void Copy(object to)
        {
            if (to is DbTransaction trans)
            {
                base.Copy(to);
                trans.Transaction = this.Transaction;
                trans.IsDisposeConnection = this.IsDisposeConnection;
            }
            else
            {
                throw new ArgumentException("指定されたオブジェクトはDbClassLib.SQLServer.DbTransactionではありません。「" + (to != null ? to.GetType().ToString() : "【NULL】") + "」です。");
            }
        }

        /// <summary>
        ///     リソースを破棄する。
        /// </summary>
        public override void Dispose()
        {
            if (this.IsDisposeConnection)
            {
                base.Dispose();
            }

            if (this.Transaction != null)
            {
                this.Transaction.Dispose();
            }
        }
    }
}
