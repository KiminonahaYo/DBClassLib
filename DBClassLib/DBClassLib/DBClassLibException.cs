using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBClassLib
{
    /// <summary>
    ///     DBClassLib例外クラス
    /// </summary>
    public class DBClassLibException : SystemException
    {
        /// <summary>
        ///     コンストラクタ
        /// </summary>
        public DBClassLibException() : base("DBClassLib内部で例外が発生しました。")
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public DBClassLibException(string message) : base(message)
        {
        }

        /// <summary>
        ///     コンストラクタ
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        /// <param name="innerException">内部で発生した例外</param>
        public DBClassLibException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
