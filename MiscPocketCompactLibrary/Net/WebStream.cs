﻿#region ディレクティブを使用する

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
    /// Webにある情報やファイルを取得するためのクラス
    /// </summary>
    public class WebStream
    {
        /// <summary>
        /// ダウンロード進捗の最小値（0）をセットするデリゲート
        /// </summary>
        /// <param name="minimum">ダウンロード進捗の最小値</param>
        public delegate void SetDownloadProgressMinimumInvoker(int minimum);

        /// <summary>
        /// ダウンロード進捗の最大値（ファイルサイズ）をセットするデリゲート
        /// </summary>
        /// <param name="maximum">ダウンロードの進捗の最大値</param>
        public delegate void SetDownloadProgressMaximumInvoker(int maximum);

        /// <summary>
        /// ダウンロード進捗の状況（すでにダウンロードしたファイルサイズ）をセットするデリゲート
        /// </summary>
        /// <param name="value">ダウンロード進捗の状況</param>
        public delegate void SetDownloadProgressValueInvoker(int value);

        /// <summary>
        /// URL
        /// </summary>
        private Uri url;

        /// <summary>
        /// リクエストメソッド
        /// </summary>
        private string method = string.Empty;

        /// <summary>
        /// リクエストメソッド
        /// </summary>
        public string Method
        {
            get { return method; }
            set { method = value; }
        }

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
        private string userAgent = string.Empty;

        /// <summary>
        /// ユーザーエージェント情報
        /// </summary>
        public string UserAgent
        {
            get { return userAgent; }
            set { userAgent = value; }
        }

        /// <summary>
        /// プロキシの接続方法列挙
        /// </summary>
        public enum ProxyConnect
        {
            Unuse, OsSetting, OriginalSetting
        }

        /// <summary>
        /// プロキシを使うか
        /// </summary>
        private ProxyConnect proxyUse = ProxyConnect.OsSetting;

        /// <summary>
        /// プロキシを使うか
        /// </summary>
        public ProxyConnect ProxyUse
        {
            get { return proxyUse; }
            set { proxyUse = value; }
        }

        /// <summary>
        /// プロキシサーバ
        /// </summary>
        private string proxyServer = string.Empty;

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
            get
            {
                if (0x00 <= proxyPort && proxyPort <= 0xFFFF)
                {
                    return proxyPort;
                }
                else
                {
                    return 0;
                }
            }
            set
            {
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
        /// Web認証情報
        /// </summary>
        NetworkCredential credential;

        /// <summary>
        /// Web認証情報
        /// </summary>
        public NetworkCredential Credential
        {
            get { return credential; }
            set { credential = value; }
        }

        /// <summary>
        /// Webヘッダのコレクション
        /// </summary>
        WebHeaderCollection headers = new WebHeaderCollection();

        /// <summary>
        /// ダウンロードするファイルのサイズ。
        /// GetWebStream()実行時にファイルサイズが分かるので、
        /// GetWebStream()実行前では0が返る。
        /// </summary>
        private long contentLength = 0;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="url">URL</param>
        public WebStream(Uri url)
        {
            this.url = url;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="credential">Web認証情報</param>
        public WebStream(Uri url, NetworkCredential credential)
        {
            this.url = url;
            this.credential = credential;
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="userName">Web認証のユーザ名</param>
        /// <param name="password">Web認証のパスワード</param>
        public WebStream(Uri url, string userName, string password)
        {
            this.url = url;
            credential = new NetworkCredential(userName, password);
        }

        /// <summary>
        /// Web認証情報を削除
        /// </summary>
        public void RemoveCredential()
        {
            credential = null;
        }

        /// <summary>
        /// ヘッダを追加する
        /// </summary>
        /// <param name="header">ヘッダの情報</param>
        public void AddHeader(string header)
        {
            headers.Add(header);
        }

        /// <summary>
        /// ヘッダを追加する
        /// </summary>
        /// <param name="name">ヘッダの名前</param>
        /// <param name="value">ヘッダの値</param>
        public void AddHeader(string name, string value)
        {
            headers.Add(name, value);
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

                // リクエストメソッドの設定
                if (Method != null && Method != string.Empty)
                {
                    req.Method = Method;
                }

                // HTTPプロトコルでネットにアクセスする場合
                if (req.GetType() == typeof(System.Net.HttpWebRequest))
                {
                    // UserAgentを付加
                    ((HttpWebRequest)req).UserAgent = UserAgent;

                    // プロキシの設定が存在した場合、プロキシを設定
                    if (ProxyUse == ProxyConnect.OriginalSetting && ProxyServer.Length != 0)
                    {
                        ((HttpWebRequest)req).Proxy =
                            new WebProxy(ProxyServer, ProxyPort);
                    }
                    // プロキシ設定を使わない場合
                    else if (ProxyUse == ProxyConnect.Unuse)
                    {
                        WebProxy proxy = new WebProxy();
                        proxy.Address = null;
                        ((HttpWebRequest)req).Proxy = proxy;
                    }

                    // ヘッダーを追加する
                    req.Headers.Add(headers);

                    // Web認証でアクセスする場合は
                    if (credential != null)
                    {
                        req.Credentials = credential;
                    }
                }

                WebResponse result = req.GetResponse();
                contentLength = result.ContentLength;
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
            try
            {
                FetchFile(fileName, null, null, null);
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
        }

        /// <summary>
        /// ネット上からファイルをダウンロードする
        /// </summary>
        /// <param name="fileName">保存先のファイル名</param>
        /// <param name="doDownloadProgressMinimum">ファイルサイズの最小値（0）をセットするデリゲート</param>
        /// <param name="doSetDownloadProgressMaximum">ファイルサイズをセットするデリゲート</param>
        /// <param name="doSetDownloadProgressValue">ダウンロード済みのファイルサイズをセットするデリゲート</param>
        public void FetchFile(string fileName,
            SetDownloadProgressMinimumInvoker doDownloadProgressMinimum,
            SetDownloadProgressMaximumInvoker doSetDownloadProgressMaximum,
            SetDownloadProgressValueInvoker doSetDownloadProgressValue)
        {
            Stream st = null;
            FileStream fs = null;

            try
            {
                st = GetWebStream();
                fs = new FileStream(fileName, FileMode.Create, FileAccess.Write);

                int maximum = (int)contentLength;

                if (doDownloadProgressMinimum != null)
                {
                    doDownloadProgressMinimum(0);
                }

                if (doSetDownloadProgressMaximum != null)
                {
                    doSetDownloadProgressMaximum(maximum);
                }

                // 応答データをファイルに書き込む
                Byte[] buf = new Byte[DownLoadBufferSize];
                int count = 0;
                int alreadyWrite = 0;

                do
                {
                    count = st.Read(buf, 0, buf.Length);
                    fs.Write(buf, 0, count);
                    // すでに読み込んだファイルサイズ
                    alreadyWrite += count;

                    // すでに読み込んだファイルサイズが全体のファイルサイズより大きくなることは
                    // 無いはずだが、一応チェックする
                    if (alreadyWrite < maximum)
                    {
                        if (doSetDownloadProgressValue != null)
                        {
                            doSetDownloadProgressValue(alreadyWrite);
                        }
                    }

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
