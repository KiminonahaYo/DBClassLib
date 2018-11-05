using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBClassLib.Common;
using DBClassLib.Common.Info;

namespace DBClassLib.All.Base
{
    /// <summary>
    ///     データベーステーブル基底クラス
    /// </summary>
    public class DbTableBase : DbAccessBase, IDbTableBase
    {
        /// <summary>
        ///     データベーステーブル基底クラス用インターフェイス
        /// </summary>
        protected internal IDbTableBase TableBase { get; set; }

        /// <summary>
        ///     テーブル情報
        /// </summary>
        public TableInfo TableInfo { get { return this.TableBase.TableInfo; } set { this.TableBase.TableInfo = value; } }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <remarks>継承専用</remarks>
        protected internal DbTableBase()
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="con">コネクション</param>
        public DbTableBase(DbConnection con)
        {
            this.PrepareDbKind(con.DbKind);
            this.CopyConnection(con);
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="trans"></param>
        public DbTableBase(DbTransaction trans)
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
                    this.TableBase = new SQLServer.DbTableBase();
                    this.AccessBase = this.TableBase;
                    this.Transaction = this.TableBase;
                    this.Connection = this.TableBase;
                    break;
                }
            }
        }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内の全レコードを取得する。
        /// </summary>
        /// <returns>データセット</returns>
        public DataSet Select()
        {
            return this.TableBase.Select();
        }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内の全レコードを取得する。カラムを指定する。
        /// </summary>
        /// <param name="lstColumns"></param>
        /// <returns>データセット</returns>
        public DataSet Select(List<string> lstColumns)
        {
            return this.TableBase.Select(lstColumns);
        }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns></returns>
        public DataSet Select(Dictionary<string, object> diWhere)
        {
            return this.TableBase.Select(diWhere);
        }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="strWhere">検索条件</param>
        /// <returns></returns>
        public DataSet Select(string strWhere)
        {
            return this.TableBase.Select(strWhere);
        }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="lstColumns">カラム名</param>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>データセット</returns>
        public DataSet Select(List<string> lstColumns, Dictionary<string, object> diWhere)
        {
            return this.TableBase.Select(lstColumns, diWhere);
        }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="lstColumns">カラム名</param>
        /// <param name="strWhere">検索条件（「Where」は含めない条件式）</param>
        /// <returns>データセット</returns>
        public DataSet Select(List<string> lstColumns, string strWhere)
        {
            return this.TableBase.Select(lstColumns, strWhere);
        }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="lstColumns">カラム名</param>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <param name="lstOrderBy">OrderBy句部分（Columns : カラム名, AscDesc : 昇順または降順）</param>
        /// <returns>データセット</returns>
        public DataSet Select(List<string> lstColumns, Dictionary<string, object> diWhere, List<OrderByInfo<string>> lstOrderBy)
        {
            return this.TableBase.Select(lstColumns, diWhere, lstOrderBy);
        }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="lstColumns">カラム名</param>
        /// <param name="strWhere">検索条件（「Where」は含めない条件式）</param>
        /// <param name="lstOrderBy">OrderBy句部分（Columns : カラム名, AscDesc : 昇順または降順）</param>
        /// <returns>データセット</returns>
        public DataSet Select(List<string> lstColumns, string strWhere, List<OrderByInfo<string>> lstOrderBy)
        {
            return this.TableBase.Select(lstColumns, strWhere, lstOrderBy);
        }

        /// <summary>
        ///     Insert文を実行する。
        /// </summary>
        /// <param name="diInsert">Insertするデータ（Key : カラム名, Value : 値）</param>
        /// <returns>影響を受けた行数</returns>
        public int Insert(Dictionary<string, object> diInsert)
        {
            return this.TableBase.Insert(diInsert);
        }

        /// <summary>
        ///     Update文を実行する。Dictioanryのカラムの中に主キーが含まれていたら、それを検索条件にする。
        /// </summary>
        /// <param name="diUpdate">Updateするデータ（Key : カラム名, Value : 値）</param>
        /// <returns>影響を受けた行数</returns>
        /// <remarks>主キーはUpdateしない。</remarks>
        public int Update(Dictionary<string, object> diUpdate)
        {
            return this.TableBase.Update(diUpdate);
        }

        /// <summary>
        ///     Update文を実行する。
        /// </summary>
        /// <param name="diUpdate">Updateするデータ（Key : カラム名, Value : 値）</param>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>影響を受けた行数</returns>
        /// <remarks>主キーもUpdateする。</remarks>
        public int Update(Dictionary<string, object> diUpdate, Dictionary<string, object> diWhere)
        {
            return this.TableBase.Update(diUpdate, diWhere);
        }

        /// <summary>
        ///     Update文を実行する。
        /// </summary>
        /// <param name="diUpdate">Updateするデータ（Key : カラム名, Value : 値）</param>
        /// <param name="strWhere">検索条件</param>
        /// <returns>影響を受けた行数</returns>
        /// <remarks>主キーもUpdateする。</remarks>
        public int Update(Dictionary<string, object> diUpdate, string strWhere)
        {
            return this.TableBase.Update(diUpdate, strWhere);
        }

        /// <summary>
        ///     Delete文を実行する。
        /// </summary>
        /// <returns>影響を受けた行数</returns>
        public int Delete()
        {
            return this.TableBase.Delete();
        }

        /// <summary>
        ///     Delete文を実行する。
        /// </summary>
        /// <param name="strWhere">検索条件</param>
        /// <returns>影響を受けた行数</returns>
        public int Delete(string strWhere)
        {
            return this.TableBase.Delete(strWhere);
        }

        /// <summary>
        ///     Delete文を実行する。
        /// </summary>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>影響を受けた行数</returns>
        public int Delete(Dictionary<string, object> diWhere)
        {
            return this.TableBase.Delete(diWhere);
        }

        /// <summary>
        ///     Select Func(*) from ...を取得する。（検索条件なし）
        /// </summary>
        /// <param name="functions">関数の種類</param>
        /// <param name="strColumn">カラム名</param>
        /// <returns>取得結果</returns>
        public string SelectFunc(Functions functions, string strColumn)
        {
            return this.TableBase.SelectFunc(functions, strColumn);
        }

        /// <summary>
        ///     Select Func(*) from ... where ...を実行する。
        /// </summary>
        /// <param name="functions">関数の種類</param>
        /// <param name="strColumn">カラム名</param>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>取得結果</returns>
        public string SelectFunc(Functions functions, string strColumn, Dictionary<string, object> diWhere)
        {
            return this.TableBase.SelectFunc(functions, strColumn, diWhere);
        }

        /// <summary>
        ///     Select Func(*) from ... where ...を実行する。
        /// </summary>
        /// <param name="functions">関数の種類</param>
        /// <param name="strColumn">カラム名</param>
        /// <param name="strWhere">検索条件</param>
        /// <returns>取得結果</returns>
        public string SelectFunc(Functions functions, string strColumn, string strWhere)
        {
            return this.TableBase.SelectFunc(functions, strColumn, strWhere);
        }

        /// <summary>
        ///     レコードの個数を取得します。
        /// </summary>
        /// <returns>レコードの個数</returns>
        public long SelectCount()
        {
            return this.TableBase.SelectCount();
        }

        /// <summary>
        ///     レコードの個数を取得します。
        /// </summary>
        /// <param name="strWhere">検索条件</param>
        /// <returns>レコードの個数</returns>
        public long SelectCount(string strWhere)
        {
            return this.TableBase.SelectCount(strWhere);
        }

        /// <summary>
        ///     レコードの個数を取得します。
        /// </summary>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>レコードの個数</returns>
        public long SelectCount(Dictionary<string, object> diWhere)
        {
            return this.TableBase.SelectCount(diWhere);
        }
    }
}
