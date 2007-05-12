#region ディレクティブを使用する

using System;

#endregion

namespace MiscPocketCompactLibrary.Net
{
    /// <summary>
    /// ゲットすべきファイルが正しくないと思われる場合の例外
    /// </summary>
    public class MismatchFetchFileException : Exception
    {
        public MismatchFetchFileException()
            : base()
        {
        }
    }
}
