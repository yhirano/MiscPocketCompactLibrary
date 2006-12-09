using System;
using System.Collections;
//using System.ComponentModel;
//using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;
//using OpenNETCF.Drawing;
//using OpenNETCF.Win32;
//using OpenNETCF.Runtime.InteropServices;
using MiscPocketCompactLibrary.Runtime.InteropServices;

// This source is based OpenNetCF 1.4  OpenNETCF.Windows.Forms.Clipboard Class.
// Rename namespace and comment out some 'using' for other sourcees include.
namespace MiscPocketCompactLibrary.Windows.Forms
{
    /// <summary>
    /// Provides methods to place data on and retrieve data from the system clipboard.
    /// </summary>
    /// <remarks>For a list of predefined formats to use with the Clipboard class, see the <see cref="DataFormats"/> class.
    /// <para>Call <see cref="SetDataObject"/> to put data on the clipboard.</para>
    /// <para>Place data on the clipboard in multiple formats to maximize the possibility that a target application, whose format requirements you might not know, can successfully retrieve the data.</para>
    /// <para>Call <see cref="GetDataObject"/> to retrieve data from the clipboard.
    /// The data is returned as an object that implements the <see cref="IDataObject"/> interface.
    /// Use the methods specified by <see cref="IDataObject"/> and fields in <see cref="DataFormats"/> to extract the data from the object. 
    /// If you do not know the format of the data you retrieved, call the <see cref="IDataObject.GetFormats"/> method of the <see cref="IDataObject"/> interface to get a list of all formats that data is stored in.
    /// Then call the <see cref="IDataObject.GetData"/> method of the <see cref="IDataObject"/> interface, and specify a format that your application can use.</para>
    /// <para>All Windows applications share the system clipboard, so the contents are subject to change when you switch to another application.</para>.
    /// Supports only Unicode text and Image (Bitmap) formats.</remarks>
    public sealed class Clipboard
    {
        private Clipboard() { }

        #region Clear
        /// <summary>
        /// Clears the contents of the Clipboard.
        /// <para><b>New in v1.3</b></para>
        /// </summary>
        public static void Clear()
        {
            if (OpenClipboard(IntPtr.Zero))
            {
                EmptyClipboard();
                CloseClipboard();
            }
        }
        #endregion

        #region Contains
        /// <summary>
        /// Determines if clipboard contains data in the specified format
        /// <para><b>New in v1.3</b></para>
        /// </summary>
        /// <param name="format">A clipboard format, see <see cref="DataFormats"/> for possible values.</param>
        /// <returns>True if clipboard contains specified format; otherwise False.</returns>
        public static bool ContainsData(string format)
        {
            return IsClipboardFormatAvailable(DataFormats.GetFormat(format).Id);

            /*IDataObject obj = GetDataObject();
            if(obj != null)
            {
                return obj.GetDataPresent(format);
            }

            return false;*/
        }

        /*
        /// <summary>
        /// Determines if clipboard contains audio.
        /// <para><b>New in v1.3</b></para>
        /// </summary>
        /// <returns>True if clipboard contains audio data; otherwise False.</returns>
        public static bool ContainsAudio()
        {
            return ContainsData(DataFormats.WaveAudio);
        }*/

        /// <summary>
        /// Determines if clipboard contains an Image.
        /// <para><b>New in v1.3</b></para>
        /// </summary>
        /// <returns>True if clipboard contains Image; otherwise False.</returns>
        public static bool ContainsImage()
        {
            return ContainsData(DataFormats.Bitmap);
        }

        /// <summary>
        /// Determines if clipboard contains Text.
        /// <para><b>New in v1.3</b></para>
        /// </summary>
        /// <returns>True if clipboard contains Text; otherwise False.</returns>
        public static bool ContainsText()
        {
            return ContainsData(DataFormats.UnicodeText);
        }
        #endregion

        #region Get helper
        /// <summary>
        /// Retrieves data from the Clipboard in the specified format.
        /// <para><b>New in v1.3</b></para>
        /// </summary>
        /// <param name="format">Clipboard format, see <see cref="DataFormats"/> for possible values.</param>
        /// <returns>Returns the specified data or null if not present.</returns>
        public static object GetData(string format)
        {
            if (ContainsData(format))
            {
                IDataObject obj = GetDataObject();
                return obj.GetData(format);
            }
            return null;
        }
        #endregion

        #region Text
        /// <summary>
        /// Places specified text onto the clipboard.
        /// <para><b>New in v1.3</b></para>
        /// </summary>
        /// <param name="text">Text to be added to the clipboard</param>
        public static void SetText(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            IDataObject obj = new DataObject(DataFormats.UnicodeText, text);
            SetDataObject(obj);
        }

        /// <summary>
        /// Retrieves data from the clipboard as text.
        /// <para><b>New in v1.3</b></para>
        /// </summary>
        /// <returns>Text representation of clipboard contents</returns>
        public static string GetText()
        {
            IDataObject obj = GetDataObject();
            if (obj.GetDataPresent(DataFormats.UnicodeText))
                return (string)obj.GetData(DataFormats.UnicodeText);
            else
                return null;
        }
        #endregion


        #region Obsolete functions
        /// <summary>
        /// Retrieves data from the clipboard as text
        /// </summary>
        /// <returns>Text representation of clipboard contents</returns>
        [Obsolete("Use Clipboard.SetText instead", true)]
        public static void SetClipboardText(string text)
        {
            SetText(text);
        }

        /// <summary>
        /// Places specified text onto the clipboard
        /// </summary>
        /// <param name="text">Text to be added to the clipboard</param>
        [Obsolete("Use Clipboard.GetText instead", true)]
        public static string GetClipboardText()
        {
            return GetText();
        }
        #endregion

        #region Set Data Object
        /// <summary>
        /// Places nonpersistent data on the system clipboard.
        /// </summary>
        /// <param name="data">The data to place on the clipboard.</param>
        /// <exception cref="System.ArgumentNullException">The value of data is null.</exception>
        public static void SetDataObject(IDataObject data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("The value of data is null");
            }
            else
            {
                if (!OpenClipboard(IntPtr.Zero))
                {
                    throw new System.Runtime.InteropServices.ExternalException("Could not open Clipboard");
                }

                if (!EmptyClipboard())
                {
                    throw new System.Runtime.InteropServices.ExternalException("Unable to empty Clipboard");
                }

                //from here on we are only supporting unicode text, but it would be possible to support other formats
                if (data.GetDataPresent(DataFormats.UnicodeText))
                {
                    string unicodedata;
                    IntPtr hClipboard;

                    try
                    {
                        //extract unicode string from supplied data
                        unicodedata = data.GetData(DataFormats.UnicodeText).ToString();
                    }
                    catch
                    {
                        throw new FormatException("Clipboard data not in a recognised format");
                    }

                    //marshall the string
                    hClipboard = MarshalEx.StringToHGlobalUni(unicodedata);

                    //pass data to clipboard
                    if (SetClipboardData(ClipboardFormats.UnicodeText, hClipboard) == IntPtr.Zero)
                    {
                        throw new System.Runtime.InteropServices.ExternalException("Could not put data on Clipboard");
                    }
                }
                else if (data.GetDataPresent(DataFormats.Bitmap))
                {
                    System.Drawing.Bitmap bmp = (System.Drawing.Bitmap)data.GetData(DataFormats.Bitmap);

                    int bs = bmp.Width * 3 * bmp.Height;
                    //allocate unmanaged memory
                    IntPtr hClipboard = MarshalEx.AllocHGlobal(bs + 41);
                    byte[] buffer = new byte[40];

                    MemoryStream strm = new MemoryStream(buffer);
                    BinaryWriter wr = new BinaryWriter(strm);
                    wr.Write(40);
                    wr.Write(bmp.Width);
                    wr.Write(bmp.Height);
                    wr.Write((short)1);
                    wr.Write((short)0x18);
                    // remaining bytes will be 0
                    wr.Close();

                    MarshalEx.WriteByteArray(hClipboard, 0, buffer);
                    int pos = 40;
                    for (int r = bmp.Height - 1; r >= 0; r--)
                        for (int c = 0; c < bmp.Width; c++)
                        {
                            int color = bmp.GetPixel(c, r).ToArgb();

                            byte red = (byte)((color & 0x00ff0000) >> 16);
                            byte green = (byte)((color & 0x0000ff00) >> 8);
                            byte blue = (byte)(color & 0x000000ff);

                            MarshalEx.WriteByte(hClipboard, pos++, blue);
                            MarshalEx.WriteByte(hClipboard, pos++, green);
                            MarshalEx.WriteByte(hClipboard, pos++, red);
                        }


                    //pass data to clipboard
                    if (SetClipboardData(ClipboardFormats.Bitmap, hClipboard) == IntPtr.Zero)
                    {
                        throw new System.Runtime.InteropServices.ExternalException("Could not put data on Clipboard");
                    }
                }

                if (!CloseClipboard())
                {
                    throw new System.Runtime.InteropServices.ExternalException("Could not close Clipboard");
                }
            }
        }
        #endregion

        #region Get Data Object
        /// <summary>
        /// Retrieves the data that is currently on the system clipboard.
        /// </summary>
        /// <returns>An <see cref="IDataObject"/> that represents the data currently on the clipboard, or null if there is no data on the clipboard.</returns>
        public static IDataObject GetDataObject()
        {
            //data object to return
            DataObject dobj = new DataObject();

            if (!OpenClipboard(IntPtr.Zero))
            {
                throw new System.Runtime.InteropServices.ExternalException("Could not open Clipboard");
            }

            //get number of clipboard types available
            int cfcount = CountClipboardFormats();

            if (cfcount == 0)
            {
                //throw new System.Runtime.InteropServices.ExternalException("No data on the Clipboard");
                return null;
            }
            else
            {
                ClipboardFormats format = 0;

                DataFormats.Format df;

                for (int i = 0; i < cfcount; i++)
                {
                    //get the next format on the clipboard
                    format = EnumClipboardFormats(format);

                    try
                    {
                        //get the dotnet data format
                        df = DataFormats.GetFormat((int)format);
                    }
                    catch
                    {
                        //if format is not recognised ignore it
                        break;
                    }

                    switch ((ClipboardFormats)df.Id)
                    {
                        case ClipboardFormats.UnicodeText:
                            //unicode text formats
                            dobj.SetData(df.Name, Marshal.PtrToStringUni(GetClipboardData((ClipboardFormats)df.Id)));
                            break;
                    }

                }

            }

            if (!CloseClipboard())
            {
                throw new System.Runtime.InteropServices.ExternalException("Could not close Clipboard");
            }

            return dobj;
        }
        #endregion


        #region Clipboard P/Invokes

        [DllImport("coredll.dll", EntryPoint = "OpenClipboard", SetLastError = true)]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("coredll.dll", EntryPoint = "CloseClipboard", SetLastError = true)]
        private static extern bool CloseClipboard();

        [DllImport("coredll.dll", EntryPoint = "EmptyClipboard", SetLastError = true)]
        private static extern bool EmptyClipboard();

        [DllImport("coredll.dll", EntryPoint = "IsClipboardFormatAvailable", SetLastError = true)]
        private static extern bool IsClipboardFormatAvailable(int uFormat);

        [DllImport("coredll.dll", EntryPoint = "GetClipboardData", SetLastError = true)]
        private static extern IntPtr GetClipboardData(ClipboardFormats uFormat);

        [DllImport("coredll.dll", EntryPoint = "SetClipboardData", SetLastError = true)]
        private static extern IntPtr SetClipboardData(ClipboardFormats uFormat, IntPtr hMem);

        [DllImport("coredll.dll", EntryPoint = "CountClipboardFormats", SetLastError = true)]
        private static extern int CountClipboardFormats();

        [DllImport("coredll.dll", EntryPoint = "EnumClipboardFormats", SetLastError = true)]
        private static extern ClipboardFormats EnumClipboardFormats(ClipboardFormats uFormat);

        #endregion
    }

    #region Clipboard Formats
    // <summary>
    // Represents CF_ constants defined in winuser.h
    // </summary>
    enum ClipboardFormats : int
    {

        Text = 1,
        Bitmap = 2,
        SymbolicLink = 4,
        Dif = 5,
        Tiff = 6,
        OemText = 7,
        Dib = 8,
        Palette = 9,
        PenData = 10,
        Riff = 11,
        WaveAudio = 12,
        UnicodeText = 13,
    }
    #endregion

    #region Data Formats
    /// <summary>
    /// Provides static, predefined <see cref="Clipboard"/> format names.
    /// Use them to identify the format of data that you store in an <see cref="IDataObject"/>.
    /// </summary>
    public class DataFormats
    {
        #region Data Formats

        //array of valid clipboard formats - defined in winuser.h
        //private static readonly string[] m_formats = {"Text", "Bitmap", "", "SymbolicLink", "Dif", "Tiff", "OemText", "Dib", "Palette", "PenData", "Riff", "WaveAudio", "UnicodeText"};

        /// <summary>
        /// Specifies a Windows bitmap format.
        /// <para>Not currently supported</para>
        /// </summary>
        public static readonly string Bitmap = ClipboardFormats.Bitmap.ToString();
        /// <summary>
        /// Specifies the Windows Device Independent Bitmap (DIB) format.
        /// <para>Not currently supported</para>
        /// </summary>
        public static readonly string Dib = ClipboardFormats.Dib.ToString();
        /// <summary>
        /// Specifies the Windows Data Interchange Format (DIF), which Windows Forms does not directly use.
        /// <para>Not currently supported</para>
        /// </summary>
        public static readonly string Dif = ClipboardFormats.Dif.ToString();
        /// <summary>
        /// Specifies the standard Windows original equipment manufacturer (OEM) text format.
        /// </summary>
        public static readonly string OemText = ClipboardFormats.OemText.ToString();
        /// <summary>
        /// Specifies the Windows palette format.
        /// <para>Not currently supported</para>
        /// </summary>
        public static readonly string Palette = ClipboardFormats.Palette.ToString();
        /// <summary>
        /// Specifies the Windows pen data format, which consists of pen strokes for handwriting software; Windows Forms does not use this format.
        /// <para>Not currently supported</para>
        /// </summary>
        public static readonly string PenData = ClipboardFormats.PenData.ToString();
        /// <summary>
        /// Specifies the Resource Interchange File Format (RIFF) audio format, which Windows Forms does not directly use.
        /// <para>Not currently supported</para>
        /// </summary>
        public static readonly string Riff = ClipboardFormats.Riff.ToString();
        /// <summary>
        /// Specifies the Windows symbolic link format, which Windows Forms does not directly use.
        /// <para>Not currently supported</para>
        /// </summary>
        public static readonly string SymbolicLink = ClipboardFormats.SymbolicLink.ToString();
        /// <summary>
        /// Specifies the standard ANSI text format.
        /// </summary>
        public static readonly string Text = ClipboardFormats.Text.ToString();
        /// <summary>
        /// Specifies the Tagged Image File Format (TIFF), which Windows Forms does not directly use.
        /// <para>Not currently supported</para>
        /// </summary>
        public static readonly string Tiff = ClipboardFormats.Tiff.ToString();
        /// <summary>
        /// Specifies the standard Windows Unicode text format.
        /// </summary>
        public static readonly string UnicodeText = ClipboardFormats.UnicodeText.ToString();
        /// <summary>
        /// Specifies the wave audio format, which Windows Forms does not directly use.
        /// <para>Not currently supported</para>
        /// </summary>
        public static readonly string WaveAudio = ClipboardFormats.WaveAudio.ToString();

        #endregion

        #region Get Format
        /// <summary>
        /// Returns a <see cref="Format"/> with the Windows Clipboard numeric ID and name for the specified ID.
        /// </summary>
        /// <param name="id">The format ID.</param>
        /// <returns>A <see cref="Format"/> that has the Windows Clipboard numeric ID and the name of the format.</returns>
        public static Format GetFormat(int id)
        {
            try
            {
                //return specified format
                return new Format(((ClipboardFormats)id).ToString(), id);
            }
            catch
            {
                //on error return null
                return null;
            }
        }
        /// <summary>
        /// Returns an OpenNETCF.Windows.Forms.DataFormats.Format with the Windows Clipboard numeric ID and name for the specified format.
        /// </summary>
        /// <param name="format">The format name.</param>
        /// <returns>An OpenNETCF.Windows.Forms.DataFormats.Format that has the Windows Clipboard numeric ID and the name of the format.</returns>
        public static Format GetFormat(string format)
        {
            try
            {
                ClipboardFormats cf = 0;
                //cf.GetType().GetField(format).GetValue(cf)
                return new Format(format, (int)(cf.GetType().GetField(format, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public).GetValue(cf)));
            }
            catch
            {
                throw new Exception("Resolving Clipboard typue failed");
            }
        }
        #endregion

        #region Format
        /// <summary>
        /// Represents a clipboard format type.
        /// </summary>
        public class Format
        {
            int m_id;
            string m_name;

            /// <summary>
            /// Create a new instance of Format.
            /// </summary>
            /// <param name="name">Name of the format.</param>
            /// <param name="id">ID number of the format.</param>
            public Format(string name, int id)
            {
                m_name = name;
                m_id = id;
            }

            /// <summary>
            /// Gets the name of this format.
            /// </summary>
            public string Name
            {
                get
                {
                    return m_name;
                }
            }

            /// <summary>
            /// Gets the ID number for this format.
            /// </summary>
            public int Id
            {
                get
                {
                    return m_id;
                }
            }
        }
        #endregion
    }
    #endregion

    #region IDataObject
    /// <summary>
    /// Provides a format-independent mechanism for transferring data.
    /// </summary>
    public interface IDataObject
    {
        /// <summary>
        /// Retrieves the data associated with the specified data format.
        /// </summary>
        /// <param name="format">The format of the data to retrieve.
        /// See <see cref="DataFormats"/> for predefined formats.</param>
        /// <returns>The data associated with the specified format, or a null reference (Nothing in Visual Basic).</returns>
        object GetData(string format);

        /// <summary>
        /// Determines whether data stored in this instance is associated with, or can be converted to, the specified format.
        /// </summary>
        /// <param name="format">The format for which to check.
        /// See <see cref="DataFormats"/> for predefined formats.</param>
        /// <returns>true if data stored in this instance is associated with, or can be converted to, the specified format; otherwise false.</returns>
        bool GetDataPresent(string format);

        /// <summary>
        /// Stores the specified data in this instance, using the class of the data for the format.
        /// </summary>
        /// <param name="format">The format associated with the data.
        /// See <see cref="DataFormats"/> for predefined formats.</param>
        /// <param name="data">The data to store.</param>
        void SetData(string format, object data);
    }
    #endregion

    #region DataObject
    /// <summary>
    /// Implements a basic data transfer mechanism.
    /// </summary>
    public class DataObject : IDataObject
    {
        //store data formats
        private System.Collections.Hashtable m_data;

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="DataObject"/> class, which can store arbitrary data.
        /// </summary>
        public DataObject()
        {
            m_data = new System.Collections.Hashtable();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DataObject"/> class, containing the specified data and its associated format.
        /// </summary>
        /// <param name="format">The class type associated with the data. See <see cref="OpenNETCF.Windows.Forms.DataFormats"/> for the predefined formats.</param>
        /// <param name="data">The data to store.</param>
        public DataObject(string format, object data)
            : this()
        {
            m_data.Add(format, data);
        }
        #endregion

        #region IDataObject Members

        /// <summary>
        /// Returns the data associated with the specified data format.
        /// </summary>
        /// <param name="format">The format of the data to retrieve. See <see cref="DataFormats"/> for predefined formats.</param>
        /// <returns>The data associated with the specified format, or null.</returns>
        public object GetData(string format)
        {
            try
            {
                //return the object of specified format
                return m_data[format];
            }
            catch
            {
                //specified format not contained
                return null;
            }
        }

        /// <summary>
        /// Determines whether data stored in this instance is associated with, or can be converted to, the specified format.
        /// </summary>
        /// <param name="format">The format to check for. See <see cref="DataFormats"/> for predefined formats.</param>
        /// <returns>true if data stored in this instance is associated with, or can be converted to, the specified format; otherwise, false.</returns>
        public bool GetDataPresent(string format)
        {
            return m_data.ContainsKey(format);
        }

        /// <summary>
        /// Stores the specified format and data in this instance.
        /// </summary>
        /// <param name="format">The format associated with the data. See <see cref="DataFormats"/> for predefined formats.</param>
        /// <param name="data">The data to store.</param>
        public void SetData(string format, object data)
        {
            m_data.Add(format, data);
        }

        #endregion
    }
    #endregion
}
