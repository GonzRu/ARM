/*#############################################################################*
 *    Copyright (C) 2006 Mehanotronika Corporation.                            *
 *    All rights reserved.                                                     *
 *	~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~*
 *                                                                             *
 *	Описание: Интерфейсный элемент типа ComboBox                                 *
 *                                                                             *
 *	Файл                     : ComboBoxVar.cs                                   *
 *	Тип конечного файла      : LabelTextBox.dll                                 *
 *	версия ПО для разработки : С#, Framework 2.0                                *
 *	Разработчик              : Юров В.И.                                        *
 *	Дата начала разработки   : 16.04.2007                                       *
 *	Дата (v1.0)              :                                                  *
 *******************************************************************************
 * Изменения:
 * 1. Дата(Автор): ...cодержание...
 *#############################################################################*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Calculator;
using InterfaceLibrary;
namespace LabelTextbox
{
   public enum TypeViewValue
   {
      Combobox, Textbox
   }
    public partial class ComboBoxVar : UserControl, ISubscribe, IHMITagAccess
    {
       /// <summary>
       /// признак того что список строк сопровождается 
       /// неупорядоченным числовым значением
       /// в списке эти номера отделятся от строк символом EnumStrRazdelitel
       /// </summary>
       public bool isThisNumEnumCB = false;
       const char chEnumStrRazdelitel = '#';
       /// <summary>
       /// список строк, ассоциированных по их уник целому значению
       /// </summary>
       public SortedList<int,string> slNumEnumStr;

        public string[] arrStrInclude;
        public uint addrLinkVar;        // аддрес связанной с данным контролом переменной
        private int IndexDefaul;        // индекс по умолчанию
        public string addrLinkVarBitMask;         // битовая маска переменной
        public string typetag = String.Empty;  // тип тега - понадобился в уставках
        public CBChangeEVT evt = new CBChangeEVT( );
        ComboBoxListWindow ffcbe;
        ToolTip toolTip;
       /// <summary>
       /// вид элемента на форме - Combobox или Textbox
       /// </summary>
       public TypeViewValue TypeView
       {
           set
           {
              typeview = value;
              /*
               * Везде исп. textbox
               * отличие только в наличии
               * кнопки расширения (с точками)
               */
              //if ( typeview == TypeViewValue.Combobox )
              //{
              //    cbVar.Visible = false;
              //    tbText.Visible = true;
              //}
              //else
              //{
              //    cbVar.Visible = false;
              //    tbText.Visible = true;
              //}

               SetControlByType();
           }
           get{return typeview;}
       }

       private TypeViewValue typeview;
       FormulaEvalNds fNDS;

       public ComboBoxVar(FormulaEvalNds fnds)
        {
            InitializeComponent();
            lblCaption.Text = "";

            fNDS = fnds;
            linkedTag = fnds.LinkVariableNewDs;
        }

       public ComboBoxVar(string[] arrStrInclude, int indexDefault, FormulaEvalNds fnds)
        {
            InitializeComponent();

			try
			{
                fNDS = fnds;
                linkedTag = fnds.LinkVariableNewDs;
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }

       public ComboBoxVar(SortedList<int, string> slenumParty, int indexDefault, FormulaEvalNds fnds)
        {
            InitializeComponent();
            try
            {
                toolTip = new ToolTip();
                toolTip.ToolTipTitle = string.Empty;
                toolTip.ReshowDelay = 1000;
                toolTip.InitialDelay = 1000;
                toolTip.Show("Отобразить перечисление", btnToEnumFrm, 3000);
                toolTip.SetToolTip(btnToEnumFrm, "Отобразить перечисление");

                isThisNumEnumCB = true;
                slNumEnumStr = slenumParty;

                arrStrInclude = new string[slNumEnumStr.Count];
                slNumEnumStr.Values.CopyTo(arrStrInclude,0);

                this.cbVar.Items.AddRange( this.arrStrInclude);

                if (arrStrInclude.Length != 0)
                    this.cbVar.SelectedIndex = 0;

                IndexDefaul = indexDefault;
                SetWidthControlInPixels();
                SetControlByType();

                btnToEnumFrm.Tag = false;  // Tag обозначает развернутость формы с перечислением

                fNDS = fnds;
                linkedTag = fnds.LinkVariableNewDs;
            }
            catch (Exception ex)
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex);
            }
        }

        /// <summary>
        /// обработчик события изменения 
        /// значения в источнике привязки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void bnd_Format(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType != typeof(string))
                return;

            LinkSetText(e.Value, string.Empty);
        }

        /// <summary>
        /// установим ширину контрола по самому длинному элементу
        /// </summary>
        /// <param Name="?"></param>
        private void SetWidthControlInPixels()
        {
           int lenControl = 0;
           int maxlenControl = 0;

			try
			{
                using (Graphics graphics = tbText.CreateGraphics())
                {
                    for (int i = 0; i < arrStrInclude.Length; i++)
                    {
                        lenControl = (int)graphics.MeasureString(arrStrInclude[i], Font).Width;
                        if (lenControl > maxlenControl)
                            maxlenControl = lenControl;
                    }

                    // чуть расширим
                    cbVar.Width = maxlenControl + maxlenControl / 3;
                    tbText.Width = maxlenControl;
                }
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }

        /*==========================================================================*
        *   private void void LinkSetText(object Value)
        *      для потокобезопасного вызова процедуры
        *==========================================================================*/
        delegate void SetTextCallback( ushort iVal, string TextCapt, string textDim ); //string textVal

		 public void LinkSetText( object Value, string format )
        {
            ushort iV;
            int valP;
            RezFormulaEval tmpRezFormulaEval;
            if( !( Value is RezFormulaEval ) )
                return;   // сгенерировать ошибку через исключение
            tmpRezFormulaEval = ( RezFormulaEval )Value;
            object val = tmpRezFormulaEval.Value;

          // проверка на развернутость формы 
            if (ffcbe != null)
            {
               ffcbe.Close();
               ffcbe = null;
            }

            try
            {
               #region ushort
               if( val is ushort )
               {
                  if( this.cbVar.InvokeRequired )
                  {
                     iV = Convert.ToUInt16( val );

                     if( iV >= arrStrInclude.Length && !isThisNumEnumCB )   //!! было iV > arrStrInclude.Length - поменял для БМЦС - может сказаться на других блоках
                        iV = (ushort) IndexDefaul;

                     SetText( iV, tmpRezFormulaEval.CaptionIE, tmpRezFormulaEval.DimIE );
                  }
                  else
                  {
                     valP = (int) Convert.ToUInt16( val  );

                     if( valP > arrStrInclude.Length && !isThisNumEnumCB )
                        valP = IndexDefaul;

                     if (isThisNumEnumCB)
                     {
                        if( slNumEnumStr.ContainsKey( valP ) )
                           this.cbVar.SelectedIndex = slNumEnumStr.IndexOfKey( valP );
                        else
                        {
                           this.cbVar.SelectedIndex = slNumEnumStr.IndexOfKey( valP );
                           slNumEnumStr.Add(valP, "ошибка");
                           this.cbVar.SelectedIndex = slNumEnumStr.IndexOfKey( valP );
                        }
                     }
                     else if( arrStrInclude.Length != 0 )
                        this.cbVar.SelectedIndex = valP;


                     // активируем нужныый тип котнрола - combo или textbox
                     SetControlByType();

                     this.lblCaption.Text = tmpRezFormulaEval.CaptionIE;

                     if (!String.IsNullOrEmpty(tmpRezFormulaEval.DimIE))
                        this.lblCaption.Text += " (" + tmpRezFormulaEval.DimIE  +")";
                  }
               } 
	            #endregion     
               #region bool
               else if( val is bool )
               {
                  if( this.cbVar.InvokeRequired )
                  {
                     iV = Convert.ToUInt16( val );

                     if( iV >= arrStrInclude.Length && !isThisNumEnumCB )   //!! было iV > arrStrInclude.Length - поменял для БМЦС - может сказаться на других блоках
                        iV = (ushort) IndexDefaul;

                     SetText( iV, tmpRezFormulaEval.CaptionIE, tmpRezFormulaEval.DimIE );
                  }
                  else
                  {
                     valP = (int) Convert.ToUInt16( val );

                     if( valP > arrStrInclude.Length && !isThisNumEnumCB )
                        valP = IndexDefaul;

                     if( isThisNumEnumCB )
                     {
                        if( slNumEnumStr.ContainsKey( valP ) )
                           this.cbVar.SelectedIndex = slNumEnumStr.IndexOfKey( valP );
                        else
                        {
                           this.cbVar.SelectedIndex = slNumEnumStr.IndexOfKey( valP );
                           slNumEnumStr.Add( valP, "ошибка" );
                           this.cbVar.SelectedIndex = slNumEnumStr.IndexOfKey( valP );
                        }
                     }
                     else if( arrStrInclude.Length != 0 )
                        this.cbVar.SelectedIndex = valP;


                     // активируем нужныый тип котнрола - combo или textbox
                     SetControlByType();

                     this.lblCaption.Text = tmpRezFormulaEval.CaptionIE;

                     if( !String.IsNullOrEmpty( tmpRezFormulaEval.DimIE ) )
                        this.lblCaption.Text += " (" + tmpRezFormulaEval.DimIE + ")";
                  }
               }               
               #endregion
            }
            catch( Exception ex )
            {
               this.lblCaption.Text = tmpRezFormulaEval.CaptionIE;
               cbVar.Text = tmpRezFormulaEval.Value.ToString();

               MessageBox.Show( "Ошибка при преобразовании значения в тип ComboBoxVar : " + ex.Message
                  + "\nПодробности:\n тег: " +  tmpRezFormulaEval.CaptionIE
                  + "\n ид. тега : " + tmpRezFormulaEval.IdTagIE
                  + "\n значение = " + tmpRezFormulaEval.Value.ToString(), "(85)ComboBoxVar.cs", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
        }

       /// <summary>
       /// вычислить длину текста в пикселах
       /// </summary>
       /// <param Name="?"></param>
       private void SetControlByType()
       {
			try
			{
                // выбрать представление на форме
                if (typeview == TypeViewValue.Combobox)
                {
                    //tbText.Visible = false;

                    tbText.Visible = true;
                    cbVar.Visible = false;
                    tbText.Text = cbVar.Text;

                    this.btnToEnumFrm.Visible = true;
                }
                else
                {
                    tbText.Visible = true;
                    cbVar.Visible = false;
                    this.btnToEnumFrm.Visible = false;
                    tbText.Text = cbVar.Text;
                }
                // разместим кнопку расшиения и подпись
                if (typeview == TypeViewValue.Combobox)
                {
                    //this.btnToEnumFrm.Location = new Point(cbVar.Left + cbVar.Width, cbVar.Top);
                    //this.lblCaption.Location = new Point(this.btnToEnumFrm.Location.X + this.btnToEnumFrm.Width,cbVar.Top);
                    //this.btnToEnumFrm.Left = cbVar.Left + cbVar.Width;
                    this.btnToEnumFrm.Left = tbText.Left + tbText.Width;
                    this.lblCaption.Left = pnlContainer.Left + pnlContainer.Width;
                }
                else
                {
                    this.lblCaption.Left = tbText.Left + tbText.Width;
                }

                tbText.ReadOnly = true;
                tbText.BackColor = Color.White;
                this.lblCaption.Left += 5;
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
       }

        /*==========================================================================*
        * private void SetText(object Value)
        * //для потокобезопасного вызова процедуры
        *==========================================================================*/
        private void SetText( ushort iVal, string TextCapt, string textDim )
        {
            int valP;

			try
			{
                if (this.cbVar.InvokeRequired)
                {
                    SetTextCallback d = new SetTextCallback(SetText);
                    this.Invoke(d, new object[] { iVal, TextCapt, textDim });
                }
                else
                {
                    valP = (int)iVal;

                    if (valP > arrStrInclude.Length && !isThisNumEnumCB)
                        valP = IndexDefaul;

                    if (isThisNumEnumCB)
                    {
                        if (slNumEnumStr.ContainsKey(valP))
                            this.cbVar.SelectedIndex = slNumEnumStr.IndexOfKey(valP);
                        else
                        {
                            this.cbVar.SelectedIndex = slNumEnumStr.IndexOfKey(valP);
                            slNumEnumStr.Add(valP, "ошибка");
                            this.cbVar.SelectedIndex = slNumEnumStr.IndexOfKey(valP);
                        }
                    }
                    else if (arrStrInclude.Length != 0)
                        this.cbVar.SelectedIndex = valP;

                    // активируем нужныый тип котнрола - combo или textbox
                    SetControlByType();

                    this.lblCaption.Text = TextCapt;

                    if (!String.IsNullOrEmpty(textDim))
                        this.lblCaption.Text += " (" + textDim + ")";
                }
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
        }

        private void cbVar_SelectedIndexChanged( object sender, EventArgs e )
        {
           if( evt != null )
              evt.OnCBChangeEvent( this );
        }

		 private void cbVar_MouseClick( object sender, MouseEventArgs e )
		 {
			 isChange = true;
		 }

       private void btnToEnumFrm_Click(object sender, EventArgs e)
       {
			try
			{
                if (((bool)btnToEnumFrm.Tag) == true)
                {
                    // форма активна, получаем значение, закрываем
                    if (ffcbe != null)
                    {
                        ffcbe.Close();
                        ffcbe = null;
                    }
                    toolTip.Show("Отобразить перечисление", btnToEnumFrm, 3000);
                    toolTip.SetToolTip(btnToEnumFrm, "Отобразить перечисление");
                }
                else
                {
                    frmWait fw;
                    if (arrStrInclude.Length > 50)
                    {
                        //fw = new frmWait();
                        //fw.ShowDialog();

                        ffcbe = new ComboBoxListWindow( this.lblCaption.Text, arrStrInclude, tbText.Text );
                        ffcbe.Left = 0;                                                  
                        ffcbe.TopMost = true;
                        //ffcbe.Top = cbVar.Top;
                        //ffcbe.TopLevel = false;
                        //ffcbe.Parent = ma;
                        ffcbe.FormClosing += new FormClosingEventHandler(ffcbe_FormClosing);
                        ffcbe.Show();
                    }
                    else
                    {
                        fw = null;
                        ffcbe = new ComboBoxListWindow( this.lblCaption.Text, arrStrInclude, tbText.Text );
                        ffcbe.Left = 0;
                        ffcbe.Top = cbVar.Top;
                        ffcbe.TopLevel = false;
                        ffcbe.Parent = pnlContainer;
                        ffcbe.FormClosing += new FormClosingEventHandler(ffcbe_FormClosing);
                        ffcbe.Show();
                    }

                    toolTip.Show("Сохранить изменение", btnToEnumFrm, 3000);
                    toolTip.SetToolTip(btnToEnumFrm, "Сохранить изменение");
                }

                btnToEnumFrm.Tag = !((bool)btnToEnumFrm.Tag);
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
       }

       /// <summary>
       /// реакция на закрытие формы с перечислениями
       /// </summary>
       /// <param Name="sender"></param>
       /// <param Name="e"></param>
       void ffcbe_FormClosing(object sender, FormClosingEventArgs e)
       {
			try
			{
                if (ffcbe == null)
                    return;
                // установить новый текущий элемент перечисления
                cbVar.SelectedItem = ffcbe.ActiveItemInCB;

                tbText.Text = (string)cbVar.SelectedItem;
                Single index = Convert.ToSingle(cbVar.SelectedIndex);
                byte[] memx = BitConverter.GetBytes(index);

                linkedTag.SetValue(memx,DateTime.Now, VarQualityNewDs.vqGood);
                linkedTag.IsHMIChange = true;

                isChange = true;
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
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
           HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.SubscribeTag(fNDS.LinkVariableNewDs);
       }
       /// <summary>
       /// отписка тега от
       /// обновления
       /// </summary>
       /// <param name="taglist"></param>
       public void UnSubscribeTagReNew()
       {
           // отписываемся от тегов
           HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTag(fNDS.LinkVariableNewDs);
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
           HMI_MT_Settings.HMI_Settings.CONFIGURATION.CfgReqEntry.UnSubscribeTag(linkedTag);
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
            get { return this.cbVar.Text; }
        }
        #endregion
    }

    public delegate void CBChangeEventHandler( object sender );
    // класс события
    public class CBChangeEVT
    {
       public event CBChangeEventHandler CBChangeEvent;

       public void OnCBChangeEvent( object sndr )
       {
			try
			{
                if (CBChangeEvent != null)
                    CBChangeEvent(sndr);
            }
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}
      }
    }

    public class TextBoxEx : TextBox
    {
       protected override void WndProc(ref Message m)
       {
          if (this.ReadOnly && (m.Msg == 0xa1 || m.Msg == 0x201))
             return;

          base.WndProc(ref m);
       }
    }
}
