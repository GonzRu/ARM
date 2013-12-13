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
using System.Windows.Shapes;

namespace WpfDashboards
{
   /// <summary>
   /// Логика взаимодействия для TestWindow.xaml
   /// </summary>
   public partial class TestWindow : Window
   {
      public TestWindow()
      {
         InitializeComponent();
      }
      private void Window_Loaded(object sender, RoutedEventArgs e)
      {
         BaseFace.CreateBlock("ЭКРА-БЭ-2704v012_056");
      }

   }
}
