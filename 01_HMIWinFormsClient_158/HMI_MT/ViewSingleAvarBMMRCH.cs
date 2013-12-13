using System;
using System.Xml;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Calculator;
using LabelTextbox;
using System.IO;


namespace HMI_MT
{
   public partial class ViewSingleAvarBMMRCH : Form
   {
      private MainForm parent;
      ArrayList arrAvarSign = new ArrayList( );
      int iFC;            // номер ФК целочисленный
      string strFC;       // номер ФК строка
      int iIDDev;         // номер устройства целочисленный
      string strIDDev;    // номер устройства строка
      int inumLoc;         // номер ячейки целочисленный
      string strnumLoc;    // номер ячейки строка
      string nfXMLConfig; // имя файла с описанием
      SortedList se = new SortedList( );
      StringBuilder sbse = new StringBuilder( );
      SortedList slFLP = new SortedList( );	// для хранения инф о FlowLayoutPanel
      ErrorProvider erp = new ErrorProvider( );

      public ViewSingleAvarBMMRCH()
      {
         InitializeComponent( );
      }
      public ViewSingleAvarBMMRCH( MainForm linkMainForm, int iFC, int iIDDev, int inumLoc, string fXML )
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
        }

        /// <summary>
        /// TestCCforFLP
        /// </summary>
        /// <param Name="cc"></param>
        private void TestCCforFLP( Control cc )
        {
           for( int i = 0 ;i < cc.Controls.Count ;i++ )
           {
              if( cc.Controls[ i ] is FlowLayoutPanel )
              {
                 FlowLayoutPanel flp = ( FlowLayoutPanel ) cc.Controls[ i ];
                 slFLP[ flp.Name ] = flp;
              }
              else
              {
                 TestCCforFLP( cc.Controls[ i ] );
              }
           }
        }

      private void ViewSingleAvarBMMRCH_Load( object sender, EventArgs e )
      {
			  // перечисляем flowLayutPanel
			  ControlCollection cc;
			  cc = ( ControlCollection ) this.Controls;

			  for( int i = 0 ; i < cc.Count ; i++ )
			  {
				  if( cc[i] is FlowLayoutPanel )
				  {
					  FlowLayoutPanel flp = ( FlowLayoutPanel ) cc[i];
					  slFLP[flp.Name] = flp;
				  }
				  else
				  {
					  TestCCforFLP( cc[i] );
				  }
			  }

            if( arrAvarSign.Count != 0 )
                return;

            CreateArrayList( arrAvarSign, "arrAvarSign" );

            // размещаем динамически на форме
            for( int i = 0; i < arrAvarSign.Count; i++ )
            {
                FormulaEval ev = ( FormulaEval ) arrAvarSign[i];
                // смотрим категорию вкладки для размещения тега и его тип
                CheckBoxVar chBV;
                ctlLabelTextbox usTB;
					 switch( ev.ToT )
					 {
						 case TypeOfTag.Analog:
							 usTB = new ctlLabelTextbox();
							 usTB.LabelText = "";
							 usTB.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 usTB.AutoSize = true;
							 ev.StrFormat = HMI_Settings.Precision;
							 ev.OnChangeValForm += usTB.LinkSetText;
							 ev.FirstValue();
							 break;
						 case TypeOfTag.Discret:
							 chBV = new CheckBoxVar();
							 chBV.Parent = ( FlowLayoutPanel ) slFLP[ev.ToP];
							 chBV.AutoSize = true;
							 ev.OnChangeValForm += chBV.LinkSetText;
							 ev.FirstValue();
							 break;
						 default:
							 MessageBox.Show( "Нет такого типа сигнала", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
							 break;
					 }

            }            
      }
      private void CreateArrayList( ArrayList arrVar, string name_arrVar )
      {
         SortedList sl = new SortedList( );
         ArrayList alVal = new ArrayList( );
         // чтение XML
         System.Xml.XmlTextReader reader = new XmlTextReader( nfXMLConfig );
         reader.WhitespaceHandling = WhitespaceHandling.Significant; // обработка только значимых пробелов

         //вывод отладочный в файл
         FileStream fs = File.Create( "bmrz.xio" );
         StreamWriter sw = new StreamWriter( fs );
         try
         {
            while( reader.Read( ) )
            {
               if( reader.NodeType == XmlNodeType.Element )
               {
                  if( reader.Name.Equals( name_arrVar ) )
                  {
                     while( reader.Read( ) )
                        if( reader.Name.Equals( "formula" ) )
                        {
                           // формируем элементы формулы
                           sl[ "formula" ] = reader.GetAttribute( "Express" );
                           sl[ "caption" ] = reader.GetAttribute( "Caption" );
                           sl[ "dim" ] = reader.GetAttribute( "Dim" );
                           sl[ "TypeOfTag" ] = reader.GetAttribute( "TypeOfTag" );
                           sl[ "TypeOfPanel" ] = reader.GetAttribute( "TypeOfPanel" );
                           TypeOfTag ToT = TypeOfTag.no;
                           string ToP = "";

                           sw.WriteLine( sl[ "caption" ] );
                           sw.Flush( );

                           switch( ( string ) sl[ "TypeOfTag" ] )
                           {
                              case "Analog":
                                 ToT = TypeOfTag.Analog;
                                 break;
                              case "Discret":
                                 ToT = TypeOfTag.Discret;
                                 break;
                              case "Combo":
                                 ToT = TypeOfTag.Combo;
                                 break;
                              case "No":
                                 ToT = TypeOfTag.no;
                                 break;
                              default:
                                 MessageBox.Show( "Нет такого типа сигнала" );
                                 break;
                           }
                           ToP = ( string ) sl[ "TypeOfPanel" ];

                           // читаем теги
                           alVal.Clear( );
                           while( reader.Read( ) )
                              if( reader.Name.Equals( "value" ) )
                                 alVal.Add( reader.GetAttribute( 0 ) );
                              else
                                 break;
                           if( alVal.Count == 2 )
                              arrVar.Add( new FormulaEval( parent.KB, "0(" + strFC + "." + strIDDev + ( string ) alVal[ 0 ] + ")1(" + strFC + "." + strIDDev + ( string ) alVal[ 1 ] + ")", sl[ "formula" ].ToString( ), sl[ "caption" ].ToString( ), sl[ "dim" ].ToString( ), ToT, ToP ) );
                           else
                              arrVar.Add( new FormulaEval( parent.KB, "0(" + strFC + "." + strIDDev + ( string ) alVal[ 0 ] + ")", sl[ "formula" ].ToString( ), sl[ "caption" ].ToString( ), sl[ "dim" ].ToString( ), ToT, ToP ) );
                        }
                        else if( reader.Name.Equals( "simple_eval" ) )
                        {
                           sbse.Length = 0;
                           sbse.Append( reader.GetAttribute( "Name" ) );
                           reader.Read( );
                           se[ sbse.ToString( ) ] = reader.GetAttribute( 0 );
                           reader.Read( );
                        }
                        else if( reader.Name.Equals( "" ) )
                           continue;
                        else
                           break;
                     break;
                  }
                  else
                     continue;
               }
               //else
               //    Console.WriteLine( "{0}:{1}", reader.NodeType, reader.Value );
            }
         }
         catch( XmlException ee )
         {
            System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : ViewSingleAvarBMMRCH : XmlException : " + ee.Message );
            sw.Close( );
            fs.Close( );
         }
         sw.Close( );
         fs.Close( );

         reader.Close( );
      }        

   }
}