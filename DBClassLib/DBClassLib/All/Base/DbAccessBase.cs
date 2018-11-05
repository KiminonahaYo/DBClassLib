using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DBClassLib.Common;

namespace DBClassLib.All.Base
{
    /// <summary>
    ///     データベースアクセスクラス
    /// </summary>
    public class DbAccessBase : DbTransaction, IDbAccessBase
    {
        /// <summary>
        ///     データベースアクセスクラス用インターフェイス
        /// </summary>
        protected internal IDbAccessBase AccessBase { get; set; }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <remarks>継承専用</remarks>
        protected internal DbAccessBase()
        {
        }
        
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="con">コネクション</param>
        public DbAccessBase(DbConnection con)
        {
            this.PrepareDbKind(con.DbKind);
            this.CopyConnection(con);
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="trans"></param>
        public DbAccessBase(DbTransaction trans)
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
                    this.AccessBase = new SQLServer.DbAccessBase();
                    this.Transaction = this.AccessBase;
                    this.Connection = this.AccessBase;
                    break;
                }
            }
        }

        /// <summary>
        ///     パラメータを追加する。
        /// </summary>
        /// <param name="name">パラメータ名</param>
        /// <param name="value">値</param>
        /// <param name="type">データベース型</param>
        public void AddParameter(string name, object value, DBDataType type)
        {
            this.AccessBase.AddParameter(name, value, type);
        }

        /// <summary>
        ///     Parameterをクリアする。
        /// </summary>
        public void ClearParameter()
        {
            this.AccessBase.ClearParameter();
        }

        /// <summary>
        ///     Select文を実行する。
        /// </summary>
        /// <param name="strQuery">SQL</param>
        /// <returns>データセット</returns>
        public DataSet ExecuteDataSet(string strQuery)
        {
            return this.AccessBase.ExecuteDataSet(strQuery);
        }

        /// <summary>
        ///     Insert文・Update文・Delete文などを実行する。
        /// </summary>
        /// <param name="strQuery">SQL</param>
        /// <returns>影響を受けた行数</returns>
        public int ExecuteNonQuery(string strQuery)
        {
            return this.AccessBase.ExecuteNonQuery(strQuery);
        }

        /// <summary>
        ///     スキーマ情報を取得します。
        /// </summary>
        /// <param name="strQuery">SQL</param>
        /// <returns>影響を受けた行数</returns>
        public DataTable GetSchema(string strQuery)
        {
            return this.AccessBase.GetSchema(strQuery);
        }

        /// <summary>
        ///     データベース上の現在時刻を取得します。
        /// </summary>
        /// <returns>データベース上の現在時刻</returns>
        public DateTime GetDBDateTimeNow()
        {
            return this.AccessBase.GetDBDateTimeNow();
        }

        /// <summary>
        ///     DataRowから取得したデータを変換する。
        /// </summary>
        /// <typeparam name="T">取得する型</typeparam>
        /// <param name="obj">DataRowにあるデータ</param>
        /// <returns>変換したデータ</returns>
        public T ConvertDataRow<T>(object obj)
        {
            return this.AccessBase.ConvertDataRow<T>(obj);
        }

        /// <summary>
        ///     DataRowから取得したデータを変換する。
        /// </summary>
        /// <typeparam name="T">取得する型</typeparam>
        /// <param name="obj">DataRowにあるデータ</param>
        /// <returns>変換したデータ</returns>
        public T? ConvertDataRowToNullable<T>(object obj) where T : struct
        {
            return this.AccessBase.ConvertDataRowToNullable<T>(obj);
        }
    }
}
