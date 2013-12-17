using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using LibraryElements;
using LibraryElements.CalculationBlocks;

using Structure;

namespace WindowsForms
{
    public partial class LinesPropertiesForm : Form
    {
        #region Parameters
        private readonly Line line;
        private List<LinePoints> modList;
        private bool init;
        private int nowSel;
        private CalculationContext newContext;
        #endregion

        #region Class Methods
        public LinesPropertiesForm( object obj, int frameMaxX, int frameMaxY )
        {
            InitializeComponent();
            line = (Line)obj;

            this.numericUpDown1.Maximum = this.numericUpDown3.Maximum = this.numericUpDown6.Maximum = this.numericUpDown8.Maximum = frameMaxX;
            this.numericUpDown2.Maximum = this.numericUpDown4.Maximum = this.numericUpDown7.Maximum = this.numericUpDown9.Maximum = frameMaxY;

            if ( line is Trunk )
            {
                this.tabControl1.Controls.Remove( this.tabPage1 );
                InitTrunk();
                return;
            }
            if ( line is PolyLine )
            {
                this.tabControl1.Controls.Remove( this.tabPage1 );
                this.tabControl1.Controls.Remove( this.tabPage4 );
                InitPolyLine();
            }
            else
            {
                this.tabControl1.Controls.Remove( this.tabPage2 );
                this.tabControl1.Controls.Remove( this.tabPage4 );
                InitLine();
            }

            this.CancelButton = button2;
        }
        private void button6_Click( object sender, EventArgs e )
        {
            var form = new ElementBehaviorForm( (ICalculationContext)this.line ) { Owner = this };
            if ( form.ShowDialog() == DialogResult.OK )
                newContext = form.GetNewCalculationContext();
        }
        private void button3_Click( object sender, EventArgs e ) { this.colorDialog1.ShowDialog(); }
        private void button2_Click( object sender, EventArgs e ) { Close(); }
        private void button1_Click( object sender, EventArgs e )
        {
            if ( line is Trunk )
            {
                SetNewTrunkParameters();
            }
            if ( line is PolyLine )
            {
                SetNewPolyLineParameters();
            }
            else
            {
                SetNewLineParameters();
            }
            Close();
        }
        private void SetLineStyle() { this.line.LineStyle = this.checkBox1.Checked ? DashStyle.Dash : DashStyle.Solid; }
        #endregion

        #region Trunk Line
        private void InitTrunk()
        {
            InitPolyLine();

            this.checkBox1.Enabled = false;
            this.button3.Enabled = false;
            this.label15.Text = ( (Trunk)line ).CalculationContext != null ? "Загружено" : " Не загружено";
        }
        private void SetNewTrunkParameters()
        {
            var calc = line as ICalculationContext;
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
                else if ( calc.CalculationContext == null && newContext != null )
                    calc.CalculationContext = newContext;
            }
        }
        #endregion

        #region Single Line
        private void InitLine()
        {
            var tmpPnt = line.GetStartPoint();
            this.numericUpDown1.Value = CommonPropertiesForm.CheckValue( this.numericUpDown1, Convert.ToInt32( tmpPnt.X ) );
            this.numericUpDown2.Value = CommonPropertiesForm.CheckValue( this.numericUpDown2, Convert.ToInt32( tmpPnt.Y ) );

            tmpPnt = line.GetFinishPoint();
            this.numericUpDown3.Value = CommonPropertiesForm.CheckValue( this.numericUpDown3, Convert.ToInt32( tmpPnt.X ) );
            this.numericUpDown4.Value = CommonPropertiesForm.CheckValue( this.numericUpDown4, Convert.ToInt32( tmpPnt.Y ) );

            this.numericUpDown5.Value = line.Thickness;
            this.colorDialog1.Color = line.ElementColor;
            if ( line.LineStyle == DashStyle.Dash )
                this.checkBox1.Checked = true;
        }
        private void SetNewLineParameters()
        {
            var x1 = Convert.ToInt32( this.numericUpDown1.Value );
            var y1 = Convert.ToInt32( this.numericUpDown2.Value );
            var x2 = Convert.ToInt32( this.numericUpDown3.Value );
            var y2 = Convert.ToInt32( this.numericUpDown4.Value );

            line.SetNewPoints( new PointF( x1, y1 ), new PointF( x2, y2 ) );
            line.Thickness = Convert.ToInt32( this.numericUpDown5.Value );
            line.ElementColor = colorDialog1.Color;
            SetLineStyle();
        }
        #endregion

        #region Poly Line
        private void InitPolyLine()
        {
            var tmp = (PolyLine)line;
            this.modList = tmp.IgetAllPoints();

            for ( var i = 0; i < this.modList.Count; i++ )
            {
                this.comboBox1.Items.Add( "Линия #" + ( i + 1 ) );
            }

            this.nowSel = 0;
            this.init = true;
            this.comboBox1.SelectedIndex = this.nowSel;
            this.init = false;
            this.numericUpDown5.Value = tmp.Thickness;
            this.colorDialog1.Color = tmp.ElementColor;
            if ( tmp.LineStyle == DashStyle.Dash )
                this.checkBox1.Checked = true;
        }
        private void ChangePoints()
        {
            var x1 = Convert.ToInt32( this.numericUpDown6.Value );
            var y1 = Convert.ToInt32( this.numericUpDown7.Value );
            var x2 = Convert.ToInt32( this.numericUpDown8.Value );
            var y2 = Convert.ToInt32( this.numericUpDown9.Value );

            var tmpln = new LinePoints { Start = new PointF( x1, y1 ), Finish = new PointF( x2, y2 ) };
            this.modList.RemoveAt( this.nowSel );
            this.modList.Insert( this.nowSel, tmpln );

            if ( this.nowSel != 0 )
            {
                tmpln = this.modList[this.nowSel - 1];
                tmpln.Finish = new PointF( x1, y1 );
                this.modList.RemoveAt( this.nowSel - 1 );
                this.modList.Insert( this.nowSel - 1, tmpln );
            }
            if ( this.nowSel + 1 < this.modList.Count )
            {
                tmpln = this.modList[this.nowSel + 1];
                tmpln.Start = new PointF( x2, y2 );
                this.modList.RemoveAt( this.nowSel + 1 );
                this.modList.Insert( this.nowSel + 1, tmpln );
            }
        }
        private void SetNewPolyLineParameters()
        {
            ChangePoints();
            line.ElementColor = this.colorDialog1.Color;
            line.Thickness = Convert.ToInt32( this.numericUpDown5.Value );
            SetLineStyle();
        }
        private void comboBox1_SelectedIndexChanged( object sender, EventArgs e )
        {
            if ( this.comboBox1.Items.Count == 0 )
                return;

            var i = this.comboBox1.SelectedIndex;
            if ( i < 0 || i >= this.modList.Count ) return;

            if ( !this.init )
                this.ChangePoints();
            this.numericUpDown6.Value = CommonPropertiesForm.CheckValue( this.numericUpDown6, Convert.ToInt32( this.modList[i].Start.X ) );
            this.numericUpDown7.Value = CommonPropertiesForm.CheckValue( this.numericUpDown7, Convert.ToInt32( this.modList[i].Start.Y ) );
            this.numericUpDown8.Value = CommonPropertiesForm.CheckValue( this.numericUpDown8, Convert.ToInt32( this.modList[i].Finish.X ) );
            this.numericUpDown9.Value = CommonPropertiesForm.CheckValue( this.numericUpDown9, Convert.ToInt32( this.modList[i].Finish.Y ) );
            this.nowSel = i;
        }
        #endregion
    }
}