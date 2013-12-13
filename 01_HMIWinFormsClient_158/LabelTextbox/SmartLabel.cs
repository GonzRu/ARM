using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Calculator;

namespace LabelTextbox
{
	public partial class SmartLabel : Label
	{
		private bool isDragging = false;
		private int clickOffsetX = 0;
		private int clickOffsetY = 0;
		private ArrayList cfg = new ArrayList();
		string varName;
		string varDim;

		public SmartLabel( )
		{
			InitializeComponent();
			Text = "";
			toolStripMenuItem1.Click += new EventHandler( toolStripMenuItem1_Click );
		}

		public SmartLabel( ArrayList cfg )
		{
			InitializeComponent();
			Text = "Привяжи меня";
			toolStripMenuItem1.Click += new EventHandler( toolStripMenuItem1_Click );
			this.cfg = cfg;
			this.BorderStyle = BorderStyle.Fixed3D;
		}

		private void toolStripMenuItem1_Click( object sender, EventArgs e )
		{
            //frmSmartLabelCustom fslc = new frmSmartLabelCustom( this, cfg );
            //fslc.Left = this.Left + this.Width;
            //fslc.Top = this.Top;

            //if( fslc.ShowDialog() == DialogResult.OK )
            //{
            //    varLinkPath = fslc.selNode;
            //    fslc.Close();
            //}
            //else
            //    return;

            //// генерим привязку к тегу
            //char[] aa = { '[', ']', '\\', '№' };
            //string[] pieces = varLinkPath.Split( aa );

            //// локализуем
            //string strFC = "";
            //string strIDDev = "";
            //string strIDGrp = "";
            //string strAdrVar = "";
            //string strBitMask = "";

            //foreach( FC aFC in cfg )
            //{
            //    if( aFC.NumFC == Convert.ToInt32( pieces[2] ) )
            //    {
            //        strFC = pieces[2];
            //        foreach( TCRZADirectDevice aDev in aFC )
            //        {
            //            if( aDev.NumDev == Convert.ToInt32( pieces[4] ) )
            //            {
            //                strIDDev = Convert.ToInt32( pieces[4] ).ToString();
            //                //strIDDev = pieces[4];

            //                foreach( TCRZAGroup aGroup in aDev )
            //                {
            //                    if( aGroup.Name == pieces[6] )
            //                    {
            //                        strIDGrp = aGroup.Id.ToString();
            //                        foreach( TCRZAVariable aVariable in aGroup )
            //                        {
            //                            if( aVariable.Name == pieces[7] )
            //                            {
            //                                varName = aVariable.Name;
            //                                varDim = aVariable.Dim;

            //                                strAdrVar = aVariable.RegInDev.ToString();
            //                                if( aVariable is TBitFieldVariable )
            //                                {
            //                                    // сравниваем маски
            //                                    TBitFieldVariable bitTmp = ( TBitFieldVariable ) aVariable;
            //                                    strBitMask = bitTmp.bitMask;
            //                                }
            //                                else
            //                                    strBitMask = "0";
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}

            //FormulaEval b_10330 = new FormulaEval( cfg, "0(" + strFC + "." + strIDDev + "." + strIDGrp + "." + strAdrVar + "." + strBitMask + ")", "0", "", "", TypeOfTag.NoN, "" );
            //b_10330.OnChangeValForm += this.LinkSetText;
            //b_10330.FirstValue();
		}

		/*==========================================================================*
		*   private void void LinkSetText(object Value)
		*      для потокобезопасного вызова процедуры
		*==========================================================================*/
		delegate void SetTextCallback( string textVal, string TextCapt, string textDim );

		public void LinkSetText( object Value, string format )
		{
			string txt;
			RezFormulaEval tmpRezFormulaEval;

			if( !( Value is RezFormulaEval ) )
				return;   // сгенерировать ошибку через исключение

			tmpRezFormulaEval = ( RezFormulaEval ) Value;
			object val = tmpRezFormulaEval.Value;

			if( val is int )
			{
				if( this.InvokeRequired )
				{
					txt = varName + " " + Convert.ToString( val ) + " (" + varDim + ")";
					SetText( txt, tmpRezFormulaEval.CaptionIE, tmpRezFormulaEval.DimIE );
				}
				else
					this.Text = varName + " " + Convert.ToString( val ) + " (" + varDim + ")";

			}
			else if( val is short )
			{
				if( this.InvokeRequired )
				{
					txt = varName + " " + Convert.ToString( val ) + " (" + varDim + ")";
					SetText( txt, tmpRezFormulaEval.CaptionIE, tmpRezFormulaEval.DimIE );
				}
				else
					this.Text = varName + " " + Convert.ToString( val ) + " (" + varDim + ")";
			}
			else if( val is ushort )
			{
				if( this.InvokeRequired )
				{
					txt = varName + " " + Convert.ToString( val ) + " (" + varDim + ")";
					SetText( txt, tmpRezFormulaEval.CaptionIE, tmpRezFormulaEval.DimIE );
				}
				else
					this.Text = varName + " " + Convert.ToString( val ) + " (" + varDim + ")";
			}
			else if( val is float )
			{
				if( this.InvokeRequired )
				{
					txt = varName + " " + Convert.ToString( val ) + " (" + varDim + ")";
					SetText( txt, tmpRezFormulaEval.CaptionIE, tmpRezFormulaEval.DimIE );
				}
				else
					this.Text = varName + " " + Convert.ToString( val ) + " (" + varDim + ")";

			}
			else if( val is string )
			{
				if( this.InvokeRequired )
				{
					txt = varName + " " + ( string ) val + " (" + varDim + ")";
					SetText( txt, tmpRezFormulaEval.CaptionIE, tmpRezFormulaEval.DimIE );
				}
				else
					this.Text = varName + " " + ( string ) val + " (" + varDim + ")";
			}
			else
			{
				if( this.InvokeRequired )
				{
					txt = "0";
					SetText( txt, tmpRezFormulaEval.CaptionIE, tmpRezFormulaEval.DimIE );
				}
				else
					this.Text = "0";
			}
		}

		/*==========================================================================*
		* private void SetText(object Value)
		* //для потокобезопасного вызова процедуры
		*==========================================================================*/
		private void SetText( string textVal, string TextCapt, string textDim )
		//private void SetText(object Value)
		{
			if( this.InvokeRequired )
			{
				SetTextCallback d = new SetTextCallback( SetText );
				this.Invoke( d, new object[] { textVal, TextCapt, textDim } );
			}
			else
				this.Text = varName + " " + textVal + " (" + varDim + ")";
		}

		private void SmartLabel_MouseDown( object sender, MouseEventArgs e )
		{
			isDragging = true;
			clickOffsetX = e.X;
			clickOffsetY = e.Y;
		}

		private void SmartLabel_MouseUp( object sender, MouseEventArgs e )
		{
			isDragging = false;
		}

		private void SmartLabel_MouseMove( object sender, MouseEventArgs e )
		{
			if( isDragging )
			{
				this.Left = e.X + this.Left - clickOffsetX;
				this.Top = e.Y + this.Top - clickOffsetY;
			}
		}
	}
}
