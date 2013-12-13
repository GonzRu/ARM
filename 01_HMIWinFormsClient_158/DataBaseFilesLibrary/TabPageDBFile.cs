using System;
using System.Windows.Forms;

namespace DataBaseFilesLibrary
{
    public class TabPageDbFile : TabPage
    {
        readonly DbFileControl dbFileControl = new DbFileControl();

        /// <summary>
        /// Конструктор вкладки
        /// </summary>
        /// <param name="uniDevId">Guid устройства</param>
        /// <param name="text">Имя вкладки</param>
        public TabPageDbFile( uint uniDevId, string text = "Файлы пользователя" ) : base( text )
        {
            Enter += this.TabPageDbFileEnter;
            dbFileControl.UniDevId = (int)uniDevId;
            dbFileControl.Dock = DockStyle.Fill;
            Controls.Add( dbFileControl );
        }
        /// <summary>
        /// Вход на вкладку
        /// </summary>
        private void TabPageDbFileEnter( object sender, EventArgs e )
        {
            dbFileControl.ReadDatas();
        }
    }
}
