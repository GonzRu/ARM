/*#############################################################################
 *    Copyright (C) 2006-2011 Mehanotronika RA                            
 *    All rights reserved.                                                     
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
 *                                                                             
 *	Описание: Пользовательский компонент CheckBox для 
 *				представления битового сигнала с возможностью
 *				задания подписи. Поддерживает обновление от FormulaEval и 
 *				адаптеров различных типов
 *                                                                             
 *	Файл                     : X:\Projects\99_MTRAhmi\Client\LabelTextbox\CheckBoxVar.cs
 *	Тип конечного файла      :                                         
 *	версия ПО для разработки : С#, Framework 4.0                                
 *	Разработчик              : Юров В.И.                                        
 *	Дата начала разработки   : xx.xx.2007 
 *	Дата посл. корр-ровки    : 13.04.2011
 *	Дата (v1.0)              :                                                  
 ******************************************************************************
* Особенности реализации:
 * Используется ...
 *#############################################################################*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Calculator;
using InterfaceLibrary;
namespace LabelTextbox
{
    public partial class CheckBoxVar : UserControl, MTRAUserControl, InterfaceLibrary.ISubscribe, IHMITagAccess
    {
        #region События
        #endregion

        #region Свойства
        /// <summary>
        /// строка подписи к CheckBox
        /// </summary>
        public string CheckBox_Text
        {
            get { return this.checkBox1.Text; }
            set { this.checkBox1.Text = value; }
        }
        public bool IsClickable
        {
            get { return this.btnCheck.Enabled; }
            set { this.btnCheck.Enabled = value; }
        }
        #endregion

        #region public
        public uint addrLinkVar;                 // адрес связанной с данным контролом переменной
        public string addrLinkVarBitMask;        // битовая маска переменной
        #endregion

        #region private
        private FormulaEvalNds fNDS;
        #endregion

        #region конструктор(ы)
        public CheckBoxVar()
        {
            InitializeComponent();
            btnCheck.BackColor = Color.White;
            IsClickable = false;
        }
        public CheckBoxVar( FormulaEvalNds fnds )
            : this()
        {
            btnCheck.Click += new EventHandler( btnCheck_Click );
            fNDS = fnds;
            linkedTag = fnds.LinkVariableNewDs;
        }
        #endregion

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

        /*==========================================================================*
        *   private void void LinkSetText(object Value)
        *      для потокобезопасного вызова процедуры
        *==========================================================================*/
        delegate void SetTextCallback( bool bVal, string TextCapt, string textDim ); //string textVal
        delegate void SetTextCallbackFromAdapter( bool bVal );

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
            tmpRezFormulaEval = fNDS.RezFormulaEval;
            object val = Value;

            try
            {
                if ( val is bool )
                {
                    if ( this.checkBox1.InvokeRequired )
                    {
                        bV = Convert.ToBoolean( val );
                        SetText( bV, tmpRezFormulaEval.CaptionIE, tmpRezFormulaEval.DimIE );
                    }
                    else
                    {
                        this.checkBox1.Checked = Convert.ToBoolean( val );
                        if ( this.checkBox1.Checked )
                            btnCheck.BackColor = Color.Red;
                        else
                            btnCheck.BackColor = Color.White;
                        this.checkBox1.Text = tmpRezFormulaEval.CaptionIE;
                    }
                }
                else if ( val is string )
                {
                    if ( string.IsNullOrWhiteSpace( (string)val ) )
                        return;

                    if ( this.checkBox1.InvokeRequired )
                    {
                        bV = Convert.ToBoolean( val );
                        SetText( bV, tmpRezFormulaEval.CaptionIE, tmpRezFormulaEval.DimIE );
                    }
                    else
                    {
                        this.checkBox1.Checked = Convert.ToBoolean( val );
                        if ( this.checkBox1.Checked )
                            btnCheck.BackColor = Color.Red;
                        else
                            btnCheck.BackColor = Color.White;
                        this.checkBox1.Text = tmpRezFormulaEval.CaptionIE;
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
                    if ( this.checkBox1.InvokeRequired )
                    {
                        bV = Convert.ToBoolean( Value );
                        SetTextFromAdapter( bV );
                    }
                    else
                    {
                        this.checkBox1.Checked = Convert.ToBoolean( Value );

                        if ( this.checkBox1.Checked )
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

        /*==========================================================================*
        * private void SetText(object Value)
        * //для потокобезопасного вызова процедуры
        *==========================================================================*/
        private void SetText( bool bVal, string TextCapt, string textDim )
        {
            try
            {
                if ( this.checkBox1.InvokeRequired )
                {
                    SetTextCallback d = new SetTextCallback( SetText );
                    this.Invoke( d, new object[] { bVal, TextCapt, textDim } );
                }
                else
                {
                    this.checkBox1.Checked = bVal;
                    if ( this.checkBox1.Checked )
                        btnCheck.BackColor = Color.Red;
                    else
                        btnCheck.BackColor = Color.White;

                    this.checkBox1.Text = TextCapt;
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
                if ( this.checkBox1.InvokeRequired )
                {
                    SetTextCallbackFromAdapter d = new SetTextCallbackFromAdapter( SetTextFromAdapter );
                    this.Invoke( d, new object[] { bVal } );
                }
                else
                {
                    this.checkBox1.Checked = bVal;
                    if ( this.checkBox1.Checked )
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

        private void btnCheck_Click( object sender, EventArgs e )
        {
            try
            {
                bool bval = bool.Parse( linkedTag.ValueAsString );

                bval = !bval;

                byte[] memX = BitConverter.GetBytes( bval );
                linkedTag.SetValue( memX , DateTime.Now, VarQualityNewDs.vqGood);
                linkedTag.IsHMIChange = true;
                this.isChange = true;
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
            }
        }

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
        /// <summary>
        /// Имя тега в отображении
        /// </summary>
        public string VisibleText
        {
            get { return checkBox1.Text; }
        }
        #endregion
    }

    public class CheckBoxEx : CheckBox
    {
        protected override void WndProc( ref Message m )
        {
            //left button down, up, doubleclick;
            //meddle button down, up, doubleclick;
            //right button down, up, doubleclick
            if ( m.Msg == 0x0201 || m.Msg == 0x0202 || m.Msg == 0x0203 ||
                 m.Msg == 0x0207 || m.Msg == 0x0208 || m.Msg == 0x0209 ||
                 m.Msg == 0x0204 || m.Msg == 0x0205 || m.Msg == 0x0206 )
                return;

            base.WndProc( ref m );
        }
    }
}
