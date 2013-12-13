using System.Windows.Forms;

namespace DeviceFormLib.BlockTabs
{
    class OscDiagTabPage : TabPage
    {
        /// <summary>
        /// Варианты отображаемых панелей
        /// </summary>
        internal enum OscDiagPanelVisible
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
        internal OscDiagTabPage( uint uniDevId, OscDiagPanelVisible panelVisible = OscDiagPanelVisible.TwoPanel, string text = "Осцилограммы и диаграммы" ) : base( text )
        {
            this.SetPanelVisible( panelVisible );
            oscDiagControl.UniDevId = uniDevId;
            oscDiagControl.Dock = DockStyle.Fill;
            Controls.Add( oscDiagControl );
        }
        /// <summary>
        /// Задание видимых панелей
        /// </summary>
        /// <param name="panelVisible">Отображаемая панель</param>
        internal void SetPanelVisible( OscDiagPanelVisible panelVisible )
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
