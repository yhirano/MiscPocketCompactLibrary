#region ディレクティブを使用する

using System;

#endregion

namespace MiscPocketCompactLibrary.Net
{
    /// <summary>
    /// ファイル取得イベントのハンドラ
    /// </summary>
    /// <param name="sender">イベントを発信したオブジェクト</param>
    /// <param name="e">イベント</param>
    public delegate void FetchEventHandler(object sender, FetchEventArgs e);
}
