using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

using DBClassLib.Common;

namespace DBClassLib.SQLServer
{
    /// <summary>
    ///     SQL Server DBアクセスクラス
    /// </summary>
    public class DbAccessBase : DbTransaction, IDbAccessBase
    {
        /// <summary>
        ///     SqlParameter
        ///     Key : カラム名, Value ; SQLパラメータ
        /// </summary>
        private Dictionary<string, SqlParameter> diParameter = new Dictionary<string, SqlParameter>();

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <remarks>DBClassLib.All.Baseで使用するときのみ有効</remarks>
        internal DbAccessBase()
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="con">コネクション</param>
        public DbAccessBase(DbConnection con)
        {
            this.CopyConnection(con);
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="trans">トランザクション</param>
        public DbAccessBase(DbTransaction trans)
        {
            this.CopyTransaction(trans);
        }

        /// <summary>
        ///     パラメータを追加する。
        /// </summary>
        /// <param name="name">パラメータ名</param>
        /// <param name="value">値</param>
        /// <param name="type">データベース型</param>
        public void AddParameter(string name, object value, DBDataType type)
        {
            if (this.diParameter.ContainsKey(name))
            {
                //重複時
                throw new ArgumentException("指定されたパラメータ名は既に存在しています。");
            }

            this.diParameter.Add(name, this.GetParameter(name, value, type));
        }

        /// <summary>
        ///     パラメータをクリアする。
        /// </summary>
        public void ClearParameter()
        {
            this.diParameter.Clear();
        }

        /// <summary>
        ///     Select文を実行する。
        /// </summary>
        /// <param name="strQuery">SQL</param>
        /// <returns>データセット</returns>
        public DataSet ExecuteDataSet(string strQuery)
        {
            try
            {
                using (SqlCommand cmd = this.Connection.CreateCommand())
                {
                    cmd.Transaction = this.Transaction;
                    cmd.CommandText = strQuery;
                    cmd.Parameters.AddRange(this.diParameter.Values.ToArray());

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataSet ds = new DataSet();

                        //SQL実行
                        adapter.Fill(ds);

                        //パラメータを削除
                        this.ClearParameter();

                        //データセットを返却
                        return ds;
                    }
                }

            }
            catch (Exception ex)
            {
                throw new DBClassLibException("Select文の実行（DataSetの取得）に失敗しました。", ex);
            }
        }

        /// <summary>
        ///     Insert文・Update文・Delete文などのデータを取得しないSQL文を実行する。
        /// </summary>
        /// <param name="strQuery">SQL</param>
        /// <returns>影響を受けた行数</returns>
        public int ExecuteNonQuery(string strQuery)
        {
            try
            {
                using (SqlCommand cmd = this.Connection.CreateCommand())
                {
                    cmd.Transaction = this.Transaction;
                    cmd.CommandText = strQuery;
                    cmd.Parameters.AddRange(this.diParameter.Values.ToArray());

                    //SQL実行
                    int intRet = cmd.ExecuteNonQuery();

                    //パラメータを削除
                    this.ClearParameter();

                    //影響を受けた行数を返却
                    return intRet;
                }
            }
            catch (Exception ex)
            {
                throw new DBClassLibException("データを取得しないSQL（Insert文・Update文・Delete文など）の実行に失敗しました。", ex);
            }
        }

        /// <summary>
        ///     スキーマ情報を取得する。
        /// </summary>
        /// <param name="strQuery">SQL</param>
        /// <returns>スキーマ情報</returns>
        public DataTable GetSchema(string strQuery)
        {
            try
            {
                using (SqlCommand cmd = this.Connection.CreateCommand())
                {
                    cmd.Transaction = this.Transaction;
                    cmd.CommandText = strQuery;
                    cmd.Parameters.AddRange(this.diParameter.Values.ToArray());

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = reader.GetSchemaTable();

                        this.ClearParameter();

                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DBClassLibException("スキーマ情報の取得に失敗しました。", ex);
            }
        }

        /// <summary>
        ///     データベース上の現在時刻を取得します。
        /// </summary>
        /// <returns>データベース上の現在時刻</returns>
        public DateTime GetDBDateTimeNow()
        {
            using (DataSet ds = this.ExecuteDataSet("Select GetDate() as VAL"))
            {
                return this.ConvertDataRow<DateTime>(ds.Tables[0].Rows[0]["VAL"]);
            }
        }

        /// <summary>
        ///     SQLParameterを取得します。
        /// </summary>
        /// <param name="name">パラメータ名</param>
        /// <param name="value">値</param>
        /// <param name="type">データベース型</param>
        /// <returns></returns>
        private SqlParameter GetParameter(string name, object value, DBDataType type)
        {
            //SqlParameterの場合は、nullはDBNullに置換する。置換しないと、エラーになる。
            value = value == null ? DBNull.Value : value;

            switch(type)
            {
                case DBDataType.NChar:
                {
                    //NCHAR型
                    SqlParameter parameter = new SqlParameter(name, SqlDbType.NChar);
                    parameter.Value = value;
                    return parameter;
                }
                case DBDataType.NVarchar:
                {
                    //NVARCHAR型
                    SqlParameter parameter = new SqlParameter(name, SqlDbType.NVarChar);
                    parameter.Value = value;
                    return parameter;
                }
                case DBDataType.Date:
                case DBDataType.Normal:
                default:
                {
                    //通常の型
                    return new SqlParameter(name, value);
                }
            }
        }

        /// <summary>
        ///     DataRowから取得したデータを変換する。
        /// </summary>
        /// <typeparam name="T">取得する型</typeparam>
        /// <param name="obj">DataRowにあるデータ</param>
        /// <returns>変換したデータ</returns>
        public T ConvertDataRow<T>(object obj)
        {
            if (obj == null || obj == DBNull.Value) return default;

            return (T)Convert.ChangeType(obj, typeof(T));
        }

        /// <summary>
        ///     DataRowから取得したデータを変換する。
        /// </summary>
        /// <typeparam name="T">取得する型</typeparam>
        /// <param name="obj">DataRowにあるデータ</param>
        /// <returns>変換したデータ</returns>
        public T? ConvertDataRowToNullable<T>(object obj) where T : struct
        {
            if (obj == null || obj == DBNull.Value) return null;

            return (T)Convert.ChangeType(obj, typeof(T));
        }
    }
}
