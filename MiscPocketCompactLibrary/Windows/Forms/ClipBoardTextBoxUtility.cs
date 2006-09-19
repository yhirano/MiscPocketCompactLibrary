#region ディレクティブを使用する

using System;
using System.Windows.Forms;
using MiscPocketCompactLibrary.Windows.Forms;

#endregion

namespace MiscPocketCompactLibrary.Windows.Forms
{
    /// <summary>
    /// テキストボックスへのクリップボードの処理ユーティリティ
    /// </summary>
    public sealed class ClipboardTextBox
    {
        /// <summary>
        /// シングルトンのためプライベート
        /// </summary>
        private ClipboardTextBox()
        {
        }

        public static void Cut(TextBox tb)
        {
            if (tb != null && tb.SelectionLength > 0)
            {
                Clipboard.SetText(tb.SelectedText);
                tb.SelectedText = "";
            }
        }

        public static void Copy(TextBox tb)
        {
            if (tb != null && tb.SelectionLength > 0)
            {
                Clipboard.SetText(tb.SelectedText);
            }
        }

        public static void Paste(TextBox tb)
        {
            string clipboardText = Clipboard.GetText();
            if (tb != null && clipboardText != null)
            {
                string before = tb.Text.Substring(0, tb.SelectionStart);
                string after = 
                    tb.Text.Substring(
                    tb.SelectionStart + tb.SelectionLength, tb.TextLength - (tb.SelectionStart + tb.SelectionLength));
                tb.Text = before + clipboardText + after;
            }
        }
    }
}
