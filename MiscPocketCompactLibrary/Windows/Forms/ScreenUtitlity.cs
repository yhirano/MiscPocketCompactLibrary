#region ディレクティブを使用する

using System;
using System.Windows.Forms;

#endregion

namespace MiscPocketCompactLibrary.Windows.Forms
{
    public class ScreenUtitlity
    {
        #region WVGA
        #region 横長WVGA

        /// <summary>
        /// 横長WVGA時のスクリーン横幅
        /// </summary>
        private const int WVGA_LANDSCAPE_WIDTH = 800;

        /// <summary>
        /// 横長WVGA時のスクリーン高さ
        /// </summary>
        private const int WVGA_LANDSCAPE_HEIGHT = 480;

        #endregion

        #region 縦長VGA

        /// <summary>
        // 縦長WVGA時のスクリーン横幅
        /// </summary>
        private const int WVGA_PORTRAIT_WIDTH = 480;

        /// <summary>
        /// 縦長VGA時のスクリーン高さ
        /// </summary>
        private const int WVGA_PORTRAIT_HEIGHT = 800;

        #endregion
        #endregion

        #region VGA
        #region 横長VGA

        /// <summary>
        /// 横長VGA時のスクリーン横幅
        /// </summary>
        private const int VGA_LANDSCAPE_WIDTH = 640;

        /// <summary>
        /// 横長VGA時のスクリーン高さ
        /// </summary>
        private const int VGA_LANDSCAPE_HEIGHT = 480;

        #endregion

        #region 縦長VGA

        /// <summary>
        // 縦長VGA時のスクリーン横幅
        /// </summary>
        private const int VGA_PORTRAIT_WIDTH = 480;

        /// <summary>
        /// 縦長VGA時のスクリーン高さ
        /// </summary>
        private const int VGA_PORTRAIT_HEIGHT = 640;

        #endregion

        #region 四角VGA

        /// <summary>
        /// 横長VGA時のスクリーン横幅
        /// </summary>
        private const int VGA_SQUARE = 480;

        #endregion
        #endregion

        #region WQVGA
        #region 横長WQVGA

        /// <summary>
        /// 横長WQVGA時のスクリーン横幅
        /// </summary>
        private const int WQVGA_LANDSCAPE_WIDTH = 400;

        /// <summary>
        /// 横長WQVGA時のスクリーン高さ
        /// </summary>
        private const int WQVGA_LANDSCAPE_HEIGHT = 240;

        #endregion

        #region 縦長WQVGA

        /// <summary>
        // 縦長WQVGA時のスクリーン横幅
        /// </summary>
        private const int WQVGA_PORTRAIT_WIDTH = 240;

        /// <summary>
        /// 縦長WQVGA時のスクリーン高さ
        /// </summary>
        private const int WQVGA_PORTRAIT_HEIGHT = 400;

        #endregion
        #endregion

        #region QVGA
        #region 横長QVGA

        /// <summary>
        /// 横長QVGA時のスクリーン横幅
        /// </summary>
        private const int QVGA_LANDSCAPE_WIDTH = 320;

        /// <summary>
        /// 横長QVGA時のスクリーン高さ
        /// </summary>
        private const int QVGA_LANDSCAPE_HEIGHT = 240;

        #endregion

        #region 縦長QVGA

        /// <summary>
        // 縦長QVGA時のスクリーン横幅
        /// </summary>
        private const int QVGA_PORTRAIT_WIDTH = 240;

        /// <summary>
        /// 縦長QVGA時のスクリーン高さ
        /// </summary>
        private const int QVGA_PORTRAIT_HEIGHT = 320;

        #endregion

        #region 四角QVGA

        /// <summary>
        // 縦長QVGA時のスクリーン横幅
        /// </summary>
        private const int QVGA_SQUARE = 240;

        #endregion
        #endregion

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
            /// 縦長WQVGA
            /// </summary>
            WqvgaPortrait,

            /// <summary>
            /// 横長WQVGA
            /// </summary>
            WqvgaLandscape,

            /// <summary>
            /// 縦長VGA
            /// </summary>
            VgaPortrait,

            /// <summary>
            /// 横長VGA
            /// </summary>
            VgaLandscape,

            /// <summary>
            /// 縦長WVGA
            /// </summary>
            WvgaPortrait,

            /// <summary>
            /// 横長WVGA
            /// </summary>
            WvgaLandscape,

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
            // 横長WVGA
            if (Screen.PrimaryScreen.Bounds.Width == WVGA_LANDSCAPE_WIDTH && Screen.PrimaryScreen.Bounds.Height == WVGA_LANDSCAPE_HEIGHT)
            {
                return ScreenSize.WvgaLandscape;
            }
            // 縦長WVGA
            else if (Screen.PrimaryScreen.Bounds.Width == WVGA_PORTRAIT_WIDTH && Screen.PrimaryScreen.Bounds.Height == WVGA_PORTRAIT_HEIGHT)
            {
                return ScreenSize.WvgaPortrait;
            }
            // 横長VGA
            else if (Screen.PrimaryScreen.Bounds.Width == VGA_LANDSCAPE_WIDTH && Screen.PrimaryScreen.Bounds.Height == VGA_LANDSCAPE_HEIGHT)
            {
                return ScreenSize.VgaLandscape;
            }
            // 縦長VGA
            else if (Screen.PrimaryScreen.Bounds.Width == VGA_PORTRAIT_WIDTH && Screen.PrimaryScreen.Bounds.Height == VGA_PORTRAIT_HEIGHT)
            {
                return ScreenSize.VgaPortrait;
            }
            // 四角VGA
            else if (Screen.PrimaryScreen.Bounds.Width == VGA_SQUARE && Screen.PrimaryScreen.Bounds.Height == VGA_SQUARE)
            {
                return ScreenSize.SquareVga;
            }
            // 横長WQVGA
            else if (Screen.PrimaryScreen.Bounds.Width == WQVGA_LANDSCAPE_WIDTH && Screen.PrimaryScreen.Bounds.Height == WQVGA_LANDSCAPE_HEIGHT)
            {
                return ScreenSize.WqvgaLandscape;
            }
            // 縦長WQVGA
            else if (Screen.PrimaryScreen.Bounds.Width == WQVGA_PORTRAIT_WIDTH && Screen.PrimaryScreen.Bounds.Height == WQVGA_PORTRAIT_HEIGHT)
            {
                return ScreenSize.WqvgaPortrait;
            }
            // 横長QVGA
            else if (Screen.PrimaryScreen.Bounds.Width == QVGA_LANDSCAPE_WIDTH && Screen.PrimaryScreen.Bounds.Height == QVGA_LANDSCAPE_HEIGHT)
            {
                return ScreenSize.QvgaLandscape;
            }
            // 縦長QVGA
            else if (Screen.PrimaryScreen.Bounds.Width == QVGA_PORTRAIT_WIDTH && Screen.PrimaryScreen.Bounds.Height == QVGA_PORTRAIT_HEIGHT)
            {
                return ScreenSize.QvgaPortrait;
            }
            // 四角QVGA
            else if (Screen.PrimaryScreen.Bounds.Width == QVGA_SQUARE && Screen.PrimaryScreen.Bounds.Height == QVGA_SQUARE)
            {
                return ScreenSize.SquareQvga;
            }
            // その他
            else
            {
                return ScreenSize.Other;
            }
        }
    }
}
