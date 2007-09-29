#region ディレクティブを使用する

using System;

#endregion

namespace MiscPocketCompactLibrary.Net
{
    /// <summary>
    /// ファイルのダウンロードイベント
    /// </summary>
    public class FetchEventArgs : EventArgs
    {
        /// <summary>
        /// ダウンロード済みのサイズ
        /// </summary>
        private long fetchedSize;

        /// <summary>
        /// ダウンロード済みのサイズ
        /// </summary>
        public long FetchedSize
        {
            get { return fetchedSize; }
        }

        /// <summary>
        /// ファイルサイズ
        /// </summary>
        private long contentSize;

        /// <summary>
        /// ファイルサイズ
        /// </summary>
        public long ContentSize
        {
            get { return contentSize; }
        }

        /// <summary>
        /// ファイルサイズは不明かを取得する
        /// </summary>
        public bool IsUnknownContentSize
        {
            get { return (contentSize < 0); }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fetchedSize">ダウンロード済みのサイズ</param>
        /// <param name="contentSize">ファイルサイズ。不明の場合はマイナスの値を入れてください。</param>
        public FetchEventArgs(long fetchedSize, long contentSize)
        {
            this.fetchedSize = fetchedSize;
            this.contentSize = contentSize;
        }
    }
}
