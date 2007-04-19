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
        /// <param name="parentControlWidth">親コントロールのベースサイズ（横幅）</param>
        /// <param name="parentControlHeight">親コントロールのベースサイズ（高さ）</param>
        public AnchorLayout(Control control, AnchorStyles anchor, int parentControlWidth, int parentControlHeight)
        {
            this.control = control;
            this.anchor = anchor;
            this.parentControlWidth = parentControlWidth;
            this.parentControlHeight = parentControlHeight;

            UpdateDistances();
        }

        /// <summary>
        /// コントロールをアンカーに従ってレイアウトし直す
        /// </summary>
        public void LayoutControl()
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
                    controlWidth = control.Parent.ClientRectangle.Width - dist_right - controlLeft;
                }
                else
                {
                    controlLeft = control.Parent.ClientRectangle.Width - dist_right - controlWidth;
                }
            }
            else if ((anchor & AnchorStyles.Left) == 0)
            {
                controlLeft = controlLeft + (control.Parent.ClientRectangle.Width - (controlLeft + controlWidth + dist_right)) / 2;
            }

            if ((anchor & AnchorStyles.Bottom) != 0)
            {
                if ((anchor & AnchorStyles.Top) != 0)
                {
                    controlHeight = control.Parent.ClientRectangle.Height - dist_bottom - controlTop;
                }
                else
                {
                    controlTop = control.Parent.ClientRectangle.Height - dist_bottom - controlHeight;
                }
            }
            else if ((anchor & AnchorStyles.Top) == 0)
            {
                controlTop = controlTop + (control.Parent.ClientRectangle.Height - (controlTop + controlHeight + dist_bottom)) / 2;
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
