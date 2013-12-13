using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HMI_MT
{
   public partial class ViewSingleAvarOZZ : frmBMRZbase
   {
      byte [] arrBlokAvar;
      public ViewSingleAvarOZZ()
      {
         InitializeComponent();

      }
      public ViewSingleAvarOZZ( MainForm linkMainForm, int iFC, int iIDDev, int inumLoc, string fXML, byte [ ] arrbl )
		{
               InitializeComponent();

               splitContainer5.Panel1Collapsed = true;
               parent = linkMainForm;
               IFC = iFC;                 // номер ФК целочисленный
               StrFC = iFC.ToString();         // номер ФК строка
               IIDDev = iIDDev;           // номер устройства целочисленный
               StrIDDev = iIDDev.ToString();   // номер устройства строка
               InumLoc = inumLoc;         // номер ячейки целочисленный
               StrnumLoc = inumLoc.ToString();    // номер ячейки строка
               nfXMLConfig = fXML;

               arrBlokAvar = arrbl;  // блок данных аварии
        }
      #region Load
      private void frmSirius_OZZ_Load( object sender, EventArgs e )
      {
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

         // формируем сортированный список с панелями
         xdoc = XDocument.Load( path2DeviceCFG );
         DevPanelTypes = new SortedList();

         if( !String.IsNullOrEmpty( (string) xdoc.Element( "Device" ).Element( "TypeOfPanelSections" ) ) )
         {
            IEnumerable<XElement> etypes = xdoc.Element( "Device" ).Element( "TypeOfPanelSections" ).Elements( "TypeOfPanel" );

            foreach( XElement xr in etypes )
               // определим вариант формата секции TypeOfPanel
               if( (string) xr.Element( "Name" ) == null )
                  DevPanelTypes.Add( xr.Value, String.Empty );
               else
                  DevPanelTypes.Add( xr.Element( "Name" ).Value, xr.Element( "Caption" ).Value );
         }
         else
            MessageBox.Show( "Типы панелей в файле Device.cfg отсутсвуют", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning );

         GetCCforFLP( (ControlCollection) this.Controls );

         int nd = GetAdrBlockData( path2DeviceCFG, 8 );
         if( nd == -1 )
         {
            MessageBox.Show( "Не задан адрес группы срабатывания в файле Device.cfg", this.Name, MessageBoxButtons.OK, MessageBoxIcon.Error );
            return;
         }

         ParseBDPacket( arrBlokAvar, Convert.ToUInt16( nd ), 8 );
         //parent.newKB.PacketToQueDev( , , iFC, iIDDev );
         //SetArhivGroupInDev( iFC, iIDDev, 8 );


         // для Сириуса группа с авариями должна называться "Срабатывание"
         //PrepareTabPagesForGroup( "Срабатывание", tabpageSrabat, ref arrVars, pnlTPSpab );
      }
      #endregion
      #region Срабатывание
      void tabpageSrabat_Enter( object sender, EventArgs e )
      {
         lstvAvar.Items.Clear();

         //-------------------------------------------------------------------
         //готовим инф. для отображения текущих значений аналоговых и дискретных сигналов
         TabPage tp_this = (TabPage) sender;
         ArrayList arrVars = (ArrayList) slTPtoArrVars [ tp_this.Text ];
         if( arrVars.Count != 0 )//arrStatusDevCommand
            return;

         PrepareTabPagesForGroup( tp_this.Text, tp_this, ref arrVars, pnlTPSpab );
         slTPtoArrVars [ tp_this.Text ] = arrVars; // ref не отрабатывает (?)
      }
      #endregion
   }
}
