using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace CommonUtils
{
    public class LongTimeAction : InterfaceLibrary.ILongTimeAction
    {
        Form parentFrm;
        public dlgLongTimeAction dlta;
        string captionstr = "Длительная операция";
        string outstr = "Сбор данных ...";
        string prestr = string.Empty;
        string poststr = string.Empty;
        string btnCaption = "Отменить";

        /// <summary>
        /// вывод сообщения по умолчанию
        /// </summary>
        /// <param Name="frm"></param>
        public void Start( Form frm )
        {
            parentFrm = frm;

            dlta = new dlgLongTimeAction();

            SetHMIElementsText( captionstr, prestr, outstr, poststr, btnCaption );

            parentFrm.AddOwnedForm( dlta );
            dlta.StartPosition = FormStartPosition.CenterScreen;
            dlta.TopLevel = true;
            dlta.Show();
            Application.DoEvents();
        }
        /// <summary>
        /// вывод заданного сообщения
        /// </summary>
        /// <param Name="frm"></param>
        /// <param Name="action_message"></param>
        public void Start( Form frm, string action_message )
        {
            outstr = action_message;

            Start( frm );
        }
        /// <summary>
        /// вывод заданного сообщения 
        /// с заданием заголовка окна
        /// </summary>
        /// <param Name="frm"></param>
        /// <param Name="action_message"></param>
        public void Start( Form frm, string action_message, string dlgCaption, string btnCaption )
        {

            captionstr = dlgCaption;
            outstr = action_message;
            this.btnCaption = btnCaption;

            Start( frm );
        }

        /// <summary>
        /// Установка (корректировка)
        /// текста в элементах диалогового окна
        /// </summary>
        /// <param name="captionDlgWinText">текст заголовка окна</param>
        /// <param name="preLabelText">текст метки предварит сообщения</param>
        /// <param name="explanationText">основное сообщение</param>
        /// <param name="postLabelText">текст метки сопроводит сообщения</param>
        /// <param name="btnText">Текст кнопки</param>
        public void SetHMIElementsText( string captionDlgWinText, string preLabelText, string explanationText, string postLabelText, string btnText )
        {
            if ( !string.IsNullOrWhiteSpace( captionDlgWinText ) )
                captionstr = captionDlgWinText;
            if ( !string.IsNullOrWhiteSpace( preLabelText ) )
                prestr = preLabelText;
            if ( !string.IsNullOrWhiteSpace( explanationText ) )
                outstr = explanationText;
            if ( !string.IsNullOrWhiteSpace( postLabelText ) )
                poststr = postLabelText;
            if ( !string.IsNullOrWhiteSpace( btnText ) )
                btnCaption = btnText;

            // устанавлимаем элементы интерфейса диалог окна
            dlta.DialogWindowCaption = captionstr;
            dlta.PreLabelStr = prestr;
            dlta.ExplanationStr = outstr;
            dlta.PostLabelStr = poststr;
            dlta.BtnCaption = btnCaption;
        }

        void dlta_OnBreakAction()
        {
            Stop();
        }
        public void Stop()
        {
            this.Dispose();
        }
        #region уничтожение объекта
        /// <summary>
        /// флаг для определения того, 
        /// вызывался ли метод Dispose()
        /// </summary>
        private bool isdisposed = false;

        public void Dispose()
        {
            // вызываем метод с кодом очистки
            DeleteThis( true );

            // подавляем финализацию
            GC.SuppressFinalize( this );
        }

        /// <summary>
        /// Код очистки
        /// </summary>
        /// <param Name="whois">true - означает, что очистку инициировало приложение, а не сборщик мусора</param>
        private void DeleteThis( bool whois )
        {
            // проверка на то, выполнялась ли очистка
            if ( !isdisposed )
            {
                // если whois == true, освобождаем все управляемые ресурсы
                if ( whois )
                {
                    // здесь осущ очистку всех управляемых ресурсов
                    // ...
                    if ( dlta != null )
                    {
                        dlta.Close(); ;
                    }
                }
                // здесь осущ очистку всех НЕуправляемых ресурсов
                // ...
            }
            isdisposed = true;
        }
        ~LongTimeAction()
        {
            // вызываем метод очистки, 
            // false - означает, что очистка была инициирована сборщиком мусора
            DeleteThis( false );
        }
        #endregion
    }
}
