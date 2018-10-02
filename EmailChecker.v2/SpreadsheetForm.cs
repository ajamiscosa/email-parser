using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;

namespace EmailChecker.v2
{
    public partial class SpreadsheetForm : Form
    {
        private PrivateFontCollection pfc;
        public SpreadsheetForm()
        {
            InitializeComponent();
            PrivateFontCollection pfc = new PrivateFontCollection();
            pfc.AddFontFile(String.Format(@"{0}\{1}",Environment.CurrentDirectory, "fa-regular-400.ttf"));
            AddFontFromResource(pfc, "fa-brands-400.ttf");
            AddFontFromResource(pfc, "fa-regular-400.ttf");
            AddFontFromResource(pfc, "fa-solid-900.ttf");

            toolStripButton1.Font = new Font(pfc.Families[0], 12);
        }

        private static void AddFontFromResource(PrivateFontCollection privateFontCollection, string fontResourceName)
        {
            var fontBytes = GetFontResourceBytes(typeof(SpreadsheetForm).Assembly, fontResourceName);
            var fontData = Marshal.AllocCoTaskMem(fontBytes.Length);
            Marshal.Copy(fontBytes, 0, fontData, fontBytes.Length);
            privateFontCollection.AddMemoryFont(fontData, fontBytes.Length);
            // Marshal.FreeCoTaskMem(fontData);  Nasty bug alert, read the comment
        }

        private static byte[] GetFontResourceBytes(Assembly assembly, string fontResourceName)
        {
            var resourceStream = assembly.GetManifestResourceStream(fontResourceName);
            if (resourceStream == null)
                throw new Exception(string.Format("Unable to find font '{0}' in embedded resources.", fontResourceName));
            var fontBytes = new byte[resourceStream.Length];
            resourceStream.Read(fontBytes, 0, (int)resourceStream.Length);
            resourceStream.Close();
            return fontBytes;
        }
    }
}
