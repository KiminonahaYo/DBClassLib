using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBClassLib.All.Base
{
    /// <summary>
    ///     トランザクションクラス
    /// </summary>
    public class DbTransaction : DbConnection, IDbTransaction
    {
        /// <summary>
        ///     このクラスのリソースを破棄するときにコネクションのリソースも破棄するかどうか
        /// </summary>
        public bool IsDisposeConnection { get { return this.Transaction.IsDisposeConnection; } set { this.Transaction.IsDisposeConnection = value; } }

        /// <summary>
        ///     トランザクション用インターフェイス
        /// </summary>
        protected internal IDbTransaction Transaction { get; set; }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <remarks>同ライブラリ内での継承専用</remarks>
        protected internal DbTransaction()
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="kind">データベースの種類</param>
        public DbTransaction(DbKind kind)
        {
            this.PrepareDbKind(kind);
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="con">コネクション</param>
        public DbTransaction(DbConnection con)
        {
            this.PrepareDbKind(con.DbKind);
            this.CopyConnection(con);
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="trans"></param>
        public DbTransaction(DbTransaction trans)
        {
            this.PrepareDbKind(trans.DbKind);
            this.CopyTransaction(trans);
        }

        /// <summary>
        ///     データベースの種類ごとに初期化する。
        /// </summary>
        /// <param name="kind">データベース種類</param>
        private void PrepareDbKind(DbKind kind)
        {
            switch (DbKind)
            {
                case DbKind.SQLServer:
                {
                    this.Transaction = new SQLServer.DbTransaction();
                    this.Connection = this.Transaction;
                    break;
                }
            }
        }

        /// <summary>
        ///     トランザクションを開始する。
        /// </summary>
        public void BeginTrans()
        {
            this.Transaction.BeginTrans();
        }

        /// <summary>
        ///     トランザクションを開始する。
        /// </summary>
        /// <param name="level">分離レベル</param>
        public void BeginTrans(IsolationLevel level)
        {
            this.Transaction.BeginTrans(level);
        }

        /// <summary>
        ///     トランザクションをコミットする
        /// </summary>
        public void CommitTrans()
        {
            this.Transaction.CommitTrans();
        }

        /// <summary>
        ///     トランザクションをロールバックする
        /// </summary>
        public void RollbackTrans()
        {
            this.Transaction.RollbackTrans();
        }

        /// <summary>
        ///     リソースを破棄する。
        /// </summary>
        public override void Dispose()
        {
            this.Transaction.Dispose();
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
        ///     コネクションのコピーを行う。
        /// </summary>
        /// <param name="to">コピー先</param>
        protected internal override void Copy(object to)
        {
            if (to is DbTransaction trans)
            {
                base.Copy(to);
                if (
                    this.Transaction is DBClassLib.SQLServer.DbTransaction transFrom &&
                    trans.Transaction is DBClassLib.SQLServer.DbTransaction transTo
                    )
                {
                    //SQL Server
                    transFrom.Copy(transTo);
                }
            }
            else
            {
                throw new ArgumentException("指定されたオブジェクトはDBClassLib.All.Base.DbTransactionではありません。「" + (to != null ? to.GetType().ToString() : "【NULL】") + "」です。");
            }
        }
    }
}
