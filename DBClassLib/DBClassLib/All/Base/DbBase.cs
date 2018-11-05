using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBClassLib.All.Base
{
    /// <summary>
    ///     データベース基底クラス
    /// </summary>
    public abstract class DbBase
    {
        /// <summary>
        ///     データベースの種類
        /// </summary>
        public virtual DbKind DbKind{ get; protected set; }

        /// <summary>
        ///     このクラスのコピーを行う。
        /// </summary>
        /// <param name="to">コピー先</param>
        protected internal virtual void Copy(object to)
        {
            if (to is DbBase dbBase)
            {
                dbBase.DbKind = this.DbKind;
            }
            else
            {
                throw new ArgumentException("指定されたオブジェクトは" + this.GetType().ToString() + "ではありません。「" + (to != null ? to.GetType().ToString() : "【NULL】") + "」です。");
            }
        }
    }
}
