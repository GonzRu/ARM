using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using NormalModeLibrary.ViewModel;

namespace NormalModeLibrary.Windows
{
    /// <summary>
    /// Логика взаимодействия для TableControl
    /// </summary>
    public partial class TableControl : UserControl
    {
        private ViewWindow _view = null;

        public TableControl(ViewWindow view)
        {
            InitializeComponent();

            _view = view;
        }

        private void ListBox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var lb = (ListBox)sender;
            BaseSignalViewModel signalViewModel = lb.SelectedItem as BaseSignalViewModel;
            if (signalViewModel == null)
                return;

            var signalWindow = new SignalWindow();
            signalWindow.SetSignal(signalViewModel.BaseSignal);

            signalWindow.ShowDialog();
        }

        private void ListBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _view.ChildOnMouseDown(this, e);
        }
    }
}
