using System.Windows.Forms;

namespace HelperControlsLibrary
{
    public class EventBlockTabPage : TabPage
    {
        private readonly EventBlockControl ebControl;

        /// <summary>
        /// Конструктор вкладки
        /// </summary>
        /// <param name="uniDevId">Guid устройства</param>
        /// <param name="text">Имя вкладки</param>
        public EventBlockTabPage( uint uniDevId, string text = "События блока" ) : base( text )
        {
            ebControl = new EventBlockControl( (int)uniDevId ) { Dock = DockStyle.Fill };
            Controls.Add( ebControl );
        }
    }
}
