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
    /// Webにある情報やファイルを取得するためのクラス
    /// </summary>
    public class WebStream : Stream
    {
        /// <summary>
        /// ストリーム
        /// </summary>
        private Stream st;

        /// <summary>
        /// Webレスポンス
        /// </summary>
        private WebResponse webres;

        /// <summary>
        /// Webレスポンス
        /// </summary>
        internal WebResponse Webres
        {
            get { return webres; }
        }

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
        /// Web認証情報
        /// </summary>
        private NetworkCredential credential;

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
        private WebHeaderCollection headers = new WebHeaderCollection();

        /// <summary>
        /// Webリクエスト
        /// </summary>
        private WebRequest req;

        /// <summary>
        /// ストリーム全体のファイルサイズ。
        /// リジュームを指定されている場合でも、全体のファイルサイズを示す。
        /// </summary>
        private long streamLength = -1;

        /// <summary>
        /// ストリーム全体のファイルサイズ。
        /// リジュームを指定されている場合でも、全体のファイルサイズを示す。
        /// ファイルサイズが分からない場合は-1。
        /// </summary>
        internal long StreamLength
        {
            get { return streamLength; }
        }

        /// <summary>
        /// リジュームをするか。
        /// リジュームするという指定があった場合にtrue。
        /// </summary>
        private bool resume;

        /// <summary>
        /// リジュームできるか。
        /// リジュームができる場合にtrue。
        /// </summary>
        private bool resumeProgressKnown;

        /// <summary>
        /// リジュームできるかを取得する。
        /// </summary>
        public bool CanResume
        {
            get { return (resume == true && resumeProgressKnown == true && (req is HttpWebRequest)); }
        }

        /// <summary>
        /// ファイルレジュームの位置（すでに取得しているバイト数）
        /// </summary>
        private long alreadyGetFile = 0;

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
        /// レジュームをするようにセットする
        /// </summary>
        /// <param name="fileName">レジュームをするファイル名</param>
        public void SetResume(string fileName)
        {
            if (File.Exists(fileName) == false)
            {
                resume = false;
                return;
            }

            alreadyGetFile = (new System.IO.FileInfo(fileName)).Length;

            // ファイルがあって、かつファイルの長さが0でない場合にレジュームフラグを立てる
            if (alreadyGetFile == 0)
            {
                resume = false;
                File.Delete(fileName);
            }
            else
            {
                resume = true;
            }
        }

        /// <summary>
        /// レジュームをしないようにする
        /// </summary>
        public void RemoveResume()
        {
            alreadyGetFile = 0;
            resume = false;
            resumeProgressKnown = false;
        }

        /// <summary>
        /// HTTPレスポンスをストリームとして返す。
        /// </summary>
        /// <returns>HTTPレスポンスのストリーム</returns>
        public void CreateWebStream()
        {
            // Urlがファイル指定の場合はfile streamを返す
            if (url.IsFile == true)
            {
                st = new FileStream(url.LocalPath, FileMode.Open, FileAccess.Read);
                return;
            }

            // ストリーム全体のファイルサイズを得る
            streamLength = GetFileSize();

            // すでにストリームのファイルが存在する場合
            if (resume == true && streamLength == alreadyGetFile)
            {
                throw new AlreadyFetchFileException();
            }

            // これからゲットするファイルよりもすでに落としたファイルの方が大きい場合
            if (resume == true && streamLength < alreadyGetFile)
            {
                throw new MismatchFetchFileException();
            }

            // Webリクエストを作成する
            req = MakeHttpWebRequest();

            // レジュームの指定がされている場合は
            if (CanResume == true)
            {
                // ここに入る場合は必ずreqの型はHttpWebRequestとなるはずだが一応チェックする
                if (req is HttpWebRequest)
                {
                    // バイトレンジを指定する
                    ((HttpWebRequest)req).AddRange((int)alreadyGetFile);
                }
            }

            webres = req.GetResponse();
            st = webres.GetResponseStream();
        }

        /// <summary>
        /// HTTP Webリクエストを作成する
        /// </summary>
        /// <returns>作成したHTTP Webリクエスト</returns>
        private WebRequest MakeHttpWebRequest()
        {
            WebRequest req = WebRequest.Create(url);
            req.Timeout = TimeOut;

            // リクエストメソッドの設定
            if (Method != null && Method != string.Empty)
            {
                req.Method = Method;
            }

            // HTTPプロトコルでネットにアクセスする場合
            if (req is HttpWebRequest)
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

                // 指定されたヘッダーを追加する
                req.Headers.Add(headers);

                // そのほかのヘッダを指定する
                ((HttpWebRequest)req).KeepAlive = false;
                req.Headers.Add("Pragma", "no-cache");
                req.Headers.Add("Cache-Control", "no-cache");

                // Web認証でアクセスする場合は
                if (credential != null)
                {
                    req.Credentials = credential;
                }
            }
            return req;
        }

        /// <summary>
        /// ストリーム全体のファイルサイズを得る
        /// </summary>
        /// <returns>ストリーム全体のファイルサイズ</returns>
        private long GetFileSize()
        {
            WebResponse response = null;
            long size = -1;

            try
            {
                WebRequest request = MakeHttpWebRequest();
                request.Method = "HEAD";
                response = request.GetResponse();

                try
                {
                    size = long.Parse(response.Headers["Content-Length"]);
                }
                catch (ArgumentException)
                {
                    ;
                }
                catch (FormatException)
                {
                    ;
                }
                catch (OverflowException)
                {
                    ;
                }

                if (size == -1)
                {
                    resumeProgressKnown = false;
                }
                else
                {
                    resumeProgressKnown = true;
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            return size;
        }

        /// <summary>
        /// ストリームが読み取りをサポートするかどうかを示す値を取得します。
        /// </summary>
        public override bool CanRead
        {
            get { return st.CanRead; }
        }

        /// <summary>
        /// ストリームがシークをサポートするかどうかを示す値を取得します。
        /// </summary>
        public override bool CanSeek
        {
            get
            {
                //return st.CanSeek;
                return false;
            }
        }

        /// <summary>
        /// ストリームが書き込みをサポートするかどうかを示す値を取得します。
        /// </summary>
        public override bool CanWrite
        {
            get { return st.CanWrite; }
        }

        /// <summary>
        /// ストリームに対応するすべてのバッファをクリアし、バッファ内のデータを基になるデバイスに書き込みます。 
        /// </summary>
        public override void Flush()
        {
            st.Flush();
        }

        /// <summary>
        /// ストリームの長さをバイト単位で取得します。
        /// </summary>
        public override long Length
        {
            get { return st.Length; }
        }

        /// <summary>
        /// ストリーム内の位置を取得または設定します。
        /// </summary>
        public override long Position
        {
            get { return st.Position; }
            set { st.Position = value; }
        }

        /// <summary>
        /// ストリームからバイト シーケンスを読み取り、読み取ったバイト数の分だけストリームの位置を進めます。 
        /// </summary>
        /// <param name="buffer">バイト配列。このメソッドが戻るとき、指定したバイト配列の offset から (offset + count -1) までの値が、現在のソースから読み取られたバイトに置き換えられます。</param>
        /// <param name="offset">ストリームから読み取ったデータの格納を開始する位置を示す buffer内のバイト オフセット。インデックス番号は 0 から始まります。</param>
        /// <param name="count">ストリームから読み取る最大バイト数。</param>
        /// <returns>バッファに読み取られた合計バイト数。要求しただけのバイト数を読み取ることができなかった場合、この値は要求したバイト数より小さくなります。ストリームの末尾に到達した場合は 0 (ゼロ) になることがあります。</returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            return st.Read(buffer, offset, count);
        }

        /// <summary>
        /// ストリーム内の位置を設定します。 
        /// </summary>
        /// <param name="offset">origin パラメータからのバイト オフセット。 </param>
        /// <param name="origin">新しい位置を取得するために使用する参照ポイントを示す SeekOrigin 型の値。 </param>
        /// <returns>現在のストリーム内の新しい位置。</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            return st.Seek(offset, origin);
        }

        /// <summary>
        /// ストリームの長さを設定します。 
        /// </summary>
        /// <param name="value">ストリームの希望の長さ (バイト数)。 </param>
        public override void SetLength(long value)
        {
            st.SetLength(value);
        }

        /// <summary>
        /// ストリームにバイト シーケンスを書き込み、書き込んだバイト数の分だけストリームの現在位置を進めます。
        /// </summary>
        /// <param name="buffer">バイト配列。このメソッドは、buffer から現在のストリームに、count で指定されたバイト数だけコピーします。</param>
        /// <param name="offset">現在のストリームへのバイトのコピーを開始する位置を示す buffer 内のバイト オフセット。インデックス番号は 0 から始まります。</param>
        /// <param name="count">現在のストリームに書き込むバイト数。</param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            st.Write(buffer, offset, count);
        }

        /// <summary>
        /// ストリームに関連付けられているすべてのリソース (ソケット、ファイル ハンドルなど) を解放します。
        /// </summary>
        public override void Close()
        {
            if (webres != null)
            {
                webres.Close();
            }
            if (st != null)
            {
                st.Close();
            }
            base.Close();
        }
    }
}
