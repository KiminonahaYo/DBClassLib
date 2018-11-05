using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBClassLib.All.Base;
using DBClassLib.Common;
using DBClassLib.Common.Info;

namespace DBClassLib.All.Middle
{
    /// <summary>
    ///     データベーステーブル中間ライブラリ基底クラス
    /// </summary>
    public abstract class DbTableBaseMiddle<TColumnEnum> : DBClassLib.All.Base.DbTableBase where TColumnEnum : Enum
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="con">コネクション</param>
        public DbTableBaseMiddle(DbConnection con) : base(con)
        {
            this.TableInfo = this.GetTableInfo();
            this.Schema = null;
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="trans"></param>
        public DbTableBaseMiddle(DbTransaction trans) : base(trans)
        {
            this.TableInfo = this.GetTableInfo();
            this.Schema = null;
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="con">コネクション</param>
        /// <param name="strSchema">スキーマ名</param>
        public DbTableBaseMiddle(DbConnection con, string strSchema) : base(con)
        {
            this.TableInfo = this.GetTableInfo();
            this.Schema = strSchema;
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="trans"></param>
        /// <param name="strSchema">スキーマ名</param>
        public DbTableBaseMiddle(DbTransaction trans, string strSchema) : base(trans)
        {
            this.TableInfo = this.GetTableInfo();
            this.Schema = strSchema;
        }

        /// <summary>
        ///     スキーマ名
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        ///     テーブル情報を取得します。
        /// </summary>
        /// <returns>テーブル情報</returns>
        public abstract TableInfo GetTableInfo();

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内の全レコードを取得する。カラムを指定する。
        /// </summary>
        /// <param name="lstColumns"></param>
        /// <returns>データセット</returns>
        public DataSet Select(List<TColumnEnum> lstColumns)
        {
            return this.Select(this.GetList(lstColumns));
        }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns></returns>
        public DataSet Select(Dictionary<TColumnEnum, object> diWhere)
        {
            return this.Select(this.GetDic(diWhere));
        }
        
        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="lstColumns">カラム名</param>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>データセット</returns>
        public DataSet Select(List<TColumnEnum> lstColumns, Dictionary<TColumnEnum, object> diWhere)
        {
            return this.Select(this.GetList(lstColumns), this.GetDic(diWhere));
        }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="lstColumns">カラム名</param>
        /// <param name="strWhere">検索条件（「Where」は含めない条件式）</param>
        /// <returns>データセット</returns>
        public DataSet Select(List<TColumnEnum> lstColumns, string strWhere)
        {
            return this.Select(this.GetList(lstColumns), strWhere);
        }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="lstColumns">カラム名</param>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <param name="lstOrderBy">OrderBy句部分（Columns : カラム名, AscDesc : 昇順または降順）</param>
        /// <returns>データセット</returns>
        public DataSet Select(List<TColumnEnum> lstColumns, Dictionary<TColumnEnum, object> diWhere, List<OrderByInfo<TColumnEnum>> lstOrderBy)
        {
            return this.Select(this.GetList(lstColumns), this.GetDic(diWhere), this.GetListOrderBy(lstOrderBy));
        }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="lstColumns">カラム名</param>
        /// <param name="strWhere">検索条件（「Where」は含めない条件式）</param>
        /// <param name="lstOrderBy">OrderBy句部分（Columns : カラム名, AscDesc : 昇順または降順）</param>
        /// <returns>データセット</returns>
        public DataSet Select(List<TColumnEnum> lstColumns, string strWhere, List<OrderByInfo<TColumnEnum>> lstOrderBy)
        {
            return this.Select(this.GetList(lstColumns), strWhere, this.GetListOrderBy(lstOrderBy));
        }

        /// <summary>
        ///     Insert文を実行する。
        /// </summary>
        /// <param name="diInsert">Insertするデータ（Key : カラム名, Value : 値）</param>
        /// <returns>影響を受けた行数</returns>
        public int Insert(Dictionary<TColumnEnum, object> diInsert)
        {
            return this.Insert(this.GetDic(diInsert));
        }

        /// <summary>
        ///     Update文を実行する。Dictioanryのカラムの中に主キーが含まれていたら、それを検索条件にする。
        /// </summary>
        /// <param name="diUpdate">Updateするデータ（Key : カラム名, Value : 値）</param>
        /// <returns>影響を受けた行数</returns>
        /// <remarks>主キーはUpdateしない。</remarks>
        public int Update(Dictionary<TColumnEnum, object> diUpdate)
        {
            return this.Update(this.GetDic(diUpdate));
        }

        /// <summary>
        ///     Update文を実行する。
        /// </summary>
        /// <param name="diUpdate">Updateするデータ（Key : カラム名, Value : 値）</param>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>影響を受けた行数</returns>
        /// <remarks>主キーもUpdateする。</remarks>
        public int Update(Dictionary<TColumnEnum, object> diUpdate, Dictionary<TColumnEnum, object> diWhere)
        {
            return this.Update(this.GetDic(diUpdate), this.GetDic(diWhere));
        }

        /// <summary>
        ///     Update文を実行する。
        /// </summary>
        /// <param name="diUpdate">Updateするデータ（Key : カラム名, Value : 値）</param>
        /// <param name="strWhere">検索条件</param>
        /// <returns>影響を受けた行数</returns>
        /// <remarks>主キーもUpdateする。</remarks>
        public int Update(Dictionary<TColumnEnum, object> diUpdate, string strWhere)
        {
            return this.Update(this.GetDic(diUpdate), strWhere);
        }
        
        /// <summary>
        ///     Delete文を実行する。
        /// </summary>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>影響を受けた行数</returns>
        public int Delete(Dictionary<TColumnEnum, object> diWhere)
        {
            return this.Delete(this.GetDic(diWhere));
        }

        /// <summary>
        ///     Select Func(*) from ...を取得する。（検索条件なし）
        /// </summary>
        /// <param name="functions">関数の種類</param>
        /// <param name="column">カラム名</param>
        /// <returns>取得結果</returns>
        public string SelectFunc(Functions functions, TColumnEnum column)
        {
            return this.SelectFunc(functions, column.ToString());
        }

        /// <summary>
        ///     Select Func(*) from ... where ...を実行する。
        /// </summary>
        /// <param name="functions">関数の種類</param>
        /// <param name="column">カラム名</param>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>取得結果</returns>
        public string SelectFunc(Functions functions, TColumnEnum column, Dictionary<TColumnEnum, object> diWhere)
        {
            return this.SelectFunc(functions, column.ToString(), this.GetDic(diWhere));
        }

        /// <summary>
        ///     Select Func(*) from ... where ...を実行する。
        /// </summary>
        /// <param name="functions">関数の種類</param>
        /// <param name="column">カラム名</param>
        /// <param name="strWhere">検索条件</param>
        /// <returns>取得結果</returns>
        public string SelectFunc(Functions functions, TColumnEnum column, string strWhere)
        {
            return this.SelectFunc(functions, column.ToString(), strWhere);
        }
        
        /// <summary>
        ///     レコードの個数を取得します。
        /// </summary>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>レコードの個数</returns>
        public long SelectCount(Dictionary<TColumnEnum, object> diWhere)
        {
            return this.SelectCount(this.GetDic(diWhere));
        }

        /// <summary>
        ///     カラムを含んだListを変換する。
        /// </summary>
        /// <param name="lst">変換前リスト</param>
        /// <returns>変換後リスト</returns>
        private List<string> GetList(List<TColumnEnum> lst)
        {
            if (lst == null) return null;

            return lst.Select(col => col.ToString()).ToList();
        }

        /// <summary>
        ///     OrderBy句部分のListを変換する。
        /// </summary>
        /// <param name="lst">変換前リスト</param>
        /// <returns>変換後リスト</returns>
        private List<OrderByInfo<string>> GetListOrderBy(List<OrderByInfo<TColumnEnum>> lst)
        {
            if (lst == null) return null;

            return lst.Select(cls => new OrderByInfo<string> { Column = cls.Column.ToString(), AscDesc = cls.AscDesc }).ToList();
        }

        /// <summary>
        ///     カラムを含んだDictionaryを変換する。
        /// </summary>
        /// <param name="di">変換前Dictionary</param>
        /// <returns>変換後Dictionary</returns>
        private Dictionary<string, object> GetDic(Dictionary<TColumnEnum, object> di)
        {
            if (di == null) return null;

            Dictionary<string, object> diRet = new Dictionary<string, object>();
            foreach (KeyValuePair<TColumnEnum, object> pair in di)
            {
                diRet.Add(pair.Key.ToString(), pair.Value);
            }

            return diRet;
        }
    }
}
