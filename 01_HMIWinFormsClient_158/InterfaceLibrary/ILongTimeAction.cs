using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace InterfaceLibrary
{
    public interface ILongTimeAction
    {
        /// <summary>
        /// вывод сообщения по умолчанию
        /// </summary>
        /// <param Name="frm"></param>
        void Start( Form frm );
        /// <summary>
        /// вывод заданного сообщения
        /// </summary>
        /// <param Name="frm"></param>
        /// <param Name="action_message"></param>
        void Start( Form frm, string action_message );
        /// <summary>
        /// вывод заданного сообщения 
        /// с заданием заголовка окна
        /// </summary>
        /// <param Name="frm"></param>
        /// <param Name="action_message"></param>
        void Start( Form frm, string action_message, string dlgCaption, string btnCaption );
        /// <summary>
        /// Установка (корректировка)
        /// текста в элементах диалогового окна
        /// </summary>
        /// <param name="captionDlgWinText">текст заголовка окна</param>
        /// <param name="preLabelText">текст метки предварит сообщения</param>
        /// <param name="explanationText">основное сообщение</param>
        /// <param name="postLabelText">текст метки сопроводит сообщения</param>
        /// <param name="btnText">Текст кнопки</param>
        void SetHMIElementsText( string captionDlgWinText, string preLabelText, string explanationText, string postLabelText, string btnText );
        void Stop();
        void Dispose();
    }
}
