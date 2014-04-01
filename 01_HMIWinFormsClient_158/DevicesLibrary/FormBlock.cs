using System;
using System.Windows.Forms;

using HelperControlsLibrary;
using InterfaceLibrary;

namespace DevicesLibrary
{
    /// <summary>
    /// Форма всех блоков
    /// </summary>
    public partial class FormBlock : Form, IDeviceForm, HelperControlsLibrary.ReportLibrary.IReport
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="typeBlock">Имя типа блока</param>
        /// <param name="unids">Идентификатор датасервера</param>
        /// <param name="unidev">Идентификатор устройства</param>
        public FormBlock( String typeBlock, UInt32 unids, UInt32 unidev )
        {
            InitializeComponent( );
            this.InitComponents( typeBlock, unids, unidev );
            this.Guid = unidev;
        }
        /// <summary>
        /// Инициализация вкладок
        /// </summary>
        /// <param name="typeBlock">Имя типа блока</param>
        /// <param name="unids">Идентификатор датасервера</param>
        /// <param name="unidev">Идентификатор устройства</param>
        private void InitComponents( String typeBlock, UInt32 unids, UInt32 unidev )
        {
            var device = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Device(unids, unidev);

            if (device.ShowGroupsAndTags)
                tabControl.Controls.Add(new BlockViewTagPage(this, unids, unidev) { Dock = DockStyle.Fill });

            if (device.ShowOscilograms && device.ShowDiagrams)
                tabControl.Controls.Add(new OscDiagTabPage(unidev));
            else if (device.ShowOscilograms)
                tabControl.Controls.Add(new OscDiagTabPage(unidev, OscDiagTabPage.OscDiagPanelVisible.Oscilograms));
            else if (device.ShowDiagrams)
                tabControl.Controls.Add(new OscDiagTabPage(unidev, OscDiagTabPage.OscDiagPanelVisible.Diagrams));

            if (device.ShowEvents)
                tabControl.Controls.Add(new EventBlockTabPage(unidev));

            if (device.ShowUserFiles)
                tabControl.Controls.Add(new DataBaseFilesLibrary.TabPageDbFile(unidev));

            if (device.ShowCommands)
                tabControl.Controls.Add(new HelperControlsLibrary.TeleMechanica.TeleMechanicaCommandTabPage(unids, unidev) { Dock = DockStyle.Fill });
        }

        #region IDeviceForm
        /// <summary>
        /// Активировать определенную вкладку
        /// </summary>
        public void ActivateTabPage( string typetabpage )
        {
           throw new NotImplementedException();
        }

        /// <summary>
        /// Активировать определенную группу устройства и показать ее значения
        /// </summary>
        public void ActivateAndShowTreeGroupWithCategory(Category groupCategory)
        {
            (tabControl.Controls[0] as BlockViewTagPage).ActiveAndShowTreeGroupWithCategory(groupCategory);
        }

        /// <summary>
        /// Действия по завершению чтения аварии 
        /// </summary>
        public void reqAvar_OnReqExecuted( IRequestData req )
        {
            /*throw new NotImplementedException();*/
        }

        /// <summary>
        /// Идентификатор блока/устройства
        /// </summary>
        public UInt32 Guid { get; private set; }
        #endregion

        /// <summary>
        /// Печать
        /// </summary>
        public void Print( ) { ( (BlockViewTagPage)tabControl.Controls[0] ).Print( ); }
    }
}