#region ディレクティブを使用する

using System;

#endregion

namespace MiscPocketCompactLibrary.Net
{
    /// <summary>
    /// すでにファイルをゲットしている場合の例外
    /// </summary>
    public class AlreadyFetchFileException : Exception
    {
        public AlreadyFetchFileException() : base()
        {
        }
    }
}
