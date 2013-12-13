using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Xml.XPath;
using System.Xml;
using System.IO;

namespace HMI_MT
{
    public partial class AboutForm : Form
    {
        public AboutForm(  )
        {
            InitializeComponent();
        }

        private void button1_Click( object sender, EventArgs e )
        {
            Close();
        }
		 
		 private void AboutForm_Load( object sender, EventArgs e )
		 {
			 // название проекта  и ДИВГ
			 // открываем файл проекта
			 string FILE_NAME = Application.StartupPath + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "Project.cfg";
			 CommonUtils.CommonUtils.LoadXml( FILE_NAME );
			 //LoadXml( FILE_NAME );

			 XmlTextReader reader = new XmlTextReader( FILE_NAME );
			 XmlDocument doc = new XmlDocument();
			 doc.Load( reader );
			 reader.Close();

			 // выделим узел по условию
			 XmlNode oldCd;
			 XmlElement root = doc.DocumentElement;
			 oldCd = root.SelectSingleNode( "/Project/Divg" );
			 label3.Text = oldCd.InnerText.Trim();

			 oldCd = root.SelectSingleNode( "/Project/NamePTK" );
			 label1.Text = oldCd.InnerText.Trim();

          // определим дату сборки
          FileInfo fvi = new FileInfo( Application.ExecutablePath );

          //sbMesIE.Text = sbMesIE.Text + " (сборка: " + Assembly.GetExecutingAssembly( ).GetName( ).Version + " от " + fvi.LastWriteTime.ToShortDateString( ) + ")";

          lblVersion.Text = "Версия сборки: " + Assembly.GetExecutingAssembly( ).GetName( ).Version + " от " + fvi.LastWriteTime.ToShortDateString( ) + ")";
		 }

       private void label1_SizeChanged( object sender, EventArgs e )
       {
          if( label1.Width > this.Width )
            this.Width = label1.Width + 20;
       }
    }
}