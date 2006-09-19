#region ディレクティブを使用する

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Collections;
using System.Xml;

#endregion

namespace MiscPocketCompactLibrary.Net
{
    /// <summary>
    /// WebStream の概要の説明です。
    /// </summary>
    public class WebStream
    {
        /// <summary>
        /// URL
        /// </summary>
        private Uri url;

        /// <summary>
        /// 接続のタイムアウト時間
        /// </summary>
        int timeOut = 20000;

        /// <summary>
        /// 接続のタイムアウト時間
        /// </summary>
        public int TimeOut
        {
            get { return timeOut; }
            set { timeOut = value; }
        }

        /// <summary>
        /// ユーザーエージェント情報
        /// </summary>
        private string userAgent = "";

        /// <summary>
        /// ユーザーエージェント情報
        /// </summary>
        public string UserAgent
        {
            get { return userAgent; }
            set { userAgent = value; }
        }

        /// <summary>
        /// プロキシを使うか
        /// </summary>
        private bool proxyUse = false;

        /// <summary>
        /// プロキシを使うか
        /// </summary>
        public bool ProxyUse
        {
            get { return proxyUse; }
            set { proxyUse = value; }
        }

        /// <summary>
        /// プロキシサーバ
        /// </summary>
        private string proxyServer = "";

        /// <summary>
        /// プロキシサーバ
        /// </summary>
        public string ProxyServer
        {
            get { return proxyServer; }
            set { proxyServer = value; }
        }

        /// <summary>
        /// プロキシポート
        /// </summary>
        private int proxyPort = 0;

        /// <summary>
        /// プロキシポート
        /// </summary>
        public int ProxyPort
        {
            get {
                if (0x00 <= proxyPort && proxyPort <= 0xFFFF)
                {
                    return proxyPort;
                }
                else
                {
                    return 0;
                }
            }
            set {
                if (0x00 <= value && value <= 0xFFFF)
                {
                    proxyPort = value;
                }
                else
                {
                    ;
                }
            }
        }

        /// <summary>
        /// ダウンロード時のバッファサイズ
        /// </summary>
        private static long downLoadBufferSize = 0x1FFFF; // 128KB

        /// <summary>
        /// ダウンロード時のバッファサイズ
        /// </summary>
        public long DownLoadBufferSize
        {
            get
            {
                if (0xFFFF <= downLoadBufferSize && downLoadBufferSize <= 0x3FFFF)
                {
                    return downLoadBufferSize;
                }
                else
                {
                    return 0x1FFFF;
                }
            }
            set
            {
                if (0xFFFF <= value && value <= 0x3FFFF)
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
        /// <param name="url">URL</param>
        public WebStream(Uri url)
        {
            this.url = url;
        }

        /// <summary>
        /// HTTPレスポンスをストリームとして返す。
        /// </summary>
        /// <returns>HTTPレスポンスのストリーム</returns>
        public Stream GetWebStream()
        {
            Stream st = null;
            FileStream fs = null;

            try
            {
                // Urlがファイル指定の場合はfile streamを返す
                if (url.IsFile == true)
                {
                    fs = new FileStream(url.LocalPath, FileMode.Open, FileAccess.Read);
                    return fs;
                }

                WebRequest req = WebRequest.Create(url);
                req.Timeout = TimeOut;

                // HTTPプロトコルでネットにアクセスする場合
                if (req.GetType() == typeof(System.Net.HttpWebRequest))
                {
                    // UserAgentを付加
                    ((HttpWebRequest)req).UserAgent = UserAgent;

                    // プロキシの設定が存在した場合、プロキシを設定
                    if (ProxyUse == true && ProxyServer.Length != 0)
                    {
                        ((HttpWebRequest)req).Proxy =
                            new WebProxy(ProxyServer, ProxyPort);
                    }
                }

                WebResponse result = req.GetResponse();
                st = result.GetResponseStream();
            }
            catch (WebException)
            {
                throw;
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (IOException)
            {
                throw;
            }
            catch (UriFormatException)
            {
                throw;
            }
            catch (SocketException)
            {
                throw;
            }

            return st;
        }

        /// <summary>
        /// ネット上からファイルをダウンロードする
        /// </summary>
        /// <param name="fileName">保存先のファイル名</param>
        public void FetchFile(string fileName)
        {
            Stream st = null;
            FileStream fs = null;

            try
            {
                st = GetWebStream();
                fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);

                // 応答データをファイルに書き込む
                Byte[] buf = new Byte[DownLoadBufferSize];
                int count = 0;

                do
                {
                    count = st.Read(buf, 0, buf.Length);
                    fs.Write(buf, 0, count);
                } while (count != 0);
            }
            catch (WebException)
            {
                throw;
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (IOException)
            {
                throw;
            }
            catch (UriFormatException)
            {
                throw;
            }
            catch (SocketException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
                if (st != null)
                {
                    st.Close();
                }
            }
        }
    }
}
