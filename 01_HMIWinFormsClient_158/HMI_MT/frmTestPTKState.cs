using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Windows.Forms;
//using CRZADevices;

namespace HMI_MT
{
	public partial class frmTestPTKState : Form
	{
		/// <summary>
		/// список ключ - DevGUID, знач. - соответсвующий CheckBox
		/// </summary>
		SortedList<int, CheckBox> slChBoxByDevGuid = new SortedList<int, CheckBox>();

		public frmTestPTKState()
		{
			InitializeComponent();
		}

		void frmTestPTKState_AdapterValueChange(object sender, string value)
		{
			//slChBoxByDevGuid[(sender as AdapterBase).DevGuid].Checked = bool.Parse(value);

			//switch ((sender as AdapterBase).Name)
			//{
			//    case "РПО":
			//        chbRPO.Checked = bool.Parse(value);
			//        break;
			//    case "РПВ":
			//        chbRPV.Checked = bool.Parse(value);
			//        break;
			//    case "ВЫЗОВ":
			//        chbVysov.Checked = bool.Parse(value);
			//        break;
			//    case "Связь":
			//        chbConnect.Checked = bool.Parse(value);
			//        break;
			//    default:
			//        break;
			//}
		}

		private void frmTestPTKState_Load(object sender, EventArgs e)
		{
			//(PTKState.Iinstance.GetAdapter4Link("34", "РПО")).AdapterValueChange += new AdapterBase.ValueChange(frmTestPTKState_AdapterValueChange);
			//(PTKState.Iinstance.GetAdapter4Link("34", "РПВ")).AdapterValueChange += new AdapterBase.ValueChange(frmTestPTKState_AdapterValueChange);
			//(PTKState.Iinstance.GetAdapter4Link("34", "ВЫЗОВ")).AdapterValueChange += new AdapterBase.ValueChange(frmTestPTKState_AdapterValueChange);
			//(PTKState.Iinstance.GetAdapter4Link("34", "Связь")).AdapterValueChange += new AdapterBase.ValueChange(frmTestPTKState_AdapterValueChange);

			//chbRPO.Checked = bool.Parse(PTKState.Iinstance.GetValueAsString("34", "РПО"));
			//chbRPV.Checked = bool.Parse(PTKState.Iinstance.GetValueAsString("34", "РПВ"));
			//chbVysov.Checked = bool.Parse(PTKState.Iinstance.GetValueAsString("34", "ВЫЗОВ"));
			//chbConnect.Checked = bool.Parse(PTKState.Iinstance.GetValueAsString("34", "Связь"));

			CreateTabPagesBySectionConfiguration();
		}

		/// <summary>
		/// сформировать вкладки tabcontrol по конфигурации секций быстрого доступа
		/// </summary>
		private void CreateTabPagesBySectionConfiguration()
		{
		   	IEnumerable<XElement> xesectioncfg = HMI_Settings.XDoc4PathToPrgDevCFG.Element("MT").Element("SectionConfiguration").Elements("Section");

			foreach (XElement xesection in xesectioncfg)
				CreateTabPage(xesection);
		}

		/// <summary>
		/// создать секцию для размещения элементов состояния связи с устройством
		/// </summary>
		/// <param name="xesection"></param>
		private void CreateTabPage(XElement xesection)
		{
			int devGuid = 0;
			try
			{
			TabPage tp = new TabPage(xesection.Attribute("name").Value) ;
			tabControl1.TabPages.Add(tp);
			FlowLayoutPanel flp = new FlowLayoutPanel();
			flp.FlowDirection = FlowDirection.TopDown;
			flp.Parent = tp;
			flp.Dock = DockStyle.Fill;
		
			IEnumerable<XElement> xeBays = xesection.Descendants("Bay");

			foreach (XElement xeBay in xeBays)
			{
				XElement xe4dev = CommonUtils.CommonUtils.GetXElementFrom_PrgDevCFG(Convert.ToInt32(xeBay.Attribute("key").Value));

				CheckBox chb = new CheckBox();
				chb.Text = xe4dev.Element("DescDev").Value;
				flp.Controls.Add(chb);

				if (int.TryParse(xeBay.Attribute("key").Value, out devGuid))
				{
					slChBoxByDevGuid.Add(devGuid, chb);					
					(PTKState.Iinstance.GetAdapter4Link(devGuid.ToString(), "Связь")).AdapterValueChange += new AdapterBase.ValueChange(frmTestPTKState_AdapterValueChange);
					chb.Checked = bool.Parse(PTKState.Iinstance.GetValueAsString(devGuid.ToString(), "Связь"));
				}
				else
					throw new Exception ("(109) frmTestPTKState.cs : CreateTabPage() : ошибка преобразования для ключа " + xeBay.Attribute("key").Value);					
			}
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
		}
	}
}
