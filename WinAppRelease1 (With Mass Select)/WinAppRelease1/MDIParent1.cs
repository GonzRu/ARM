using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

using Structure;
using WindowsForms;

namespace WinAppRelease1
{
   public partial class MDIParent1 : Form
   {
      const string Savequest = "Сохраним изменения?";    
      int childFormNumber;       //Номер окна-потомка
      private BarsMenu.ToolBar tools;
      private BarsMenu.StatusBar statbar;

      public MDIParent1()
      {         
         InitializeComponent();
         InitElements();
         ActivateMenu();
      }
      /// <summary>
      /// Проверка на присудствие изменений на форме
      /// </summary>
      /// <param name="sender">object</param>
      private void ChangeMessage(object sender, FormClosingEventArgs e)
      {
          if (((MnemoSchemaForm)sender).ExistChange)
         {
            var res = MessageBox.Show(Savequest, "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
               this.SaveToolStripMenuItemClick(null, null);
            if (res == DialogResult.Cancel)
               e.Cancel = true;
         }
      }
      /// <summary>
      /// Инициализация элементов формы
      /// </summary>
      private void InitElements()
      {
         childFormNumber = 0;
         tools = new BarsMenu.ToolBar(toolStrip);
         statbar = new BarsMenu.StatusBar(statusStrip);
      }
      /// <summary>
      /// Инициализация файлового диалога
      /// </summary>
      /// <param name="filedialog">файловый диалог</param>
      private static void InitFileDialog(FileDialog filedialog)
      {
          filedialog.InitialDirectory = Properties.Settings.Default.FileDialogLastDirectory;
         filedialog.RestoreDirectory = true;
         filedialog.Filter = ProgrammExtensions.GetSchemaFilter() + ProgrammExtensions.GetAnyFilesFilter();
         filedialog.FilterIndex = 1;
      }
      /// <summary>
      /// Активация и дезактивация пунктов меню
      /// </summary>
      private void ActivateMenu()
      {
         //отключено
         exportToolStripMenuItem.Enabled = false;
         exportToolStripMenuItem.Visible = false;
      }
      /// <summary>
      /// Обнуление данных о кол-ве окон
      /// </summary>
      private void ZeroingofIndex()
      {
         if (MdiChildren.Length == 0)
            childFormNumber = 0;
      }
      /// <summary>
      /// Создание окна
      /// </summary>
      private void CreateNewWindow(object sender, EventArgs e)
      {
         ZeroingofIndex();

         var frm = new NewMnemoSchemaForm();
         if (frm.ShowDialog() == DialogResult.OK)
         {
             var childForm = new MnemoSchemaForm( tools, statbar ) { MdiParent = this }; // Create a new instance of the child form. Make it a child of this MDI form before showing it.
             childForm.FormClosing += this.ChildFormFormClosing;

            switch (frm.WinSanction)
            {
               case Sanction.Sanction_1024:
                  {
                     childForm.WorkSpaceSize = new Size(1024, 768);
                     childForm.MaximumSize = new Size(1024 + childForm.WidthShift, 768 + childForm.HeightShift);
                     childForm.ClientSize = childForm.MaximumSize;
                     childForm.Text = CreateTitleText(childForm.WorkSpaceSize, childFormNumber++);
                  }
                  break;
               case Sanction.Sanction_1280:
                  {
                     childForm.WorkSpaceSize = new Size(1280, 1024);
                     childForm.MaximumSize = new Size(1280 + childForm.WidthShift, 1024 + childForm.HeightShift);
                     childForm.ClientSize = childForm.MaximumSize;
                     childForm.Text = CreateTitleText(childForm.WorkSpaceSize, childFormNumber++);
                  }
                  break;
               case Sanction.Sanction_1600:
                  {
                     childForm.WorkSpaceSize = new Size(1600, 1200);
                     childForm.MaximumSize = new Size(1600 + childForm.WidthShift, 1200 + childForm.HeightShift);
                     childForm.ClientSize = childForm.MaximumSize;
                     childForm.Text = CreateTitleText(childForm.WorkSpaceSize, childFormNumber++);
                  }
                  break;
               case Sanction.Custom:
                  {
                     int w = frm.GetCustomSanctWidth(), h = frm.GetCustomSanctHeight();
                     childForm.WorkSpaceSize = new Size(w, h);
                     childForm.MaximumSize = new Size(w + childForm.WidthShift, h + childForm.HeightShift);
                     childForm.ClientSize = childForm.MaximumSize;
                     childForm.Text = CreateTitleText(childForm.WorkSpaceSize, childFormNumber++);
                  }
                  break;
               default:
                  {
                     childForm.WorkSpaceSize = new Size(640, 480);
                     childForm.MaximumSize = new Size(640 + childForm.WidthShift, 480 + childForm.HeightShift);
                     childForm.ClientSize = childForm.MaximumSize;
                     childForm.Text = "The Empty Blank #" + childFormNumber++;
                  } break;
            }        
            childForm.Show();
         }
         frm.Dispose();
      }
      /// <summary>
      /// Подготовка формы при создании
      /// </summary>
      private void OpenMethod(string strname)
      {
          var childForm = new MnemoSchemaForm( tools, statbar ) { MdiParent = this }; // Create a new instance of the child form. Make it a child of this MDI form before showing it.
          childForm.FormClosing += this.ChildFormFormClosing;

         childForm.OpenMethod(strname);

         childForm.Text = childForm.MnenoCaption != String.Empty ? CreateTitleText( childForm.WorkSpaceSize, childForm.MnenoCaption ) : CreateTitleText( childForm.WorkSpaceSize, this.childFormNumber++ );

         childForm.Show();
      }
      /// <summary>
      /// Создание строки размера окна
      /// </summary>
      /// <param name="sz">Размер окна</param>
      private static String CreateTitleSize(Size sz)
      {                                   
         return "[" + sz.Width.ToString( CultureInfo.InvariantCulture ) + "x" + sz.Height.ToString( CultureInfo.InvariantCulture ) + "]";
      }
      /// <summary>
      /// Создание строки зоголовка окна
      /// </summary>
      /// <param name="sz">Размер окна</param>
      /// <param name="number">Порядковый номер окна</param>
      private static String CreateTitleText(Size sz, int number)
      {
         return "Schema #" + number + " " + CreateTitleSize(sz);
      }
      /// <summary>
      /// Создание строки зоголовка окна
      /// </summary>
      /// <param name="sz">Размер окна</param>
      /// <param name="caption">Текст с описанием</param>
      private static String CreateTitleText(Size sz, String caption)
      {
         return caption + " " + CreateTitleSize(sz);
      }

      /// <summary>
      /// Обработка события при закрытии чилд-формы
      /// </summary>
      private void ChildFormFormClosing(object sender, FormClosingEventArgs e)
      {
         ChangeMessage(sender, e);
      }
      private void OpenFile(object sender, EventArgs e)
      {
         var openFileDialog = new OpenFileDialog();
         InitFileDialog(openFileDialog);

         if (openFileDialog.ShowDialog(this) == DialogResult.OK)
         {
            var fname = openFileDialog.FileName;
            Refresh();

            OpenMethod(fname);
         }

          Properties.Settings.Default.FileDialogLastDirectory = openFileDialog.InitialDirectory;

         openFileDialog.Dispose();
      }
      private void SaveAsToolStripMenuItemClick(object sender, EventArgs e)
      {
         if (MdiChildren.Length != 0)
         {
            var saveFileDialog = new SaveFileDialog();
            InitFileDialog(saveFileDialog);

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
               var fname = saveFileDialog.FileName;               
               Refresh();

               var frm = (MnemoSchemaForm)ActiveMdiChild;
               frm.SaveMethod(fname);
            }

            Properties.Settings.Default.FileDialogLastDirectory = saveFileDialog.InitialDirectory;

            saveFileDialog.Dispose();
         }
      }
      private void SaveToolStripMenuItemClick(object sender, EventArgs e)
      {
         if (MdiChildren.Length != 0)
         {
            var frm = (MnemoSchemaForm)ActiveMdiChild;
            if (frm.FileNameExist)
               frm.SaveMethod();
            else
               this.SaveAsToolStripMenuItemClick(sender, e);
         }
      }      
      private void ExportToolStripMenuItemClick(object sender, EventArgs e)
      {
      }
      private void CloseToolStripMenuItemClick(object sender, EventArgs e)
      {
         if (MdiChildren.Length != 0)
         {
            var frm = (MnemoSchemaForm)ActiveMdiChild;
            frm.Close();
         }
         ZeroingofIndex();
      }      
      private void ExitToolsStripMenuItemClick(object sender, EventArgs e)
      {
         Application.Exit();
      }
      private void CutToolStripMenuItemClick(object sender, EventArgs e)
      {
         if (MdiChildren.Length != 0)
         {
            var frm = (MnemoSchemaForm)ActiveMdiChild;
            frm.cutToolStripMenuItem_Click( sender, e );
         }
      }
      private void CopyToolStripMenuItemClick(object sender, EventArgs e)
      {
         if (MdiChildren.Length != 0)
         {
            var frm = (MnemoSchemaForm)ActiveMdiChild;
            frm.copyToolStripMenuItem_Click( sender, e );
         }
      }
      private void PasteToolStripMenuItemClick(object sender, EventArgs e)
      {
         if (MdiChildren.Length != 0)
         {
            var frm = (MnemoSchemaForm)ActiveMdiChild;
            frm.pasteToolStripMenuItem_Click( sender, e );
         }
      }
      private void DeleteToolStripMenuItemClick(object sender, EventArgs e)
      {
         if (MdiChildren.Length != 0)
         {
            var frm = (MnemoSchemaForm)ActiveMdiChild;
            frm.deleteToolStripMenuItem_Click( sender, e );
         }
      }      
      private void SelectAllToolStripMenuItemClick(object sender, EventArgs e)
      {
         if (MdiChildren.Length != 0)
         {
            var frm = (MnemoSchemaForm)ActiveMdiChild;
            frm.SelectedAll();
         }
      }      
      private void CascadeToolStripMenuItemClick(object sender, EventArgs e)
      {
         LayoutMdi(MdiLayout.Cascade);
      }
      private void TileVerticleToolStripMenuItemClick(object sender, EventArgs e)
      {
         LayoutMdi(MdiLayout.TileVertical);
      }
      private void TileHorizontalToolStripMenuItemClick(object sender, EventArgs e)
      {
         LayoutMdi(MdiLayout.TileHorizontal);
      }
      private void CloseAllToolStripMenuItemClick(object sender, EventArgs e)
      {
         foreach (MnemoSchemaForm childForm in MdiChildren)
         {
            childForm.Close();
         }
         ZeroingofIndex();
      }
      private void AboutToolStripMenuItemClick(object sender, EventArgs e)
      {
         var frm = new AboutBox1();
         frm.ShowDialog();
      }
      private void ElementEditorToolStripMenuItemClick(object sender, EventArgs e)
      {
          var form = new ElementBehaviorForm { Owner = this };
          form.Show( this );
      }      
      private void BuildToolStripMenuItemClick(object sender, EventArgs e)
      {
         if (MdiChildren.Length != 0)
         {
            var saveFileDialog = new SaveFileDialog();
            InitFileDialog(saveFileDialog);

            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
               var frm = (MnemoSchemaForm)ActiveMdiChild;
               frm.BuildMethod(saveFileDialog.FileName);
            }

            saveFileDialog.Dispose();
         }
      }
      private void MnenoCaptionToolStripMenuItemClick(object sender, EventArgs e)
      {
         if (MdiChildren.Length != 0)
         {
            var frm = (MnemoSchemaForm)ActiveMdiChild;
             var cap_frm = new EditMnemoCaptionForm { Owner = this };

             if (frm.MnenoCaption != String.Empty)
               cap_frm.MnenoCaption = frm.MnenoCaption;

            if (cap_frm.ShowDialog() == DialogResult.OK)
            {
               frm.MnenoCaption = cap_frm.MnenoCaption;
               frm.Text = CreateTitleText(frm.WorkSpaceSize, cap_frm.MnenoCaption);
               cap_frm.Close();
            }
            cap_frm.Dispose();
         }
      }
      private void PreviewToolStripMenuItemClick(object sender, EventArgs e)
      {
         if (MdiChildren.Length != 0)
         {
            var frm = (MnemoSchemaForm)ActiveMdiChild;
            frm.PreviewMethod();
         }
      }

      private void Menu31ToolStripMenuItemClick(object sender, EventArgs e)
      {
         if (MdiChildren.Length != 0)
         {
            var frm = (MnemoSchemaForm)ActiveMdiChild;
            frm.Convertto1024();
         }
      }
      private void Menu32ToolStripMenuItemClick(object sender, EventArgs e)
      {
         if (MdiChildren.Length != 0)
         {
            var frm = (MnemoSchemaForm)ActiveMdiChild;
            frm.Convertto1280();
         }
      }
      private void WitchBackgroundToolStripMenuItemClick( object sender, EventArgs e )
      {
          if ( MdiChildren.Length != 0 )
          {
              var frm = (MnemoSchemaForm)ActiveMdiChild;
              frm.SaveSubStrateMethod(
                  string.Format( "{0}\\SubStrate_{1}.bmp", Environment.CurrentDirectory, frm.MnenoCaption ),
                  Color.FromArgb( 236, 233, 216 ) );
          }
      }
      private void WithoutBackgroundToolStripMenuItemClick( object sender, EventArgs e )
      {
          if ( MdiChildren.Length != 0 )
          {
              var frm = (MnemoSchemaForm)ActiveMdiChild;
              frm.SaveSubStrateMethod( string.Format( "{0}\\SubStrate_{1}.bmp", Environment.CurrentDirectory, frm.MnenoCaption ) );
          }
      }

      private void SaveToolStripButtonClick(object sender, EventArgs e)
      {
         if (MdiChildren.Length != 0)
         {
            var frm = (MnemoSchemaForm)ActiveMdiChild;
            if (frm.FileNameExist)
               frm.SaveMethod();
            else
               this.SaveAsToolStripMenuItemClick(sender, e);
         }
      }

      private void ConvertSchemaToolStripMenuItemClick( object sender, EventArgs e )
      {
          FileDialog dialog = new OpenFileDialog( );
          InitFileDialog( dialog );

          if ( dialog.ShowDialog( ) != DialogResult.OK ) return;

          using ( var convert = new FileManager.ElementConverter( ) )
          {
              convert.LoadFile( dialog.FileName );
              if ( convert.Error_Status ) return;
              convert.ConvertSchema( );

              if ( !convert.IsConvert )
              {
                  MessageBox.Show( "Данные схемы не сконвертированы" );
                  return;
              }

              dialog = new SaveFileDialog( );
              InitFileDialog( dialog );

              if ( dialog.ShowDialog() != DialogResult.OK ) return;

              convert.SaveFile( dialog.FileName );
          }
      }
      private void ConvertBehavoirToolStripMenuItemClick( object sender, EventArgs e )
      {
          FileDialog dialog = new OpenFileDialog( );
          InitFileDialog( dialog );

          if ( dialog.ShowDialog( ) != DialogResult.OK ) return;

          using ( var convert = new FileManager.ElementConverter( ) )
          {
              convert.LoadFile( dialog.FileName );
              if ( convert.Error_Status ) return;
              convert.ConvertBehavoir();

              if ( !convert.IsConvert )
              {
                  MessageBox.Show( "Данные не сконвертированы" );
                  return;
              }

              dialog = new SaveFileDialog( );
              InitFileDialog( dialog );

              if ( dialog.ShowDialog() != DialogResult.OK ) return;

              convert.SaveFile( dialog.FileName );
          }
      }
   }
}