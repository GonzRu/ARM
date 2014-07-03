using System;
using System.Data;
using System.Windows.Forms;
using OscillogramsLib;
using System.Collections;

namespace HelperControlsLibrary
{
    public partial class OscDiagControl : UserControl
    {
        internal uint UniDevId;
        readonly OscDiagViewer oscdg = new OscDiagViewer();
        /// <summary>
        /// для хранения имен файлов в случае для объединения осциллограмм
        /// </summary>
        readonly ArrayList asb = new ArrayList();
        /// <summary>
        /// таблица с осциллограммами
        /// </summary>
        DataTable dtO;
        /// <summary>
        /// таблица с диаграммами
        /// </summary>
        DataTable dtG;

        public OscDiagControl()
        {
            InitializeComponent();

            selectUserControl3.btnUpdate.Click += SelectUserControl3BtnUpdateClick;
            dgvOscill.CellContentClick += DgvOscillCellContentClick;
            dgvDiag.CellContentClick += DgvDiagCellContentClick;
        }

        /// <summary>
        /// Осцилограммы
        /// </summary>
        private void SelectUserControl3BtnUpdateClick( object sender, EventArgs e )
        {
            oscdg.IdDev = (int)UniDevId;
            oscdg.DTStartData = selectUserControl3.dtpStartDateAvar.Value;
            oscdg.DTStartTime = selectUserControl3.dtpStartTimeAvar.Value;
            oscdg.DTEndData = selectUserControl3.dtpEndDateAvar.Value;
            oscdg.DTEndTime = selectUserControl3.dtpEndTimeAvar.Value;
            oscdg.TypeRec =
                CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(
                    InterfaceLibrary.TypeBlockData4Req.TypeBlockData4Req_Osc, (uint)(oscdg.IdDev ) );

            // извлекаем данные по осциллограммам
            dtO = oscdg.Do_SQLProc();

            dgvOscill.Rows.Clear();
            for ( var curRow = 0; curRow < dtO.Rows.Count; curRow++ )
            {
                var i = dgvOscill.Rows.Add();   // номер строки
                dgvOscill["clmChBoxOsc", i].Value = false;
                dgvOscill["clmBlockNameOsc", i].Value = dtO.Rows[curRow]["BlockName"];
                dgvOscill["clmBlockIdOsc", i].Value = dtO.Rows[curRow]["BlockID"];
                dgvOscill["clmBlockTimeOsc", i].Value = dtO.Rows[curRow]["TimeBlock"];
                dgvOscill["clmCommentOsc", i].Value = dtO.Rows[curRow]["Comment"];
                dgvOscill["clmID", i].Value = dtO.Rows[curRow]["ID"];
            }


            oscdg.TypeRec =
                CommonUtils.CommonUtils.GetTypeBlockData4ThisDev(
                    InterfaceLibrary.TypeBlockData4Req.TypeBlockData4Req_Diagramm, (uint)(oscdg.IdDev) );
            
            // извлекаем данные по диаграммам
            dtG = oscdg.Do_SQLProc();
            dgvDiag.Rows.Clear();
            for ( var curRow = 0; curRow < dtG.Rows.Count; curRow++ )
            {
                var i = dgvDiag.Rows.Add();   // номер строки
                dgvDiag["clmChBoxDiag", i].Value = false;
                dgvDiag["clmBlockNameDiag", i].Value = dtG.Rows[curRow]["BlockName"];
                dgvDiag["clmBlockIdDiag", i].Value = dtG.Rows[curRow]["BlockID"];
                dgvDiag["clmBlockTimeDiag", i].Value = dtG.Rows[curRow]["TimeBlock"];
                dgvDiag["clmCommentDiag", i].Value = dtG.Rows[curRow]["Comment"];
                dgvDiag["clmIDDiag", i].Value = dtG.Rows[curRow]["ID"];
            }
        }
        /// <summary>
        /// кнопка чтение одной осциллограммы
        /// </summary>     
        private void DgvDiagCellContentClick( object sender, DataGridViewCellEventArgs e )
        {
            if ( e.ColumnIndex == 0 )
            {
                dgvDiag[e.ColumnIndex, e.RowIndex].Value = !( (bool)dgvDiag[e.ColumnIndex, e.RowIndex].Value );
                btnUnionDiag.Enabled = true;
                return;
            }

            if ( e.ColumnIndex != 5 )
                return;

            asb.Clear();
            btnUnionDiag.Enabled = false;

            // сбрасываем все флажки
            for ( var i = 0; i < dgvDiag.RowCount; i++ )
                dgvDiag[0, i].Value = false;

            try
            {
                var de = dgvDiag["clmIDDiag", e.RowIndex];
                /*
                 * первый аргумент номер DS,
                 * сейсчас для отработки механизма задана константа (0)
                 * в дальнейшем нужно придумать механизм когда на данном этапе
                 * будет известен реальный номер DS
                 */
                oscdg.ShowOSCDg( 0, (int)de.Value );
            }
            catch
            {
                MessageBox.Show( "dgvDiag_CellContentClick - исключение" );
            }
        }
        /// <summary>
        /// кнопка чтение одной осциллограммы
        /// </summary>
        private void DgvOscillCellContentClick( object sender, DataGridViewCellEventArgs e )
        {
            if ( e.ColumnIndex == 0 )
            {
                dgvOscill[e.ColumnIndex, e.RowIndex].Value = !( (bool)this.dgvOscill[e.ColumnIndex, e.RowIndex].Value );
                btnUnionOsc.Enabled = true;
                return;
            }
            if ( e.ColumnIndex != 5 )
                return;

            asb.Clear();
            btnUnionOsc.Enabled = false;

            // сбрасываем все флажки
            for ( var i = 0; i < dgvOscill.RowCount; i++ )
                dgvOscill[0, i].Value = false;

            try
            {
                var de = dgvOscill["clmID", e.RowIndex];
                oscdg.ShowOSCDg( 0, (int)de.Value );
            }
            catch
            {
                MessageBox.Show( "dgvOscill_CellContentClick - исключение" );
            }
        }
    }
}
