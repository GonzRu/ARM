using NormalModeLibrary.ViewModel;

namespace NormalModeLibrary.Windows
{
    interface INormalModePanel
    {
        /// <summary>
        /// Месторасположение панели
        /// </summary>
        Places Place { get; set; }

        /// <summary>
        /// Panel's data source
        /// </summary>
        PanelViewModel Component { get; set; }

        /// <summary>
        /// Activate panel
        /// </summary>
        void ActivatedComponent();

        /// <summary>
        /// Deactivate panel
        /// </summary>
        void DeactivatedComponent();

        /// <summary>
        /// Set on in Edit Mode
        /// </summary>
        void SetOnEditMode();

        /// <summary>
        /// Set off edit mode
        /// </summary>
        void SetOffEditMode();

        /// <summary>
        /// Update work mode (Always visible, Automaticaly, Never visible)
        /// </summary>
        void UpdateWorkMode();

        void ShowIfNeed();
    }
}
