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
        /// 画面の大きさを返す
        /// </summary>
        /// <returns>画面の大きさ</returns>
        public static ScreenSizes GetScreenSize()
        {
            // 横長WVGA
            if (Screen.PrimaryScreen.Bounds.Width >= WVGA_LANDSCAPE_WIDTH && Screen.PrimaryScreen.Bounds.Height >= WVGA_LANDSCAPE_HEIGHT)
            {
                return ScreenSizes.WvgaLandscape;
            }
            // 縦長WVGA
            else if (Screen.PrimaryScreen.Bounds.Width >= WVGA_PORTRAIT_WIDTH && Screen.PrimaryScreen.Bounds.Height >= WVGA_PORTRAIT_HEIGHT)
            {
                return ScreenSizes.WvgaPortrait;
            }
            // 横長VGA
            else if (Screen.PrimaryScreen.Bounds.Width >= VGA_LANDSCAPE_WIDTH && Screen.PrimaryScreen.Bounds.Height >= VGA_LANDSCAPE_HEIGHT)
            {
                return ScreenSizes.VgaLandscape;
            }
            // 縦長VGA
            else if (Screen.PrimaryScreen.Bounds.Width >= VGA_PORTRAIT_WIDTH && Screen.PrimaryScreen.Bounds.Height >= VGA_PORTRAIT_HEIGHT)
            {
                return ScreenSizes.VgaPortrait;
            }
            // 四角VGA
            else if (Screen.PrimaryScreen.Bounds.Width >= VGA_SQUARE && Screen.PrimaryScreen.Bounds.Height >= VGA_SQUARE)
            {
                return ScreenSizes.SquareVga;
            }
            // 横長WQVGA
            else if (Screen.PrimaryScreen.Bounds.Width >= WQVGA_LANDSCAPE_WIDTH && Screen.PrimaryScreen.Bounds.Height >= WQVGA_LANDSCAPE_HEIGHT)
            {
                return ScreenSizes.WqvgaLandscape;
            }
            // 縦長WQVGA
            else if (Screen.PrimaryScreen.Bounds.Width >= WQVGA_PORTRAIT_WIDTH && Screen.PrimaryScreen.Bounds.Height >= WQVGA_PORTRAIT_HEIGHT)
            {
                return ScreenSizes.WqvgaPortrait;
            }
            // 横長QVGA
            else if (Screen.PrimaryScreen.Bounds.Width >= QVGA_LANDSCAPE_WIDTH && Screen.PrimaryScreen.Bounds.Height >= QVGA_LANDSCAPE_HEIGHT)
            {
                return ScreenSizes.QvgaLandscape;
            }
            // 縦長QVGA
            else if (Screen.PrimaryScreen.Bounds.Width >= QVGA_PORTRAIT_WIDTH && Screen.PrimaryScreen.Bounds.Height >= QVGA_PORTRAIT_HEIGHT)
            {
                return ScreenSizes.QvgaPortrait;
            }
            // 四角QVGA
            else if (Screen.PrimaryScreen.Bounds.Width >= QVGA_SQUARE && Screen.PrimaryScreen.Bounds.Height >= QVGA_SQUARE)
            {
                return ScreenSizes.SquareQvga;
            }
            // その他
            else
            {
                return ScreenSizes.Other;
            }
        }

        /// <summary>
        /// 画面の方向を返す
        /// </summary>
        /// <returns>画面の方向</returns>
        public static ScreenOrientations GetScreenOrientation()
        {
            if (Screen.PrimaryScreen.Bounds.Width > Screen.PrimaryScreen.Bounds.Height)
            {
                return ScreenOrientations.Landscape;
            }
            else if (Screen.PrimaryScreen.Bounds.Width < Screen.PrimaryScreen.Bounds.Height)
            {
                return ScreenOrientations.Portrait;
            }
            else
            {
                return ScreenOrientations.Square;
            }
        }
    }
}
