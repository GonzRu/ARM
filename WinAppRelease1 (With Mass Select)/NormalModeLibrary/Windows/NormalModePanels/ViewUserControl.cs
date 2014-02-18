using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NormalModeLibrary.ViewModel;

namespace NormalModeLibrary.Windows
{
    public class ViewUserControl : ListView, INormalModePanel
    {
        #region private
        private PanelViewModel _panelViewModel;
        #endregion

        #region Properties
        public Places Place { get; set; }

        public PanelViewModel Component
        {
            get { return _panelViewModel; }
            set
            {
                if (_panelViewModel != null)
                {
                    _panelViewModel.Collection.CollectionChanged -= CollectionOnCollectionChanged;
                    Items.Clear();
                }

                _panelViewModel = value;
                _panelViewModel.Collection.CollectionChanged += CollectionOnCollectionChanged;
                ActivateListView();
            }
        }

        #endregion

        #region Constructor
        public ViewUserControl()
        {
            this.MultiSelect = false;
        }
        #endregion

        #region Public metods
        public void ActivatedComponent()
        {
            if (Items.Count != 0)
            {
                Items.Clear();
            }

            if (Component != null)
            {
                ActivateListView();

                this.Text = Component.Caption;

                this.Top = Component.Top;
                this.Left = Component.Left;
                this.Width = Component.Width;
                this.Height = Component.Height;

                if (Component.IsAutomaticaly)
                    Visible = false;

                Component.Collection.CollectionChanged += CollectionOnCollectionChanged;
            }
        }

        private void CollectionOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            switch (notifyCollectionChangedEventArgs.Action)
            {
                case NotifyCollectionChangedAction.Remove:
                    foreach (ViewModelBase viewModel in notifyCollectionChangedEventArgs.OldItems)
                        foreach (ListViewItem item in Items)
                            if (item.Tag == viewModel)
                                Items.Remove(item);
                    break;
                case NotifyCollectionChangedAction.Add:
                    foreach (ViewModelBase viewModel in notifyCollectionChangedEventArgs.NewItems)
                        if (viewModel is CaptionViewModel)
                            Items.Insert(0, CreateListViewItem(viewModel));
                        else
                            Items.Add(CreateListViewItem(viewModel));
                    break;
            }
        }

        public void DeactivatedComponent()
        {
            Parent.Controls[0].Controls.Remove(this);
        }

        public void SetOnEditMode()
        {
        }

        public void SetOffEditMode()
        {
        }

        public void UpdateWorkMode()
        {
            if (Component.IsVisible)
            {
                if (Component.IsAutomaticaly)
                    Hide();
            }
            else
                Hide();
        }

        public void ShowIfNeed()
        {
            if (Component.IsVisible)
                if (!Component.IsAutomaticaly)
                {
                    Visible = true;
                }
        }
        #endregion

        #region Private Metods
        private void ActivateListView()
        {
            View = View.List;

            foreach (var baseView in Component.Collection)
                Items.Add(CreateListViewItem(baseView));
        }

        private ListViewItem CreateListViewItem(ViewModelBase modelBase)
        {
            ListViewItem item = new ListViewItem();

            SetListViewItem(item, modelBase);
            item.Tag = modelBase;
            modelBase.PropertyChanged += ListViewItemOnPropertyChanged;

            return item;
        }

        private void SetListViewItem(ListViewItem item, ViewModelBase modelBase)
        {
            var analogViewModel = modelBase as AnalogViewModel;
            if (analogViewModel != null)
            {
                item.Text = String.Format("{0} {1} {2}", analogViewModel.Caption, analogViewModel.Value, analogViewModel.Dim);
                item.Font = new Font(new FontFamily(GenericFontFamilies.SansSerif), (float)analogViewModel.FontSize);
                if (analogViewModel.IsOutOfRange)
                    item.ForeColor = Color.Red;
                else
                    item.ForeColor = Color.Black;

                return;
            }

            var discretViewModel = modelBase as DigitalViewModel;
            if (discretViewModel != null)
            {
                item.Text = discretViewModel.Caption;

                item.Font = new Font(new FontFamily(GenericFontFamilies.SansSerif), (float)discretViewModel.FontSize);

                return;
            }

            var captionViewModel = modelBase as CaptionViewModel;
            if (captionViewModel != null)
            {
                item.Text = captionViewModel.CaptionText;
                item.Font = new Font(new FontFamily(GenericFontFamilies.SansSerif), (float)captionViewModel.FontSize, FontStyle.Bold);
                item.ForeColor = Color.Blue;

                return;
            }
        }

        private void ListViewItemOnPropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            foreach (ListViewItem item in Items)
                if (item.Tag == sender)
                {
                    SetListViewItem(item, (ViewModelBase)sender);
                    return;
                }
        }
        #endregion
    }
}
