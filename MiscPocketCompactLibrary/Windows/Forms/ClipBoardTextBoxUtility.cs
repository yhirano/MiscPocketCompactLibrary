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
            try
            {
                if (tb != null && tb.SelectionLength > 0)
                {
                    Clipboard.SetText(tb.SelectedText);
                    tb.SelectedText = "";
                }
            }
            catch (ArgumentException)
            {
                ;
            }
            catch (System.Runtime.InteropServices.ExternalException e)
            {
                MessageBox.Show(e.Message, "警告");
            }
        }

        public static void Copy(TextBox tb)
        {
            try
            {
                if (tb != null && tb.SelectionLength > 0)
                {
                    Clipboard.SetText(tb.SelectedText);
                }
            }
            catch (ArgumentException)
            {
                ;
            }
            catch (System.Runtime.InteropServices.ExternalException e)
            {
                MessageBox.Show(e.Message, "警告");
            }
        }

        public static void Paste(TextBox tb)
        {
            try
            {
                string clipboardText = Clipboard.GetText();
                if (tb != null && clipboardText != null)
                {
                    // 選択部分より前の文字列
                    string before = tb.Text.Substring(0, tb.SelectionStart);
                    // 選択部分より後の文字列
                    string after =
                        tb.Text.Substring(
                        tb.SelectionStart + tb.SelectionLength, tb.TextLength - (tb.SelectionStart + tb.SelectionLength));
                    tb.Text = before + clipboardText + after;
                    // カーソル位置を貼り付けた内容の最後に持ってくる
                    tb.SelectionStart = before.Length + clipboardText.Length;
                }
            }
            catch (ArgumentException)
            {
                ;
            }
            catch (System.Runtime.InteropServices.ExternalException e)
            {
                MessageBox.Show(e.Message, "警告");
            }
        }
    }
}
