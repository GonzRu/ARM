using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Calculator;
using LabelTextbox;
using CRZADevices;
using CommonUtils;
using System.Xml.Linq;
using System.Data.Common;
using System.Data.SqlClient;
using FileManager;
using LibraryElements;
using WindowsForms;
using Structure;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace HMI_MT
{
   public partial class ViewSingleAvar4Sirius : frmBMRZbase
   {
      private MainForm parent;
      ArrayList arrAvarSign = new ArrayList();
      int iFC;            // номер ФК целочисленный
      string strFC;       // номер ФК строка
      int iIDDev;         // номер устройства целочисленный
      string strIDDev;    // номер устройства строка
      int inumLoc;         // номер ячейки целочисленный
      string strnumLoc;    // номер ячейки строка
      string nfXMLConfig; // имя файла с описанием
      SortedList se = new SortedList();
      StringBuilder sbse = new StringBuilder();
      SortedList slFLP = new SortedList();	// для хранения инф о FlowLayoutPanel
      ErrorProvider erp = new ErrorProvider();
      byte[]arrBlokAvar;
      //public string path2PrgDevCFG = string.Empty;          // PrgDevCFG.cdp
      //public string path2FrmDev = string.Empty;             // путь к файлу описания формы проекта
      //public string path2DeviceCFG = string.Empty;          // device.cfg
      //public ErrorProvider erp = new ErrorProvider();

      public ViewSingleAvar4Sirius( MainForm linkMainForm, int iFC, int iIDDev, int inumLoc, string fXML, byte[] arrblokavar )
         : base( linkMainForm, iFC, iIDDev, fXML )
      {
         InitializeComponent();

         parent = linkMainForm;
         this.iFC = iFC;                 // номер ФК целочисленный
         strFC = iFC.ToString();         // номер ФК строка
         this.iIDDev = iIDDev;           // номер устройства целочисленный
         strIDDev = iIDDev.ToString();   // номер устройства строка
         this.inumLoc = inumLoc;         // номер ячейки целочисленный
         strnumLoc = inumLoc.ToString();    // номер ячейки строка
         nfXMLConfig = fXML;
         arrBlokAvar = arrblokavar;
      }

      private void ViewSingleAvar4Sirius_Load(object sender, EventArgs e)
      {
         // подправим базовую форму
         splitContainer1.Panel2Collapsed = true;
         tc_Main.Visible = false;

         Panel pnlTPSpab = new Panel();
         pnlTPSpab.Dock = DockStyle.Fill;
         pnlTPSpab.Parent = splitContainer1.Panel1;
         ArrayList arrVars = new ArrayList();

         tabpageSrabat.Enter += new EventHandler( tabpageSrabat_Enter );
         slTPtoArrVars.Add( tabpageSrabat.Text, new ArrayList() );

         #region по источнику и номеру устройства опрделяем пути к файлам
         path2PrgDevCFG = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + "Project" + Path.DirectorySeparatorChar + "PrgDevCFG.cdp";
         XDocument xdoc = XDocument.Load( path2PrgDevCFG );
         IEnumerable<XElement> xes = xdoc.Descendants( "FC" );
         var xe = ( from nn in
                       ( from n in xes
                         where n.Attribute( "numFC" ).Value == StrFC
                         select n ).Descendants( "Device" )
                    where nn.Element( "NumDev" ).Value == IIDDev.ToString()  //( IIDDev - ( 256 * IFC ) )
                    select nn ).First();

         path2DeviceCFG = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + xe.Element( "nameR" ).Value + Path.DirectorySeparatorChar + "Device.cfg";
         path2FrmDev = AppDomain.CurrentDomain.BaseDirectory + Path.DirectorySeparatorChar + xe.Element( "nameR" ).Value + Path.DirectorySeparatorChar + "frm" + xe.Element( "nameELowLevel" ).Value + ".xml";
         if( !File.Exists( path2DeviceCFG ) )
         {
            MessageBox.Show( "Файл =" + path2DeviceCFG + " = не существует.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning );
            return;
         }
         if( !File.Exists( path2FrmDev ) )
         {
            MessageBox.Show( "Файл =" + path2FrmDev + " = не существует.", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Warning );
            return;
         }
         #endregion

         int nd = GetAdrBlockData( path2DeviceCFG, 8 );
         if( nd == -1 )
         {
            MessageBox.Show( "Не задан адрес группы срабатывания в файле Device.cfg", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
            return;
         }

         ParseBDPacket(arrBlokAvar,Convert.ToUInt16( nd ),8);
         //parent.newKB.PacketToQueDev( , , iFC, iIDDev );
         //SetArhivGroupInDev( iFC, iIDDev, 8 );


         // для Сириуса группа с авариями должна называться "Срабатывание"
         PrepareTabPagesForGroup("Срабатывание", tabpageSrabat, ref arrVars, pnlTPSpab);
      }

      void tabpageSrabat_Enter( object sender, EventArgs e )
      {
         /*
         * скрываем панели
         */
         foreach( UserControl p in arDopPanel )
            p.Visible = false;

         //pnlSrabat.Visible = true;

         //lstvAvar.Items.Clear();

        // AvarBD();

         //-------------------------------------------------------------------
         //готовим инф. для отображения текущих значений аналоговых и дискретных сигналов
         TabPage tp_this = (TabPage) sender;
         ArrayList arrVars = (ArrayList) slTPtoArrVars [ tp_this.Text ];
         if( arrVars.Count != 0 )//arrStatusDevCommand
            return;

         PrepareTabPagesForGroup( tp_this.Text, tp_this, ref arrVars, null/*pnlTPSpab*/ );
         slTPtoArrVars [ tp_this.Text ] = arrVars; // ref не отрабатывает (?)
      }

   }
}
