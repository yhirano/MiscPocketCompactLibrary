#region ディレクティブを使用する

using System;

#endregion

namespace MiscPocketCompactLibrary.Windows.Forms
{
    /// <summary>
    /// アンカー
    /// </summary>
    [Flags]
    public enum AnchorStyles
    {
        None = 0x00000000,
        Top = 0x00000001,
        Bottom = 0x00000002,
        Left = 0x00000004,
        Right = 0x00000008
    }
}
