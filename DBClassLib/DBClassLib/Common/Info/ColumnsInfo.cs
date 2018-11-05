using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBClassLib.Common.Info
{
    /// <summary>
    ///     カラム情報コレクションクラス
    /// </summary>
    public class ColumnsInfo
    {
        private Dictionary<string, ColumnInfo> diSource = new Dictionary<string, ColumnInfo>();

        /// <summary>
        ///     カラム情報を取得する。
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <returns>カラム情報</returns>
        public ColumnInfo this[int index]
        {
            get { return this.diSource[this.GetColumnNameFromIndex(index)]; }
            set { this.diSource[this.GetColumnNameFromIndex(index)] = value; }
        }

        /// <summary>
        ///     カラム情報を取得する。
        /// </summary>
        /// <param name="strColumnName">カラム名</param>
        /// <returns>カラム情報</returns>
        public ColumnInfo this[string strColumnName]
        {
            get { return this.diSource[strColumnName]; }
            set { this.diSource[strColumnName] = value; }
        }

        /// <summary>
        ///     イテレータ
        /// </summary>
        /// <returns>カラム情報のコレクション</returns>
        public IEnumerator<ColumnInfo> GetEnumerator()
        {
            foreach (ColumnInfo column in this.diSource.Values)
                yield return column;
        }

        /// <summary>
        ///     コレクションにカラムを追加する。
        /// </summary>
        /// <param name="column">カラム情報</param>
        public void Add(ColumnInfo column)
        {
            this.diSource.Add(column.ColumnName, column);
        }

        /// <summary>
        ///     コレクションにカラムを追加する・
        /// </summary>
        /// <param name="strName">カラム名</param>
        /// <param name="type">データベース型</param>
        /// <param name="blIsPrimaryKey">主キーかどうか</param>
        /// <param name="blIsNullable">NULL許可するかどうか</param>
        public void Add(string strName, DBDataType type, bool blIsPrimaryKey, bool blIsNullable)
        {
            this.diSource.Add(strName, new ColumnInfo() { ColumnName = strName, DBDataType = type, IsPrimaryKey = blIsPrimaryKey, IsNullable = blIsNullable });
        }

        /// <summary>
        ///     DictionaryでIndexからカラム名を取得します。
        /// </summary>
        /// <param name="intIndex">カラム名</param>
        /// <returns>インデックス</returns>
        private string GetColumnNameFromIndex(int intIndex)
        {
            List<string> lstColumnName = this.diSource.Keys.ToList();

            if (intIndex < 0 || intIndex >= lstColumnName.Count)
            {
                throw new IndexOutOfRangeException("指定されたインデックスは無効です。");
            }

            return lstColumnName[intIndex];
        }
    }
}
