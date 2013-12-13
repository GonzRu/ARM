using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using FileManager;
using LibraryElements;
using WindowsForms;
using System.IO;
using Structure;

namespace HMI_MT
{
   public partial class frm2Panels : MainMnemo
   {
      MainForm parent;
      List<Element> listPanel1;
      List<Element> listPanel2;
      Bitmap t;
      MainMnemo Form_ez;
      List<DinamicControlTrunck> trunklist;  // список шин
      bool isFirstPaint = false;

      Panel pnl = new Panel( );  // панель для отображения мнемосхемы. ее можно двигать в пределах контейнера для правильного позиционирования при смене монитора

      #region конструктор
      public frm2Panels( MainForm mf )
         : base( mf, "none" )
      {
         InitializeComponent( );

         parent = mf;
         #region читаем конфигурационный файл - загружаем мнемосхемы
         string fileName = "";
         //string mnemoCaption = String.Empty;

         this.DoubleBuffered = true;
         this.FormClosing += new FormClosingEventHandler( frm2Panels_FormClosing );

         // выясним конфигурацию мнемосхем
         XDocument xdoc = XDocument.Load(parent.PathToPrjFile);
         IEnumerable<XElement> mnemoshems = xdoc.Element("Project").Element("Mnemoshems").Elements("Mnemo");

         Debug.Assert(mnemoshems.Count() == 2, "Количество мнемосхем != 2");

         // загружаем первую мнемосхему
         XElement xm = mnemoshems.ElementAt(0);

         // настраиваем панель
         pnl.BorderStyle = BorderStyle.None;
         pnl.Dock = DockStyle.None;
         int x = Int32.Parse( xm.Element( "Mnemolevel1" ).Element( "location" ).Attribute( "x" ).Value );
         int y = Int32.Parse( xm.Element( "Mnemolevel1" ).Element( "location" ).Attribute( "y" ).Value );
         pnl.Location = new Point( x, y );
         int width = Int32.Parse( xm.Element( "Mnemolevel1" ).Element( "size" ).Attribute( "width" ).Value );
         int height = Int32.Parse( xm.Element( "Mnemolevel1" ).Element( "size" ).Attribute( "height" ).Value );
         pnl.Size = new Size( width, height );
         this.Controls.Add( pnl );
         pnl.Controls.Add( splitContainer1 );

         fileName = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + xm.Element( "FileName" ).Value;
         //mnemoCaption = xm.Element( "MnemoCaption" ).Value;
         listPanel1 = new List<Element>();

         LoadSchem(fileName, listPanel1, /*splitContainer1.Panel1,*/ dctrnk_ChangeValue); // для загрузки двух схем доделать: вместо панелей исп формы(см. Mainmnemo)

         splitContainer1.Panel1.Tag = "Panel1";
         splitContainer1.Panel1.MouseClick += new MouseEventHandler(Panel1_MouseClick);

         // загружаем вторую мнемосхему
         xm = mnemoshems.ElementAt(1);
         fileName = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + xm.Element( "FileName" ).Value;
         //mnemoCaption = xm.Element("MnemoCaption").Value;
         listPanel2 = new List<Element>();

         LoadSchem(fileName, listPanel2, /*splitContainer1.Panel2,*/ dctrnk_ChangeValue); // для загрузки двух схем доделать: вместо панелей исп формы(см. Mainmnemo)

         splitContainer1.Panel2.Tag = "Panel2";
         splitContainer1.Panel2.MouseClick += new MouseEventHandler(Panel1_MouseClick);

         this.Text = "Главная мнемосхема объекта";
         #endregion
      }

      private void dctrnk_ChangeValue( object sender, EventArgs e )
      {
         this.Refresh( );
      }

      void frm2Panels_FormClosing( object sender, FormClosingEventArgs e )
      {
         pnl.Dispose( );
      } 
      #endregion

      void Panel1_MouseClick( object sender, MouseEventArgs e )
      {
         SplitterPanel panel = ( SplitterPanel ) sender;

         CreateMainMnemo( ( string ) panel.Tag );
      }

      #region вторичная мнемосхема
      /// <summary>
      /// создание вторичной мнемосхемы арм'а
      /// </summary>
      private void CreateMainMnemo( string panelName )
      {
         #region MainMnemo
         Form_ez = new MainMnemo( parent, panelName );
         Form_ez.MdiParent = parent;
         Form_ez.MaximumSize = parent.Size;
         Form_ez.Width = parent.ClientSize.Width;
         Form_ez.Height = parent.ClientSize.Height;
         Form_ez.WindowState = FormWindowState.Maximized;

         foreach (Form f in parent.arrFrm)
         {
            if (f.Text == Form_ez.Text)
            {
               f.Focus();
               Form_ez.Dispose();
               return;
            }
         }

         parent.arrFrm.Add(Form_ez);

         Form_ez.Show( );

         #endregion
      }
      #endregion   

      #region формирование фонового изображения
      private void frm2Panels_Shown( object sender, EventArgs e )
      {
         splitContainer1.BorderStyle = BorderStyle.None;
         splitContainer1.SplitterWidth = 1;
         splitContainer1.IsSplitterFixed = true;

         // отрисовка list в фоновое изображение
         if ( !isFirstPaint )
         {
            t = new Bitmap( splitContainer1.Panel1.ClientSize.Width, splitContainer1.Panel1.ClientSize.Height );
            // Поверхность для рисования на битовой карте!
            Graphics ee = Graphics.FromImage( t );

            foreach ( Element _search in listPanel1 )
            {
               //if ( _search.ElementModel != Model.Dinamic )
               //{
                  _search.DrawElement( ee );
               //}
            }
            splitContainer1.Panel1.BackgroundImage = t;

            t = new Bitmap( splitContainer1.Panel2.ClientSize.Width, splitContainer1.Panel2.ClientSize.Height );
            // Поверхность для рисования на битовой карте!
            ee = Graphics.FromImage( t );

            foreach ( Element _search in listPanel2 )
            {
               //if ( _search.ElementModel != Model.Dinamic )
               //{
                  _search.DrawElement( ee );
               //}
            }
            splitContainer1.Panel2.BackgroundImage = t;

            isFirstPaint = true;
         }
      }
	   #endregion
   }
}
