using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NormalModeLibrary.Sources;

namespace NormalModeLibrary.ViewModel
{
    public class PanelViewModel : BaseCollectionViewModel
    {
        Panel panel;

        internal PanelViewModel( Panel panel )
        {
            this.panel = panel;
            Core = panel;

            foreach ( BaseSignal signal in panel.Collection )
            {
                BaseSignalViewModel signalModel = BaseSignalViewModel.GetSignalViewModel( signal );
                if ( signalModel != null )
                {
                    InterfaceLibrary.IDevice idevice = HMI_MT_Settings.HMI_Settings.CONFIGURATION.GetLink2Device( DsGuid, ObjectGuid );
                    if ( idevice != null )
                    {
                        InterfaceLibrary.ITag iTag = idevice.GetTag( signalModel.Guid );
                        if ( iTag != null )
                        {
                            signalModel.SetTag( iTag );
                            signalModel.IsChecked = true;
                            signalModel.Subscribe();
                        }
                    }
                    Collection.Add( signalModel );
                }
            }

            if (IsCaptionVisible)
            {
                CaptionViewModel captionViewModel = new CaptionViewModel(Caption);
                captionViewModel.FontSize = Collection.First().FontSize;
                Collection.Insert(0, captionViewModel);
            }
        }
        internal override ViewModelBase Copy()
        {
            Panel pnl = (Panel)panel.Copy();
            return new PanelViewModel( pnl );
        }

        public Panel BasePanel
        {
            get { return panel; }
            private set { panel = value; }
        }

        public String Caption
        {
            get { return panel.Caption; }
            set { panel.Caption = value; }
        }
        public Panel.LinkType Type
        {
            get { return panel.Type; }
            set { panel.Type = value; }
        }
        public UInt32 DsGuid
        {
            get { return panel.DsGuid; }
            set { panel.DsGuid = value; }
        }
        public UInt32 ObjectGuid
        {
            get { return panel.ObjectGuid; }
            set { panel.ObjectGuid = value; }
        }
        public Boolean IsVisible
        {
            get { return panel.IsVisible; }
            set { panel.IsVisible = value; }
        }
        public Boolean IsCaptionVisible
        {
            get { return panel.IsCaptionVisible; }
            set { panel.IsCaptionVisible = value; }
        }
        public Boolean IsAutomaticaly
        {
            get { return panel.IsAutomaticaly; }
            set { panel.IsAutomaticaly = value; }
        }
        public Int32 Left
        {
            get { return panel.Left; }
            set { panel.Left = value; }
        }
        public Int32 Top
        {
            get { return panel.Top; }
            set { panel.Top = value; }
        }
        public Int32 Width
        {
            get { return panel.Width; }
            set { panel.Width = value; }
        }
        public Int32 Height
        {
            get { return panel.Height; }
            set { panel.Height = value; }
        }

        public UInt16 FontSize
        {
            get
            {
                if (Collection == null || Collection.Count == 0)
                    return 12;

                return (Collection.First() as BaseSignalViewModel).FontSize;
            }
            set {
                foreach (var signal in Collection)
                {
                    var baseSignalViewModel = signal as BaseSignalViewModel;
                    baseSignalViewModel.FontSize = value;
                }
            }
        }
    }
}
