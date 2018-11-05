using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using DBClassLib.Common;

namespace DBClassLib
{
    /// <summary>
    ///     テーブルアクセスインターフェイス
    /// </summary>
    public interface IDbAccessBase : IDbTransaction
    {
        /// <summary>
        ///     パラメータを追加する。
        /// </summary>
        /// <param name="name">パラメータ名</param>
        /// <param name="value">値</param>
        /// <param name="type">データベース型</param>
        void AddParameter(string name, object value, DBDataType type);

        /// <summary>
        ///     Parameterをクリアする。
        /// </summary>
        void ClearParameter();

        /// <summary>
        ///     Select文を実行する。
        /// </summary>
        /// <param name="strQuery">SQL</param>
        /// <returns>データセット</returns>
        DataSet ExecuteDataSet(string strQuery);

        /// <summary>
        ///     Insert文・Update文・Delete文などを実行する。
        /// </summary>
        /// <param name="strQuery">SQL</param>
        /// <returns>影響を受けた行数</returns>
        int ExecuteNonQuery(string strQuery);

        /// <summary>
        ///     スキーマ情報を取得します。
        /// </summary>
        /// <param name="strQuery">SQL</param>
        /// <returns>影響を受けた行数</returns>
        DataTable GetSchema(string strQuery);

        /// <summary>
        ///     データベース上の現在時刻を取得します。
        /// </summary>
        /// <returns>データベース上の現在時刻</returns>
        DateTime GetDBDateTimeNow();

        /// <summary>
        ///     DataRowから取得したデータを変換する。
        /// </summary>
        /// <typeparam name="T">取得する型</typeparam>
        /// <param name="obj">DataRowにあるデータ</param>
        /// <returns>変換したデータ</returns>
        T ConvertDataRow<T>(object obj);

        /// <summary>
        ///     DataRowから取得したデータを変換する。
        /// </summary>
        /// <typeparam name="T">取得する型</typeparam>
        /// <param name="obj">DataRowにあるデータ</param>
        /// <returns>変換したデータ</returns>
        T? ConvertDataRowToNullable<T>(object obj) where T : struct;
    }
}
