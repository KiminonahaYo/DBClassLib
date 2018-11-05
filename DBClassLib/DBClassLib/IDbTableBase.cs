using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBClassLib.Common.Info;
using DBClassLib.Common;

namespace DBClassLib
{
    /// <summary>
    ///     データベーステーブル基底クラスインターフェイス
    /// </summary>
    public interface IDbTableBase : IDbAccessBase
    {
        /// <summary>
        ///     テーブル情報
        /// </summary>
        TableInfo TableInfo { get; set; }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内の全レコードを取得する。
        /// </summary>
        /// <returns>データセット</returns>
        DataSet Select();

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内の全レコードを取得する。カラムを指定する。
        /// </summary>
        /// <param name="lstColumns"></param>
        /// <returns>データセット</returns>
        DataSet Select(List<string> lstColumns);

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns></returns>
        DataSet Select(Dictionary<string, object> diWhere);

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="strWhere">検索条件</param>
        /// <returns></returns>
        DataSet Select(string strWhere);

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="lstColumns">カラム名</param>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>データセット</returns>
        DataSet Select(List<string> lstColumns, Dictionary<string, object> diWhere);

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="lstColumns">カラム名</param>
        /// <param name="strWhere">検索条件（「Where」は含めない条件式）</param>
        /// <returns>データセット</returns>
        DataSet Select(List<string> lstColumns, string strWhere);

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="lstColumns">カラム名</param>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <param name="lstOrderBy">OrderBy句部分（Columns : カラム名, AscDesc : 昇順または降順）</param>
        /// <returns>データセット</returns>
        DataSet Select(List<string> lstColumns, Dictionary<string, object> diWhere, List<OrderByInfo<string>> lstOrderBy);

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="lstColumns">カラム名</param>
        /// <param name="strWhere">検索条件（「Where」は含めない条件式）</param>
        /// <param name="lstOrderBy">OrderBy句部分（Columns : カラム名, AscDesc : 昇順または降順）</param>
        /// <returns>データセット</returns>
        DataSet Select(List<string> lstColumns, string strWhere, List<OrderByInfo<string>> lstOrderBy);

        /// <summary>
        ///     Insert文を実行する。
        /// </summary>
        /// <param name="diInsert">Insertするデータ（Key : カラム名, Value : 値）</param>
        /// <returns>影響を受けた行数</returns>
        int Insert(Dictionary<string, object> diInsert);

        /// <summary>
        ///     Update文を実行する。Dictioanryのカラムの中に主キーが含まれていたら、それを検索条件にする。
        /// </summary>
        /// <param name="diUpdate">Updateするデータ（Key : カラム名, Value : 値）</param>
        /// <returns>影響を受けた行数</returns>
        /// <remarks>主キーはUpdateしない。</remarks>
        int Update(Dictionary<string, object> diUpdate);

        /// <summary>
        ///     Update文を実行する。
        /// </summary>
        /// <param name="diUpdate">Updateするデータ（Key : カラム名, Value : 値）</param>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>影響を受けた行数</returns>
        /// <remarks>主キーもUpdateする。</remarks>
        int Update(Dictionary<string, object> diUpdate, Dictionary<string, object> diWhere);

        /// <summary>
        ///     Update文を実行する。
        /// </summary>
        /// <param name="diUpdate">Updateするデータ（Key : カラム名, Value : 値）</param>
        /// <param name="strWhere">検索条件</param>
        /// <returns>影響を受けた行数</returns>
        /// <remarks>主キーもUpdateする。</remarks>
        int Update(Dictionary<string, object> diUpdate, string strWhere);

        /// <summary>
        ///     Delete文を実行する。
        /// </summary>
        /// <returns>影響を受けた行数</returns>
        int Delete();

        /// <summary>
        ///     Delete文を実行する。
        /// </summary>
        /// <param name="strWhere">検索条件</param>
        /// <returns>影響を受けた行数</returns>
        int Delete(string strWhere);

        /// <summary>
        ///     Delete文を実行する。
        /// </summary>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>影響を受けた行数</returns>
        int Delete(Dictionary<string, object> diWhere);

        /// <summary>
        ///     Select Func(*) from ...を取得する。（検索条件なし）
        /// </summary>
        /// <param name="functions"></param>
        /// <param name="strColumn"></param>
        /// <returns>取得結果</returns>
        string SelectFunc(Functions functions, string strColumn);

        /// <summary>
        ///     Select Func(*) from ... where ...を実行する。
        /// </summary>
        /// <param name="functions">関数の種類</param>
        /// <param name="strColumn">カラム名</param>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>取得結果</returns>
        string SelectFunc(Functions functions, string strColumn, Dictionary<string, object> diWhere);

        /// <summary>
        ///     Select Func(*) from ... where ...を実行する。
        /// </summary>
        /// <param name="functions">関数の種類</param>
        /// <param name="strColumn">カラム名</param>
        /// <param name="strWhere">検索条件</param>
        /// <returns>取得結果</returns>
        string SelectFunc(Functions functions, string strColumn, string strWhere);

        /// <summary>
        ///     レコードの個数を取得します。
        /// </summary>
        /// <returns>レコードの個数</returns>
        long SelectCount();

        /// <summary>
        ///     レコードの個数を取得します。
        /// </summary>
        /// <param name="strWhere">検索条件</param>
        /// <returns>レコードの個数</returns>
        long SelectCount(string strWhere);

        /// <summary>
        ///     レコードの個数を取得します。
        /// </summary>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>レコードの個数</returns>
        long SelectCount(Dictionary<string, object> diWhere);
    }
}
