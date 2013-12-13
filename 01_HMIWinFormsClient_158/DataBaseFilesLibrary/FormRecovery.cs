using System;
using System.Windows.Forms;

namespace DataBaseFilesLibrary
{
    public partial class FormRecovery : Form
    {
        private readonly Label textLabel = new Label();
        private readonly SqlTransaction transation = new SqlTransaction();
        public int UniDevId;

        public FormRecovery()
        {
            InitializeComponent();
            transation.SqlTransactionEvent += this.TransationSqlTransactionEvent;
        }
        /// <summary>
        /// Чтение доступных данных из базы данных
        /// </summary>
        internal void ReadDatas()
        {
            try
            {
                DbFileControl.InitInfoLabel( "Загрузка данных...", textLabel, Size );
                Controls.Add( textLabel );
                textLabel.BringToFront();

                dataSet.Tables[0].Clear();
                transation.ExecuteOperation( SqlTransaction.GetDataList( this.UniDevId, true ), dataSet,
                                             SqlTransaction.ActionSqlTransaction.List );
                dataGridView1.ClearSelection();
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
            finally
            {
                Controls.Remove( textLabel );
            }
        }
        /// <summary>
        /// Восстановление
        /// </summary>
        private void Button1Click( object sender, EventArgs e )
        {
            if ( dataGridView1.RowCount == 0 )
                return;

            try
            {
                DbFileControl.InitInfoLabel( "Востановление данных...", textLabel, Size );
                Controls.Add( textLabel );
                textLabel.BringToFront();

                transation.ExecuteOperation( SqlTransaction.RecoveryFile( 0 ),
                                             SqlTransaction.ActionSqlTransaction.Recovery );
            }
            catch ( Exception ex )
            {
                MessageBox.Show( ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error );
            }
            finally
            {
                Controls.Remove( textLabel );
            }

            ReadDatas();
        }
        /// <summary>
        /// Загрузка формы
        /// </summary>
        protected override void OnLoad( EventArgs e )
        {
            base.OnLoad( e );

            ReadDatas();
        }
        /// <summary>
        /// Событие срабатывания выполнения sql комманд
        /// </summary>
        private void TransationSqlTransactionEvent( object sender, EventArgs e )
        {
            DbFileControl.ParseActionSqlTransaction( (SqlTransactionEvenArgs)e, listBox1 );
        }
        /// <summary>
        /// Действие по изменению выделенных пунктов таблицы
        /// </summary>
        private void DataGridView1SelectionChanged( object sender, EventArgs e )
        {
            this.button1.Enabled = this.dataGridView1.SelectedRows.Count != 0;
        }
    }
}
