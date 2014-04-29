using System.Windows.Forms;

namespace HelperControlsLibrary
{
    public class OscDiagTabPage : TabPage
    {
        /// <summary>
        /// Варианты отображаемых панелей
        /// </summary>
        public enum OscDiagPanelVisible
        {
            /// <summary>
            /// Осцилограммы и диаграммы
            /// </summary>
            TwoPanel,
            /// <summary>
            /// Осцилограммы
            /// </summary>
            Oscilograms,
            /// <summary>
            /// Диаграммы
            /// </summary>
            Diagrams
        }
        readonly OscDiagControl oscDiagControl = new OscDiagControl();

        /// <summary>
        /// Конструктор вкладки
        /// </summary>
        /// <param name="uniDevId">Guid устройства</param>
        /// <param name="panelVisible">Отображаемая панель</param>
        /// <param name="text">Имя вкладки</param>
        public OscDiagTabPage( uint uniDevId, OscDiagPanelVisible panelVisible = OscDiagPanelVisible.TwoPanel, string text = "Осциллограммы и диаграммы" ) : base( text )
        {
            if (panelVisible == OscDiagPanelVisible.Oscilograms)
                this.Text = "Осциллограммы";
            else if (panelVisible == OscDiagPanelVisible.Diagrams)
                this.Text = "Диаграммы";

            SetPanelVisible( panelVisible );
            oscDiagControl.UniDevId = uniDevId;
            oscDiagControl.Dock = DockStyle.Fill;
            Controls.Add( oscDiagControl );
        }
        /// <summary>
        /// Задание видимых панелей
        /// </summary>
        /// <param name="panelVisible">Отображаемая панель</param>
        public void SetPanelVisible( OscDiagPanelVisible panelVisible )
        {
            switch ( panelVisible )
            {
                case OscDiagPanelVisible.Oscilograms:
                    oscDiagControl.splitContainer_OscDiag.Panel2Collapsed = true;
                    break;
                case OscDiagPanelVisible.Diagrams:
                    oscDiagControl.splitContainer_OscDiag.Panel1Collapsed = true;
                    break;
            }
        }
    }
}
