using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using CRZADevices;
using LabelTextbox;
using NetCrzaDevices;
using DataModule;
using NSNetNetManager;

namespace HMI_MT
{
	public partial class dlgTagBrowser : Form
	{
		ArrayList cfg = new ArrayList();
		string varLinkPath;
		string varName;
		string varDim;
		Form parent;

		/// <summary>
		/// public dlgTagBrowser( ArrayList cfg )
		/// конструктор с параметром
		/// </summary>
		/// <param Name="cfg"></param>
		public dlgTagBrowser( Form prnt, ArrayList cfg )
		{
			InitializeComponent();
			this.cfg = cfg;
			parent = prnt;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param Name="sender"></param>
		/// <param Name="e"></param>
		private void btnClose_Click_1( object sender, EventArgs e )
		{
			this.Close();
		}
		#region Загрузка формы и заполнение TreeView
		/// <summary>
		/// private void dlgTagBrowser_Load( object sender, EventArgs e )
		/// Действия при загрузке формы
		/// </summary>
		/// <param Name="sender"></param>
		/// <param Name="e"></param>
		private void dlgTagBrowser_Load( object sender, EventArgs e )
		{
			//размер формы
			this.Width = parent.Width * 2 / 3;
			this.Height = parent.Height * 2 / 3;
			gb1_CurrentConfig.Width = Width / 3;
			gb2_HiLevel.Width = Width / 3;
			gb3_LowLevel.Width = Width / 3;

			// добавление конфигурации
			treeViewKB.Nodes.Add( "конфигурация проекта" );
			PopulateTreeView( treeViewKB.Nodes[0] );
         //treeViewKB.ExpandAll( );

         // по умодчанию - не эмуляция
         btnSetEmulValue.Enabled = false;
		}
		/// <summary>
		/// public void PopulateTreeView( TreeNode parentNode )
		/// заполнение узлов TreeView
		/// </summary>
		/// <param Name="parentNode"></param>
		public void PopulateTreeView( TreeNode parentNode )
		{
			foreach( DataSource aFC in cfg )
			{
				TreeNode aFCNode = new TreeNode( "Функциональный контроллер №" + aFC.NumFC );
            aFCNode.Tag = aFC;
				parentNode.Nodes.Add( aFCNode );
				foreach( TCRZADirectDevice aDev in aFC )
				{
					TreeNode aDevNode = new TreeNode( "[ " + aDev.NumDev.ToString() + " ] " + aDev.ToString() );//.NumSlot
               aDevNode.Tag = aDev;
					aFCNode.Nodes.Add( aDevNode );
					foreach( TCRZAGroup aGroup in aDev )
					{
						TreeNode aGroupNode = new TreeNode( aGroup.Name );
                  aGroupNode.Tag = aGroup;
						aDevNode.Nodes.Add( aGroupNode );
						foreach( TCRZAVariable aVariable in aGroup )
						{
							TreeNode aVariableNode = new TreeNode( aVariable.Name );
                     aVariableNode.Tag = aVariable;
							aGroupNode.Nodes.Add( aVariableNode );
							// распознать по типам и присвоить иконку для битовых и аналоговых сигналов
						}
					}
				}
			}
		}
		#endregion

		#region Выделение узла TreeView
		/// <summary>
		/// private void treeViewKB_MouseClick( object sender, MouseEventArgs e )
		/// обработка выделения узла TreeView
		/// </summary>
		/// <param Name="sender"></param>
		/// <param Name="e"></param>
		private void treeViewKB_MouseClick( object sender, MouseEventArgs e )
		{
         if ( e.Button != MouseButtons.Left )
            return;

			TreeNode node = treeViewKB.GetNodeAt( e.X, e.Y );

			if( node == null )
				return;

			treeViewKB.SelectedNode = node;
			varLinkPath = node.FullPath;

         #region Новый код
         //string strFC = "";
         string strIDDev = "";
         string strIDGrp = "";
         string strAdrVar = "";
         string strBitMask = "";

         chbEmulateValue.Text = "Эмулировать значение = =";
         //далее действия в зависимости от типа узла
         if (node.Tag is FC)
         {
            FC aFC = (FC)node.Tag;
            //выводим информацию об ФК
            GetFCInfo( aFC );
         }
         else if ( node.Tag is TCRZADirectDevice )
         {
            TCRZADirectDevice aDev = ( TCRZADirectDevice ) node.Tag;
            strIDDev = aDev.NumDev.ToString( );

            // Для начала меняем курсор
            Cursor crs = this.Cursor;
            this.Cursor = Cursors.WaitCursor;

            // очистим richtext
            rtbASULevel.Clear( );

            GetDeviceInfoNetLevel( aDev );	// отображаем память сетевого уровня
            GetDeviceInfoASULevel( aDev );	// отображаем группы и теги устройства уровня АСУ

            // восстанавливаем курсор
            this.Cursor = crs;
         }
         else if ( node.Tag is TCRZAGroup )
         {
            TCRZAGroup aGroup = ( TCRZAGroup ) node.Tag;
            strIDGrp = aGroup.Id.ToString( );

            // очистим richtext
            rtbASULevel.Clear( );

            GetInfoAboutGroup( aGroup );		// заполняем панель состояния и отображаем теги группы уровня АСУ
         }
         else if ( node.Tag is TCRZAVariable )
         {
            TCRZAVariable aVariable = ( TCRZAVariable ) node.Tag;
            GetInfoAboutVariable( aVariable );		// заполняем панель состояния информацией о теге группы уровня АСУ

            varName = aVariable.Name;
            varDim = aVariable.Dim;

            strAdrVar = aVariable.RegInDev.ToString( );
            if ( aVariable is TBitFieldVariable )
            {
               // сравниваем маски
               TBitFieldVariable bitTmp = ( TBitFieldVariable ) aVariable;
               strBitMask = bitTmp.bitMask;
            }
            else
               strBitMask = "0";

            btnSetEmulValue.Tag = aVariable;
            chbEmulateValue.Text = "Эмулировать значение =" + aVariable.Name + "=";
         }


         #endregion

         //return;

         //// генерим привязку к тегу
         //char[] aa = { '[', ']', '\\', '№' };
         //string[] pieces = varLinkPath.Split( aa );

         //if ( pieces.Length < 8 )
         //   chbEmulateValue.Text = "Эмулировать значение = =";

         //// локализуем
         //string strFC = "";
         //string strIDDev = "";
         //string strIDGrp = "";
         //string strAdrVar = "";
         //string strBitMask = "";

         //foreach( FC aFC in cfg )
         //{
         //   if( pieces.Length < 2 )
         //      return;
         //   if( aFC.NumFC == Convert.ToInt32( pieces[2] ) )
         //   {
         //      strFC = pieces[2];

         //      //выводим информацию об ФК
         //      GetFCInfo( aFC );

         //      if( pieces.Length < 4 )
         //         return;

         //      foreach( TCRZADirectDevice aDev in aFC )
         //      {
         //         if( aDev.NumDev == Convert.ToInt32( pieces[4] ) && aDev.ToString() == pieces[5].Trim())  //.NumSlot
         //         {
         //            strIDDev = aDev.NumDev.ToString();

         //            // Для начала меняем курсор
         //            Cursor crs = this.Cursor;
         //            this.Cursor = Cursors.WaitCursor;

         //            // очистим richtext
         //            rtbASULevel.Clear();

         //            GetDeviceInfoNetLevel( aDev );	// отображаем память сетевого уровня
         //            GetDeviceInfoASULevel( aDev );	// отображаем группы и теги устройства уровня АСУ

         //            // восстанавливаем курсор
         //            this.Cursor = crs;


         //            if( pieces.Length < 7 )
         //               return;

         //            foreach( TCRZAGroup aGroup in aDev )
         //            {
         //               if( aGroup.Name == pieces[6] )
         //               {
         //                  strIDGrp = aGroup.Id.ToString();

         //                  // очистим richtext
         //                  rtbASULevel.Clear();

         //                  GetInfoAboutGroup( aGroup );		// заполняем панель состояния и отображаем теги группы уровня АСУ

         //                  if( pieces.Length < 8 )
         //                     return;

         //                  foreach( TCRZAVariable aVariable in aGroup )
         //                  {
         //                     if( aVariable.Name == pieces[7] )
         //                     {
         //                        GetInfoAboutVariable( aVariable );		// заполняем панель состояния информацией о теге группы уровня АСУ

         //                        varName = aVariable.Name;
         //                        varDim = aVariable.Dim;

         //                        strAdrVar = aVariable.RegInDev.ToString();
         //                        if( aVariable is TBitFieldVariable )
         //                        {
         //                           // сравниваем маски
         //                           TBitFieldVariable bitTmp = ( TBitFieldVariable ) aVariable;
         //                           strBitMask = bitTmp.bitMask;
         //                        }
         //                        else
         //                           strBitMask = "0";

         //                        btnSetEmulValue.Tag = aVariable;
         //                        chbEmulateValue.Text = "Эмулировать значение =" + aVariable.Name + "=";
         //                     }
         //                  }
         //               }
         //            }
         //         }
         //      }
         //   }
         //}
		}
		#endregion

		#region Вывод информации об ФК в панель свойств нашей формы
		/// <summary>
		/// private void GetFCInfo( string sFC )
		/// вывести информацию об ФК в панель свойств нашей формы
		/// </summary>
		/// <param Name="sFC"></param>
		private void GetFCInfo( FC aFC )
		{
		}
		#endregion

		#region Вывод информации об устройстве в панель свойств и памяти устройства в панель сетевого уровня
		/// <summary>
		/// private void GetDeviceInfoNetLevel( TCRZADirectDevice aDev )
		/// Вывод информации об устройстве в панель свойств и памяти устройства в панель сетевого уровня
		/// </summary>
		/// <param Name="aDev"></param>
		private void GetDeviceInfoNetLevel( TCRZADirectDevice aDev )
		{
			// чистим richtext
			rtbNetLevel.Clear();

			StringBuilder sb = new StringBuilder();

			// перечисляем нижний уровень			
			foreach( MTDevice aBMRZ in Configurator.MTD )
            if( aDev.NumDev == aBMRZ.NDev && aBMRZ.ToString() != "FC_net" && aDev.NumFC == aBMRZ.NFC )
            //if( aDev.NumDev == aBMRZ.NDev )
				{
					// ... сортируем по возрастанию ключей

					SortedList sl = new SortedList();
					foreach( DictionaryEntry de in aBMRZ.varDev )
					{
						sl[de.Key] = de.Value;
					}

					foreach(DictionaryEntry sle in sl)
					{
						sb.Length = 0;
						switch( sle.Value.ToString() )
						{
							case "Int_FieldMT":
							Int_FieldMT tmp_Int_FieldMT = (Int_FieldMT)sle.Value;
							sb.AppendLine( String.Format( "{0:D5} = ({1}) {2}", sle.Key, sle.Value.ToString(), tmp_Int_FieldMT.varMT_Value ) );
								break;
                            case "DInt_FieldMT":
                                DInt_FieldMT tmp_DInt_FieldMT = (DInt_FieldMT)sle.Value;
                                sb.AppendLine(String.Format("{0:D5} = ({1}) {2}", sle.Key, sle.Value.ToString(), tmp_DInt_FieldMT.varMT_Value));
                                break;
							case "Bit_FieldMT":
								Bit_FieldMT tmp_Bit_FieldMT = ( Bit_FieldMT ) sle.Value;

								sb.Append( String.Format( "{0:D5} = ({1}) ", sle.Key, sle.Value.ToString() ) );
								for( int i = 0 ; i < tmp_Bit_FieldMT.varMT_Value.Length ; i++ )
								{
									sb.Append( String.Format( "{0:X2} ", tmp_Bit_FieldMT.varMT_Value[i] ) );
								}
								sb.Append("\n");
								break;
                            case "Byte_FieldMT":
                                Byte_FieldMT tmp_Byte_FieldMT = (Byte_FieldMT)sle.Value;
                                
                                sb.Append(String.Format("{0:D5} = ({1}) ", sle.Key, sle.Value.ToString()));
                                for (int i = 0; i < tmp_Byte_FieldMT.varMT_Value.Length; i++)
                                {
                                    sb.Append(String.Format("{0:X2} ", tmp_Byte_FieldMT.varMT_Value[i]));
                                }
                                sb.Append("\n");

                                break;
							case "UInt_FieldMT":
								UInt_FieldMT tmp_UInt_FieldMT = ( UInt_FieldMT ) sle.Value;
								sb.AppendLine( String.Format( "{0:D5} = ({1}) {2}", sle.Key, sle.Value.ToString(), tmp_UInt_FieldMT.varMT_Value ) );
								break;
							case "DateTimeUTC_FieldMT":
								DateTimeUTC_FieldMT tmp_DateTimeUTC_FieldMT = ( DateTimeUTC_FieldMT ) sle.Value;

								sb.AppendLine( String.Format( "{0:D5} = ({1}) {2}", sle.Key, sle.Value.ToString(), tmp_DateTimeUTC_FieldMT.varMT_Value.ToString() ) );
								break;
							case "Stringz_FieldMT":
								Stringz_FieldMT tmp_Stringz_FieldMT = ( Stringz_FieldMT ) sle.Value;

								sb.Append( String.Format( "{0:D5} = ({1}) {2}", sle.Key, sle.Value.ToString(), tmp_Stringz_FieldMT.varMT_Value ) );

								foreach( char ch in sb.ToString() )
								{
									if( ch == 0 )
										sb.Length--;
								}
								sb.Append("\n");
								break;
							case "UDInt_FieldMT":
								UDInt_FieldMT tmp_UDInt_FieldMT = ( UDInt_FieldMT ) sle.Value;
								sb.AppendLine( String.Format( "{0:D5} = ({1}) {2}", sle.Key, sle.Value.ToString(), tmp_UDInt_FieldMT.varMT_Value ) );
								break;
                     case "UDInt_FieldMT_0123":
                        UDInt_FieldMT_0123 tmp_UDInt_FieldMT_0123 = (UDInt_FieldMT_0123) sle.Value;
                        sb.AppendLine( String.Format( "{0:D5} = ({1}) {2}", sle.Key, sle.Value.ToString(), tmp_UDInt_FieldMT_0123.varMT_Value ) );
                        break;
							case "BCD_FieldMT":
								BCD_FieldMT tmp_BCD_FieldMT = ( BCD_FieldMT ) sle.Value;

								sb.AppendLine( String.Format( "{0:D5} = ({1}) {2}", sle.Key, sle.Value.ToString(), tmp_BCD_FieldMT.varMT_Value ) );
								break;
							case "DateTime4_FieldMT":
								DateTime4_FieldMT tmp_DateTime4_FieldMT = ( DateTime4_FieldMT ) sle.Value;

								sb.AppendLine( String.Format( "{0:D5} = ({1}) {2}", sle.Key, sle.Value.ToString(), tmp_DateTime4_FieldMT.varMT_Value.ToString() ) );
								break;
							case "Real_FieldMT":
								Real_FieldMT tmp_Real_FieldMT = ( Real_FieldMT ) sle.Value;
								sb.AppendLine( String.Format( "{0:D5} = ({1}) {2:F5}", sle.Key, sle.Value.ToString(), tmp_Real_FieldMT.varMT_Value ) );
								break;
							case "BCDPack_FieldMT":
								BCDPack_FieldMT tmp_BCDPack_FieldMT = ( BCDPack_FieldMT ) sle.Value;

								sb.Append( String.Format( "{0:D5} = ({1}) ", sle.Key, sle.Value.ToString() ) );
								for( int i = 0 ; i < tmp_BCDPack_FieldMT.varMT_Value.Length ; i++ )
								{
									sb.Append( String.Format( "{0:X2} ", tmp_BCDPack_FieldMT.varMT_Value[i] ) );
								}
								sb.Append( "\n" );
								break;
							case "DateTime3_FieldMT":
								DateTime3_FieldMT tmp_DateTime3_FieldMT = ( DateTime3_FieldMT ) sle.Value;

								sb.AppendLine( String.Format( "{0:D5} = ({1}) {2}", sle.Key, sle.Value.ToString(), tmp_DateTime3_FieldMT.varMT_Value.ToString() ) );
								break;
                     case "IPAdress_FieldMT":
                        IPAdress_FieldMT tmp_IPAdress_FieldMT = (IPAdress_FieldMT)sle.Value;

                        sb.Append(String.Format("{0:D5} = ({1}) {2}", sle.Key, sle.Value.ToString(), tmp_IPAdress_FieldMT.varMT_Value));

                        foreach (char ch in sb.ToString())
                        {
                           if (ch == 0)
                              sb.Length--;
                        }
                        sb.Append("\n");
                        break;
                        case "DateTimeSirius_FieldMT":
                            DateTimeSirius_FieldMT tmp_DateTimeSirius_FieldMT = (DateTimeSirius_FieldMT)sle.Value;

                            sb.AppendLine(String.Format("{0:D5} = ({1}) {2}", sle.Key, sle.Value.ToString(), tmp_DateTimeSirius_FieldMT.varMT_Value.ToString()));
                        break;
							default:
								// сообщение об ошибке или исключение
                        MessageBox.Show("Тип " + sle.Value.ToString() + " не поддерживается просмотрщиком тегов.", "dlgTagBrowser", MessageBoxButtons.OK, MessageBoxIcon.Warning);
								break;
						}
						
						rtbNetLevel.AppendText( sb.ToString() );
						
						sb.Length = 0;
					}
				}
		}
		#endregion
		#region Вывод информации о группах и переменных в панель уровня АСУ
		/// <summary>
		/// private void GetDeviceInfoASULevel( TCRZADirectDevice aDev )
		/// выводим информацию о группах и переменных в панель rtbASULevel
		/// </summary>
		/// <param Name="?"></param>
		private void GetDeviceInfoASULevel( TCRZADirectDevice aDev )
		{
			foreach( DataSource aFc in cfg )
				foreach( TCRZADirectDevice tdd in aFc )

						if( tdd.NumDev == aDev.NumDev && tdd.NumFC == aDev.NumFC )
							foreach( TCRZAGroup tdg in tdd )
								GetGroupInfoASULevel( tdg );
		}
		/// <summary>
		///	private void GetInfoAboutGroup( TCRZAGroup aGroup )
		/// </summary>
		/// <param Name="aGroup"></param>
		private void GetInfoAboutGroup( TCRZAGroup aGroup )
		{
			// заполняем информацию о тегах группы
			GetGroupInfoASULevel( aGroup );
		}

		/// <summary>
		/// private void GetGroupInfoASULevel( TCRZAGroup aGroup )
		/// отображаем теги группы уровня АСУ
		/// </summary>
		/// <param Name="aGroup"></param>
		private void GetGroupInfoASULevel( TCRZAGroup aGroup )	
		{
			StringBuilder sb = new StringBuilder();
			//TCRZAGroup tdg = aGroup;
			sb.Length = 0;
			sb.AppendLine( aGroup.Name + ":" );
         rtbASULevel.Clear( );
			rtbASULevel.AppendText( sb.ToString() );
			foreach( TCRZAVariable tgv in aGroup )
			{
				sb.Length = 0;

				switch( tgv.ToString() )
				{
					case "TBitFieldVariable":
						TBitFieldVariable tmp_TBitFieldVariable = ( TBitFieldVariable ) tgv;
						sb.AppendLine( String.Format( "\t({0}) {1} \t({2:D5}) = {3}", tgv.ToString(), tmp_TBitFieldVariable.Name, tmp_TBitFieldVariable.RegInDev, tmp_TBitFieldVariable.Value ) );
						break;
                    case "TByteField2UIntVariable":
                        TByteField2UIntVariable tmp_TByteField2UIntVariable = (TByteField2UIntVariable)tgv;
                        sb.AppendLine(String.Format("\t({0}) {1} \t({2:D5}) = {3}", tgv.ToString(), tmp_TByteField2UIntVariable.Name, tmp_TByteField2UIntVariable.RegInDev, tmp_TByteField2UIntVariable.Value));
                        break;
					case "TUIntVariable":
						TUIntVariable tmp_TUIntVariable = ( TUIntVariable ) tgv;
						sb.AppendLine( String.Format( "\t({0}) {1} \t({2:D5}) = {3}", tgv.ToString(), tmp_TUIntVariable.Name, tmp_TUIntVariable.RegInDev, tmp_TUIntVariable.Value ) );
						break;
                    case "TDIntVariable":
                        TDIntVariable tmp_TDIntVariable = (TDIntVariable)tgv;
                        sb.AppendLine(String.Format("\t({0}) {1} \t({2:D5}) = {3}", tgv.ToString(), tmp_TDIntVariable.Name, tmp_TDIntVariable.RegInDev, tmp_TDIntVariable.Value));
                        break;
					case "TDateTimeVariable":
						TDateTimeVariable tmp_TDateTimeVariable = ( TDateTimeVariable ) tgv;
						sb.AppendLine( String.Format( "\t({0}) {1} \t({2:D5}) = {3}", tgv.ToString(), tmp_TDateTimeVariable.Name, tmp_TDateTimeVariable.RegInDev, tmp_TDateTimeVariable.Value ) );
						break;
					case "TStringVariable":
						TStringVariable tmp_TStringVariable = ( TStringVariable ) tgv;
						sb.Append( String.Format( "\t({0}) {1} \t({2:D5}) = {3}", tgv.ToString(), tmp_TStringVariable.Name, tmp_TStringVariable.RegInDev, tmp_TStringVariable.Value ) );

						foreach( char ch in sb.ToString() )
						{
							if( ch == 0 )
								sb.Length--;
						}
						sb.Append( "\n" );

						break;
					case "TUDIntVariable":
						TUDIntVariable tmp_TUDIntVariable = ( TUDIntVariable ) tgv;
						sb.AppendLine( String.Format( "\t({0}) {1} \t({2:D5}) = {3}", tgv.ToString(), tmp_TUDIntVariable.Name, tmp_TUDIntVariable.RegInDev, tmp_TUDIntVariable.Value ) );
						break;
              case "TUDIntVariable_0123":
                  TUDIntVariable_0123 tmp_TUDIntVariable_0123 = (TUDIntVariable_0123) tgv;
                  sb.AppendLine( String.Format( "\t({0}) {1} \t({2:D5}) = {3}", tgv.ToString(), tmp_TUDIntVariable_0123.Name, tmp_TUDIntVariable_0123.RegInDev, tmp_TUDIntVariable_0123.Value ) );
                  break;
					case "TBCDVariable":
						TBCDVariable tmp_TBCDVariable = ( TBCDVariable ) tgv;
						sb.AppendLine( String.Format( "\t({0}) {1} \t({2:D5}) = {3}", tgv.ToString(), tmp_TBCDVariable.Name, tmp_TBCDVariable.RegInDev, tmp_TBCDVariable.Value ) );
						break;
					case "TIntVariable":
						TIntVariable tmp_TIntVariable = ( TIntVariable ) tgv;
						sb.AppendLine( String.Format( "\t({0}) {1} \t({2:D5}) = {3}", tgv.ToString(), tmp_TIntVariable.Name, tmp_TIntVariable.RegInDev, tmp_TIntVariable.Value ) );
						break;
					case "TFloatVariable":
						TFloatVariable tmp_TFloatVariable = ( TFloatVariable ) tgv;
						sb.AppendLine( String.Format( "\t({0}) {1} \t({2:D5}) = {3:F5}", tgv.ToString(), tmp_TFloatVariable.Name, tmp_TFloatVariable.RegInDev, tmp_TFloatVariable.Value ) );
						break;
					case "TBCDPackVariable":
						TBCDPackVariable tmp_TBCDPackVariable = ( TBCDPackVariable ) tgv;
						sb.AppendLine( String.Format( "\t({0}) {1} \t({2:D5}) = {3}", tgv.ToString(), tmp_TBCDPackVariable.Name, tmp_TBCDPackVariable.RegInDev, tmp_TBCDPackVariable.Value ) );
						break;
					case "TUIntCBVariable":
						TUIntCBVariable tmp_TUIntCBVariable = ( TUIntCBVariable ) tgv;
						sb.AppendLine( String.Format( "\t({0}) {1} \t({2:D5}) = {3}", tgv.ToString(), tmp_TUIntCBVariable.Name, tmp_TUIntCBVariable.RegInDev, tmp_TUIntCBVariable.Value ) );
						break;
               case "TIPAdressVariable":
                  TIPAdressVariable tmp_TIPAdressVariable = (TIPAdressVariable)tgv;
                  sb.Append(String.Format("\t({0}) {1} \t({2:D5}) = {3}", tgv.ToString(), tmp_TIPAdressVariable.Name, tmp_TIPAdressVariable.RegInDev, tmp_TIPAdressVariable.Value));

                  foreach (char ch in sb.ToString())
                  {
                     if (ch == 0)
                        sb.Length--;
                  }
                  sb.Append("\n");
                  break;

               case "TByteField2CBVariable":
                  TByteField2CBVariable tmp_TByteField2CBVariable = (TByteField2CBVariable)tgv;
                  sb.AppendLine(String.Format("\t({0}) {1} \t({2:D5}) = {3}", tgv.ToString(), tmp_TByteField2CBVariable.Name, tmp_TByteField2CBVariable.RegInDev, tmp_TByteField2CBVariable.Value));
						break;
					default:
						// сообщение об ошибке или исключение
                  MessageBox.Show("Тип " + tgv.ToString() + " не поддерживается просмотрщиком тегов.", "dlgTagBrowser", MessageBoxButtons.OK,MessageBoxIcon.Warning );
						break;
				}
				rtbASULevel.AppendText( sb.ToString() );
			}
		}

		/// <summary>
		/// private void GetInfoAboutVariable( TCRZAVariable aVariable )
		/// Информация о теге в панель состояния
		/// </summary>
		/// <param Name="aVariable"></param>
        //private void GetInfoAboutVariable( TCRZAVariable tgv )
        //{
        // switch ( tgv.ToString( ) )
        // {
        //    case "TBitFieldVariable":
        //       rbBitValue_0.Checked = true;
        //       TBitFieldVariable tmp_TBitFieldVariable = ( TBitFieldVariable ) tgv;
        //       if ( tmp_TBitFieldVariable.Value )
        //          rbBitValue_1.Checked = true;
        //       else
        //          rbBitValue_0.Checked = true;
        //       break;
        //    case "TByteField2UIntVariable":
        //       //TByteField2UIntVariable tmp_TByteField2UIntVariable = ( TByteField2UIntVariable ) tgv;
        //       //tbAnalogValue.Text = tgv.
        //       break;
        //    case "TUIntVariable":
        //       TUIntVariable tmp_TUIntVariable = ( TUIntVariable ) tgv;
        //       tbAnalogValue.Text = Convert.ToString(tmp_TUIntVariable.Value);
        //       break;
        //    case "TDateTimeVariable":
        //       TDateTimeVariable tmp_TDateTimeVariable = ( TDateTimeVariable ) tgv;
        //       break;
        //    case "TStringVariable":
        //       TStringVariable tmp_TStringVariable = ( TStringVariable ) tgv;
        //       //sb.Append( String.Format( "\t({0}) {1} \t({2:D5}) = {3}", tgv.ToString( ), tmp_TStringVariable.Name, tmp_TStringVariable.RegInDev, tmp_TStringVariable.Value ) );

        //          //foreach ( char ch in sb.ToString( ) )
        //          //{
        //          //   if ( ch == 0 )
        //          //      sb.Length--;
        //          //}
        //          //sb.Append( "\n" );

        //       break;
        //    case "TUDIntVariable":
        //       TUDIntVariable tmp_TUDIntVariable = ( TUDIntVariable ) tgv;
        //       tbAnalogValue.Text = Convert.ToString(tmp_TUDIntVariable.Value );
        //       break;
        //   case "TUDIntVariable_0123":
        //       TUDIntVariable tmp_TUDIntVariable_0123 = (TUDIntVariable) tgv;
        //       tbAnalogValue.Text = Convert.ToString( tmp_TUDIntVariable_0123.Value );
        //       break;
        //    case "TBCDVariable":
        //       TBCDVariable tmp_TBCDVariable = ( TBCDVariable ) tgv;
        //       tbAnalogValue.Text = Convert.ToString( tmp_TBCDVariable.Value );
        //       break;
        //    case "TIntVariable":
        //       TIntVariable tmp_TIntVariable = ( TIntVariable ) tgv;
        //       tbAnalogValue.Text = Convert.ToString( tmp_TIntVariable.Value );
        //       break;
        //    case "TFloatVariable":
        //       TFloatVariable tmp_TFloatVariable = ( TFloatVariable ) tgv;
        //       tbAnalogValue.Text = Convert.ToString( tmp_TFloatVariable.Value );
        //       break;
        //    case "TBCDPackVariable":
        //       TBCDPackVariable tmp_TBCDPackVariable = ( TBCDPackVariable ) tgv;
        //       tbAnalogValue.Text = Convert.ToString( tmp_TBCDPackVariable.Value );
        //       break;
        //    case "TUIntCBVariable":
        //       //TUIntCBVariable tmp_TUIntCBVariable = ( TUIntCBVariable ) tgv;
        //       break;

        //    default:
        //       // сообщение об ошибке или исключение
        //    break;
        // }         
        //}

      /// <summary>
      /// установить тег из панели состояния
      /// </summary>
      /// <param Name="tgv">ссылка на тег</param>
      private void SetVariableValue( TCRZAVariable tgv )
      {
         if ( tgv == null )
            return;

         switch ( tgv.ToString( ) )
         {
            case "TBitFieldVariable":
               TBitFieldVariable tmp_TBitFieldVariable = ( TBitFieldVariable ) tgv;
               if ( rbBitValue_1.Checked )
                  tmp_TBitFieldVariable.SetEmulateValue(true, VarQuality.vqGood);
               else
                  tmp_TBitFieldVariable.SetEmulateValue(false, VarQuality.vqGood);
               break;
               /*
                * Дополнять по мере необходимости
            case "TByteField2UIntVariable":
               //TByteField2UIntVariable tmp_TByteField2UIntVariable = ( TByteField2UIntVariable ) tgv;
               //tbAnalogValue.Text = tgv.
               break;
                */
            case "TUIntVariable":
               TUIntVariable tmp_TUIntVariable = ( TUIntVariable ) tgv;
               tmp_TUIntVariable.SetEmulateValue( Convert.ToUInt16( tbAnalogValue.Text ), VarQuality.vqGood );
               break;
                /*
            case "TDateTimeVariable":
               TDateTimeVariable tmp_TDateTimeVariable = ( TDateTimeVariable ) tgv;
               break;
            case "TStringVariable":
               TStringVariable tmp_TStringVariable = ( TStringVariable ) tgv;
               //sb.Append( String.Format( "\t({0}) {1} \t({2:D5}) = {3}", tgv.ToString( ), tmp_TStringVariable.Name, tmp_TStringVariable.RegInDev, tmp_TStringVariable.Value ) );

               //foreach ( char ch in sb.ToString( ) )
               //{
               //   if ( ch == 0 )
               //      sb.Length--;
               //}
               //sb.Append( "\n" );

               break;
            case "TUDIntVariable":
               TUDIntVariable tmp_TUDIntVariable = ( TUDIntVariable ) tgv;
               tbAnalogValue.Text = Convert.ToString( tmp_TUDIntVariable.Value );
               break;
            case "TBCDVariable":
               TBCDVariable tmp_TBCDVariable = ( TBCDVariable ) tgv;
               tbAnalogValue.Text = Convert.ToString( tmp_TBCDVariable.Value );
               break;
            case "TIntVariable":
               TIntVariable tmp_TIntVariable = ( TIntVariable ) tgv;
               tbAnalogValue.Text = Convert.ToString( tmp_TIntVariable.Value );
               break;
            case "TFloatVariable":
               TFloatVariable tmp_TFloatVariable = ( TFloatVariable ) tgv;
               tbAnalogValue.Text = Convert.ToString( tmp_TFloatVariable.Value );
               break;
            case "TBCDPackVariable":
               TBCDPackVariable tmp_TBCDPackVariable = ( TBCDPackVariable ) tgv;
               tbAnalogValue.Text = Convert.ToString( tmp_TBCDPackVariable.Value );
               break;
            case "TUIntCBVariable":
               //TUIntCBVariable tmp_TUIntCBVariable = ( TUIntCBVariable ) tgv;
               break;
            */
            default:
               // сообщение об ошибке или исключение
               break;
         }
         // обновить панели
      }
		#endregion

		#region Разные методы
		/// <summary>
		/// private void dlgTagBrowser_SizeChanged( object sender, EventArgs e )
		/// реакция на изменение размера формы
		/// </summary>
		/// <param Name="sender"></param>
		/// <param Name="e"></param>
		private void dlgTagBrowser_SizeChanged( object sender, EventArgs e )
		{
			gb1_CurrentConfig.Width = Width / 3;
			gb2_HiLevel.Width = Width / 3;
			gb3_LowLevel.Width = Width / 3;
		}
		#endregion

      #region Эмуляция значений для тестирования АРМа
      TreeNode selected_node; // выделенный тег

      private void treeViewKB_NodeMouseClick( object sender, TreeNodeMouseClickEventArgs e )
      {
         if ( e.Button != MouseButtons.Right )
            return;

         selected_node = treeViewKB.GetNodeAt( e.X, e.Y );

         //if ( selected_node == null )
         //   return;

         //contextMenuStrip1.Show( e.X, e.Y );
      }

      private void btnSetEmulValue_Click( object sender, EventArgs e )
      {
         chbEmulateValue.Checked = false;
         TCRZAVariable tgv = ( TCRZAVariable ) btnSetEmulValue.Tag;
         SetVariableValue( tgv );
         // обновить панель с содержимым для верхнего уровня
         GetInfoAboutGroup( tgv.Group );
      }

      private void chbEmulateValue_CheckedChanged( object sender, EventArgs e )
      {
         btnSetEmulValue.Enabled = chbEmulateValue.Checked;
      }

	   #endregion
   }
}