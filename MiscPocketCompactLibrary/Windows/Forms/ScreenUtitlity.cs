#region ディレクティブを使用する

using System;
using System.Windows.Forms;

#endregion

namespace MiscPocketCompactLibrary.Windows.Forms
{
    public class ScreenUtitlity
    {
        /// <summary>
        /// 横長QVGA時のスクリーン横幅
        /// </summary>
        private const int QVGA_LANDSCAPE_WIDTH = 320;

        /// <summary>
        /// 横長QVGA時のスクリーン高さ
        /// </summary>
        private const int QVGA_LANDSCAPE_HEIGHT = 240;

        /// <summary>
        // 縦長QVGA時のスクリーン横幅
        /// </summary>
        private const int QVGA_PORTRAIT_WIDTH = 240;
        
        /// <summary>
        /// 縦長QVGA時のスクリーン高さ
        /// </summary>
        private const int QVGA_PORTRAIT_HEIGHT = 320;

        /// <summary>
        /// シングルトンのためプライベート
        /// </summary>
        private ScreenUtitlity()
        {
        }

        /// <summary>
        /// スクリーンのサイズの構造体
        /// </summary>
        public enum ScreenSize
        {
            /// <summary>
            /// 縦長QVGA
            /// </summary>
            QvgaPortrait,

            /// <summary>
            /// 横長QVGA
            /// </summary>
            QvgaLandscape,

            /// <summary>
            /// 縦長VGA
            /// </summary>
            VgaPortrait,

            /// <summary>
            /// 横長VGA
            /// </summary>
            VgaLandscape,

            /// <summary>
            /// 四角QVGA
            /// </summary>
            SquareQvga,

            /// <summary>
            /// 四角VGA
            /// </summary>
            SquareVga,

            /// <summary>
            /// その他
            /// </summary>
            Other
        }

        /// <summary>
        /// 画面の大きさの種類を返す
        /// </summary>
        /// <returns>画面の大きさの種類</returns>
        public static ScreenSize GetScreenSize()
        {
            // 横長VGA
            if ((Screen.PrimaryScreen.Bounds.Width > Screen.PrimaryScreen.Bounds.Height)
                && (Screen.PrimaryScreen.Bounds.Width > QVGA_LANDSCAPE_WIDTH))
            {
                return ScreenSize.VgaLandscape;
            }
            // 縦長VGA
            else if ((Screen.PrimaryScreen.Bounds.Width < Screen.PrimaryScreen.Bounds.Height)
                && (Screen.PrimaryScreen.Bounds.Width > QVGA_PORTRAIT_WIDTH))
            {
                return ScreenSize.VgaPortrait;
            }
            // 四角VGA
            else if ((Screen.PrimaryScreen.Bounds.Width == Screen.PrimaryScreen.Bounds.Height)
                && (Screen.PrimaryScreen.Bounds.Width > QVGA_PORTRAIT_WIDTH)
                && (Screen.PrimaryScreen.Bounds.Height > QVGA_LANDSCAPE_HEIGHT))
            {
                return ScreenSize.SquareVga;
            }
            // 四角QVGA
            else if ((Screen.PrimaryScreen.Bounds.Width == Screen.PrimaryScreen.Bounds.Height)
                && (Screen.PrimaryScreen.Bounds.Width <= QVGA_PORTRAIT_WIDTH)
                && (Screen.PrimaryScreen.Bounds.Height <= QVGA_LANDSCAPE_HEIGHT))
            {
                return ScreenSize.SquareQvga;
            }
            // 横長QVGA
            if ((Screen.PrimaryScreen.Bounds.Width > Screen.PrimaryScreen.Bounds.Height)
                && (Screen.PrimaryScreen.Bounds.Width <= QVGA_LANDSCAPE_WIDTH))
            {
                return ScreenSize.QvgaLandscape;
            }
            // 縦長QVGA
            else if ((Screen.PrimaryScreen.Bounds.Width < Screen.PrimaryScreen.Bounds.Height)
                && (Screen.PrimaryScreen.Bounds.Width <= QVGA_PORTRAIT_WIDTH))
            {
                return ScreenSize.QvgaPortrait;
            }
            // その他
            else
            {
                return ScreenSize.Other;
            }
        }
    }
}
