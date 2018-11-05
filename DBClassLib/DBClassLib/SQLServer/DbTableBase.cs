using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBClassLib.Common;
using DBClassLib.Common.Info;

namespace DBClassLib.SQLServer
{
    /// <summary>
    ///     SQL Server データベーステーブル基底クラス
    /// </summary>
    public class DbTableBase : DbAccessBase, IDbTableBase
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <remarks>DBClassLib.All.Baseで使用するときのみ有効</remarks>
        internal DbTableBase()
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="con">コネクション</param>
        public DbTableBase(DbConnection con) : base(con)
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="trans">トランザクション</param>
        public DbTableBase(DbTransaction trans) : base(trans)
        {
        }

        /// <summary>
        ///     テーブル情報
        /// </summary>
        public TableInfo TableInfo { get; set; }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内の全レコードを取得する。
        /// </summary>
        /// <returns>データセット</returns>
        public DataSet Select()
        {
            return this.Select(null, "");
        }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内の全レコードを取得する。カラムを指定する。
        /// </summary>
        /// <param name="lstColumns"></param>
        /// <returns>データセット</returns>
        public DataSet Select(List<string> lstColumns)
        {
            return this.Select(lstColumns, "");
        }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns></returns>
        public DataSet Select(Dictionary<string, object> diWhere)
        {
            return this.Select(null, diWhere);
        }

        /// <summary>
        ///     Select文を実行する。
        ///     テーブル内のレコードを取得する。
        /// </summary>
        /// <param name="strWhere">検索条件</param>
        /// <returns></returns>
        public DataSet Select(string strWhere)
        {
            return this.Select(null, strWhere);
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
            return this.Select(lstColumns, diWhere, null);
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
            return this.Select(lstColumns, strWhere, null);
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
            return this.Select(lstColumns, this.CreateSimpleWhere(diWhere, false), lstOrderBy);
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
            try
            {
                string strQuery = "";

                string strTableName = this.GetTableName();

                strQuery += "Select ";

                if (lstColumns == null || lstColumns.Count == 0)
                {
                    //カラム名が指定されていない場合
                    strQuery += "* ";
                }
                else
                {
                    //カラム名が指定されている場合
                    strQuery += string.Join(", ", lstColumns.ToArray());
                }

                strQuery += " from " + strTableName;

                if (strWhere != null && strWhere.Trim().Length > 0)
                {
                    //Where句が指定されている場合
                    strQuery += " Where " + strWhere;
                }

                if (lstOrderBy != null && lstOrderBy.Count > 0)
                {
                    //Order By句が指定されている場合
                    strQuery += " order by ";
                    strQuery += string.Join(", ", lstOrderBy.Select(cls => cls.Column + " " + cls.AscDesc.ToString()).ToArray());
                }

                return this.ExecuteDataSet(strQuery);
            }
            catch (Exception ex)
            {
                throw new DBClassLibException("Select文の実行に失敗しました。", ex);
            }
        }

        /// <summary>
        ///     Insert文を実行する。
        /// </summary>
        /// <param name="diInsert">Insertするデータ（Key : カラム名, Value : 値）</param>
        /// <returns>影響を受けた行数</returns>
        public int Insert(Dictionary<string, object> diInsert)
        {
            try
            {
                string strQuery = "";

                string strTableName = this.GetTableName();

                strQuery += "Insert Into " + strTableName;
                strQuery += " (" + string.Join(", ", diInsert.Keys.ToArray()) + ") ";
                strQuery += " values ";
                strQuery += " (" + string.Join(", ", diInsert.Select(
                    pair =>
                    {
                        if (this.TableInfo.Columns[pair.Key].DBDataType == DBDataType.Date && pair.Value != null && pair.Value is string && (new string[] { "SYSDATE", "GETDATE", "GETDATE()" }).Contains(pair.Value.ToString().ToUpper()))
                        {
                            //現在時刻を格納するための文字列が指定された
                            return "GetDate()";
                        }
                        else
                        {
                            //通常時
                            this.AddParameter("@Insert_" + pair.Key, pair.Value, this.TableInfo.Columns[pair.Key].DBDataType);
                            return "@Insert_" + pair.Key;
                        }
                    }
                    ).ToArray()) + ") ";

                return this.ExecuteNonQuery(strQuery);
            }
            catch (Exception ex)
            {
                throw new DBClassLibException("Insert文の実行に失敗しました。", ex);
            }
        }

        /// <summary>
        ///     Update文を実行する。Dictioanryのカラムの中に主キーが含まれていたら、それを検索条件にする。
        /// </summary>
        /// <param name="diUpdate">Updateするデータ（Key : カラム名, Value : 値）</param>
        /// <returns>影響を受けた行数</returns>
        /// <remarks>主キーはUpdateしない。</remarks>
        public int Update(Dictionary<string, object> diUpdate)
        {
            return this.Update(diUpdate, this.CreateSimpleWhere(diUpdate, true), false);
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
            return this.Update(diUpdate, this.CreateSimpleWhere(diWhere, false), true);
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
            return this.Update(diUpdate, strWhere, true);
        }

        /// <summary>
        ///     Update文を実行する。
        /// </summary>
        /// <param name="diUpdate">Updateするデータ（Key : カラム名, Value : 値）</param>
        /// <param name="strWhere">検索条件</param>
        /// <param name="blAcceptUpdatePrimaryKey">主キーをUpdateするかどうか</param>
        /// <returns>影響を受けた行数</returns>
        private int Update(Dictionary<string, object> diUpdate, string strWhere, bool blAcceptUpdatePrimaryKey)
        {
            try
            {
                string strQuery = "";

                string strTableName = this.GetTableName();

                strQuery += "Update " + strTableName + " Set ";
                strQuery += string.Join(", ", diUpdate.Where(pair => !this.TableInfo.Columns[pair.Key].IsPrimaryKey || blAcceptUpdatePrimaryKey).Select(
                    pair =>
                    {
                        ColumnInfo column = this.TableInfo.Columns[pair.Key];

                        if (column.DBDataType == DBDataType.Date && pair.Value != null && pair.Value is string && (new string[] { "SYSDATE", "GETDATE", "GETDATE()" }).Contains(pair.Value.ToString().ToUpper()))
                        {
                            //現在時刻を格納するための文字列が指定された
                            return pair.Key + " = GetDate()";
                        }
                        else
                        {
                            //通常時
                            this.AddParameter("@Update_" + pair.Key, pair.Value, this.TableInfo.Columns[pair.Key].DBDataType);
                            return pair.Key + " = @Update_" + pair.Key;
                        }
                    }
                    ).ToArray());

                if (strWhere != null && strWhere.Trim().Length > 0)
                {
                    //Where句が指定されている場合
                    strQuery += " Where " + strWhere;
                }

                return this.ExecuteNonQuery(strQuery);
            }
            catch (Exception ex)
            {
                throw new DBClassLibException("Update文の実行に失敗しました。", ex);
            }
        }

        /// <summary>
        ///     Delete文を実行する。
        /// </summary>
        /// <returns>影響を受けた行数</returns>
        public int Delete()
        {
            return this.Delete("");
        }

        /// <summary>
        ///     Delete文を実行する。
        /// </summary>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>影響を受けた行数</returns>
        public int Delete(Dictionary<string, object> diWhere)
        {
            return this.Delete(this.CreateSimpleWhere(diWhere, false));
        }

        /// <summary>
        ///     Delete文を実行する。
        /// </summary>
        /// <param name="strWhere">検索条件</param>
        /// <returns>影響を受けた行数</returns>
        public int Delete(string strWhere)
        {
            try
            {
                string strQuery = "";

                string strTableName = this.GetTableName();

                strQuery += "Delete From " + strTableName;

                if (strWhere != null && strWhere.Trim().Length > 0)
                {
                    //Where句が指定されている場合
                    strQuery += " Where " + strWhere;
                }

                return this.ExecuteNonQuery(strQuery);
            }
            catch (Exception ex)
            {
                throw new DBClassLibException("Delete文の実行に失敗しました。", ex);
            }
        }

        /// <summary>
        ///     Select Func(*) from ...を取得する。（検索条件なし）
        /// </summary>
        /// <param name="functions">関数の種類</param>
        /// <param name="strColumn">カラム名</param>
        /// <returns>取得結果</returns>
        public string SelectFunc(Functions functions, string strColumn)
        {
            return this.SelectFunc(functions, strColumn, "");
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
            return this.SelectFunc(functions, strColumn, this.CreateSimpleWhere(diWhere, false));
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
            try
            {
                string strQuery = "";

                string strTableName = this.GetTableName();

                strQuery += "Select " + functions.ToString();
                strQuery += "(";
                strQuery += (strColumn == null || strColumn.Trim().Length == 0) ? "*" : strColumn;
                strQuery += ") as VAL";
                strQuery += " From " + strTableName;

                if (strWhere != null && strWhere.Trim().Length > 0)
                {
                    //Where句が指定されている場合
                    strQuery += " Where " + strWhere;
                }

                using (DataSet ds = this.ExecuteDataSet(strQuery))
                {
                    object obj = ds.Tables[0].Rows[0]["VAL"];
                    return obj == DBNull.Value || obj == null ? null : obj.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new DBClassLibException("Select文（関数付き）の実行に失敗しました。", ex);
            }
        }

        /// <summary>
        ///     レコードの個数を取得します。
        /// </summary>
        /// <returns>レコードの個数</returns>
        public long SelectCount()
        {
            return this.SelectCount("");
        }

        /// <summary>
        ///     レコードの個数を取得します。
        /// </summary>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <returns>レコードの個数</returns>
        public long SelectCount(Dictionary<string, object> diWhere)
        {
            return this.SelectCount(this.CreateSimpleWhere(diWhere, false));
        }

        /// <summary>
        ///     レコードの個数を取得します。
        /// </summary>
        /// <param name="strWhere">検索条件</param>
        /// <returns>レコードの個数</returns>
        public long SelectCount(string strWhere)
        {
            return long.Parse(this.SelectFunc(Functions.Count, null, strWhere));
        }

        /// <summary>
        ///     シンプルなWhere句を作成します。（「Where」は含みません。）
        ///     カラム = 値 And カラム = 値 And...の式でつなぎます。
        /// </summary>
        /// <param name="diWhere">検索条件（Key : カラム名, Value : 値）</param>
        /// <param name="blPrimaryKeyOnly">主キーのみを検索条件に含めるかどうか</param>
        /// <returns></returns>
        public string CreateSimpleWhere(Dictionary<string, object> diWhere, bool blPrimaryKeyOnly)
        {
            string strQuery = "";

            foreach ((string Column, object value) pair in diWhere.Select(pair => (pair.Key, pair.Value)))
            {
                ColumnInfo columnInfo = this.TableInfo.Columns[pair.Column];
                string strParameterKey = string.Format("Where_{0}", pair.Column);

                if ((columnInfo.IsPrimaryKey && blPrimaryKeyOnly) || !blPrimaryKeyOnly)
                {
                    //※主キーのみを検索条件に含める場合は主キーのときのみこのロジックを通過する。
                    if (pair.value != null && pair.value != DBNull.Value)
                    {
                        //null以外のとき
                        strQuery += string.Format("{0} = @{1} And", pair.Column, strParameterKey);
                        this.AddParameter(strParameterKey, pair.value, columnInfo.DBDataType);
                    }
                    else
                    {
                        //nullの時
                        strQuery += string.Format("{0} is null And", pair.Column);
                    }
                }
            }

            if (strQuery.EndsWith("And")) strQuery = strQuery.Substring(0, strQuery.Length - 3);

            return strQuery;
        }

        /// <summary>
        ///     テーブル名を取得する。
        /// </summary>
        /// <returns>テーブル名</returns>
        private string GetTableName()
        {
            if (this.TableInfo.Schema == null || this.TableInfo.Schema.Trim().Length == 0)
            {
                //スキーマ名が指定されていない場合
                return this.TableInfo.TableName;
            }
            else
            {
                return this.TableInfo.Schema + "." + this.TableInfo.TableName;
            }
        }
    }
}
