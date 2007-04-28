#region ディレクティブを使用する

using System;
using System.IO;

#endregion

namespace MiscPocketCompactLibrary.Diagnostics
{
    public class Log
    {
        /// <summary>
        /// ログのファイルパス
        /// </summary>
        private string logPath = "log.txt";

        /// <summary>
        /// ログのファイルパス
        /// </summary>
        public string LogPath
        {
            get { return logPath; }
        }

        /// <summary>
        /// ログのプレフィクスの種類
        /// </summary>
        public enum LogPrefix
        {
            /// <summary>
            /// プレフィクス無し
            /// </summary>
            none,
            /// <summary>
            /// 日付
            /// </summary>
            date
        }
        
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Log()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="logPath">ログのファイルパス</param>
        public Log(string logPath)
        {
            this.logPath = logPath;
        }

        /// <summary>
        /// ログを書き出す
        /// </summary>
        /// <param name="logText">書き出す内容</param>
        public void LogThis(string logText)
        {
            LogThis(logText, LogPrefix.date);
        }

        /// <summary>
        /// ログを書き出す
        /// </summary>
        /// <param name="logText">書き出す内容</param>
        /// <param name="logPrefix">ログのプレフィクス</param>
        public void LogThis(string logText, LogPrefix logPrefix)
        {
            string output;

            switch (logPrefix) {
                case LogPrefix.date:
                    output = DateTime.Now.ToString("yyyy/MM/dd-hh:mm:ss") + "\r\n"
                        + logText + "\r\n";
                    break;
                case LogPrefix.none:
                default:
                    output = logText + "\r\n";
                    break;
            }

            AppendToFile(LogPath, output);
        }

        /// <summary>
        /// ログをファイルに書き出す
        /// </summary>
        /// <param name="logPath">ログのファイルパス</param>
        /// <param name="logText">書き出す内容</param>
        private void AppendToFile(string logPath, string logText)
        {
            StreamWriter sw=null;
            try
            {
                sw = File.AppendText(logPath);
                sw.WriteLine(logText);
            }
            catch (UnauthorizedAccessException)
            {
                ;
            }
            catch (ArgumentNullException)
            {
                ;
            }
            catch (ArgumentException)
            {
                ;
            }
            catch (PathTooLongException)
            {
                ;
            }
            catch (DirectoryNotFoundException)
            {
                ;
            }
            catch (NotSupportedException)
            {
                ;
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }
    }
}
