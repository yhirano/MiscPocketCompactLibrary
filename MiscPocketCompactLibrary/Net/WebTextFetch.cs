#region ディレクティブを使用する

using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;

#endregion

namespace MiscPocketCompactLibrary.Net
{
    /// <summary>
    /// Web上のテキストを取得する
    /// </summary>
    public class WebTextFetch
    {
        /// <summary>
        /// WebStream
        /// </summary>
        private WebStream stream;

        /// <summary>
        /// ダウンロード時のバッファサイズ
        /// </summary>
        private static long downLoadBufferSize = 1024;

        /// <summary>
        /// ダウンロード時のバッファサイズ
        /// </summary>
        public long DownLoadBufferSize
        {
            get
            {
                // 512B～64KB
                if (512 <= downLoadBufferSize && downLoadBufferSize <= 0x10000)
                {
                    return downLoadBufferSize;
                }
                else
                {
                    return 0x8000;
                }
            }
            set
            {
                if (512 <= value && value <= 0x10000)
                {
                    downLoadBufferSize = value;
                }
                else
                {
                    ;
                }
            }
        }

        /// <summary>
        /// テキストのエンコード
        /// </summary>
        private Encoding encoding;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="stream">WebStream</param>
        /// <param name="encoding">テキストのエンコード</param>
        public WebTextFetch(WebStream stream, Encoding encoding)
        {
            this.stream = stream;
            this.encoding = encoding;
        }

        /// <summary>
        /// テキストを最後まで読んで返す
        /// </summary>
        /// <returns>Webの内容のテキスト</returns>
        public string ReadToEnd()
        {
            StringBuilder sb = new StringBuilder();
            StreamReader sr = null;

            try
            {
                sr = new StreamReader(stream, encoding);

                // 応答データをファイルに書き込む
                char[] buf = new char[DownLoadBufferSize / 2];
                int count = 0;
                int alreadyRead = 0;

                OnFetch(new FetchEventArgs(0, stream.StreamLength));

                do
                {
                    count = sr.Read(buf, 0, buf.Length);
                    sb.Append(buf);
                    // すでに読み込んだファイルサイズ
                    alreadyRead += encoding.GetByteCount(buf);
                    OnFetching(new FetchEventArgs(alreadyRead, stream.StreamLength));
                } while (count != 0);

                if (stream.StreamLength != -1 && stream.StreamLength > alreadyRead)
                {
                    throw new WebException();
                }

                OnFetched(new FetchEventArgs(alreadyRead, stream.StreamLength));
            }
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// ファイル取得前イベント
        /// </summary>
        public event FetchEventHandler Fetch;

        /// <summary>
        /// ファイル取得前のイベントの実行
        /// </summary>
        /// <param name="e">イベント</param>
        public void OnFetch(FetchEventArgs e)
        {
            if (Fetch != null)
            {
                Fetch(this, e);
            }
        }

        /// <summary>
        /// ファイル取得中イベント
        /// </summary>
        public event FetchEventHandler Fetching;

        /// <summary>
        /// ファイル取得中のイベントの実行
        /// </summary>
        public void OnFetching(FetchEventArgs e)
        {
            if (Fetching != null)
            {
                Fetching(this, e);
            }
        }

        /// <summary>
        /// ファイル取得後イベント
        /// </summary>
        public event FetchEventHandler Fetched;

        /// <summary>
        /// ファイル取得後のイベントの実行
        /// </summary>
        public void OnFetched(FetchEventArgs e)
        {
            if (Fetched != null)
            {
                Fetched(this, e);
            }
        }
    }
}
