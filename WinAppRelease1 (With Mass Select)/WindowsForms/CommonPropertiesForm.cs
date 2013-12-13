using System;
using System.Drawing;
using System.Windows.Forms;

using LibraryElements;
using LibraryElements.CalculationBlocks;
using LibraryElements.Sources;

using Structure;

namespace WindowsForms
{
   public partial class CommonPropertiesForm : Form
   {
      protected readonly Figure SelectElement;
      private String imgpath;
      private Image img;
      private CalculationContext newContext;

      public CommonPropertiesForm(object elem, int frameMaxX, int frameMaxY)
      {
         InitializeComponent();
         SelectElement = (Figure)elem;
         InitFileDialog();
         InitPage1(frameMaxX, frameMaxY);

         if (elem is IDynamicParameters)
            InitPage2();
         else
            this.tabControl1.Controls.Remove(this.tabPage2);

         if (elem is Rotate)
            InitPage5();
         else
            this.tabControl1.Controls.Remove(this.tabPage5);

         if (elem is StaticElement)
            InitPage4();
         else
            this.tabControl1.Controls.Remove(this.tabPage4);

         if (elem is BlockText)
         {
             InitPage6();
             if (elem is SchemaButton) groupBox11.Text = "Ссылка:";
         }
         else
            this.tabControl1.Controls.Remove(this.tabPage6);

         if (elem is ICalculationContext)
            this.InitPage3();
         else
            this.tabControl1.Controls.Remove(this.tabPage3);
         
         if ( elem is IFormText )
            this.InitPage7();
         else
            this.tabControl1.Controls.Remove(this.tabPage7);
      }
      private void InitFileDialog()
      {
         openFileDialog1.InitialDirectory = Environment.CurrentDirectory;
         openFileDialog1.RestoreDirectory = true;
         openFileDialog1.FileName = String.Empty;
         openFileDialog1.FilterIndex = 1;
      }
      private void InitImageFileDialog()
      {
         openFileDialog1.Filter = ProgrammExtensions.GetImageFilter() + ProgrammExtensions.GetAnyFilesFilter();
      }
      private void InitPage1(int frameMaxX, int frameMaxY)
      {
          this.numericUpDown1.Maximum = frameMaxX;
          this.numericUpDown2.Maximum = frameMaxY;
          this.numericUpDown3.Maximum = SelectElement.MaxSizeX;
          this.numericUpDown4.Maximum = SelectElement.MaxSizeY;
          this.numericUpDown3.Minimum = SelectElement.MinSizeX;
          this.numericUpDown4.Minimum = SelectElement.MinSizeY;

          this.numericUpDown1.Value = CheckValue( this.numericUpDown1, SelectElement.GetPosition().X );
          this.numericUpDown2.Value = CheckValue( this.numericUpDown2, SelectElement.GetPosition().Y );
          this.numericUpDown3.Value = CheckValue( this.numericUpDown3, SelectElement.GetPosition().Width );
          this.numericUpDown4.Value = CheckValue( this.numericUpDown4, SelectElement.GetPosition().Height );

          this.colorDialog1.Color = SelectElement.ElementColor;
      }
      private void InitPage2()
      {
          var tmp = ( (IDynamicParameters)SelectElement ).Parameters;
          this.textBox1.Text = tmp.ToolTipMessage;
          this.textBox7.Text = tmp.Type;
          this.numericUpDown5.Value = CheckValue(this.numericUpDown5, (int)tmp.DsGuid);
          this.numericUpDown7.Value = CheckValue(this.numericUpDown7, (int)tmp.Cell);
          this.numericUpDown8.Value = CheckValue(this.numericUpDown8, (int)tmp.DeviceGuid);
          this.checkBox2.Checked = tmp.ExternalDescription;
      }
      private void InitPage3()
      {
          var calc = SelectElement as ICalculationContext;
          if ( calc != null && calc.CalculationContext != null )
              label19.Text = "Загружено";
          else
              label19.Text = "Не Загружено";
      }
      private void InitPage4()
      {
          var tmp = (StaticElement)SelectElement;
          this.label16.Text = tmp.ExistImage() ? "Файл загружен" : "Файл не загружен";
      }
      private void InitPage5()
      {
          var rot = (Rotate)SelectElement;

          this.checkBox3.Checked = rot.Mirror;

          this.comboBox3.Items.Add( "Вверх" );
          this.comboBox3.Items.Add( "Вниз" );
          this.comboBox3.Items.Add( "Влево" );
          this.comboBox3.Items.Add( "Вправо" );

          switch ( rot.TurnPosition )
          {
              case DrawRotate.Up:
                  this.comboBox3.SelectedIndex = 0;
                  break;
              case DrawRotate.Down:
                  this.comboBox3.SelectedIndex = 1;
                  break;
              case DrawRotate.Left:
                  this.comboBox3.SelectedIndex = 2;
                  break;
              case DrawRotate.Right:
                  this.comboBox3.SelectedIndex = 3;
                  break;
              default:
                  this.comboBox3.SelectedIndex = 0;
                  break;
          }
      }
      private void InitPage6()
      {
          var blk = (BlockText)SelectElement;
          textBox5.Text = blk.Text;
          textBox6.Text = blk.Group;
      }
      private void InitPage7()
      {
          var orgtext = (IFormText)SelectElement;
          this.textBox4.Text = orgtext.Text;
          this.fontDialog1.Font = orgtext.TextFont;
          this.checkBox6.Checked = orgtext.VerticalView;
      }

      protected virtual void ToElement()
      {
         var x = Convert.ToInt32(numericUpDown1.Value);
         var y = Convert.ToInt32(numericUpDown2.Value);
         var w = Convert.ToInt32(numericUpDown3.Value);
         var h = Convert.ToInt32(numericUpDown4.Value);

         SelectElement.ResetMouseOffset();//сброс чтоб не считал поправку на мышь
         SelectElement.SetPosition( new Point( x, y ) );
         SelectElement.SetSize( new Size( w, h ) );
         SelectElement.ElementColor = this.colorDialog1.Color;

          var @static = SelectElement as StaticElement;
          if ( @static != null )
          {
              if ( img != null && !string.IsNullOrEmpty( imgpath ) )
              {
                  if ( !@static.ExistImage() )
                      @static.SetImage( new ImageData( img, WorkFile.GetWayPart( imgpath ) ) );
                  else
                  {
                      var res = MessageBox.Show( this, "Изображение загружено, заменить?", "Информация",
                                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question );
                      if ( res == DialogResult.Yes )
                          @static.SetImage( new ImageData( img, WorkFile.GetWayPart( imgpath ) ) );
                  }
              }
          }
          
          var calc = SelectElement as ICalculationContext;
          if ( calc != null )
          {
              if ( calc.CalculationContext != null && newContext != null )
              {
                  var res = MessageBox.Show( "Имеются присвоенные данные, заменить?", "Информация",
                                             MessageBoxButtons.YesNo, MessageBoxIcon.Question );
                  if ( res == DialogResult.Yes )
                  {
                      calc.CalculationContext = newContext;
                      MessageBox.Show( "Данные изменены", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information );
                  }
              }
              else if (calc.CalculationContext == null && newContext != null)
                  calc.CalculationContext = newContext;
          }
          
          var @params = SelectElement as IDynamicParameters;
          if ( @params != null )
          {
              @params.Parameters.ToolTipMessage = this.textBox1.Text;
              @params.Parameters.Type = this.textBox7.Text;
              @params.Parameters.DsGuid = Convert.ToUInt32( this.numericUpDown5.Value );
              @params.Parameters.Cell = Convert.ToUInt32( this.numericUpDown7.Value );
              @params.Parameters.DeviceGuid = Convert.ToUInt32( this.numericUpDown8.Value );
              @params.Parameters.ExternalDescription = this.checkBox2.Checked;
          }
          
          var rotate = SelectElement as Rotate;
          if ( rotate != null )
          {
              rotate.Mirror = this.checkBox3.Checked;
              switch ( this.comboBox3.SelectedIndex )
              {
                  case 0: rotate.TurnPosition = DrawRotate.Up; break;
                  case 1: rotate.TurnPosition = DrawRotate.Down; break;
                  case 2: rotate.TurnPosition = DrawRotate.Left; break;
                  case 3: rotate.TurnPosition = DrawRotate.Right; break;
              }
          }
          
          var blockText = SelectElement as BlockText;
          if ( blockText != null )
          {
             blockText.Text = textBox5.Text;
             blockText.Group = textBox6.Text;
          }
          
          var formText = SelectElement as IFormText;
          if ( formText != null )
          {
              formText.Text = this.textBox4.Text;
              formText.TextFont = this.fontDialog1.Font;
              formText.VerticalView = this.checkBox6.Checked;
          }
      }
      protected virtual void Button2Click(object sender, EventArgs e)
      {
         ToElement();
         Close();
      }
      private void Button1Click(object sender, EventArgs e)
      {
         Close();
      }
      private void Button3Click(object sender, EventArgs e)
      {
         InitImageFileDialog();
         if (openFileDialog1.ShowDialog() == DialogResult.OK)
             try
             {
                 img = WorkFile.ReadImageFile( openFileDialog1.FileName );
                 imgpath = openFileDialog1.FileName;
                 label22.Text = "Выбрано";
             }
             catch
             {
                 MessageBox.Show( this, "Не удалось загрузить файл", "Ошибка", MessageBoxButtons.OK,
                                  MessageBoxIcon.Error );
             }
      }
      private void Button4Click( object sender, EventArgs e )
      {
          var form = new Form8( (ICalculationContext)this.SelectElement ) { Owner = this };
          if ( form.ShowDialog() == DialogResult.OK )
              newContext = form.GetNewCalculationContext();
      }
      private void Button6Click( object sender, EventArgs e )
      {
          this.colorDialog1.ShowDialog();
      }
      private void Button8Click( object sender, EventArgs e )
      {
          this.fontDialog1.ShowDialog();
      }

      /// <summary>
      /// Проверка присваимого значения на выход за границы значений контрола
      /// </summary>
      /// <param name="numeric">Контрол чисел</param>
      /// <param name="value">Значение</param>
      /// <returns>Провереное значение</returns>
      internal static decimal CheckValue(NumericUpDown numeric, int value)
      {
          if ( value > numeric.Maximum )
              return numeric.Maximum;
          if ( value < numeric.Minimum )
              return numeric.Minimum;

          return value;
      }
   }
}