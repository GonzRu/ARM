using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace HMI_MTConfig
{
   public partial class Form1 : Form
   {
        CustomizeForm openForm;

       /// <summary>
        /// StringBuilder для использования вместо String
        /// там где это возможно
       /// </summary>
        StringBuilder sb = new StringBuilder( );
                
        public Form1()
        {
            InitializeComponent();
            StartCustomize();
        }

      private void StartCustomize()
      {
          OpenStartForm("btnPublicPrjCaption");
      }

      private void OpenStartForm(string p)
      {
          frm4btnPublicPrjCaption frm4btnPPC = new frm4btnPublicPrjCaption();
          btnCaption.Text = btnPublicPrjCaption.Text;
          frm4btnPPC.Text = btnPublicPrjCaption.Text;
          frm4btnPPC.TopLevel = false;
          frm4btnPPC.Dock = DockStyle.Fill;
          frm4btnPPC.Parent = splitContainer3.Panel2;
          // запомнить название для перезагрузки формы
          frm4btnPPC.Tag = p;
          openForm = frm4btnPPC;
          frm4btnPPC.Show();
      }

      private void btnLeft_Click(object sender, EventArgs e)
      {
         if (openForm != null)
            openForm.Close();

         OpenFormInRightPanel((sender as Button).Name);
      }

      private void OpenFormInRightPanel(string p)
      {
          switch (p)
          {
              case "btnPublicPrjCaption":
                  OpenStartForm(p);
                  break;
              case "btnMnemo":
                  frm4btnMnemo frm4btnMnm = new frm4btnMnemo();
                  btnCaption.Text = frm4btnMnm.Text = btnMnemo.Text;
                  frm4btnMnm.TopLevel = false;
                  frm4btnMnm.Dock = DockStyle.Fill;
                  frm4btnMnm.Parent = splitContainer3.Panel2;
                  // запомнить название для перезагрузки формы
                  frm4btnMnm.Tag = p;
                  openForm = frm4btnMnm;
                  frm4btnMnm.Show();
                  break;
              case "btnSQLConnection":
                  frm4btnSQLConnection frm4btnSQLCnt = new frm4btnSQLConnection();
                  btnCaption.Text = frm4btnSQLCnt.Text = btnSQLConnection.Text;
                  frm4btnSQLCnt.TopLevel = false;
                  frm4btnSQLCnt.Dock = DockStyle.Fill;
                  frm4btnSQLCnt.Parent = splitContainer3.Panel2;
                  // запомнить название для перезагрузки формы
                  frm4btnSQLCnt.Tag = p;
                  openForm = frm4btnSQLCnt;
                  frm4btnSQLCnt.Show();
                  break;
              case "btnCustomTcpIp":
                  frm4btnCustomTcpIp frm4btnCustTcpIp = new frm4btnCustomTcpIp();
                  btnCaption.Text = frm4btnCustTcpIp.Text = btnCustomTcpIp.Text;
                  frm4btnCustTcpIp.TopLevel = false;
                  frm4btnCustTcpIp.Dock = DockStyle.Fill;
                  frm4btnCustTcpIp.Parent = splitContainer3.Panel2;
                  // запомнить название для перезагрузки формы
                  frm4btnCustTcpIp.Tag = p;
                  openForm = frm4btnCustTcpIp;
                  frm4btnCustTcpIp.Show();
                  break;
              case "btnSecurity":
                  frm4btnSecurity frm4btnSec = new frm4btnSecurity();
                  btnCaption.Text = frm4btnSec.Text = btnSecurity.Text;
                  frm4btnSec.TopLevel = false;
                  frm4btnSec.Dock = DockStyle.Fill;
                  frm4btnSec.Parent = splitContainer3.Panel2;
                  // запомнить название для перезагрузки формы
                  frm4btnSec.Tag = p;
                  openForm = frm4btnSec;
                  frm4btnSec.Show();
                  break;
			  case "btnEditDataServerFiles":
					frmEditDataServerFiles frmEditDS = new frmEditDataServerFiles();
					btnCaption.Text = frmEditDS.Text = btnEditDataServerFiles.Text;
					frmEditDS.TopLevel = false;
					frmEditDS.Dock = DockStyle.Fill;
					frmEditDS.Parent = splitContainer3.Panel2;
				  // запомнить название для перезагрузки формы
					frmEditDS.Tag = p;
					openForm = frmEditDS;
					frmEditDS.Show();
				  break;
			  case "btnControlDiagOsc":
				  frm4btnControlDiagOsc frm4CntOscDiag = new frm4btnControlDiagOsc();
				  btnCaption.Text = frm4CntOscDiag.Text = btnControlDiagOsc.Text;
				  frm4CntOscDiag.TopLevel = false;
				  frm4CntOscDiag.Dock = DockStyle.Fill;
				  frm4CntOscDiag.Parent = splitContainer3.Panel2;
				  // запомнить название для перезагрузки формы
				  frm4CntOscDiag.Tag = p;
				  openForm = frm4CntOscDiag;
				  frm4CntOscDiag.Show();
				  break;
			  case "btnGentralCustomise":
				  frm4btnGentralCustomise frm4GenCustom = new frm4btnGentralCustomise();
				  btnCaption.Text = frm4GenCustom.Text = btnGentralCustomise.Text;
				  frm4GenCustom.TopLevel = false;
				  frm4GenCustom.Dock = DockStyle.Fill;
				  frm4GenCustom.Parent = splitContainer3.Panel2;
				  // запомнить название для перезагрузки формы
				  frm4GenCustom.Tag = p;
				  openForm = frm4GenCustom;
				  frm4GenCustom.Show();
				  break;
			  default:
                  break;
          }
      }

      private void btnOk_Click(object sender, EventArgs e)
      {
        if (openForm != null)
            openForm.IsDgwEdit = false;

        CloseApp();
      }
      private void CloseApp()
      {
         if (openForm != null)
            openForm.Close();

         Close();
      }

      private void btnApply_Click(object sender, EventArgs e)
      {
         DoApplayChanges();
      }

      private void DoApplayChanges()
      {
         if (openForm != null)
            openForm.AplayChangesCF();
      }

      private void CancelApplayChanges()
      {
          string name4ButtonOpenForm = string.Empty;
          if (openForm == null)
              return;

          // рестарт формы
          name4ButtonOpenForm = (string)openForm.Tag;
          openForm.CancelChangesCF();
          openForm.Close();
          OpenFormInRightPanel(name4ButtonOpenForm);
      }

      private void btnSaveAndExit_Click(object sender, EventArgs e)
      {
         DoApplayChanges();

         // реальная запись в файл
         Program.xdoc_Project_cfg.Save(Program.Path2Project_cfg);
         Program.xdoc_PrgDevCFG_cdp.Save(Program.Path2PrgDevCFG_cdp);
		  //if (Program.IsTCPClient)
         Program.xdoctcpclient.Save(Program.Path2TCPClientCFG_xml);

         CloseApp();
      }

      private void btnCancel_Click(object sender, EventArgs e)
      {
          CancelApplayChanges();
      }
   }
}
