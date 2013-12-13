/*#############################################################################*
 *    Copyright (C) 2006 Mehanotronika Corporation.                            *
 *    All rights reserved.                                                     *
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 *                                                                             *
 *	��������: ������������ ������� ���� TextBox                                 *
 *                                                                             *
 *	����                     : UserControl.cs                                   *
 *	��� ��������� �����      : LabelTextBox.dll                                 *
 *	������ �� ��� ���������� : �#, Framework 2.0                                *
 *	�����������              : ���� �.�.                                        *
 *	���� ������ ����������   : 01.03.2007                                       *
 *	���� (v1.0)              :                                                  *
 *******************************************************************************
 * ���������:
 * 1. ����(�����): ...c���������...
 *#############################################################################*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Threading;
using Calculator;
using InterfaceLibrary;

namespace LabelTextbox
{
	/// <summary>
	/// Summary description for UserControl1.
	/// </summary>
    public partial class ctlLabelTextbox : System.Windows.Forms.UserControl, MTRAUserControl, ISubscribe, IHMITagAccess
   {
      #region ��������� ������������
      // ���� ������ � ����
      struct TBColors
      {
         public Color textColor;
         public Color backgroundColor;
      }

      // �������� ����������������� �������� � TextBox'�
      public enum PositionEnum
      {
         Right,
         Below
      }
      
      #endregion    

      #region ��������
	  /// <summary>
	  /// ������ ������� � UserControl
	  /// </summary>
	  public string Caption_Text
	  {
		  get { return this.lblCaption.Text; }
		  set { this.lblCaption.Text = value; }
	  }
	  /// <summary>
	  /// ������ ������� ����������� � UserControl
	  /// </summary>
	  public string Dim_Text
	  {
		  get { return this.lblDim.Text; }
		  set { this.lblDim.Text = value; }
	  }

      public PositionEnum Position
      {
         get
         {
            return mPosition;
         }
         set
         {
            mPosition = value;

            if ( PositionChanged != null ) // Make sure there are any subscribers
            {
               // Get the list of methods to call
               System.Delegate[] subscribers = PositionChanged.GetInvocationList( );

               // Loop through the methods
               foreach ( System.EventHandler target in subscribers )
               {
                  target( this, new EventArgs( ) ); // Call the method
               }
            }
         }
      }

      public int TextboxMargin
      {
         get
         {
            return mTextboxMargin;
         }
         set
         {
            mTextboxMargin = value;
         }
      }

      public string LabelText
      {
         get
         {
            return lblCaption.Text;
         }
         set
         {
            lblCaption.Text = value;
         }
      }

      public string TextboxText
      {
         get
         {
            return txtLabelText.Text;
         }
         set
         {
			try
			{
                txtLabelText.Text = value;
                //Single ftmp = 0;
                //if (Single.TryParse(value,out ftmp))
                //    txtLabelText.Text = string.Format("{0:F3}", ftmp);
                //else
                //    txtLabelText.Text = "bad value";
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
         }
      }
      #endregion

      #region public
      public System.Windows.Forms.Label lblCaption;
      //public System.Windows.Forms.TextBox txtLabelText;
      public TextBoxEx txtLabelText;
      public uint addrLinkVar;         // ����� ��������� � ������ ��������� ����������
      public string mask;					// ����� ��� ��������� ����� �������� (��� �������)
      public string typetag = String.Empty;  // ��� ���� - ����������� � ��������
      public event System.EventHandler PositionChanged;      
      public Label lblDim;
	  public FormulaEvalNds fNDS;
      #endregion

      #region private
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.Container components = null;
      // Member field that will hold the choices the user makes
      private PositionEnum mPosition = PositionEnum.Right;
      private Label lblTest;
      private int mTextboxMargin = 0;
      private TBColors TBC;  // ��������� ��� �������� ����� ������ � ����
      #endregion

      #region �����������, dispose()
	  public ctlLabelTextbox()
	  {
		  // This call is required by the Windows.Forms Form Designer.
		  InitializeComponent();

			try
			{
                this.txtLabelText.TextAlign = HorizontalAlignment.Right;

                // Handle the Load event
                this.Load += new EventHandler(this.OnLoad);

                // Textbox Keybord events
                this.txtLabelText.KeyDown += new KeyEventHandler(this.txtLabelText_KeyDown);
                this.txtLabelText.KeyUp += new KeyEventHandler(this.txtLabelText_KeyUp);
                this.txtLabelText.KeyPress += new KeyPressEventHandler(this.txtLabelText_KeyPress);

                throw new Exception(string.Format(@"(177) : {0} : X:\Projects\01_HMIWinFormsClient\LabelTextbox\UserControl1.cs :  ctlLabelTextbox() : ��� ������ �� ���, �������� �� ��������� �������� ����������.", DateTime.Now.ToString()));
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
	  }

      public ctlLabelTextbox( FormulaEvalNds fnds )
      {
         // This call is required by the Windows.Forms Form Designer.
         InitializeComponent( );

			try
			{
                 this.txtLabelText.TextAlign = HorizontalAlignment.Right;

                 // Handle the Load event
                 this.Load += new EventHandler( this.OnLoad );

                 // Textbox Keybord events
                 this.txtLabelText.KeyDown += new KeyEventHandler( this.txtLabelText_KeyDown );
                 this.txtLabelText.KeyUp += new KeyEventHandler( this.txtLabelText_KeyUp );
                 this.txtLabelText.KeyPress += new KeyPressEventHandler( this.txtLabelText_KeyPress );

                 fNDS = fnds;
                 linkedTag = fnds.LinkVariableNewDs;
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
      }

      /*==========================================================================*
      *     protected override void Dispose( bool disposing )
      *     Clean up any resources being used.
      *==========================================================================*/
      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      protected override void Dispose( bool disposing )
      {
			try
			{
                if (disposing)
                {
                    if (components != null)
                        components.Dispose();
                }

                SetText_lblCaption();     //lblCaption.Dispose();
                SetText_txtLabelText();   //txtLabelText.Dispose();
                SetText_lblDim();         //lblDim.Dispose();
                SetText_lblTest();        // lblTest.Dispose();

                base.Dispose(disposing);
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
      }      
      #endregion        

      #region ������������� ��������
	  /// <summary>
	  /// ������������� ���������� �� �������
	  /// </summary>
	  /// <param name="parentControl"></param>
	  public void Init(Control parentControl)
	  {
		  this.Parent = parentControl;
	  }

		public delegate void SetText_lblCaption_Delegate( );
      public void SetText_lblCaption( )
      {
			try
			{
                if (lblCaption.InvokeRequired)
                {
                    SetText_lblCaption_Delegate sett = new SetText_lblCaption_Delegate(SetText_lblCaption);
                    this.Invoke(sett, new object[] { });
                }
                else
                    lblCaption.Dispose();
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
      }

      public delegate void SetText_txtLabelText_Delegate( );
      public void SetText_txtLabelText( )
      {
			try
			{
                if (txtLabelText.InvokeRequired)
                {
                    SetText_txtLabelText_Delegate sett = new SetText_txtLabelText_Delegate(SetText_txtLabelText);
                    this.Invoke(sett, new object[] { });
                }
                else
                    txtLabelText.Dispose();
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
      }

      public delegate void SetText_lblDim_Delegate( );
      public void SetText_lblDim( )
      {
			try
			{
                if (lblDim.InvokeRequired)
                {
                    SetText_lblDim_Delegate sett = new SetText_lblDim_Delegate(SetText_lblDim);
                    this.Invoke(sett, new object[] { });
                }
                else
                    lblDim.Dispose();
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
      }

      public delegate void SetText_lblTest_Delegate( );
      public void SetText_lblTest( )
      {
			try
			{
                if (lblTest.InvokeRequired)
                {
                    SetText_lblTest_Delegate sett = new SetText_lblTest_Delegate(SetText_lblTest);
                    this.Invoke(sett, new object[] { });
                }
                else
                    lblTest.Dispose();
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
      } 
      #endregion

      #region ��������� ������� ��������
      /*==========================================================================*
      * private void txtLabelText_KeyDown(object sender, KeyEventArgs e)
       * ��������� ������� �� �������
      *==========================================================================*/
      private void txtLabelText_KeyDown( object sender, KeyEventArgs e )
      {
         OnKeyDown( e );
      }

      /*==========================================================================*
      * private void txtLabelText_KeyUp(object sender, KeyEventArgs e)
       * ��������� ���������� �������
      *==========================================================================*/
      private void txtLabelText_KeyUp( object sender, KeyEventArgs e )
      {
         OnKeyUp( e );
      }

      /*==========================================================================*
      * private void txtLabelText_KeyPress(object sender, KeyEventArgs e)
      *==========================================================================*/
      private void txtLabelText_KeyPress( object sender, KeyPressEventArgs e )
      {
         OnKeyPress( e );
         //isChange = true; // ������������ ������ ������ ������� � ������ �������
      }

      /*==========================================================================*
      *     private void OnLoad(object sender, EventArgs e)
       * �������� ��� �������� ����������
      *==========================================================================*/
      private void OnLoad( object sender, EventArgs e )
      {
         //lblCaption.Text = this.Name;
         this.Height = txtLabelText.Height + lblCaption.Height;
      }
      #endregion

      #region ��������� ��������, ��������� ������
    delegate void SetTextCallback( string textVal, string TextCapt, string textDim, bool isValOutOfBounds );//Color textcolor);

	/// <summary>
	/// ���������� ������� ��������� 
	/// �������� � ��������� ��������
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	public void bnd_Format(object sender, ConvertEventArgs e)
	{
			try
			{
                if (e.DesiredType != typeof(string))
                    return;

                LinkSetText(e.Value, string.Empty);
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
	}
    /// <summary>
    /// ���������� ������� ��������� 
    /// �������� � �������� ��� �������� 
    /// � �������� ��������
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void bnd_Parse(object sender, ConvertEventArgs e)
    {
        try
        {
            if (e.DesiredType != typeof(string))
                return;
            string str =(string) e.Value;

            // ��������� �������� �� ������������
            Single sval = 0;
            if (Single.TryParse(str, out sval))
            {
                byte[] memX = BitConverter.GetBytes(sval);
                linkedTag.SetValue(memX, DateTime.Now, VarQualityNewDs.vqGood);
                linkedTag.IsHMIChange = true;
                this.isChange = true;
            }
            else
                MessageBox.Show(string.Format("������� ������ ������������� �������� ���� : {0} ; TagGuid = {1}", linkedTag.TagName, linkedTag.TagGUID.ToString()));
        }
        catch (Exception ex)
        {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
        }
    }

    public void LinkSetText( object Value, string format )
    {
       string txt;
       RezFormulaEval tmpRezFormulaEval;
	   tmpRezFormulaEval = fNDS.RezFormulaEval;
	   object val = Value;
			try
			{
                if (this.txtLabelText.InvokeRequired)
                {
                    txt = (string)val;
                    SetText(txt, tmpRezFormulaEval.CaptionIE, tmpRezFormulaEval.DimIE, false /*tmpRezFormulaEval.IsValOutOfBounds*/ );
                }
                else
                {
                    this.TextboxText = (string)val;
                    lblTest.Text = " " + Convert.ToString(val) + " ";
                    int differ = 0;
                    if (lblTest.Width > this.txtLabelText.Width)
                    {
                        differ = lblTest.Width - this.txtLabelText.Width;
                        this.txtLabelText.Width = lblTest.Width + 30;
                    }
                    this.lblCaption.Left += differ;
                    this.lblCaption.Text = tmpRezFormulaEval.CaptionIE;
                    this.lblDim.Left = this.lblCaption.Left + lblCaption.Width + 2; //+ differ
                    if (tmpRezFormulaEval.DimIE == null || tmpRezFormulaEval.DimIE == "" || tmpRezFormulaEval.DimIE == " ")
                        this.lblDim.Text = "";
                    else
                        this.lblDim.Text = "(" + tmpRezFormulaEval.DimIE + ")";
                    if (tmpRezFormulaEval.DimIE == "")
                    {
                        this.lblDim.Text = tmpRezFormulaEval.DimIE;
                    }
                }
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
    }

    /*==========================================================================*
    *	private void SetText(string textVal, string TextCapt, string textDim)
    * ��� ����������������� ������ ���������
    *==========================================================================*/
    private void SetText( string textVal, string TextCapt, string textDim, bool isValOutOfBounds )
    {
			try
			{
                if (this.txtLabelText.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    try
                    {
                        this.Invoke(d, new object[] { textVal, TextCapt, textDim, isValOutOfBounds });
                    }
                    catch (Exception ex)
                    {
                        char[] arrch = { '\0' };
                        string msg = ex.GetType() + " : " + ex.Message +
                        "\ntextVal = " + textVal.TrimEnd(arrch) +
                        "\nTextCapt = " + TextCapt +
                        "\ntextDim = " + textDim;
                        System.Diagnostics.Trace.TraceInformation("\n" + DateTime.Now.ToString() + " :ctlLabelTextbox: " + msg);
                    }
                }
                else
                {
                    SetEPTextAndColor(isValOutOfBounds);
                    this.TextboxText = textVal;
                    lblTest.Text = " " + textVal + " ";
                    int differ = 0;
                    if (lblTest.Width > this.txtLabelText.Width)
                    {
                        differ = lblTest.Width - this.txtLabelText.Width;
                        this.txtLabelText.Width = lblTest.Width;
                    }
                    this.lblCaption.Left += differ;
                    this.lblCaption.Text = TextCapt;
                    this.lblDim.Left = this.lblCaption.Left + lblCaption.Width + 2; //+ differ
                    if (textDim == null || textDim == "" || textDim == " ")
                        return;
                    this.lblDim.Text = "(" + textDim + ")";
                }
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
    }

    /// <summary>
    /// private void SetText(object Value)
    /// ��������� textbox � ������ ����� ������
    /// ������������  � ����� � ������� �����������
    /// </summary>
    /// <param Name="textVal"></param>
    public void SetText( string textVal )
    {
       try
       {
          this.TextboxText = textVal;
          lblTest.Text = " " + textVal + " ";
          int differ = 0;
          if ( lblTest.Width >= this.txtLabelText.Width )
          {
             differ = lblTest.Width - this.txtLabelText.Width;
             this.txtLabelText.Width = lblTest.Width;
             this.lblCaption.Left += differ;
             this.lblDim.Left = this.lblCaption.Left + lblCaption.Width + 2; //+ differ
          }
          else if ( lblTest.Width < this.txtLabelText.Width )
          {
             differ = this.txtLabelText.Width - lblTest.Width;
             this.txtLabelText.Width = lblTest.Width;
             this.lblCaption.Left -= differ;
             this.lblDim.Left = this.lblCaption.Left + lblCaption.Width + 2; //+ differ
          }

       }
       catch
       {
          System.Diagnostics.Trace.TraceInformation( "\n" + DateTime.Now.ToString( ) + " : ctlLabelTextbox : SetText - ���������� ��� �������" );
          throw new Exception( "ctlLabelTextbox : SetText - ���������� ��� �������" );
       }
    }

    ErrorProvider errProv;
    public void SetErrorProvider( ref ErrorProvider ep )
    {
       errProv = ep;
    }

    /// <summary>
    /// ����������/����� ��������� ������ � ����
    /// </summary>
    /// <param Name="isOutOfRange"></param>
    /// <param Name="ep"></param>
    /// <returns></returns>
    private void SetEPTextAndColor( bool isOutOfRange )
    {
      // ����� ������ � ���� �� ���������
      this.txtLabelText.ForeColor = Color.Black;
      this.txtLabelText.BackColor = Color.White;
      this.txtLabelText.Font = new Font( this.Font, FontStyle.Regular);

      if ( errProv == null )
      return;

			try
			{
                if (isOutOfRange)
                {
                    try
                    {
                        SetErrPrvd(this, "����� �� ������� ��������� ��������");
                        //errProv.SetError( this, "����� �� ������� ��������� ��������" );
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceInformation("\n" + DateTime.Now.ToString() + "(691) :ctlLabelTextbox: " + ex.GetType().ToString());

                    }
                    // ����� ������ � ���� ��� ������ �� ������� ������ ��������
                    this.txtLabelText.ForeColor = Color.Red;
                    this.txtLabelText.BackColor = Color.Yellow;
                    this.txtLabelText.Font = new Font(this.Font, FontStyle.Bold);
                }
                else
                {
                    try
                    {
                        SetErrPrvd(this, String.Empty);
                        //errProv.SetError( this, String.Empty );
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceInformation("\n" + DateTime.Now.ToString() + "(709) :ctlLabelTextbox: " + ex.GetType().ToString());
                    }
                }
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
    }

	#region ������������� �������� ����������� ��������� ���������
	/// <summary>
	/// ������� �� ��������� �������� ���������� ��������
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="value"></param>
	public void AdapterValueChange(object sender, string value)
	{
			try
			{
                LinkSetTextFromAdapter(Single.Parse(value));
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
	}
	/// <summary>
	/// ������� ���������� ������ �� ���������� ��������
	/// </summary>
	/// <param name="Value"></param>
	/// <param name="format"></param>
	delegate void SetTextCallbackFromAdapter(string textVal);//, string TextCapt, string textDim, bool isValOutOfBounds;
	public void LinkSetTextFromAdapter(object Value)//, string format
	{
		string txt;

		object val = Value;
			try
			{
                if (val is float)
                {
                    float f_val = (float)val;
                    string t_val = f_val.ToString();

                    if (this.txtLabelText.InvokeRequired)
                    {
                        txt = t_val;
                        SetTextFromAdapter(txt);
                    }
                }
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
	}

	/*==========================================================================*
	*	private void SetText(string textVal, string TextCapt, string textDim)
	* ��� ����������������� ������ ���������
	*==========================================================================*/
	private void SetTextFromAdapter(string textVal)
	{
        try
        {
            if (this.txtLabelText.InvokeRequired)
            {
                SetTextCallbackFromAdapter d = new SetTextCallbackFromAdapter(SetText);
                try
                {
                    this.Invoke(d, new object[] { textVal });
                }
                catch (Exception ex)
                {
                    char[] arrch = { '\0' };
                    string msg = ex.GetType() + " : " + ex.Message +
                    "\ntextVal = " + textVal.TrimEnd(arrch)
                    ;
                    System.Diagnostics.Trace.TraceInformation("\n" + DateTime.Now.ToString() + " :ctlLabelTextbox: " + msg);
                }
            }
            else
            {
                //SetEPTextAndColor(isValOutOfBounds);
                this.TextboxText = textVal;
                lblTest.Text = " " + textVal + " ";
                int differ = 0;
                if (lblTest.Width > this.txtLabelText.Width)
                {
                    differ = lblTest.Width - this.txtLabelText.Width;
                    this.txtLabelText.Width = lblTest.Width;
                }
                this.lblCaption.Left += differ;
            }
        }
        catch (Exception ex)
        {
            TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
        }
	}
	#endregion

    private delegate void SetErrPrvdDelegate(Control cntrl, string strErr);
    private void SetErrPrvd(Control cntrl, string strErr) 
    {
			try
			{
                if (cntrl.InvokeRequired)
                {
                    SetErrPrvdDelegate sepd = new SetErrPrvdDelegate(SetErrPrvd);
                    this.Invoke(sepd, new object[] { cntrl, strErr });
                }
                else
                    errProv.SetError(cntrl, strErr);
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
   }
    #endregion

      #region Component Designer generated code
    /// <summary>
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
     /*==========================================================================*
     * private void InitializeComponent()
     *==========================================================================*/
     private void InitializeComponent()
    {
            this.lblCaption = new System.Windows.Forms.Label();
            this.txtLabelText = new LabelTextbox.TextBoxEx();
            this.lblDim = new System.Windows.Forms.Label();
            this.lblTest = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblCaption
            // 
            this.lblCaption.AutoSize = true;
            this.lblCaption.Location = new System.Drawing.Point(86, 7);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(43, 13);
            this.lblCaption.TabIndex = 0;
            this.lblCaption.Text = "Caption";
            // 
            // txtLabelText
            // 
            this.txtLabelText.BackColor = System.Drawing.SystemColors.Window;
            this.txtLabelText.Location = new System.Drawing.Point(0, 4);
            this.txtLabelText.MinimumSize = new System.Drawing.Size(80, 0);
            this.txtLabelText.Name = "txtLabelText";
            this.txtLabelText.ReadOnly = true;
            this.txtLabelText.Size = new System.Drawing.Size(80, 20);
            this.txtLabelText.TabIndex = 1;
            // 
            // lblDim
            // 
            this.lblDim.AutoSize = true;
            this.lblDim.Location = new System.Drawing.Point(135, 7);
            this.lblDim.Name = "lblDim";
            this.lblDim.Size = new System.Drawing.Size(25, 13);
            this.lblDim.TabIndex = 2;
            this.lblDim.Text = "Dim";
            // 
            // lblTest
            // 
            this.lblTest.AutoSize = true;
            this.lblTest.BackColor = System.Drawing.Color.Transparent;
            this.lblTest.Location = new System.Drawing.Point(5, 29);
            this.lblTest.Name = "lblTest";
            this.lblTest.Size = new System.Drawing.Size(35, 13);
            this.lblTest.TabIndex = 3;
            this.lblTest.Text = "label1";
            this.lblTest.Visible = false;
            // 
            // ctlLabelTextbox
            // 
            this.Controls.Add(this.lblTest);
            this.Controls.Add(this.lblDim);
            this.Controls.Add(this.txtLabelText);
            this.Controls.Add(this.lblCaption);
            this.Name = "ctlLabelTextbox";
            this.Size = new System.Drawing.Size(163, 35);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

     #region ISubscribe
     /// <summary>
     /// �������� ���� ��
     /// ����������
     /// </summary>
     /// <param name="taglist"></param>
     public void SubscribeTagReNew()
     {
         // ������������� �� ����
         HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTag(linkedTag);
     }
     /// <summary>
     /// ������� ���� ��
     /// ����������
     /// </summary>
     /// <param name="taglist"></param>
     public void UnSubscribeTagReNew()
     {
         // ������������ �� �����
         HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTag(linkedTag);
     }
     /// <summary>
     /// ������� ���� ��
     /// ���������� c ���������� 
     /// �������� �� ��������� (����������)
     /// </summary>
     /// <param name="taglist"></param>     
     public void UnSubscribeTagReNewAndClear()
     {
         // ������������ �� �����
         linkedTag.SetDefaultValue();  // ������������� � �������� �� ��������� � ... ������������
         HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTag(linkedTag);
     }
     #endregion

        #region IHMITagAccess
        /// <summary>
        /// ���, ��������� � ������ 
        /// ��������� 
        /// </summary>
        public ITag LinkedTag { get { return linkedTag; } }
        ITag linkedTag = null;

        /// <summary>
        /// ����, ��������� � ������ 
        /// ��������� 
        /// </summary>
        public List<ITag> LinkedTags { get{return linkedTags;} }
        List<ITag> linkedTags = null;
        /// <summary>
        /// ������� ���������
        /// �������� ���������� ����
        /// ��� ������
        /// </summary>
        public bool IsChange { get{return isChange;} set{isChange = value;} }
        bool isChange = false;
        /// <summary>
        /// ��� ���� � �����������
        /// </summary>
        public string VisibleText
        {
            get { return this.txtLabelText.Text; }
        }
	    #endregion
   }
}