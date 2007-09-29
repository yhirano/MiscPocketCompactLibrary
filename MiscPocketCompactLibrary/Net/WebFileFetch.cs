#region ディレクティブを使用する

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;

#endregion

namespace MiscPocketCompactLibrary.Net
{
    /// <summary>
    /// Web上のファイルを取得する
    /// </summary>
    public class WebFileFetch
    {
        /// <summary>
        /// WebStream
        /// </summary>
        private WebStream stream;

        /// <summary>
        /// ダウンロード時のバッファサイズ
        /// </summary>
        private static long downLoadBufferSize = 0x8000;    // 32KB

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
        /// コンストラクタ
        /// </summary>
        /// <param name="stream">WebStream</param>
        public WebFileFetch(WebStream stream)
        {
            this.stream = stream;
        }

        /// <summary>
        /// ネット上からファイルをダウンロードする
        /// </summary>
        /// <param name="fileName">保存先のファイル名</param>
        public void FetchFile(string fileName)
        {
            FileStream fs = null;

            try
            {
                fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);

                long seekPos = 0;

                // ファイルに書き込む位置を決定する
                // 206Partial Contentステータスコードが返された時はContent-Rangeヘッダを調べる
                // それ以外のときは、先頭から書き込む
                if (stream.CanResume == true && stream.Webres is HttpWebResponse)
                {
                    if (((HttpWebResponse)stream.Webres).StatusCode == HttpStatusCode.PartialContent)
                    {
                        string contentRange = ((HttpWebResponse)stream.Webres).GetResponseHeader("Content-Range");
                        Match m = Regex.Match(
                            contentRange,
                            @"bytes\s+(?:(?<first>\d*)-(?<last>\d*)|\*)/(?:(?<len>\d+)|\*)");
                        if (m.Groups["first"].Value == string.Empty)
                        {
                            seekPos = 0;
                        }
                        else
                        {
                            seekPos = int.Parse(m.Groups["first"].Value);
                        }
                    }
                    //書き込み位置を変更する
                    fs.SetLength(seekPos);
                    fs.Position = seekPos;
                }

                // 応答データをファイルに書き込む
                byte[] buf = new byte[DownLoadBufferSize];
                int count = 0;
                int alreadyWrite = (int)seekPos;

                OnFetch(new FetchEventArgs(0, stream.StreamLength));

                do
                {
                    count = stream.Read(buf, 0, buf.Length);
                    fs.Write(buf, 0, count);
                    // すでに読み込んだファイルサイズ
                    alreadyWrite += count;
                    OnFetching(new FetchEventArgs(alreadyWrite, stream.StreamLength));
                } while (count != 0);

                if (stream.StreamLength != -1 && stream.StreamLength > alreadyWrite)
                {
                    throw new WebException();
                }

                OnFetched(new FetchEventArgs(alreadyWrite, stream.StreamLength));
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
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
