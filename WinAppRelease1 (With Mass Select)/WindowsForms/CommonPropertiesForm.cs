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
         {
             InitPage2();
             InitExternalProgramPage();
         }
         else
         {
             this.tabControl1.Controls.Remove(this.BindingTtabPage);
             this.tabControl1.Controls.Remove(this.externalProgramTabPage);
         }

          if (elem is Rotate)
            InitPage5();
         else
            this.tabControl1.Controls.Remove(this.RotationTabPage);

         if (elem is StaticElement)
            InitPage4();
         else
            this.tabControl1.Controls.Remove(this.ImageTabPage);

         if (elem is BlockText)
         {
             InitPage6();
             if (elem is SchemaButton) groupBox11.Text = "Ссылка:";
         }
         else
            this.tabControl1.Controls.Remove(this.BlockTextTabPage);

         if (elem is ICalculationContext)
            this.InitPage3();
         else
            this.tabControl1.Controls.Remove(this.FormulaBindingTabPage);
         
         if ( elem is IFormText )
            this.InitPage7();
         else
            this.tabControl1.Controls.Remove(this.TextTabPage);

          this.CancelButton = button1;
          this.AcceptButton = button2;
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
          this.dsGuidDeviceBindingNumericUpDown.Value = CheckValue(this.dsGuidDeviceBindingNumericUpDown, (int)tmp.DsGuid);
          this.cellDeviceBindingNumericUpDown.Value = CheckValue(this.cellDeviceBindingNumericUpDown, (int)tmp.Cell);
          this.devGuidDeviceBindingNumericUpDown.Value = CheckValue(this.devGuidDeviceBindingNumericUpDown, (int)tmp.DeviceGuid);
          this.checkBox2.Checked = tmp.ExternalDescription;

          this.dsGuidCommandBindingNumericUpDown.Value = CheckValue(this.dsGuidCommandBindingNumericUpDown, (int)tmp.DsGuidForCommandBinding);
          this.deviceGuidCommandBindingNumericUpDown.Value = CheckValue(this.deviceGuidCommandBindingNumericUpDown, (int)tmp.DeviceGuidForCommandBinding);
          this.commandGuidCommandBindingNumericUpDown.Value = CheckValue(this.commandGuidCommandBindingNumericUpDown, (int)tmp.CommandGuidForCommandBinding);
      }
      private void InitPage3()
      {
          var calc = SelectElement as ICalculationContext;
          var dynamicParam = SelectElement as IDynamicParameters;

          if (calc.CalculationContext != null)
          {
              if (calc.CalculationContext.IsDeviceFromDeviceBinding)
              {
                  StateFromBindingDeviceCheckBox.Checked = true;
                  StateGroupBox.Enabled = false;
              }
              else
              {
                  StateFromBindingDeviceCheckBox.Checked = false;
                  StateGroupBox.Enabled = true;

                  stateDSGuidNumericUpDown.Value = calc.CalculationContext.StateDSGuid;
                  stateDeviceGuidNumericUpDown.Value = calc.CalculationContext.StateDeviecGuid; 
              }
          }
          else
          {
              StateFromBindingDeviceCheckBox.Checked = true;
              StateGroupBox.Enabled = false;
          }

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

       private void InitExternalProgramPage()
       {
           var parameters = ((IDynamicParameters) SelectElement).Parameters;

           if (parameters.IsExecExternalProgram)
           {
               IsExecExternalProgramCheckBox.Checked = true;
               ExternalProgramParametersGroupBox.Enabled = true;
               PathToExternalProgramTextBox.Text = parameters.PathToExternalProgram;
           }
           else
           {
               IsExecExternalProgramCheckBox.Checked = false;
               ExternalProgramParametersGroupBox.Enabled = false;
               PathToExternalProgramTextBox.Text = String.Empty;
           }
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

              if (calc.CalculationContext != null)
              {
                  calc.CalculationContext.IsDeviceFromDeviceBinding = StateFromBindingDeviceCheckBox.Checked;
                  calc.CalculationContext.StateDSGuid = Convert.ToUInt32(this.stateDSGuidNumericUpDown.Value);
                  calc.CalculationContext.StateDeviecGuid = Convert.ToUInt32(this.stateDeviceGuidNumericUpDown.Value);
              }
          }
          
          var @params = SelectElement as IDynamicParameters;
          if ( @params != null )
          {
              @params.Parameters.ToolTipMessage = this.textBox1.Text;
              @params.Parameters.Type = this.textBox7.Text;
              @params.Parameters.DsGuid = Convert.ToUInt32( this.dsGuidDeviceBindingNumericUpDown.Value );
              @params.Parameters.Cell = Convert.ToUInt32( this.cellDeviceBindingNumericUpDown.Value );
              @params.Parameters.DeviceGuid = Convert.ToUInt32( this.devGuidDeviceBindingNumericUpDown.Value );
              @params.Parameters.ExternalDescription = this.checkBox2.Checked;

              @params.Parameters.DsGuidForCommandBinding = Convert.ToUInt32(this.dsGuidCommandBindingNumericUpDown.Value);
              @params.Parameters.DeviceGuidForCommandBinding = Convert.ToUInt32(this.deviceGuidCommandBindingNumericUpDown.Value);
              @params.Parameters.CommandGuidForCommandBinding = Convert.ToUInt32(this.commandGuidCommandBindingNumericUpDown.Value);

              @params.Parameters.IsExecExternalProgram = IsExecExternalProgramCheckBox.Checked;
              @params.Parameters.PathToExternalProgram = PathToExternalProgramTextBox.Text;
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

      #region Handlers
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
          var form = new ElementBehaviorForm( (ICalculationContext)this.SelectElement ) { Owner = this };
          if ( form.ShowDialog() == DialogResult.OK )
              newContext = form.GetNewCalculationContext();

          this.button2.Focus();
      }
      private void Button6Click( object sender, EventArgs e )
      {
          this.colorDialog1.ShowDialog();
      }
      private void Button8Click( object sender, EventArgs e )
      {
          this.fontDialog1.ShowDialog();
      }

      private void StateFromBindingDeviceCheckBox_CheckedChanged(object sender, EventArgs e)
      {
          var checkBox = (CheckBox)sender;

          if (checkBox.Checked)
          {
              StateGroupBox.Enabled = false;
              stateDSGuidNumericUpDown.Value = dsGuidDeviceBindingNumericUpDown.Value;
              stateDeviceGuidNumericUpDown.Value = devGuidDeviceBindingNumericUpDown.Value;
          }
          else
          {
              StateGroupBox.Enabled = true;
          }
      }

      private void dsGuidDeviceBindingNumericUpDown_ValueChanged(object sender, EventArgs e)
      {
          if (StateFromBindingDeviceCheckBox.Checked)
              stateDSGuidNumericUpDown.Value = dsGuidDeviceBindingNumericUpDown.Value;
      }

      private void devGuidDeviceBindingNumericUpDown_ValueChanged(object sender, EventArgs e)
      {
          if (StateFromBindingDeviceCheckBox.Checked)
              stateDeviceGuidNumericUpDown.Value = devGuidDeviceBindingNumericUpDown.Value;
      }
      #endregion

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

      private void IsExecExternalProgramCheckBox_CheckedChanged(object sender, EventArgs e)
      {
          if (IsExecExternalProgramCheckBox.Checked)
              ExternalProgramParametersGroupBox.Enabled = true;
          else
              ExternalProgramParametersGroupBox.Enabled = false;
      }
   }
}