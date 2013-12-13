using System;
using System.Windows.Forms;
using System.Collections;

using HelperLibrary;
using LibraryElements;

namespace LabelTextbox
{
    public enum ToDo
    {
        SetFont,
        SetLink
    }
    public partial class frmSmartLabelCustom : Form
    {
        ArrayList cfg = new ArrayList();
        public string selNode;
        Label lblCustom;
        public ToDo todo;
        Form fr;
        IDynamicParameters idp;

        public frmSmartLabelCustom()
        {
            InitializeComponent();
        }
        public frmSmartLabelCustom( ArrayList cfg, BaseRegion region )
        {
            InitializeComponent();
            this.cfg = cfg;
            todo = ToDo.SetLink;
            this.idp = region as IDynamicParameters;

            if ( tabControl1.TabPages.Contains( tbpCustom ) )
                this.tabControl1.TabPages.Remove( tbpCustom );

            fr = this.Owner;
        }
        public frmSmartLabelCustom( Label lbl, ArrayList cfg )
        {
            InitializeComponent();
            this.cfg = cfg;
            lblCustom = lbl;
            lblTest.Font = lbl.Font;
            lblTest.BackColor = lbl.BackColor;
            lblTest.ForeColor = lbl.ForeColor;
            lblTest.BorderStyle = lblCustom.BorderStyle;
            switch ( lblCustom.BorderStyle )
            {
                case BorderStyle.None:
                    rbWoFrame.Checked = true;
                    break;
                case BorderStyle.FixedSingle:
                    rbFlatFrame.Checked = true;
                    break;
                case BorderStyle.Fixed3D:
                    rb3DFrame.Checked = true;
                    break;
            }
        }
        public frmSmartLabelCustom( Label lbl, ArrayList cfg, ToDo todo )
            : this( lbl, cfg )
        {
            this.todo = todo;
        }
        private void btnCancel_Click( object sender, EventArgs e )
        {
            Close();
        }

        private void frmSmartLabelCustom_Load( object sender, EventArgs e )
        {
            fr = this.Owner;
            // по потребности настраиваем интерфейс
            switch ( todo )
            {
                case ToDo.SetFont:
                    // настройка кнопок
                    btnChangeBackColor.BackColor = lblTest.BackColor;

                    if ( tabControl1.TabPages.Contains( tbpTreeView ) )
                        this.tabControl1.TabPages.Remove( tbpTreeView );

                    btnChangeBackColor.Enabled = false;
                    btnChangeTextColor.Enabled = false;
                    groupBox1.Enabled = false;

                    break;
                case ToDo.SetLink:
                    if ( tabControl1.TabPages.Contains( tbpCustom ) )
                        this.tabControl1.TabPages.Remove( tbpCustom );
                    break;
            }

            // добавление конфигурации
            treeViewKB.Nodes.Add( "конфигурация проекта" );
            PopulateTreeView( treeViewKB.Nodes[0] );
            treeViewKB.MouseDoubleClick += new MouseEventHandler( SmartLabel_MouseDoubleClick );

            // настройка кнопок
            btnChangeBackColor.BackColor = lblTest.BackColor;

            if ( idp == null || idp.Parameters == null )
                return;

            //раскрыть узел с заданным устройством
            // Ищем узел для определенного устройства
            TreeNode tn = FindNode( treeViewKB, (int)(idp.Parameters.DeviceGuid % 256) );
            // Если нашли,
            if ( tn != null )
            {
                // то выделяем и раскрываем.
                treeViewKB.SelectedNode = tn;
                tn.ExpandAll();
                treeViewKB.Focus();
            }
        }

        /// <summary>
        /// private TreeNode FindNode( TreeView tv, int iD )
        /// Поиск узла в дереве по номеру устройства.
        /// </summary>
        /// <param Name="tv"></param>
        /// <param Name="Name"></param>
        /// <returns></returns>
        private TreeNode FindNode( TreeView tv, int iD )
        {
            // Ищем в узлах первого уровня.
            foreach ( TreeNode tn in tv.Nodes )
            {
                // Если нашли,
                if ( tn.Tag != null )
                    if ( (int)tn.Tag == iD )
                    {
                        // то возвращаем.
                        return tn;
                    }
            }

            // Ищем в подузлах.
            TreeNode node;
            foreach ( TreeNode tn in tv.Nodes )
            {
                // Делаем поиск в узлах.
                node = FindNode( tn, iD );
                // Если нашли,
                if ( node != null )
                {
                    // то возвращаем.
                    return node;
                }
            }
            // Ничего не нашли.
            return null;
        }
        /// <summary>
        /// private TreeNode FindNode( TreeNode treenode, int iD )
        /// Поиск подузла в узле по названию.
        /// </summary>
        /// <param Name="treenode"></param>
        /// <param Name="Name"></param>
        /// <returns></returns>
        private TreeNode FindNode( TreeNode treenode, int iD )
        {
            // Ищем в узлах первого уровня.
            foreach ( TreeNode tn in treenode.Nodes )
            {
                if ( tn.Tag != null )
                    // Если нашли,
                    if ( (int)tn.Tag == iD )
                    {
                        // то возвращаем.
                        return tn;
                    }
            }

            // Ищем в подузлах.
            TreeNode node;
            foreach ( TreeNode tn in treenode.Nodes )
            {
                // Делаем поиск в узлах.
                node = FindNode( tn, iD );
                // Если нашли,
                if ( node != null )
                {
                    // то возвращаем.
                    return node;
                }
            }
            // Ничего не нашли.
            return null;
        }
        /// <summary>
        ///	private void SmartLabel_MouseDoubleClick( object sender, MouseEventArgs e )
        /// </summary>
        /// <param Name="sender"></param>
        /// <param Name="e"></param>
        private void SmartLabel_MouseDoubleClick( object sender, MouseEventArgs e )
        {
            TreeNode node = treeViewKB.GetNodeAt( e.X, e.Y );

            if ( node == null )
                return;

            treeViewKB.SelectedNode = node;
            selNode = node.FullPath;
            DialogResult = DialogResult.OK;
        }
        public void PopulateTreeView( TreeNode parentNode )
        {
            //  int tg = 0;
            //foreach( FC aFC in cfg )
            //{
            //    TreeNode aFCNode = new TreeNode( "Функциональный контроллер №" + aFC.NumFC );
            //    parentNode.Nodes.Add( aFCNode );
            //    foreach( TCRZADirectDevice aDev in aFC )
            //    {
            //        TreeNode aDevNode = new TreeNode( "[ яч. № " + aDev.NumSlot.ToString() + " ] " + aDev.ToString() + " [ ид. уст. № " + aDev.NumDev.ToString() + " ]");
            //              aDevNode.Tag = (int) aDev.NumDev;

            //        aFCNode.Nodes.Add( aDevNode );
            //        foreach( TCRZAGroup aGroup in aDev )
            //        {
            //            TreeNode aGroupNode = new TreeNode( aGroup.Name );
            //                    tg = aGroup.Id;
            //                    if( ( tg == 3 ) || ( tg == 6 ) )
            //                    {
            //                        aDevNode.Nodes.Add( aGroupNode );
            //                        foreach( TCRZAVariable aVariable in aGroup )
            //                        {
            //                            TreeNode aVariableNode = new TreeNode( aVariable.Name );
            //                            aGroupNode.Nodes.Add( aVariableNode );
            //                            // распознать по типам и присвоить иконку для битовых и аналоговых сигналов
            //                        }
            //                    }
            //                  else
            //                        continue;
            //        }
            //    }
            //}
        }

        private void btnChangeFont_Click( object sender, EventArgs e )
        {
            fontDialog1.Font = lblTest.Font;
            fr.TopMost = false;

            if ( fontDialog1.ShowDialog() == DialogResult.OK )
            {
                lblTest.Font = fontDialog1.Font;
            }
            fr.TopMost = true;
        }

        private void btnChangeBackColor_Click( object sender, EventArgs e )
        {

            fr.TopMost = false;

            colorDialog1.Color = lblTest.BackColor;
            if ( colorDialog1.ShowDialog() == DialogResult.OK )
            {
                lblTest.BackColor = colorDialog1.Color;
                btnChangeBackColor.BackColor = colorDialog1.Color;
            }

            fr.TopMost = true;
        }

        private void btnChangeTextColor_Click( object sender, EventArgs e )
        {

            fr.TopMost = false;

            colorDialog1.Color = lblTest.ForeColor;
            if ( colorDialog1.ShowDialog() == DialogResult.OK )
            {
                lblTest.ForeColor = colorDialog1.Color;
                btnChangeTextColor.ForeColor = colorDialog1.Color;
            }

            fr.TopMost = true;
        }

        private void btnOk_Click( object sender, EventArgs e )
        {
            lblCustom.BackColor = lblTest.BackColor;
            lblCustom.ForeColor = lblTest.ForeColor;
            lblCustom.Font = lblTest.Font;
            lblCustom.BorderStyle = lblTest.BorderStyle;

            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void rbWoFrame_Click( object sender, EventArgs e )
        {
            RadioButton rb = (RadioButton)sender;
            switch ( rb.Name )
            {
                case "rbWoFrame":
                    lblTest.BorderStyle = BorderStyle.None;
                    break;
                case "rbFlatFrame":
                    lblTest.BorderStyle = BorderStyle.FixedSingle;
                    break;
                case "rb3DFrame":
                    lblTest.BorderStyle = BorderStyle.Fixed3D;
                    break;
            }
        }
    }
}