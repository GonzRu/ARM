using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using DataModule;
using NSNetNetManager;
using CRZADevices;
using System.Threading;
using Calculator;
using Mnemo;

namespace HMI
{
	enum TextFontSize 
	{
		FontSizeHuge = 30,
		FontSizeNormal = 20,
		FontSizetiny = 8
	}

	enum DataTimeFormat
	{
		ShowClock,
		ShowDay
	}

	public partial class MainForm : Form
	{
		// ����� ���������� ����/ �������
		private DataTimeFormat dtFormat = DataTimeFormat.ShowClock;

		//��������� ���������� ������� ����
		private ToolStripMenuItem currentCheckedItem;

		private TextFontSize CurrFontSize = TextFontSize.FontSizeNormal;
		public MainMnemo Form_ez;
		public MainForm()
		{
			// ����� ��������� �����
			#region
			// ����� ���������
         Text = "��� ��� 10�� �� \"������\" (��� \"�������������\")";

			// ��������� ������ ������� �����
         /*Width = 1024;
         Height = 768;
			CenterToScreen();       // ������������� �����

			// ������� ���� � ��� �������
			BackColor = Color.LightCyan;
         Cursor = Cursors.Hand;*/

			InitializeComponent();

			//---------------------------------------
			// ����������� ������� � ���� � StatusBar
			currentCheckedItem = miToolStrip_currentTime;
			currentCheckedItem.Checked = true;

			// ��������� (����������) � StatusBar
			sbMesIE.Text = "��� ���������";

			toolStripTextBoxColor.LostFocus += new EventHandler(toolStripTextBoxColor_LostFocus);
			#endregion
			
			// ������� ������� ����������
			#region
			/*Form_ez = new MainMnemo();
			Form_ez.MdiParent = this;
			Form_ez.Show();*/
			#endregion
			//--------------------------------------------------------------------
         // ������� ��
         #region
         ArrayList KB; // ������������

         FC FC1 = new FC(0);  //������� ��
         // ��������� ����� � ������� ��
         //TBMRZVV_14_31_12 dev1_BMRZVV_14_31_12 = new TBMRZVV_14_31_12( 1 );
         //FC1.Devices.Add(dev1_BMRZVV_14_31_12);

         // ��������� �� � ������������ ������
         KB = new ArrayList();
         KB.Add(FC1);

         // ���������� ��������
         // ������� �����������
         /*FormulaEval a_0033 = new FormulaEval(KB, "0(0.1.3.33.0)", "0", "Ia=", "A");
         a_0033.OnChangeValForm += this.ctlLabelTextbox_0033.LinkSetText;

         FormulaEval a_0034 = new FormulaEval(KB, "0(0.1.3.34.0)", "0", "Ib=", "A");
         a_0034.OnChangeValForm += this.ctlLabelTextbox_0034.LinkSetText;

         FormulaEval a_0035 = new FormulaEval(KB, "0(0.1.3.35.0)", "0", "Ic=", "A");
         a_0035.OnChangeValForm += this.ctlLabelTextbox_0035.LinkSetText;

         FormulaEval a_0037 = new FormulaEval(KB, "0(0.1.3.37.0)", "0", "3Uo=", "B");
         a_0037.OnChangeValForm += this.ctlLabelTextbox_0037.LinkSetText;

         FormulaEval a_0038 = new FormulaEval(KB, "0(0.1.3.38.0)", "0", "Uab=", "B");
         a_0038.OnChangeValForm += this.ctlLabelTextbox_0038.LinkSetText;

         FormulaEval a_0039 = new FormulaEval(KB, "0(0.1.3.39.0)", "0", "Ubc=", "B");
         a_0039.OnChangeValForm += this.ctlLabelTextbox_0039.LinkSetText;

         FormulaEval a_0040 = new FormulaEval(KB, "0(0.1.3.40.0)", "0", "U���=", "B");
         a_0040.OnChangeValForm += this.ctlLabelTextbox_0040.LinkSetText;

         FormulaEval a_0041 = new FormulaEval(KB, "0(0.1.3.41.0)", "0", "U2=", "B");
         a_0041.OnChangeValForm += this.ctlLabelTextbox_0041.LinkSetText;

         FormulaEval a_0042 = new FormulaEval(KB, "0(0.1.3.42.0)", "0", "I2=", "A");
         a_0042.OnChangeValForm += this.ctlLabelTextbox_0042.LinkSetText;

         FormulaEval a_0045 = new FormulaEval(KB, "0(0.1.3.45.0)", "0", "��=", "��");
         a_0045.OnChangeValForm += this.ctlLabelTextbox_0045.LinkSetText;

         FormulaEval a_0046 = new FormulaEval(KB, "0(0.1.3.46.0)", "0", "��=", "��");
         a_0046.OnChangeValForm += this.ctlLabelTextbox_0046.LinkSetText;

         FormulaEval a_0047 = new FormulaEval(KB, "0(0.1.3.47.0)", "0", "��=", "��");
         a_0047.OnChangeValForm += this.ctlLabelTextbox_0047.LinkSetText;

         FormulaEval a_0050 = new FormulaEval(KB, "0(0.1.3.50.0)", "0", "F=", "��");
         a_0050.OnChangeValForm += this.ctlLabelTextbox_0050.LinkSetText;*/

         // ���������� ��������
         /*FormulaEval b_0024_0100 = new FormulaEval(KB, "0(0.1.3.24.0100)", "0", "���", "");
         b_0024_0100.OnChangeValForm += this.checkBoxVar1.LinkSetText;
         b_0024_0100.OnChangeValForm += this.controlSwitch1.LinkSetText;

         FormulaEval b_0024_0200 = new FormulaEval(KB, "0(0.1.3.24.0200)", "0", "���", "");
         b_0024_0200.OnChangeValForm += this.checkBoxVar2.LinkSetText;
         b_0024_0200.OnChangeValForm += this.controlSwitch1.LinkSetText;*/

         #endregion
         //--------------------------------------------------------------------
         // ��������� �����, ������� ����� ��������� ��� �� ������� �� MTD
         ThreadPool.QueueUserWorkItem(DoElapse, KB);
         #region

         #endregion
         //--------------------------------------------------------------------
         // �������������� ������������ ���������� ��� ������������ ������ ����������
         #region

         #endregion
      }

		void toolStripTextBoxColor_LostFocus(object sender, EventArgs e)
		{
			try
			{
				BackColor = Color.FromName(toolStripTextBoxColor.Text);
			}
			catch { }
		}
      private static void DoElapse(Object state)
      {
         //--------------------------------------------------------------------
         // ����������� ������������ ������ �� �������� ��������� (raw) ������
         ArrayList MTD;
         #region
         NetNetManager nnetman = new NetNetManager();
         INetManager netman;
         netman = nnetman;

         MTD = netman.SetConfig();
         // ������� ����� ������ �� �� � ������ raw-������ MTD
         Thread readFCThread = new Thread(new ThreadStart(netman.getdata));
         readFCThread.Name = "ReaderFC";
         readFCThread.Start();
         #endregion
         //--------------------------------------------------------------------
         // ������ ��������� ������ � KB
         #region
         do
         {
         ArrayList KB = (ArrayList)state;
         foreach (FC aFC in KB)
            for (int i = 0; i < aFC.Devices.Count; i++)
            {
               TCRZADirectDevice aDev = (TCRZADirectDevice)aFC.Devices[i];
               for (int j = 0; j < aDev.Groups.Count; j++)
               {
                  TCRZAGroup aGroup = (TCRZAGroup)aDev.Groups[j];
                  for (int p = 0; p < aGroup.Variables.Count; p++)
                  {
                     TCRZAVariable aVariable = (TCRZAVariable)aGroup.Variables[p];
                     aVariable.ExtractVarFrom(MTD);
                  }
               }
            }
         }while(true);
         #endregion
      }

      private void Form1_Load(object sender, EventArgs e)
      {

      }

		private void �����ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawString("�������� ����� ������ ������� ����...", new Font("Times New Roman", (float)CurrFontSize), new SolidBrush(Color.Black), 50, 50);
		}

		private void �������ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// ��������� �������� �� ������� �������� ������
			ToolStripMenuItem miClicked;
			miClicked = (ToolStripMenuItem)sender;

			if (miClicked.Name == "hugeToolStripMenuItem")
				CurrFontSize = TextFontSize.FontSizeHuge;
			if (miClicked.Name == "normalToolStripMenuItem")
				CurrFontSize = TextFontSize.FontSizeNormal;
			if (miClicked.Name == "tinyToolStripMenuItem")
				CurrFontSize = TextFontSize.FontSizetiny;

			Invalidate();
		}

		private void timerDataTimeUpdate_Tick(object sender, EventArgs e)
		{
			string panelInfo = "";

			// �������� �������� �������
			if (dtFormat == DataTimeFormat.ShowClock)
				panelInfo = DateTime.Now.ToLongTimeString();
			else
				panelInfo = DateTime.Now.ToShortDateString();

			// ��������� ������ ��� ������
			toolStripStatusLabelClock.Text = panelInfo;
		}

		private void miToolStrip_currentTime_Click(object sender, EventArgs e)
		{
			// ��������� ������� � ������� ������� ��� ������
			currentCheckedItem.Checked = false;
			dtFormat = DataTimeFormat.ShowClock;
			currentCheckedItem = miToolStrip_currentTime;
			currentCheckedItem.Checked = true;
		}

		private void miToolStrip_currentData_Click(object sender, EventArgs e)
		{
			// ��������� ������� � ������� ���� ��� ������
			currentCheckedItem.Checked = false;
			dtFormat = DataTimeFormat.ShowDay;
			currentCheckedItem = miToolStrip_currentData;
			currentCheckedItem.Checked = true;
		}

		private void MainForm_ResizeEnd(object sender, EventArgs e)
		{
			double dT = Convert.ToDouble(this.Width);
			double scale = dT / 1024;
			dT = scale * 768;	// Convert.ToDouble(this.Height);
			this.Height = Convert.ToInt32(dT);

			if (this.Width < 1024 | this.Height < 768)
			{
				this.Width = 1024;
				this.Height = 768;
			}

		}

		private void ��������ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.Cascade);
		}

		private void �����������ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileVertical);
		}

		private void �������������ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			LayoutMdi(MdiLayout.TileHorizontal);
		}

		private void ����������ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Form_ez = new MainMnemo();
			Form_ez.MdiParent = this;
			this.Form_ez.Show();
		}
	}
}