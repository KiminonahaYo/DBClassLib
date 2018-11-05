using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace DBClassLib.SQLServer
{
    /// <summary>
    ///     SQL Server コネクションクラス
    /// </summary>
    public class DbConnection : DbBase, IDbConnection
    {
        /// <summary>
        ///     接続文字列
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        ///     SQL Server コネクション
        /// </summary>
        internal SqlConnection Connection { get; set; }

        /// <summary>
        ///     接続が開かれているかどうか
        /// </summary>
        public bool IsOpen
        {
            get
            {
                if (this.Connection == null) return false;

                switch (this.Connection.State)
                {
                    case System.Data.ConnectionState.Broken:
                    case System.Data.ConnectionState.Closed:
                    default:
                    {
                        return false;
                    }
                    case System.Data.ConnectionState.Connecting:
                    case System.Data.ConnectionState.Executing:
                    case System.Data.ConnectionState.Fetching:
                    case System.Data.ConnectionState.Open:
                    {
                        return true;
                    }
                }
            }
        }


        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DbConnection()
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="con">コピーするコネクション</param>
        public DbConnection(DbConnection con)
        {
            this.CopyConnection(con);
        }

        /// <summary>
        ///     データベースをオープンする。
        /// </summary>
        public void Open()
        {
            if (this.Connection != null) return;

            try
            {
                this.Connection = new SqlConnection(this.ConnectionString);
                this.Connection.Open();
            }
            catch (Exception ex)
            {
                this.Connection = null;
                throw new DBClassLibException("データベースのオープンに失敗しました。", ex);
            }
        }

        /// <summary>
        ///     データベースをクローズする。
        /// </summary>
        public void Close()
        {
            if (this.Connection == null) return;

            try
            {
                this.Connection.Close();
                this.Connection = null;
            }
            catch (Exception ex)
            {
                throw new DBClassLibException("データベースのクローズに失敗しました。", ex);
            }
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
        internal virtual void Copy(object to)
        {
            if (to is DbConnection con)
            {
                con.Connection = this.Connection;
                con.ConnectionString = this.ConnectionString;
            }
            else
            {
                throw new ArgumentException("指定されたオブジェクトはDbConnection.SQLServer.DbConnectionではありません。「" + (to != null ? to.GetType().ToString() : "【NULL】") + "」です。");
            }
        }

        /// <summary>
        ///     リソースを破棄する。
        /// </summary>
        public virtual void Dispose()
        {
            if (this.Connection != null)
            {
                this.Connection.Dispose();
            }
        }
    }
}
