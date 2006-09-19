#region ディレクティブを使用する

using System;
using System.IO;
using System.Runtime.InteropServices;

#endregion

namespace MiscPocketCompactLibrary.Runtime.InteropServices
{
    /// <summary>
    /// ProcessUtility の概要の説明です。
    /// </summary>
    public class ProcessUtility
    {
        /// <summary>
        /// シングルトンのためプライベート
        /// </summary>
        public ProcessUtility()
        {
        }

        /// <summary>
        /// プロセスを生成する
        /// </summary>
        /// <param name="lpApplicationName">実行可能モジュールの名前</param>
        /// <param name="lpCommandLine">コマンドラインの文字列</param>
        /// <param name="lpProcess"></param>
        /// <param name="lpThread"></param>
        /// <param name="bInheritHandles"></param>
        /// <param name="dwCreationFlags"></param>
        /// <param name="lpEnvironment"></param>
        /// <param name="lpCurrentDirectory"></param>
        /// <param name="lpStartupInfo"></param>
        /// <param name="lpProcessInformation"></param>
        /// <returns></returns>
        [DllImport("coredll.dll")]
        private static extern int CreateProcess(
            string lpApplicationName,
            string lpCommandLine,
            int lpProcess,
            int lpThread,
            bool bInheritHandles,
            uint dwCreationFlags,
            int lpEnvironment,
            int lpCurrentDirectory,
            int lpStartupInfo,
            int lpProcessInformation);

        /// <summary>
        /// 新たにプロセスを起動する
        /// </summary>
        /// <param name="applicationPath">実行するアプリケーションのパス</param>
        /// <returns>プロセスが起動できたか</returns>
        private static int CreateProcess(string applicationPath)
        {
            if (File.Exists(applicationPath) == true)
            {
                return CreateProcess(applicationPath, null, 0, 0, false, 0, 0, 0, 0, 0);
            }

            return -1;
        }

        /// <summary>
        /// 新たにプロセスを起動する
        /// </summary>
        /// <param name="applicationPath">実行するアプリケーションのパス</param>
        /// <param name="commandLine">アプリケーションに渡すコマンドライン</param>
        /// <returns>プロセスが起動できたか</returns>
        public static int CreateProcess(string applicationPath, string commandLine)
        {
            if (File.Exists(applicationPath) == true)
            {
                return CreateProcess(applicationPath, commandLine, 0, 0, false, 0, 0, 0, 0, 0);
            }

            return -1;
        }
    }
}
