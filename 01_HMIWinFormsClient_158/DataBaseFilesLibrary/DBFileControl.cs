using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DataBaseFilesLibrary
{
    public partial class DbFileControl : UserControl
    {
        private readonly Label textLabel = new Label();
        private readonly SqlTransaction transation = new SqlTransaction();
        private FormRecovery formRecovery;
        internal int UniDevId;

        internal DbFileControl()
        {
            InitializeComponent();
            transation.SqlTransactionEvent += this.TransationSqlTransactionEvent;
        }
        /// <summary>
        /// Чтение данных в таблицу
        /// </summary>
        /// <returns>Хранилище данных</returns>
        private DataSet ReadFile()
        {
            var ds = new DataSet();
            try
            {
                InitInfoLabel( "Чтение данных...", textLabel, Size );
                Controls.Add( textLabel );
                textLabel.BringToFront();

                var commands = ( from DataGridViewRow row in this.dataGridView1.SelectedRows
                                 select SqlTransaction.ReadFile( (int)row.Cells[0].Value ) ).ToList();

                transation.ExecuteOperation( commands, ds, SqlTransaction.ActionSqlTransaction.Read );
            }
            catch ( Exception ex )
            {
                AddStringToListBox( listBox1, string.Format( "Ошибка: {0}", ex.Message ) );
                // MessageBox.Show( ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
            finally
            {
                Controls.Remove( textLabel );
            }

            return ds;
        }
        /// <summary>
        /// Чтение доступных данных из базы данных
        /// </summary>
        internal void ReadDatas()
        {
            try
            {
                InitInfoLabel( "Загрузка данных...", textLabel, Size );
                Controls.Add( textLabel );
                textLabel.BringToFront();

                dataSet.Tables[0].Clear();
                transation.ExecuteOperation( SqlTransaction.GetDataList( this.UniDevId, false ), dataSet,
                                             SqlTransaction.ActionSqlTransaction.List );
                
                dataGridView1.ClearSelection();
            }
            catch ( Exception ex )
            {
                AddStringToListBox( listBox1, string.Format( "Ошибка: {0}", ex.Message ) );
                // MessageBox.Show( ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
            finally
            {
                Controls.Remove( textLabel );
            }
        }
        /// <summary>
        /// Открыть
        /// </summary>
        private void DataGridView1CellDoubleClick( object sender, DataGridViewCellEventArgs e )
        {
            var grid = (DataGridView)sender;
            if ( grid.SelectedRows.Count == 0 || e.ColumnIndex < 0 || e.RowIndex < 0 )
                return;
            
            var ds = new DataSet();
            try
            {
                InitInfoLabel( "Чтение данных...", textLabel, Size );
                Controls.Add( textLabel );
                textLabel.BringToFront();

                transation.ExecuteOperation( SqlTransaction.ReadFile( (int)grid[0, e.RowIndex].Value ), ds, SqlTransaction.ActionSqlTransaction.Read );

                foreach ( DataRow row in ds.Tables[0].Rows )
                    OpenFile( row );
            }
            catch ( Exception ex )
            {
                AddStringToListBox( listBox1, string.Format( "Ошибка: {0}", ex.Message ) );
                //MessageBox.Show( ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
            finally
            {
                ds.Dispose();
                Controls.Remove( textLabel );
            }
        }
        /// <summary>
        /// Открыть
        /// </summary>
        private void Button1Click( object sender, EventArgs e )
        {
            if ( dataGridView1.SelectedRows.Count == 0 )
                return;

            var ds = ReadFile();
            foreach ( DataRow row in ds.Tables[0].Rows )
                OpenFile( row );
        }
        /// <summary>
        /// Сохранить как
        /// </summary>
        private void Button2Click( object sender, EventArgs e )
        {
            if ( dataGridView1.SelectedRows.Count == 0 )
                return;

            using ( var ds = ReadFile() )
            {
                var saveDialog = new SaveFileDialog
                    {
                        Filter = "Любые файлы|*.*",
                        InitialDirectory = Environment.CurrentDirectory,
                        RestoreDirectory = true
                    };

                foreach ( DataRow row in ds.Tables[0].Rows )
                    try
                    {
                        var file = CreateFile( row );
                        saveDialog.FileName = file.Name;
                        if ( saveDialog.ShowDialog() == DialogResult.OK )
                            System.IO.File.Copy( file.Name, saveDialog.FileName, true );

                        AddStringToListBox( listBox1,
                                            string.Format( "Файл {0} успешно скопирован в {1}",
                                                           System.IO.Path.GetFileName( file.Name ), saveDialog.FileName ) );
                    }
                    catch
                    {
                        AddStringToListBox( listBox1,
                                            string.Format( "Файл {0} не был скопирован в указанное место",
                                                           System.IO.Path.GetFileName( saveDialog.FileName ) ) );
                        throw;
                    }
            }
        }
        /// <summary>
        /// Добавить файл
        /// </summary>
        private void Button3Click( object sender, EventArgs e )
        {
            var openDialog = new OpenFileDialog
                {
                    Filter = "Любые файлы|*.*",
                    InitialDirectory = Environment.CurrentDirectory,
                    Multiselect = true
                };

            if ( openDialog.ShowDialog() != DialogResult.OK )
                return;

            var commands = new List<SqlCommand>();
            foreach ( var name in openDialog.FileNames )
            {
                var form = new AddCommentForm { label2 = { Text = System.IO.Path.GetFileName( name ) } };
                form.ShowDialog();

                try
                {
                    using ( var file = new System.IO.FileStream( name, System.IO.FileMode.Open ) )
                    {
                        var bytes = new byte[file.Length];
                        file.Read( bytes, 0, bytes.Length );
                        commands.Add( SqlTransaction.AddFile( this.UniDevId, HMI_MT_Settings.HMI_Settings.UserID,
                                                              form.richTextBox1.Text, form.label2.Text, bytes ) );
                    }
                }
                catch
                {
                    //AddStringToListBox( listBox1, string.Format( "Ошибка доступа к файлу {0} Файл будет пропущен.", name ) );
                     MessageBox.Show( string.Format( "Ошибка доступа к файлу {0}\nФайл будет пропущен.", name ), "Ошибка",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error );
                }
            }

            try
            {
                InitInfoLabel( "Отправка данных серверу...", this.textLabel, Size );
                Controls.Add( this.textLabel );
                this.textLabel.BringToFront();

                this.transation.ExecuteOperation( commands, SqlTransaction.ActionSqlTransaction.Add );
            }
            catch ( Exception ex )
            {
                AddStringToListBox( listBox1, string.Format( "Ошибка: {0}", ex.Message ) );
                // MessageBox.Show( ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }
            finally
            {
                Controls.Remove( this.textLabel );
            }
            
            this.ReadDatas();
        }
        /// <summary>
        /// Удалить файл
        /// </summary>
        private void Button4Click( object sender, EventArgs e )
        {
            if ( dataGridView1.SelectedRows.Count == 0 )
                return;

            if ( MessageBox.Show( "Удалить выбранные данные?", "Удаление", MessageBoxButtons.YesNo,
                                 MessageBoxIcon.Question ) != DialogResult.Yes )
                return;

            var commands = new List<SqlCommand>();
            foreach ( DataGridViewRow row in this.dataGridView1.SelectedRows )
            {
                var form = new AddCommentForm
                    { label2 = { Text = System.IO.Path.GetFileName( row.Cells[3].Value.ToString() ) } };
                form.ShowDialog();

                commands.Add( SqlTransaction.DeleteFile( (int)row.Cells[0].Value, HMI_MT_Settings.HMI_Settings.UserID, form.richTextBox1.Text ) );
            }

            try
            {
                InitInfoLabel( "Удаление данных...", this.textLabel, Size );
                Controls.Add( this.textLabel );
                this.textLabel.BringToFront();

                this.transation.ExecuteOperation( commands, SqlTransaction.ActionSqlTransaction.Delete );
            }
            catch ( Exception ex )
            {
                AddStringToListBox( listBox1, string.Format( "Ошибка: {0}", ex.Message ) );
                // MessageBox.Show( ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }
            finally
            {
                Controls.Remove( this.textLabel );
            }

            this.ReadDatas();
        }
        /// <summary>
        /// Удаленные файлы
        /// </summary>
        private void Button5Click( object sender, EventArgs e )
        {
            if ( formRecovery == null || formRecovery.IsDisposed )
            {
                formRecovery = new FormRecovery { UniDevId = this.UniDevId };
                formRecovery.FormClosing += this.FormFormClosing;
                formRecovery.Show();
            }
            else
                formRecovery.Focus();
        }
        /// <summary>
        /// Закрытие окна Удаленные файлы
        /// </summary>
        private void FormFormClosing( object sender, FormClosingEventArgs e )
        {
            ReadDatas();
        }
        /// <summary>
        /// Событие срабатывания выполнения sql комманд
        /// </summary>
        private void TransationSqlTransactionEvent( object sender, EventArgs e ) { ParseActionSqlTransaction( (SqlTransactionEvenArgs)e, listBox1 ); }
        /// <summary>
        /// Действие по изменению выделенных пунктов таблицы
        /// </summary>
        private void DataGridView1SelectionChanged( object sender, EventArgs e )
        {
            if ( dataGridView1.SelectedRows.Count == 0 )
                button1.Enabled = button2.Enabled = button4.Enabled = false;
            else button1.Enabled = button2.Enabled = button4.Enabled = true;
        }

        /// <summary>
        /// Инициализация уведомления
        /// </summary>
        /// <param name="text">Текст уведомления</param>
        /// <param name="label">Объект текста</param>
        /// <param name="winSize">Размеры окна</param>
        internal static void InitInfoLabel( string text, Label label, Size winSize )
        {
            label.AutoSize = true;
            label.Text = text;
            label.Font = new Font( "Times New Roman", 20 );
            label.ForeColor = Color.Red;
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Left = winSize.Width / 2 - label.Width / 2;
            label.Top = winSize.Height / 2 - label.Height / 2;
        }
        /// <summary>
        /// Разбор данных события выполнения SQL транзакции
        /// </summary>
        /// <param name="sqlArgs"></param>
        /// <param name="listBox"></param>
        internal static void ParseActionSqlTransaction( SqlTransactionEvenArgs sqlArgs, ListBox listBox )
        {
            if ( sqlArgs == null )
                return;

            switch ( sqlArgs.Action )
            {
                case SqlTransaction.ActionSqlTransaction.List:
                    AddStringToListBox( listBox, "Данные считаны" );
                    break;
                case SqlTransaction.ActionSqlTransaction.Add:
                    AddStringToListBox( listBox,
                                        sqlArgs.Result != 0
                                            ? "Файл успешно добавлен"
                                            : "Файл не был добавлен. Обратитесь к системному администратору SQL." );
                    break;
                case SqlTransaction.ActionSqlTransaction.Delete:
                    AddStringToListBox( listBox,
                                        sqlArgs.Result == 1
                                            ? "Файл успешно удален"
                                            : "Файл не был удален. Обратитесь к системному администратору SQL." );
                    break;
                case SqlTransaction.ActionSqlTransaction.Recovery:
                    AddStringToListBox( listBox,
                                        sqlArgs.Result == 1
                                            ? "Файл успешно восстановлен"
                                            : "Файл не был восстановлен. Обратитесь к системному администратору SQL." );
                    break;
                case SqlTransaction.ActionSqlTransaction.Read:
                    AddStringToListBox( listBox, "Данные файла прочитаны из хранилища" );
                    break;
            }
        }
        /// <summary>
        /// Инициализация облости уведомлений
        /// </summary>
        private static void AddStringToListBox( ListBox listBox, string text )
        {
            if ( listBox.Items.Count > 100 )
                listBox.Items.Clear();

            if ( listBox.Items.Count == 0 )
                listBox.Items.Add( "Информационная строка:" );

            listBox.Items.Add( text );
        }
        /// <summary>
        /// Создание файла во временной дирректории
        /// </summary>
        /// <param name="row">Строка таблицы</param>
        /// <returns>Созданный файл</returns>
        private static System.IO.FileStream CreateFile( DataRow row )
        {
            var temp = System.IO.Path.GetTempPath();
            temp += row[0].ToString();
            temp = temp.Trim();

            using ( var file = System.IO.File.Create( temp ) )
            {
                var bytes = (byte[])row[1];
                file.Write( bytes, 0, bytes.Length );

                return file;
            }
        }
        /// <summary>
        /// Запуск файла
        /// </summary>
        /// <param name="row">Строка таблицы</param>
        private static void OpenFile( DataRow row )
        {
            try
            {
                var file = CreateFile( row );
                System.Diagnostics.Process.Start( file.Name );
            }
            catch ( Exception ex )
            {
                TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG( ex );
                MessageBox.Show( string.Format( "Не удалось запустить файл: {0}", row[0] ), "Ошибка запуска файла",
                                 MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
        }
    }
}
