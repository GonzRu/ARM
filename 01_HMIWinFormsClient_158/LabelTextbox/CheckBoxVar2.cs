using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Calculator;
using InterfaceLibrary;

namespace LabelTextbox
{
    public partial class CheckBoxVar2 : UserControl, MTRAUserControl, InterfaceLibrary.ISubscribe, IHMITagAccess
    {
        delegate void SetTextCallback( bool bVal, string TextCapt, string textDim ); //string textVal
        delegate void SetTextCallbackFromAdapter( bool bVal );

        #region IHMITagAccess
        /// <summary>
        /// Тег, связанный с данной 
        /// сущностью 
        /// </summary>
        public ITag LinkedTag { get { return linkedTag; } }
        ITag linkedTag = null;

        /// <summary>
        /// Теги, связанные с данной 
        /// сущностью 
        /// </summary>
        public List<ITag> LinkedTags { get { return linkedTags; } }
        List<ITag> linkedTags = null;
        /// <summary>
        /// признак изменения
        /// значения связанного тега
        /// для записи
        /// </summary>
        public bool IsChange { get { return isChange; } set { isChange = value; } }
        bool isChange = false;
        #endregion

        public uint addrLinkVar;                 // адрес связанной с данным контролом переменной
        public string addrLinkVarBitMask;        // битовая маска переменной
        private FormulaEvalNDS fNDS;

        public CheckBoxVar2()
        {
            InitializeComponent();
        }
        public CheckBoxVar2( FormulaEvalNDS fnds )
            : this()
        {
            fNDS = fnds;
            linkedTag = fnds.LinkVariableNewDS;
        }
        /// <summary>
        /// Инициализация компонента из фабрики
        /// </summary>
        /// <param name="parentControl"></param>
        public void Init( Control parentControl )
        {
            this.Parent = parentControl;
        }
        /// <summary>
        /// обработчик события изменения 
        /// значения в источнике привязки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void bnd_Format( object sender, ConvertEventArgs e )
        {
            LinkSetText( e.Value, string.Empty );
        }
        /// <summary>
        /// обработчик события изменения 
        /// значения в источнике привязки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void bnd_Parse( object sender, ConvertEventArgs e )
        {
            try
            {
                if ( e.DesiredType != typeof( string ) )
                    return;
                string str = (string)e.Value;
                //LinkSetText(e.Value, string.Empty);
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Функция обновления данных от связанной формулы
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="format"></param>
        public void LinkSetText( object Value, string format )
        {
            System.Diagnostics.Trace.TraceInformation( DateTime.Now.ToString() + " (338) : CheckBoxVar.cs: LinkSetText :  " );

            bool bV;
            RezFormulaEval tmpRezFormulaEval;
            tmpRezFormulaEval = fNDS.tRezFormulaEval;
            object val = Value;

            try
            {
                if ( val is bool )
                {
                    if ( this.btnCheck.InvokeRequired /*this.checkBox1.InvokeRequired*/ )
                    {
                        bV = Convert.ToBoolean( val );
                        SetText( bV, tmpRezFormulaEval.CaptionIE, tmpRezFormulaEval.DimIE );
                    }
                    else
                    {
                        IsChecked = Convert.ToBoolean( val );//this.checkBox1.Checked = Convert.ToBoolean( val );
                        if ( IsChecked /*this.checkBox1.Checked*/ )
                            btnCheck.BackColor = Color.Red;
                        else
                            btnCheck.BackColor = Color.White;
                        CheckBox_Text = tmpRezFormulaEval.CaptionIE;//this.checkBox1.Text = tmpRezFormulaEval.CaptionIE;
                    }
                }
                else if ( val is string )
                {
                    if ( string.IsNullOrWhiteSpace( (string)val ) )
                        return;

                    if ( this.btnCheck.InvokeRequired /*this.checkBox1.InvokeRequired*/ )
                    {
                        bV = Convert.ToBoolean( val );
                        SetText( bV, tmpRezFormulaEval.CaptionIE, tmpRezFormulaEval.DimIE );
                    }
                    else
                    {
                        IsChecked = Convert.ToBoolean( val );//this.checkBox1.Checked = Convert.ToBoolean( val );
                        if ( IsChecked /*this.checkBox1.Checked*/ )
                            btnCheck.BackColor = Color.Red;
                        else
                            btnCheck.BackColor = Color.White;
                        CheckBox_Text = tmpRezFormulaEval.CaptionIE;//this.checkBox1.Text = tmpRezFormulaEval.CaptionIE;
                    }
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// Функция обновления данных от связанного адаптера
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="format"></param>
        public void LinkSetTextFromAdapter( object Value )
        {
            bool bV;

            try
            {
                if ( Value is bool )
                {
                    if ( this.btnCheck.InvokeRequired /*this.checkBox1.InvokeRequired*/ )
                    {
                        bV = Convert.ToBoolean( Value );
                        SetTextFromAdapter( bV );
                    }
                    else
                    {
                        IsChecked = Convert.ToBoolean( Value ); //this.checkBox1.Checked = Convert.ToBoolean( Value );

                        if ( IsChecked /*this.checkBox1.Checked*/ )
                            btnCheck.BackColor = Color.Red;
                        else
                            btnCheck.BackColor = Color.White;
                    }
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        private void SetText( bool bVal, string TextCapt, string textDim )
        {
            try
            {
                if ( this.btnCheck.InvokeRequired /*this.checkBox1.InvokeRequired*/ )
                {
                    SetTextCallback d = new SetTextCallback( SetText );
                    this.Invoke( d, new object[] { bVal, TextCapt, textDim } );
                }
                else
                {
                    IsChecked = bVal; //this.checkBox1.Checked = bVal;
                    if ( IsChecked /*this.checkBox1.Checked*/ )
                        btnCheck.BackColor = Color.Red;
                    else
                        btnCheck.BackColor = Color.White;

                    CheckBox_Text = TextCapt; //this.checkBox1.Text = TextCapt;
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bVal"></param>
        /// <param name="TextCapt"></param>
        /// <param name="textDim"></param>
        private void SetTextFromAdapter( bool bVal )
        {
            try
            {
                if ( this.btnCheck.InvokeRequired /*this.checkBox1.InvokeRequired*/ )
                {
                    SetTextCallbackFromAdapter d = new SetTextCallbackFromAdapter( SetTextFromAdapter );
                    this.Invoke( d, new object[] { bVal } );
                }
                else
                {
                    IsChecked = bVal; //this.checkBox1.Checked = bVal;
                    if ( IsChecked /*this.checkBox1.Checked*/ )
                        btnCheck.BackColor = Color.Red;
                    else
                        btnCheck.BackColor = Color.White;
                }
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }
        //void btnCheck_Click( object sender, EventArgs e )
        //{
        //    try
        //    {
        //        bool bval = bool.Parse( linkedTag.ValueAsString );

        //        bval = !bval;

        //        byte[] memX = BitConverter.GetBytes( bval );
        //        linkedTag.SetValue( memX );
        //        linkedTag.IsHMIChange = true;
        //        this.isChange = true;
        //    }
        //    catch ( Exception ex )
        //    {
        //        TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
        //    }
        //}
        /// <summary>
        /// функция по изменению значения связанного адаптера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="value"></param>
        public void AdapterValueChange( object sender, string value )
        {
            LinkSetTextFromAdapter( bool.Parse( value ) );
        }

        #region ISubscribe
        /// <summary>
        /// подписка тега на
        /// обновление
        /// </summary>
        /// <param name="taglist"></param>
        public void SubscribeTagReNew()
        {
            // подписываемся на теги
            HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTag( linkedTag );
        }
        /// <summary>
        /// отписка тега от
        /// обновления
        /// </summary>
        /// <param name="taglist"></param>
        public void UnSubscribeTagReNew()
        {
            // отписываемся от тегов
            HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTag( linkedTag );
        }
        /// <summary>
        /// отписка тега от
        /// обновления c установкой 
        /// значения по умолчанию (обнулением)
        /// </summary>
        /// <param name="taglist"></param>     
        public void UnSubscribeTagReNewAndClear()
        {
            // отписываемся от тегов
            linkedTag.SetDefaultValue();  // устанавливаем в значение по умолчанию и ... отписываемся
            HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTag( linkedTag );
        }
        #endregion

        /// <summary>
        /// строка подписи
        /// </summary>
        public string CheckBox_Text
        {
            get { return this.label1.Text; }
            set { this.label1.Text = value; }
        }
        public Boolean IsChecked { get; set; }
        public Boolean IsClickable
        {
            get { return this.btnCheck.Enabled; }
            set { this.btnCheck.Enabled = value; }
        }
    }
}
