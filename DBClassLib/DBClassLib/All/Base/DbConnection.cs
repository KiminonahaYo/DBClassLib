using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBClassLib.All.Base
{
    /// <summary>
    ///     コネクションクラス
    /// </summary>
    public class DbConnection : DbBase, IDbConnection
    {
        /// <summary>
        ///     コネクション用インターフェイス
        /// </summary>
        protected internal IDbConnection Connection { get; set; }

        /// <summary>
        ///     接続文字列
        /// </summary>
        public string ConnectionString { get { return this.Connection.ConnectionString; } set { this.Connection.ConnectionString = value; } }

        /// <summary>
        ///     接続が開かれているかどうか
        /// </summary>
        public bool IsOpen { get { return this.Connection.IsOpen; } }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <remarks>同ライブラリ内での継承専用</remarks>
        protected internal DbConnection()
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="kind">DB種類</param>
        public DbConnection(DbKind kind)
        {
            this.PrepareDbKind(kind);
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="con">コネクション</param>
        public DbConnection(DbConnection con)
        {
            this.PrepareDbKind(con.DbKind);
            this.CopyConnection(con);
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
                    this.Connection = new SQLServer.DbConnection();
                    break;
                }
            }
        }

        /// <summary>
        ///     データベースをオープンする。
        /// </summary>
        public void Open()
        {
            this.Connection.Open();
        }

        /// <summary>
        ///     データベースをクローズする。
        /// </summary>
        public void Close()
        {
            this.Connection.Close();
        }

        /// <summary>
        ///     リソースを破棄する。
        /// </summary>
        public virtual void Dispose()
        {
            this.Connection.Dispose();
        }

        /// <summary>
        ///     コネクションをコピーする。
        /// </summary>
        /// <param name="con">コネクション</param>
        internal virtual void CopyConnection(DbConnection con)
        {
            con.Copy(this);
        }

        /// <summary>
        ///     コネクションのコピーを行う。
        /// </summary>
        /// <param name="to">コピー先</param>
        protected internal override void Copy(object to)
        {
            if (to is DbConnection con)
            {
                base.Copy(to);
                if (
                    this.Connection is DBClassLib.SQLServer.DbConnection conFrom &&
                    con.Connection is DBClassLib.SQLServer.DbConnection conTo
                    )
                {
                    //SQL Server
                    conFrom.Copy(conTo);
                }
            }
            else
            {
                throw new ArgumentException("指定されたオブジェクトはDBClassLib.All.Base.DbConnectionではありません。「" + (to != null ? to.GetType().ToString() : "【NULL】") + "」です。");
            }
        }
    }
}
