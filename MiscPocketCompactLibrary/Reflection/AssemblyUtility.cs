#region ディレクティブを使用する

using System;
using System.IO;
using System.Text;
using System.Reflection;

#endregion

namespace MiscPocketCompactLibrary.Reflection
{
    public class AssemblyUtility
    {
        // <summary>
        /// シングルトンのためプライベート
        /// </summary>
        private AssemblyUtility()
        {
        }

        /// <summary>
        /// アプリケーションの実行ディレクトリのパスを返す
        /// </summary>
        /// <returns>アプリケーションの実行ディレクトリのパス</returns>
        public static string GetExecutablePath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName);
        }
    }
}
