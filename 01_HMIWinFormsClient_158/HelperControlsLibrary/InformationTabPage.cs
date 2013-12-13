using System.Windows.Forms;

namespace HelperControlsLibrary
{
    public class InformationTabPage : TabPage
    {
        readonly InformationControl infoControl = new InformationControl();

        /// <summary>
        /// Конструктор вкладки
        /// </summary>
        /// <param name="uniDevId">Guid устройства</param>
        /// <param name="text">Имя вкладки</param>
        public InformationTabPage( uint uniDevId, string text = "Информация" ) : base( text )
        {
            infoControl.UniDevId = uniDevId;
            infoControl.Dock = DockStyle.Fill;
            Controls.Add( infoControl );
        }
    }
}
