using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LabelTextbox;
using Mnemo;
using CRZADevices;
using Calculator;
using FileManager;
using LibraryElements;
using WindowsForms;
using Structure;

namespace HMI_MT
{
	public partial class frmPribor : Form
	{
		// свойства
		bool isCollectMin;
		bool isCollectMax;

		public bool IsCollectMin
		{
			get
			{
				return isCollectMin;
			}
			set
			{
				isCollectMin = value;
			}
		}
		public bool IsCollectMax
		{
			get
			{
				return isCollectMax;
			}
			set
			{
				isCollectMax = value;
			}
		}

		public Color ColorArrowMax
		{
			set
			{
				//btnMax.BackColor = Pribor.MaxArrowColor;
				btnMax.BackColor = value;
			}
		}
		public Color ColorArrowMin
		{
			set
			{
				//btnMin.BackColor = Pribor.MinArrowColor;
				btnMin.BackColor = value;
			}
		}

		// значения пуска-возврата-гистерезиса
		public float topPusk = 0;
		public float topGist = 0;
		public float topVozvrat = 0;

		public float bottomPusk = 0;
		public float bottomGist = 0;
		public float bottomVozvrat = 0;

		private ArrayList cfg = new ArrayList();
		DinamicControl tt;
		FormulaEval b_10330;

		// локализуем для первого значения
		public string strFC = String.Empty;
		public string strIDDev = String.Empty;
		public string strIDGrp = String.Empty;
		public string strAdrVar = String.Empty;
		public string strBitMask = String.Empty;

		// второе значение - коэффициент трансформации	
		public string strFC2 = String.Empty;
		public string strIDDev2 = String.Empty;
		public string strIDGrp2 = String.Empty;
		public string strAdrVar2 = String.Empty;
		public string strBitMask2 = String.Empty;

		public string varName = "";
		public string varDim = "";
		public string varName2 = "";
		public string varDim2 = "";

		BlinkContainer blinkpnlMin;
		BlinkContainer blinkpnlMax;

		DataTable dtparent;	// таблица с описанием приборов

		public frmPribor( )
		{
			InitializeComponent();
		}

		public frmPribor( DinamicControl tt, ArrayList KB)
		{
			InitializeComponent();
			cfg = KB;	// инициализировали конфигурацию
			this.tt = tt;
		}
		/// <summary>
		/// кнопка закрыть настройки
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click( object sender, EventArgs e )
		{
			splitContainer1.Panel2Collapsed = true;
			this.Width = Pribor.Width + Pribor.BorderWidth;
			this.Height = Pribor.Height + Pribor.BorderWidth + Height - ClientSize.Height;
		}

		private void настроитьToolStripMenuItem_Click( object sender, EventArgs e )
		{
			dlgPribor dp = new dlgPribor( Pribor, tt, cfg, this );
			dp.Left = this.Left + this.Width;
			dp.Top = this.Top;
			dp.ShowDialog();
		}

		private void закрытьToolStripMenuItem_Click( object sender, EventArgs e )
		{
			Close();
		}

		private void frmPribor_Load( object sender, EventArgs e )
		{
			splitContainer1.Panel2Collapsed = true;
			this.Height = this.Width;
			
			// формируем заголовок формы
			Text = tt.GetDinamicType() + " ( яч. № " + tt.GetSection().ToString() + ")";

			this.TopMost = false;

			blinkpnlMin = BlinkContainer.AddBlinkContainer(pnlBlinkMin);
			blinkpnlMax = BlinkContainer.AddBlinkContainer( pnlBlinkMax );
		}

		private void frmPribor_ResizeEnd( object sender, EventArgs e )
		{
			Pribor.Dock = DockStyle.Fill;			
		}

		public void SetFE_1( string strFC, string strIDDev, string strIDGrp, string strAdrVar, string strBitMask, string varName, string varDim )
		{
			if( b_10330 != null )
				b_10330.OnChangeValForm -= this.LinkSetText;

			b_10330 = new FormulaEval( cfg, "0(" + strFC + "." + strIDDev + "." + strIDGrp + "." + strAdrVar + "." + strBitMask + ")", "0", "", "", TypeOfTag.no, "" );
			b_10330.OnChangeValForm += this.LinkSetText;
			b_10330.FirstValue();
			this.strFC = strFC;
			this.strIDDev = strIDDev;
			this.strIDGrp = strIDGrp;
			this.strAdrVar = strAdrVar;
			this.strBitMask = strBitMask;
			// убрать равно
			char[] chardel = { '=' };
			string[] sdel = varName.Split( chardel );
			//this.varName = varName;			
			this.varName = sdel[0];
			this.varDim = varDim;
			lblCapDim.Text = this.varName + " (" + varDim + ")";
		}
		/*==========================================================================*
		*   private void void LinkSetText(object Value)
		*      для потокобезопасного вызова процедуры
		*==========================================================================*/
		delegate void SetTextCallback( object Val, string TextCapt, string textDim );	//string textVal

		public void LinkSetText( object Value, string format )
		{
			object txt;
			RezFormulaEval tmpRezFormulaEval;

			if( !( Value is RezFormulaEval ) )
				return;   // сгенерировать ошибку через исключение

			tmpRezFormulaEval = ( RezFormulaEval ) Value;
			object val = tmpRezFormulaEval.Value;

			if( val is int || val is short || val is ushort || val is float)
			{
				//if (blinkTextBox1.IsBlinking)
				//    blinkTextBox1.Stop();
				//else
				//    blinkTextBox1.Start();
				if( this.InvokeRequired )
				{
					txt = val;
					SetText( txt, varName, varDim );
				}
				else
				{
					Pribor.Value = Convert.ToSingle( val );
					Pribor.TextVarValue = Pribor.Value.ToString("F2");
					lblValue.Text = Pribor.Value.ToString( "F2" );
					Pribor.TextUnit = varDim;
					Pribor.TextDescription = varName;
					if( IsCollectMax )
					{
						lblMaxVal.Text = Pribor.StoredMax.ToString( "F2" );
						lblDTFixMax.Text = Pribor.StoredMaxDate.ToShortDateString() + " " + Pribor.StoredMaxDate.ToLongTimeString();
						if( Pribor.Value > topPusk && !blinkpnlMax.IsBlinking && topPusk != 0)
							blinkpnlMax.Start();
						if (Pribor.Value < topVozvrat && blinkpnlMax.IsBlinking)
							blinkpnlMax.Stop();
					}
					else
					{
						lblMaxVal.Text = "";
						lblDTFixMax.Text = "";
						blinkpnlMax.Stop();
					}
					if( IsCollectMin )
					{
						lblMinVal.Text = Pribor.StoredMin.ToString( "F2" );
						lblDTFixMin.Text = Pribor.StoredMinDate.ToShortDateString() + " " + Pribor.StoredMinDate.ToLongTimeString();
						if( Pribor.Value < bottomPusk && !blinkpnlMin.IsBlinking && bottomPusk != 0)
							blinkpnlMin.Start();
						if( Pribor.Value > bottomVozvrat && blinkpnlMin.IsBlinking )
							blinkpnlMin.Stop();
					}
					else
					{
						lblMinVal.Text = "";
						lblDTFixMin.Text = "";
						blinkpnlMin.Stop();
					}
				}
			}
			else if( val is string )
			{
				if( this.InvokeRequired )
				{
					txt = 0;
					SetText( txt, tmpRezFormulaEval.CaptionIE, tmpRezFormulaEval.DimIE );
				}
				else
				{
					Pribor.Value = Convert.ToSingle( 0 );
					Pribor.TextVarValue = Pribor.Value.ToString( "F2" );
					lblValue.Text = Pribor.Value.ToString( "F2" );
					Pribor.TextUnit = varDim;
					Pribor.TextDescription = varName;
					if( IsCollectMax )
					{
						lblMaxVal.Text = Pribor.StoredMax.ToString( "F2" );
						lblDTFixMax.Text = Pribor.StoredMaxDate.ToShortDateString() + " " + Pribor.StoredMaxDate.ToLongTimeString();
						if( Pribor.Value > topPusk && !blinkpnlMax.IsBlinking && topPusk != 0)
							blinkpnlMax.Start();
						if( Pribor.Value < topVozvrat && blinkpnlMax.IsBlinking )
							blinkpnlMax.Stop();
					}
					else
					{
						lblMaxVal.Text = "";
						lblDTFixMax.Text = "";
						blinkpnlMax.Stop();
					}
					if( IsCollectMin )
					{
						lblMinVal.Text = Pribor.StoredMin.ToString( "F2" );
						lblDTFixMin.Text = Pribor.StoredMinDate.ToShortDateString() + " " + Pribor.StoredMinDate.ToLongTimeString();
						if( Pribor.Value < bottomPusk && !blinkpnlMin.IsBlinking && bottomPusk != 0)
							blinkpnlMin.Start();
						if( Pribor.Value > bottomVozvrat && blinkpnlMin.IsBlinking )
							blinkpnlMin.Stop();
					}
					else
					{
						lblMinVal.Text = "";
						lblDTFixMin.Text = "";
						blinkpnlMin.Stop();
					}
				}
			}
			else
			{
				if( this.InvokeRequired )
				{
					txt = 0;
					SetText( txt, tmpRezFormulaEval.CaptionIE, tmpRezFormulaEval.DimIE );
				}
				else
					//this.Text = "0";
				{
					Pribor.Value = Convert.ToSingle( 0 );
					Pribor.TextVarValue = Pribor.Value.ToString( "F2" );
					lblValue.Text = Pribor.Value.ToString( "F2" );
					Pribor.TextUnit = varDim;
					Pribor.TextDescription = varName;
					if( IsCollectMax )
					{
						lblMaxVal.Text = Pribor.StoredMax.ToString( "F2" );
						lblDTFixMax.Text = Pribor.StoredMaxDate.ToShortDateString() + " " + Pribor.StoredMaxDate.ToLongTimeString();
						if( Pribor.Value > topPusk && !blinkpnlMax.IsBlinking && topPusk != 0)
							blinkpnlMax.Start();
						if( Pribor.Value < topVozvrat && blinkpnlMax.IsBlinking )
							blinkpnlMax.Stop();
					}
					else
					{
						lblMaxVal.Text = "";
						lblDTFixMax.Text = "";
						blinkpnlMax.Stop();
					}
					if( IsCollectMin )
					{
						lblMinVal.Text = Pribor.StoredMin.ToString( "F2" );
						lblDTFixMin.Text = Pribor.StoredMinDate.ToShortDateString() + " " + Pribor.StoredMinDate.ToLongTimeString();
						if( Pribor.Value < bottomPusk && !blinkpnlMin.IsBlinking && bottomPusk != 0)
							blinkpnlMin.Start();
						if( Pribor.Value > bottomVozvrat && blinkpnlMin.IsBlinking )
							blinkpnlMin.Stop();
					}
					else
					{
						lblMinVal.Text = "";
						lblDTFixMin.Text = "";
						blinkpnlMin.Stop();
					}
				}
			}
		}

		/*==========================================================================*
		* private void SetText(object Value)
		* //для потокобезопасного вызова процедуры
		*==========================================================================*/
		private void SetText( object Val, string TextCapt, string textDim )
		{
			if( this.InvokeRequired )
			{
				SetTextCallback d = new SetTextCallback( SetText );
				this.Invoke( d, new object[] { Val, TextCapt, textDim } );
			}
			else
			{
				Pribor.Value = Convert.ToSingle( Val );
				Pribor.TextVarValue = Pribor.Value.ToString( "F2" );
				lblValue.Text = Pribor.Value.ToString( "F2" );
				Pribor.TextUnit = varDim;
				Pribor.TextDescription = varName;
				if( IsCollectMax )
				{
					lblMaxVal.Text = Pribor.StoredMax.ToString( "F2" );
					lblDTFixMax.Text = Pribor.StoredMaxDate.ToShortDateString() + " " + Pribor.StoredMaxDate.ToLongTimeString();
					if( Pribor.Value > topPusk && !blinkpnlMax.IsBlinking && topPusk != 0)
						blinkpnlMax.Start();
					if( Pribor.Value < topVozvrat && blinkpnlMax.IsBlinking )
						blinkpnlMax.Stop();
				}
				else
				{
					lblMaxVal.Text = "";
					lblDTFixMax.Text = "";
					blinkpnlMax.Stop();
				}
				if( IsCollectMin )
				{
					lblMinVal.Text = Pribor.StoredMin.ToString( "F2" );
					lblDTFixMin.Text = Pribor.StoredMinDate.ToShortDateString() + " " + Pribor.StoredMinDate.ToLongTimeString();
					if( Pribor.Value < bottomPusk && !blinkpnlMin.IsBlinking && bottomPusk != 0)
						blinkpnlMin.Start();
					if( Pribor.Value > bottomVozvrat && blinkpnlMin.IsBlinking )
						blinkpnlMin.Stop();
				}
				else
				{
					lblMinVal.Text = "";
					lblDTFixMin.Text = "";
					blinkpnlMin.Stop();
				}
			}
		}

		public void SetFE_2( string strFC2, string strIDDev2, string strIDGrp2, string strAdrVar2, string strBitMask2, string varName2, string varDim2 )
		{
			if( b_10330 != null )
				b_10330.OnChangeValForm -= this.LinkSetText;

			this.strFC2 = strFC2;
			this.strIDDev2 = strIDDev2;
			this.strIDGrp2 = strIDGrp2;
			this.strAdrVar2 = strAdrVar2;
			this.strBitMask2 = strBitMask2;

			//new FormulaEval( cfg, "0(" + strFC + "." + strIDDev + "." + strIDGrp + "." + strAdrVar + "." + strBitMask + ")", "0", "", "", TypeOfTag.no, TypeOfPanel.no );
			b_10330 = new FormulaEval( cfg, "0(" + strFC + "." + strIDDev + "." + strIDGrp + "." + strAdrVar + "." + strBitMask + ")1(" + strFC2 + "." + strIDDev2 + "." + strIDGrp2 + "." + strAdrVar2 + "." + strBitMask2 + ")", "0 * 1", "", "", TypeOfTag.no, "" );
			b_10330.OnChangeValForm += this.LinkSetText;
			b_10330.FirstValue();

			this.varName2 = varName2;
			this.varDim2 = varDim2;
		}

		private void поверхВсехОконToolStripMenuItem_CheckStateChanged( object sender, EventArgs e )
		{
			this.TopMost = tsmiCS.Checked;
		}

		private void tsmiDopPnl_CheckStateChanged( object sender, EventArgs e )
		{
			if( !tsmiDopPnl.Checked )
			{
				splitContainer1.Panel2Collapsed = true;
				this.Height = this.Width;
				return;
			}
			splitContainer1.Panel2Collapsed = false;
			this.Height = this.Width + splitContainer1.Panel2.Height;
		}

		private void btnMin_Click( object sender, EventArgs e )
		{
			dlgPriborGisterezis dpg = new dlgPriborGisterezis();
			Button btn = ( Button ) sender;
			switch( btn.Name )
			{
				case "btnMin":
					blinkpnlMin.Stop();
					dpg.IsCustomBottom = true;
                    //blinkpnlMin.Start();  // для отладки
					break;
				case "btnMax":
					blinkpnlMax.Stop();
					dpg.IsCustomTop = true;
                    //blinkpnlMax.Start();  // для отладки
					break;
			}

			dpg.TopPusk = topPusk;
			dpg.TopGist = topGist;
			dpg.TopVozvrat = topVozvrat;
			dpg.BottomPusk = bottomPusk;
			dpg.BottomGist = bottomGist;
			dpg.BottomVozvrat = bottomVozvrat;

			dpg.ShowDialog();

			topPusk = dpg.TopPusk;
			topGist = dpg.TopGist;
			topVozvrat = dpg.TopVozvrat;
			bottomPusk = dpg.BottomPusk;
			bottomGist = dpg.BottomGist;
			bottomVozvrat = dpg.BottomVozvrat;
		}		
	}
	public class BlinkContainer : System.Windows.Forms.PictureBox
	{
		System.Windows.Forms.Timer mTimer = new System.Windows.Forms.Timer();
		//static Color blinkColor;
		public BlinkContainer()
		{
			mTimer.Interval = 500;
			mTimer.Tick += new EventHandler( mTimer_Tick );
		}
		protected override void OnPaintBackground( System.Windows.Forms.PaintEventArgs e )
		{
			base.OnPaintBackground( e );
			if( drawCustomBorder )
			{
				// here is the place of making your custom drawing
				Rectangle rect = e.ClipRectangle;
				rect.Height -= 2;
				rect.Width -= 4;
				
				using( Pen pen = new Pen( Color.Red, 3 ) )
				{
					e.Graphics.DrawRectangle( pen, rect );
				}
			}
		}
		void mTimer_Tick( object sender, EventArgs e )
		{
			drawCustomBorder = !drawCustomBorder;
			this.Invalidate( true );
		}
		bool drawCustomBorder = false;
		public void Start( )
		{
			mTimer.Start();
		}
		public void Stop( )
		{
			mTimer.Stop();
			drawCustomBorder = false;
			Invalidate();
		}
		public bool IsBlinking
		{
			get
			{
				return mTimer.Enabled;
			}
		}
		public static BlinkContainer AddBlinkContainer( System.Windows.Forms.Control control )
		{
			System.Windows.Forms.Control parentControl = control.Parent;
			if( control == null || parentControl == null )
				return null;
			BlinkContainer container = new BlinkContainer();
            container.Size = new Size(control.Width + 8, control.Height + 4);//4
			container.Location = new Point( control.Location.X - 4, control.Location.Y - 2 );//2
			parentControl.Controls.Add( container );
			container.Controls.Add( control );
            control.Location = new Point(4, 2); //new Point(2, 2);
			return container;
		}
	}
}