#region ディレクティブを使用する

using System;
using System.Windows.Forms;

#endregion

namespace MiscPocketCompactLibrary.Windows.Forms
{
    /// <summary>
    /// コントロールのアンカーを保持するクラス
    /// </summary>
    public class AnchorLayout
    {
        /// <summary>
        /// アンカーを設定するコントロール
        /// </summary>
        private Control control;

        /// <summary>
        /// アンカー
        /// </summary>
        private AnchorStyles anchor = AnchorStyles.Top | AnchorStyles.Left;

        private int dist_right = 0;

        private int dist_bottom = 0;

        /// <summary>
        /// 親コントロールのベースサイズ（横幅）
        /// </summary>
        private int parentControlWidth;

        /// <summary>
        /// 親コントロールのベースサイズ（横幅）
        /// </summary>
        public int ParentControlWidth
        {
            set
            {
                parentControlWidth = value;
                UpdateDistances();
            }
        }

        /// <summary>
        /// 親コントロールのベースサイズ（高さ）
        /// </summary>
        private int parentControlHeight;

        /// <summary>
        /// 親コントロールのベースサイズ（高さ）
        /// </summary>
        public int ParentControlHeight
        {
            set
            {
                parentControlHeight = value;
                UpdateDistances();
            }
        }

        /// <summary>
        /// アンカー
        /// </summary>
        public AnchorStyles Anchor
        {
            get { return anchor; }
            set
            {
                if (anchor == value)
                {
                    return;
                }
                anchor = value;

                UpdateDistances();
            }
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="control">アンカーを設定するコントロール</param>
        /// <param name="anchor">アンカー</param>
        public AnchorLayout(Control control, AnchorStyles anchor)
            : this(control, anchor, control.Parent.ClientSize.Width, control.Parent.ClientSize.Height)
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="control">アンカーを設定するコントロール</param>
        /// <param name="anchor">アンカー</param>
        /// <param name="parentControlBaseWidth">親コントロールのベースサイズ（横幅）</param>
        /// <param name="parentControlBaseHeight">親コントロールのベースサイズ（高さ）</param>
        public AnchorLayout(Control control, AnchorStyles anchor, int parentControlBaseWidth, int parentControlBaseHeight)
        {
            this.control = control;
            this.anchor = anchor;
            this.parentControlWidth = parentControlBaseWidth;
            this.parentControlHeight = parentControlBaseHeight;

            UpdateDistances();
        }

        /// <summary>
        /// コントロールをアンカーに従ってレイアウトし直す
        /// </summary>
        public void LayoutControl()
        {
            LayoutControl(control.Parent.ClientRectangle.Width, control.Parent.ClientRectangle.Height);
        }

        /// <summary>
        /// コントロールをアンカーに従ってレイアウトし直す
        /// </summary>
        /// <param name="parentClientWidth">レイアウトし直す親コントロールの領域の横幅</param>
        /// <param name="parentClientHeight">レイアウトし直す親コントロールの領域の高さ</param>
        public void LayoutControl(int parentClientWidth, int parentClientHeight)
        {
            if (control.Parent == null)
            {
                return;
            }

            int controlTop = control.Top;
            int controlLeft = control.Left;
            int controlWidth = control.Width;
            int controlHeight = control.Height;

            if ((anchor & AnchorStyles.Right) != 0)
            {
                if ((anchor & AnchorStyles.Left) != 0)
                {
                    controlWidth = parentClientWidth - dist_right - controlLeft;
                }
                else
                {
                    controlLeft = parentClientWidth - dist_right - controlWidth;
                }
            }
            else if ((anchor & AnchorStyles.Left) == 0)
            {
                controlLeft = controlLeft + (parentClientWidth - (controlLeft + controlWidth + dist_right)) / 2;
            }

            if ((anchor & AnchorStyles.Bottom) != 0)
            {
                if ((anchor & AnchorStyles.Top) != 0)
                {
                    controlHeight = parentClientHeight - dist_bottom - controlTop;
                }
                else
                {
                    controlTop = parentClientHeight - dist_bottom - controlHeight;
                }
            }
            else if ((anchor & AnchorStyles.Top) == 0)
            {
                controlTop = controlTop + (parentClientHeight - (controlTop + controlHeight + dist_bottom)) / 2;
            }

            if (controlWidth < 0)
            {
                controlWidth = 0;
            }

            if (controlHeight < 0)
            {
                controlHeight = 0;
            }

            control.Top = controlTop;
            control.Left = controlLeft;
            control.Width = controlWidth;
            control.Height = controlHeight;
        }

        /// <summary>
        /// コントロールと親コントロールの距離を測る
        /// </summary>
        private void UpdateDistances()
        {
            if (control.Parent != null)
            {
                if (control.Width > 0)
                {
                    dist_right = parentControlWidth - control.Left - control.Width;
                }
                if (control.Height > 0)
                {
                    dist_bottom = parentControlHeight - control.Top - control.Height;
                }
            }
        }
    }
}
