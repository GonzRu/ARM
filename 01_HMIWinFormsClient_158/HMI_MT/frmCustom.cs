using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Resources;
using System.Globalization;
using System.Threading;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using CommonUtils;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
//using CRZADevices;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace HMI_MT
{
   //public partial class frmCustom : Form
   //{
   //   #region Свойства
   //   #endregion
   //   #region private-члены класса
   //      MainForm parent;
   //      private bool isNeedRestart = false;
   //      string pathToPrjFile = String.Empty;   // путь к файлу конф. проекта
   //      XDocument xdoc;
   //   #endregion
   //   #region public-члены класса
   //   #endregion
   //   #region protected-члены класса
   //   protected ArrayList FMemDev;
   //   #endregion
   //   #region связанные члены класса
   //   #endregion
   //   #region Конструкторы
   //      public frmCustom()
   //      {
   //         InitializeComponent();
   //      }
   //      public frmCustom(MainForm f)
   //      {
   //         InitializeComponent();
   //         ToolTip tips = new ToolTip();
   //         parent = f;
   //         pathToPrjFile = parent.PathToPrjFile;
   //         xdoc = XDocument.Load( pathToPrjFile );
   //      }
   //   #endregion

   //   #region Кнопка Принять
   //     /// <summary>
   //     /// Кнопка Принять
   //     /// </summary>
   //     /// <param Name="sender"></param>
   //     /// <param Name="e"></param>
   //     private void button1_Click( object sender, EventArgs e )
   //     {
   //         // рег выраж для проверки правильности IP-адреса
   //         string strRegPattern = @"^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";

   //           if( long.TryParse( tbAliveInterval.Text, out HMI_Settings.AliveInterval ) )
   //           {
   //               HMI_Settings.AliveInterval = HMI_Settings.AliveInterval * 1000;
   //              parent.AliveTimer.Interval  = (int)HMI_Settings.AliveInterval; 
   //           }
   //           else
   //           {
   //               MessageBox.Show("Некорректное значение интервала времени для проверки работоспособности", "Настройки", MessageBoxButtons.OK, MessageBoxIcon.Error);
   //              HMI_Settings.AliveInterval = HMI_MT.Properties.Settings.Default.AliveInterval;
   //              tbAliveInterval.Text = ( ( long ) ( HMI_Settings.AliveInterval / 1000) ).ToString();
   //              return;
   //           }

   //        // проверка корректности значения ip-адреса
   //        //if ( String.IsNullOrEmpty( tbIPPointForSerializeMesPan.Text ) )
   //        //{
   //        //    switch (MessageBox.Show("Не задано значение ip-адреса для сериализации панели сообщений.\nПропустить?", "Настройки", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
   //        //    { 
   //        //        case DialogResult.Yes:
   //        //            HMI_Settings.IPPointForSerializeMesPan = tbIPPointForSerializeMesPan.Text;
   //        //            xdoc.Element("Project").Element("MesPanSerialization").Element("IPPointForSerializeMesPan").Value = tbIPPointForSerializeMesPan.Text;
   //        //            isNeedRestart = true;
   //        //            break;
   //        //        case DialogResult.No:
   //        //            return;
   //        //    }
   //        //}
   //        //else
   //        //{
   //        //    if (Regex.IsMatch(tbIPPointForSerializeMesPan.Text, strRegPattern))
   //        //    {
   //        //        HMI_Settings.IPPointForSerializeMesPan = tbIPPointForSerializeMesPan.Text;
   //        //        xdoc.Element("Project").Element("MesPanSerialization").Element("IPPointForSerializeMesPan").Value = tbIPPointForSerializeMesPan.Text;
   //        //        isNeedRestart = true;
   //        //    }
   //        //    else
   //        //    {
   //        //        MessageBox.Show("Некорректное значение ip-адреса для сериализации панели сообщений.", "Настройки", MessageBoxButtons.OK, MessageBoxIcon.Error);
   //        //        return;
   //        //    }
   //        //}

   //        //// проверка корректности значения номера порта
   //        //if ( !CommonUtils.CommonUtils.IsNumeric( tbPortPointForSerializeMesPan.Text ) )
   //        //{
   //        //    switch (MessageBox.Show("Некорректное значение номера порта для сериализации панели сообщений.\nПропустить?", "Настройки", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1))
   //        //   {
   //        //       case DialogResult.Yes:
   //        //           HMI_Settings.PortPointForSerializeMesPan = 0;
   //        //           xdoc.Element("Project").Element("MesPanSerialization").Element("PortPointForSerializeMesPan").Value = "0";
   //        //           isNeedRestart = true;
   //        //           break;
   //        //       case DialogResult.No:
   //        //           return;
   //        //   }
   //        //}
   //        //else
   //        //{
   //        //   HMI_Settings.PortPointForSerializeMesPan = Convert.ToUInt32( tbPortPointForSerializeMesPan.Text );
   //        //   xdoc.Element( "Project" ).Element( "MesPanSerialization" ).Element( "PortPointForSerializeMesPan" ).Value = tbPortPointForSerializeMesPan.Text ;
   //        //   isNeedRestart = true;
   //        //}

   //        // проверка корректности значения номера порта ФК для команд
   //        if ( !CommonUtils.CommonUtils.IsNumeric( tbPortFCForCMD.Text ) )
   //        {
   //            MessageBox.Show("Некорректное значение номера порта ФК для команд", "Настройки", MessageBoxButtons.OK, MessageBoxIcon.Error);
   //           tbPortFCForCMD.Text = xdoc.Element( "Project" ).Element( "FCEntryForCMD" ).Element( "Port" ).Value;
   //           return;
   //        }
   //        else
   //           xdoc.Element( "Project" ).Element( "FCEntryForCMD" ).Element( "Port" ).Value = tbPortFCForCMD.Text.Trim();

   //        xdoc.Element( "Project" ).Element( "FCEntryForCMD" ).Element( "IP" ).Value = tbIPFCForCMD.Text.Trim();

   //        // проверка корректности интервала обновления
   //        if( !CommonUtils.CommonUtils.IsNumeric( tbDataReNew.Text ) )
   //        {
   //           MessageBox.Show( "Некорректное значение интервала обновления", "Настройки", MessageBoxButtons.OK, MessageBoxIcon.Error );
   //           tbDataReNew.Text = "1000";
   //           return;
   //        }
   //        else
   //        {
   //           if( Convert.ToInt32( tbDataReNew.Text) < 500  || Convert.ToInt32( tbDataReNew.Text) > 30000  )
   //           {
   //               MessageBox.Show("Значение интервала обновления должно быть в диапазоне от 500 до 30000 мс", "Настройки", MessageBoxButtons.OK, MessageBoxIcon.Error);
   //              tbDataReNew.Text = "1000";  //                 HMI_Settings.IntervalDataReNew.ToString( );
   //              return;
   //           }
   //        }
   //        xdoc.Element( "Project" ).Element( "Interval" ).Value = tbDataReNew.Text;
   //        HMI_Settings.IntervalDataReNew = Convert.ToInt32( tbDataReNew.Text );
   //        HMI_MT.Properties.Settings.Default.IntervalDataReNew = Convert.ToInt32(tbDataReNew.Text);

   //        // parent.IntervalTimer = HMI_Settings.IntervalDataReNew;

           
   //         SaveCurAppSettings();
   //         xdoc.Save( pathToPrjFile );

   //           if (isNeedRestart)
   //                 MessageBox.Show( "Для активизации новых настроек необходимо перезапустить приложение", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information );

   //         parent.isReqPassword = cbReqPass.Checked;	// надо ли требовать запрос пароля для выполнения опасных действий пользователем
   //        //------------------------------------
   //        // удаленное взаимодействие
   //         if( cbRemoutOn.Checked && rbServer.Checked )
   //            parent.isTCPServer = true;
   //        else
   //            parent.isTCPServer = false;

   //         this.Close();
   //     }

   //     /// <summary>
   //     /// private void SaveCurAppSettings()
   //     /// сохранить настройки приложения
   //     /// </summary>
   //     private void SaveCurAppSettings() 
   //     {
   //             parent.isReqPassword = HMI_Settings.isRegPass = HMI_MT.Properties.Settings.Default.isReqPass = this.cbReqPass.Checked;

   //             HMI_MT.Properties.Settings.Default.PathToLogFile = HMI_Settings.pathLogEvent_pnl4 = lblLogPlace_pnl4.Text;
   //             HMI_MT.Properties.Settings.Default.LogFileMaxSize = HMI_Settings.sizeLog_pnl4 = Convert.ToInt32(lblLogMaxSize_pnl4.Text);
   //             HMI_MT.Properties.Settings.Default.WhatToDoLogFileMaxSize = HMI_Settings.whatToDoLog_pnl4;
   //             HMI_MT.Properties.Settings.Default.IsToolTipShow = HMI_Settings.IsShowToolTip = cbIsShowToolTip.Checked;
   //             HMI_MT.Properties.Settings.Default.IsToolTipRefDesign = HMI_Settings.IsToolTipRefDesign = cbIsToolTipRefDesign.Checked;
   //             HMI_MT.Properties.Settings.Default.IsShowTabForms = HMI_Settings.IsShowTabForms = cbIsShowTabForms.Checked;
   //             HMI_MT.Properties.Settings.Default.Precision = HMI_Settings.Precision = nudPrecesion.Value.ToString();
   //             HMI_MT.Properties.Settings.Default.AliveInterval = HMI_Settings.AliveInterval;
   //             HMI_MT.Properties.Settings.Default.LogOnlyDisk = HMI_Settings.LogOnlyDisk;
   //         HMI_MT.Properties.Settings.Default.IntervalDataReNew = HMI_Settings.IntervalDataReNew;
   //         HMI_MT.Properties.Settings.Default.IPPointForSerializeMesPan = HMI_Settings.IPPointForSerializeMesPan;
   //         HMI_MT.Properties.Settings.Default.PortPointForSerializeMesPan = HMI_Settings.PortPointForSerializeMesPan;
   //         //HMI_MT.Properties.Settings.Default.PortFCForCMD = HMI_Settings.PortFCForCMD;

   //         // запомнить в конф файле
   //         if (cbRemoutOn.Checked)
   //         {
   //            HMI_Settings.IsTCPServer = rbServer.Checked;
   //            HMI_Settings.IsTCPClient = rbClient.Checked;
   //            HMI_Settings.IsUDPSecondClient = rbClientSecond.Checked;

   //            if( cbMemorizeInProfile.Checked )
   //            {
   //               if ( rbServer.Checked )
   //               {
   //                  xdoc.Element( "Project" ).Element( "ARMRole" ).Attribute( "role" ).Value = "IsTCPServer";
   //                  xdoc.Element( "Project" ).Element( "Repeater" ).Element( "IsNeedRepeater" ).Value = "no";
   //                  xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPServer" ).Element( "IPServer" ).Value = tbIPServer.Text;
   //                  xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPServer" ).Element( "PortNumIn" ).Value = tbPortNumIn.Text;
   //                  xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPServer" ).Element( "PortNumOut" ).Value = tbPortNumOut.Text;
   //                  xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPServer" ).Element( "ConnectNumber" ).Value = tbConnectNumber.Text;
   //               }
   //               else if ( rbClient.Checked )
   //               {
   //                  xdoc.Element( "Project" ).Element( "ARMRole" ).Attribute( "role" ).Value = "IsTCPClient";
   //                  xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPClient" ).Element( "IPServer" ).Value = tbIPServer.Text;
   //                  xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPClient" ).Element( "PortNumIn" ).Value = tbPortNumIn.Text;
   //                  xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPClient" ).Element( "PortNumOut" ).Value = tbPortNumOut.Text;
   //                  HMI_Settings.PORTin = Convert.ToInt32( tbPortNumIn.Text );

   //                 // режим ретрансляции
   //                 if ( chbIsRepeater.Checked )
   //                 {
   //                    xdoc.Element( "Project" ).Element( "Repeater" ).Element( "IsNeedRepeater" ).Value = "yes";

   //                    if ( String.IsNullOrEmpty( tbIPForRepeater.Text ) )
   //                    {
   //                       MessageBox.Show( "Не задан IP-адрес хоста для ретрансляции пакетов", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning );
   //                       return;
   //                    }
   //                    else
   //                       xdoc.Element( "Project" ).Element( "Repeater" ).Element( "IPHostForRepeater" ).Value = tbIPForRepeater.Text;

   //                    if ( String.IsNullOrEmpty( tbPortForRepeater.Text ) )
   //                    {
   //                       MessageBox.Show( "Не задан порт хоста для ретрансляции пакетов", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning );
   //                       return;
   //                    }
   //                    else
   //                       xdoc.Element( "Project" ).Element( "Repeater" ).Element( "NumPortOnHostForRepeater" ).Value = tbPortForRepeater.Text;

   //                 }
   //               }
   //               else if ( rbClientSecond.Checked )
   //               {
   //                  xdoc.Element( "Project" ).Element( "ARMRole" ).Attribute( "role" ).Value = "IsUDPSecondClient";
   //                  xdoc.Element( "Project" ).Element( "Repeater" ).Element( "IsNeedRepeater" ).Value = "no";
   //                  xdoc.Element( "Project" ).Element( "Repeater" ).Element( "NumPortOnHostForRepeater" ).Value = tbPortNumIn.Text;
   //               }

   //               HMI_MT.Properties.Settings.Default.IsTCPServer = HMI_Settings.IsTCPServer;
   //               HMI_MT.Properties.Settings.Default.IsTCPClient = HMI_Settings.IsTCPClient;
   //               HMI_MT.Properties.Settings.Default.IsUDPSecondClient = HMI_Settings.IsUDPSecondClient;
   //            }         
  
   //            // передача-ретрансляция команд
   //            //if ( chbIsRetransmittingCMD.Checked )
   //            //{
   //            //   xdoc.Element( "Project" ).Element( "RetransmittingCMD" ).Element( "IsNeedRetransmittingCMD" ).Value = "yes";
   //            //   xdoc.Element( "Project" ).Element( "RetransmittingCMD" ).Element( "IPHostForRetransmittingCMD_In" ).Value = tbIPCMDGateIn.Text;
   //            //   xdoc.Element( "Project" ).Element( "RetransmittingCMD" ).Element( "NumPortOnHostForRetransmittingCMD_In" ).Value = tbPortCMDGateIn.Text;
   //            //   xdoc.Element( "Project" ).Element( "RetransmittingCMD" ).Element( "IPHostForRetransmittingCMD" ).Value = tbIPCMDGate.Text;
   //            //   xdoc.Element( "Project" ).Element( "RetransmittingCMD" ).Element( "NumPortOnHostForRetransmittingCMD" ).Value = tbPortCMDGate.Text;
   //            //}
   //            //else
   //            //{
   //            //   xdoc.Element("Project").Element("RetransmittingCMD").Element("IsNeedRetransmittingCMD").Value = "no";
   //            //   xdoc.Element("Project").Element("RetransmittingCMD").Element("IPHostForRetransmittingCMD_In").Value = String.Empty;
   //            //   xdoc.Element("Project").Element("RetransmittingCMD").Element("NumPortOnHostForRetransmittingCMD_In").Value = String.Empty;
   //            //   xdoc.Element("Project").Element("RetransmittingCMD").Element("IPHostForRetransmittingCMD").Value = String.Empty;
   //            //   xdoc.Element("Project").Element("RetransmittingCMD").Element("NumPortOnHostForRetransmittingCMD").Value = String.Empty;
   //            //}
   //         }
   //         else
   //         {
   //            xdoc.Element( "Project" ).Element( "ARMRole" ).Attribute( "role" ).Value = "";
   //            HMI_MT.Properties.Settings.Default.IsTCPServer = false;
   //            HMI_MT.Properties.Settings.Default.IsTCPClient = false;
   //            HMI_MT.Properties.Settings.Default.IsUDPSecondClient = false;
   //         }

   //         HMI_MT.Properties.Settings.Default.Save();
   //         isNeedRestart = true;
   //       }

   //     /// <summary>
   //     /// private void CreateCurAppSettings()
   //     /// создать файл с текущими настройками приложения
   //     /// </summary>
   //     private void CreateCurAppSettings()
   //     {
            
   //     }
   //   #endregion
        
   //   /// <summary>
   //   /// загрузка формы
   //   /// </summary>
   //   /// <param Name="sender"></param>
   //   /// <param Name="e"></param>
   //   private void frmCustom_Load( object sender, EventArgs e )
   //   {
   //      // связываем кнопки для лаконичности
   //      btnChAvar_pnlColors_pnl2.Tag = btnChAvarCC_pnlColors_pnl2;
   //      btnChWarn_pnlColors_pnl2.Tag = btnChWarnCC_pnlColors_pnl2;
   //      btnChInfo_pnlColors_pnl2.Tag = btnChInfoCC_pnlColors_pnl2;
   //      // привязываем ноды treeview к панелям
   //      TreeNode tn;
			  
   //          treeView1.SelectedNode = null;

   //          tn = CommonUtils.CommonUtils.FindNode( treeView1, "Параметры запуска" );
   //          if( tn != null )
   //              tn.Tag = tabPage1;

   //          tn = CommonUtils.CommonUtils.FindNode( treeView1, "Цвета" );
   //           if( tn != null )
   //           tn.Tag = tabPage2;

   //           tn = CommonUtils.CommonUtils.FindNode( treeView1, "Шрифт" );
   //           if( tn != null )
   //           tn.Tag = tabPage3;

   //           tn = CommonUtils.CommonUtils.FindNode( treeView1, "Журнал событий" );
   //           if( tn != null )
   //           tn.Tag = tabPage4;

   //           tn = CommonUtils.CommonUtils.FindNode( treeView1, "Безопасность" );
   //           if( tn != null )
   //           tn.Tag = tabPage5;

   //           tn = CommonUtils.CommonUtils.FindNode( treeView1, "Параметры вывода значений" );
   //           if( tn != null )
   //           tn.Tag = tabPage6;

   //         parent.isReqPassword = cbReqPass.Checked = HMI_Settings.isRegPass = HMI_MT.Properties.Settings.Default.isReqPass;
   //         //журнал сообщений
   //           lblLogPlace_pnl4.Text = HMI_Settings.pathLogEvent_pnl4;
   //           lblLogMaxSize_pnl4.Text = HMI_Settings.sizeLog_pnl4.ToString();

   //         //подсказки
   //           cbIsShowToolTip.Checked = HMI_Settings.IsShowToolTip;
   //           cbIsToolTipRefDesign.Checked = HMI_Settings.IsToolTipRefDesign;
   //         // заголовки вкладок на главной форме
   //         cbIsShowTabForms.Checked = HMI_Settings.IsShowTabForms;
   //         // точность чисел с плавающей точкой
   //         HMI_Settings.Precision = HMI_MT.Properties.Settings.Default.Precision;
   //         nudPrecesion.Value = Convert.ToInt32(HMI_MT.Properties.Settings.Default.Precision);
   //         // интервал для проверки работоспособности
   //         tbAliveInterval.Text = ( ( long ) ( HMI_Settings.AliveInterval / 1000) ).ToString();
   //         cbLogOnlyDisk.Checked = HMI_Settings.LogOnlyDisk;
   //         // интервал обновления данных
   //         tbDataReNew.Text = xdoc.Element( "Project" ).Element( "Interval" ).Value;//            HMI_Settings.IntervalDataReNew.ToString( );
   //         // ip для сериализации 
   //         //tbIPPointForSerializeMesPan.Text = xdoc.Element( "Project" ).Element( "MesPanSerialization" ).Element( "IPPointForSerializeMesPan" ).Value;               //HMI_Settings.IPPointForSerializeMesPan;
   //         // порт для сериализации 
   //         //tbPortPointForSerializeMesPan.Text = xdoc.Element( "Project" ).Element( "MesPanSerialization" ).Element( "PortPointForSerializeMesPan" ).Value;
   //            //HMI_Settings.PortPointForSerializeMesPan.ToString( );
   //         // порт ФК для команд
   //         tbPortFCForCMD.Text = xdoc.Element( "Project" ).Element( "FCEntryForCMD" ).Element( "Port" ).Value;
   //         tbIPFCForCMD.Text = xdoc.Element( "Project" ).Element( "FCEntryForCMD" ).Element( "IP" ).Value;

   //             switch(HMI_Settings.whatToDoLog_pnl4)
   //             {
   //                 case "Удалить":
   //                     rbClear_pnl4.Checked = true;
   //                     break;
   //                 case "Сохранить":
   //                     rbSaveAs_pnl4.Checked = true;
   //                     break;
   //                 default:
   //                     rbClear_pnl4.Checked = true;
   //                     HMI_Settings.whatToDoLog_pnl4 = "Удалить";
   //                     break;
   //             }

   //         // Сервер или клиент
   //         chbIsRepeater.Visible = false;
   //         switch( xdoc.Element( "Project" ).Element( "ARMRole" ).Attribute( "role" ).Value )
   //         {
          
   //            case "IsTCPServer":
   //               cbRemoutOn.Checked = true;
   //               cbMemorizeInProfile.Checked = true;
   //               rbServer.Checked = true;
   //               tbIPServer.Enabled = true;
   //               chbIsRepeater.Visible = false;
   //               tbIPServer.Text = xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPServer" ).Element("IPServer").Value;
   //               tbPortNumIn.Text = xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPServer" ).Element("PortNumIn").Value;
   //               tbPortNumOut.Text = xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPServer" ).Element( "PortNumOut" ).Value;
   //               tbConnectNumber.Text = xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPServer" ).Element( "ConnectNumber" ).Value;
   //               break;
   //            case "IsTCPClient":
   //               gbCMDGate.Enabled = true;
   //               cbRemoutOn.Checked = true;
   //               cbMemorizeInProfile.Checked = true;
   //               rbClient.Checked = true;
   //               tbIPServer.Enabled = true;
   //               tbPortNumOut.Enabled = true;
   //               tbIPServer.Text = xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPClient" ).Element( "IPServer" ).Value;
   //               tbPortNumIn.Text = xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPClient" ).Element( "PortNumIn" ).Value;
   //               tbPortNumOut.Text = xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPClient" ).Element( "PortNumOut" ).Value;

   //               //// режим ретрансляции
   //               //chbIsRepeater.Visible = true;
   //               //if ( xdoc.Element( "Project" ).Element( "Repeater" ).Element( "IsNeedRepeater" ).Value == "yes" )
   //               //{
   //               //   chbIsRepeater.Enabled = true;
   //               //   chbIsRepeater.Checked = true;
   //               //   tbIPForRepeater.Text = xdoc.Element( "Project" ).Element( "Repeater" ).Element( "IPHostForRepeater" ).Value;
   //               //   tbPortForRepeater.Text =  xdoc.Element( "Project" ).Element( "Repeater" ).Element( "NumPortOnHostForRepeater" ).Value;
   //               //}
   //               break;
   //            case "IsUDPSecondClient":
   //               gbCMDGate.Enabled = true;
   //               cbRemoutOn.Checked = true;
   //               cbMemorizeInProfile.Checked = true;
   //               rbClientSecond.Checked = true;
   //               tbIPServer.Enabled = false;
   //               chbIsRepeater.Visible = false;
   //               gbRepeateMode.Enabled = false;
   //               tbPortNumIn.Text = xdoc.Element( "Project" ).Element( "Repeater" ).Element( "NumPortOnHostForRepeater" ).Value;
   //               break;
   //            case "":
   //               cbRemoutOn.Checked = false;
   //            cbMemorizeInProfile.Checked = false;
   //            break;
   //            default:
   //            break;
   //         }
   //      // передача-ретрансляция команд
   //         //if ( xdoc.Element( "Project" ).Element( "RetransmittingCMD" ).Element( "IsNeedRetransmittingCMD" ).Value == "yes" )
   //         //{
   //         //   chbIsRetransmittingCMD.Checked = true;
   //         //   tbIPCMDGateIn.Text = xdoc.Element( "Project" ).Element( "RetransmittingCMD" ).Element( "IPHostForRetransmittingCMD_In" ).Value;
   //         //   tbPortCMDGateIn.Text = xdoc.Element( "Project" ).Element( "RetransmittingCMD" ).Element( "NumPortOnHostForRetransmittingCMD_In" ).Value;
   //         //   tbIPCMDGate.Text = xdoc.Element( "Project" ).Element( "RetransmittingCMD" ).Element( "IPHostForRetransmittingCMD" ).Value;
   //         //   tbPortCMDGate.Text = xdoc.Element( "Project" ).Element( "RetransmittingCMD" ).Element( "NumPortOnHostForRetransmittingCMD" ).Value;
   //         //}
   //         //else
   //         //{
   //            chbIsRetransmittingCMD.Checked = false;
   //            tbIPCMDGateIn.Text = String.Empty;
   //            tbPortCMDGateIn.Text = String.Empty;
   //            tbIPCMDGate.Text = String.Empty;
   //            tbPortCMDGate.Text = String.Empty;
   //         //}
   //   }

   //     /// <summary>
   //     /// Обработка Active формы
   //     /// </summary>
   //     /// <param Name="sender"></param>
   //     /// <param Name="e"></param> 
   //   private void frmCustom_Activated( object sender, EventArgs e )
   //      {
   //       pnlRemExchange.Visible = cbRemoutOn.Checked;			 
   //      }

   //      /// <summary>
   //      /// единый обработчик для кнопок изменения цвета
   //      /// </summary>
   //      /// <param Name="sender"></param>
   //      /// <param Name="e"></param>
   //      private void btnChAvar_pnlColors_pnl2_Click( object sender, EventArgs e )
   //      {
   //          Button btn = ( Button ) ( ( Button ) sender ).Tag;

   //         colorDialog1.Color = btn.BackColor;

   //          if( DialogResult.OK != colorDialog1.ShowDialog() )
   //              return;
   //          btn.BackColor = colorDialog1.Color;
   //       isNeedRestart = true;
   //      }

   //     /// <summary>
   //   /// treeView1_NodeMouseClick
   //     /// </summary>
   //     /// <param Name="sender"></param>
   //     /// <param Name="e"></param> 
   //   private void treeView1_NodeMouseClick( object sender, TreeNodeMouseClickEventArgs e )
   //      {
   //          TreeNode tnSel = e.Node;
   //       tabControl1.SelectedTab = ( TabPage ) tnSel.Tag;
   //      }
      
   //   /// <summary>
   //   /// linklblLogPlace_pnl4_LinkClicked
   //   /// </summary>
   //   /// <param Name="sender"></param>
   //   /// <param Name="e"></param>
   //   private void linklblLogPlace_pnl4_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
   //      {
   //          folderBrowserDialog1.SelectedPath = Application.StartupPath;

   //          if( DialogResult.OK != folderBrowserDialog1.ShowDialog() )
   //              return;
   //          lblLogPlace_pnl4.Text = folderBrowserDialog1.SelectedPath;
   //      }
		 
   //   /// <summary>
   //   /// tb_KeyPress
   //   /// </summary>
   //   /// <param Name="sender"></param>
   //   /// <param Name="e"></param>
   //   private void tb_KeyPress( object sender, System.Windows.Forms.KeyPressEventArgs e )
   //      {
   //          // Если это не цифра.
   //          if( Char.IsLetter( e.KeyChar ) )
   //          {
   //                 MessageBox.Show( "Вводить можно только десятичное число", "Настройки", MessageBoxButtons.OK, MessageBoxIcon.Error );
   //                 e.Handled = true;
   //          }
   //      }
		 
   //   /// <summary>
   //   /// tb_KeyDown
   //   /// </summary>
   //   /// <param Name="sender"></param>
   //   /// <param Name="e"></param>
   //   private void tb_KeyDown( object sender, System.Windows.Forms.KeyEventArgs e )
   //      {
   //         if( e.KeyCode == Keys.Enter )
   //              {
   //                     lblLogMaxSize_pnl4.Text = ( ( TextBox ) sender ).Text;
   //                  ( ( TextBox ) sender ).Visible = false;
   //                  lblLogMaxSize_pnl4.Visible = true;
   //              }
   //              else if ( e.KeyCode == Keys.Escape )
   //              {
   //                  ( ( TextBox ) sender ).Visible = false;
   //                  lblLogMaxSize_pnl4.Visible = true;
   //              }
   //      }
		 
   //   /// <summary>
   //   /// linklblLogSize_pnl4_LinkClicked
   //   /// </summary>
   //   /// <param Name="sender"></param>
   //   /// <param Name="e"></param>
   //   private void linklblLogSize_pnl4_LinkClicked( object sender, LinkLabelLinkClickedEventArgs e )
   //   {
   //      TextBox tb = new TextBox();
   //      tb.Width = lblLogMaxSize_pnl4.Width;
   //      tb.Parent = this.pnl4;
   //      tb.Left = lblLogMaxSize_pnl4.Left;
   //      tb.Top = lblLogMaxSize_pnl4.Top;
   //      tb.Text = lblLogMaxSize_pnl4.Text;
   //      tb.KeyDown += new KeyEventHandler(tb_KeyDown);
   //      tb.KeyPress +=new KeyPressEventHandler(tb_KeyPress);
   //      tb.Focus();
   //      lblLogMaxSize_pnl4.Visible = false;
   //   }

   //   /// <summary>
   //   /// radioButton1_CheckedChanged
   //   /// </summary>
   //   /// <param Name="sender"></param>
   //   /// <param Name="e"></param>
   //      private void radioButton1_CheckedChanged( object sender, EventArgs e )
   //      {
   //          switch( ( ( RadioButton ) sender ).Name )
   //          {
   //              case "rbClear_pnl4":
   //                  HMI_Settings.whatToDoLog_pnl4 = "Удалить";
   //                  break;
   //              case "rbSaveAs_pnl4":
   //                  HMI_Settings.whatToDoLog_pnl4 = "Сохранить";
   //                  break;
   //          }
   //      }

   //     /// <summary>
   //    /// cbIsToolTipRefDesign_CheckedChanged
   //     /// </summary>
   //     /// <param Name="sender"></param>
   //     /// <param Name="e"></param> 
   //   private void cbIsToolTipRefDesign_CheckedChanged( object sender, EventArgs e )
   //      {
   //          isNeedRestart = true;
   //      }

   //     /// <summary>
   //   /// nudPrecesion_Validated
   //     /// </summary>
   //     /// <param Name="sender"></param>
   //     /// <param Name="e"></param> 
   //   private void nudPrecesion_Validated( object sender, EventArgs e )
   //      {
   //          isNeedRestart = true;
   //      }

   //   /// <summary>
   //   /// cbLogOnlyDisk_CheckedChanged
   //   /// </summary>
   //   /// <param Name="sender"></param>
   //   /// <param Name="e"></param>
   //      private void cbLogOnlyDisk_CheckedChanged( object sender, EventArgs e )
   //      {
   //          HMI_Settings.LogOnlyDisk = cbLogOnlyDisk.Checked;
   //          isNeedRestart = true;
   //      }		 
      
   //   /// <summary>
   //    /// cbRemoutOn_CheckedChanged
   //   /// </summary>
   //   /// <param Name="sender"></param>
   //   /// <param Name="e"></param>
   //   private void cbRemoutOn_CheckedChanged(object sender, EventArgs e)
   //     {
   //        pnlRemExchange.Visible = cbRemoutOn.Checked;

   //        //gbRoleARM.Visible = cbRemoutOn.Checked;
   //        // роль по умолчанию - сервер
   //        rbServer.Checked = cbRemoutOn.Checked;

   //        if( !cbRemoutOn.Checked )
   //        {
   //           HMI_Settings.IsTCPServer = false;
   //           HMI_Settings.IsTCPClient = false;
   //           HMI_Settings.IsUDPSecondClient = false;
   //        }
   //        else
   //        {
   //           tbIPServer.Text = xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPServer" ).Element( "IPServer" ).Value;
   //           tbPortNumIn.Text = xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPServer" ).Element( "PortNumIn" ).Value;
   //           tbConnectNumber.Text = xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPServer" ).Element( "ConnectNumber" ).Value;
   //           tbPortNumOut.Text = xdoc.Element( "Project" ).Element( "ARMRole" ).Element( "IsTCPServer" ).Element( "PortNumOut" ).Value;
   //        }
   //        isNeedRestart = true;
   //     }

   //   /// <summary>
   //   /// rbServer_CheckedChanged
   //   /// </summary>
   //   /// <param Name="sender"></param>
   //   /// <param Name="e"></param>
   //   private void rbServer_CheckedChanged(object sender, EventArgs e)
   //     {
   //         RadioButton rb = (RadioButton)sender;
   //         switch (rb.Name)
   //         {
   //             case "rbServer":
   //               pnlServerSetup.Enabled = rb.Checked;
   //               tbIPServer.Enabled = rb.Checked;
   //               tbPortNumOut.Enabled = true;
   //               tbPortNumIn.Enabled = true;
   //               gbRepeateMode.Enabled = gbCMDGate.Enabled = !rb.Checked;
   //               chbIsRepeater.Visible = false;
   //               break;
   //             case "rbClient":
   //               pnlServerSetup.Enabled = false;
   //               tbIPServer.Enabled = true;
   //               tbPortNumOut.Enabled = true;
   //               chbIsRepeater.Visible = true;
   //               gbRepeateMode.Enabled = false;
   //               gbCMDGate.Enabled = true;
   //               break;
   //             case "rbClientSecond":
   //               pnlServerSetup.Enabled = false;
   //               tbIPServer.Enabled = false;
   //               chbIsRepeater.Visible = false;
   //               tbPortNumOut.Enabled = false;
   //               tbPortNumIn.Enabled = true;
   //               gbCMDGate.Enabled = rb.Checked;
   //               gbRepeateMode.Enabled = false;
   //               break;
   //             default:
   //                 break;
   //         }
   //     }
        
   //   /// <summary>
   //   /// Кнопка Отменить
   //   /// </summary>
   //   /// <param Name="sender"></param>
   //   /// <param Name="e"></param>
   //   private void btnCancel_Click( object sender, EventArgs e )
   //   {
   //      Close( );
   //   }

   //   /// <summary>
   //   /// chbIsRepeater_CheckedChanged
   //   /// </summary>
   //   /// <param Name="sender"></param>
   //   /// <param Name="e"></param>
   //   private void chbIsRepeater_CheckedChanged( object sender, EventArgs e )
   //   {
   //      gbRepeateMode.Enabled = chbIsRepeater.Checked;

   //      if ( chbIsRepeater.Checked )
   //         xdoc.Element( "Project" ).Element( "Repeater" ).Element( "IsNeedRepeater" ).Value = "yes";
   //      else
   //         xdoc.Element( "Project" ).Element( "Repeater" ).Element( "IsNeedRepeater" ).Value = "no";
   //      xdoc.Save( pathToPrjFile );
   //   }

   //   private void chbIsRetransmittingCMD_CheckedChanged( object sender, EventArgs e )
   //   {
   //      gbCMDGate.Enabled = chbIsRetransmittingCMD.Checked;
   //   }		 
   // }

   /// <summary>
   /// HMI_Settings
   /// </summary>
   public static class HMI_Settings
	{
      /// <summary>
      /// запрос пароля для опасных действий
      /// </summary>
		public static bool isRegPass;	// 
		//журнал событий
		public static string pathLogEvent_pnl4;	// путь к журналу событий
		public static int sizeLog_pnl4;			// размер журнала
		public static string whatToDoLog_pnl4;			// что делать с файлом журнала при его переполнении
      /// <summary>
      /// показывать подсказку при наведении мыши на блок
      /// </summary>
		public static bool IsShowToolTip;
      /// <summary>
      /// показывать условные обозначения устройств вместо названий блоков
      /// </summary>
		public static bool IsToolTipRefDesign;
      /// <summary>
      /// показывать tabControl на главной форме
      /// </summary>
      public static bool IsShowTabForms;
		// точность значений с плавающей точкой
		public static string Precision;
		// интервал времени для проверки работоспособности
		public static long AliveInterval;
		// ведение локального журнала только на диске, без окна текущих сообщений
		public static bool LogOnlyDisk;
      // Роль АРМ - TCP-сервер или TCP-клиент (вторичный)
      public static bool IsTCPServer;
      public static bool IsTCPClient;
      public static bool IsUDPSecondClient;
      // порт для входящих соединений
      public static int PORTin;
      // порт для исходящих соединений
      public static int PORTout;
      /// <summary>
      /// тип запускаемой системы - локальная или удаленная - уточняется при загрузке MainForm
      /// </summary>
      //public static bool IsLocalSystem;
      /// <summary>
      /// Необходимость ввода логина и пароля при входе в АРМ
      /// </summary>
       public static bool isNeedLoginAndPassword = true;       
       // IP-адрес сервера - используется в клиенте
      public static string IPADDRES_SERVER;
      // IP-адрес клиента - используется для ping в сервере
      public static string IPADDRES_CLIENT;
      // количество подключений
      public static int NUMBER_CONNECTING;
      // интервал обновления
      public static int IntervalDataReNew;
      // ip клиента для сериализации панели сообщений
      public static string IPPointForSerializeMesPan;
      // порт клиента для сериализации панели сообщений
      public static uint PortPointForSerializeMesPan;
      // список серверов, которым посылаются теги (сервера OPC, тег менеджер)
      public static SortedList slOPCTaglServers = new SortedList();
      /// <summary>
      /// строка соединения с БД - SqlProviderPTK
      /// </summary>
      public static string cstr;
      /// <summary>
      /// строка соединения с БД - OleProviderPTK
      /// </summary>
      public static string ostr;
      /// <summary>
      /// путь к файлу проекта Project.cfg 
      /// </summary>
      public static string PathToPrjFile;
	  /// <summary>
	  /// xml-представление файла PathToPrjFile
	  /// </summary>
	  public static XDocument XDoc4PathToPrjFile;
	  /// <summary>
      /// путь к файлу проекта PrgDevCFG.cdp
      /// </summary>
      public static string PathToPrgDevCFG_cdp;
	  /// <summary>
	  /// xml-представление файла PathToPrgDevCFG
	  /// </summary>
	  public static XDocument XDoc4PathToPrgDevCFG;
	  /// <summary>
	  /// путь к файлу описания адаптеров PanelState.xml
	  /// для панели состояний устройств ПТК 
	  /// </summary>
	  public static string PathPanelState_xml;
	  /// <summary>
	  /// xml-представление файла PathPanelState_xml
	  /// </summary>
	  public static XDocument XDoc4PathPanelState_xml;
	  /// <summary>
      /// сортированный список для установки соответствия имени устройства и класса устройства 
      /// </summary>
      public static SortedList slDevClasses = new SortedList( );
      /// <summary>
      /// список активных трендов
      /// </summary>
      public static List<Trend> ListOfTrends = new List<Trend>();
      /// <summary>
      /// имя канала для транспортного уровня 
      /// </summary>      
      public static string PipeName;
	  /// <summary>
	  /// Показывать стандартные кнопки управления окном?
	  /// </summary>      
	  public static string ViewBtn4MainWindow;
	  /// <summary>
	  /// Скрывать строку статуса Windows?
	  /// </summary>      
	  public static string HideWindowLineStatus;
	   /// <summary>
	   /// Факт наличия активной антенны GPS
	   /// </summary>
	  public static bool IsGPSActive;
	  /// <summary>
	  /// Код состояния антенны GPS
	  /// </summary>
	  public static byte GPSActiveCode;
	  /// <summary>
	  /// Строка описывающая состояние GPS
	  /// </summary>
	  public static string GPSActiveCodeMessage;
	   /// <summary>
	  /// класс (используемый в TCP-клиенте) с таблицами для организации целевого запроса данных с сервера
	   /// </summary>
      //public static ClientDataForExchange ClientDFE;
	}
}	