using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HMI_MT
{
    public partial class GetCommentWindow : Form
    {
        #region Public Properties
        /// <summary>
        /// Возвращает комментарий, введенный пользователем
        /// </summary>
        public String Comment
        {
            get
            {
                if (!_isShown)
                    throw new Exception("Окно не было запущено.");
                return CommentTextBox.Text;
            }
        }
        #endregion Public Properties

        #region Private Fields
        /// <summary>
        /// Показывает было ли показано окно
        /// </summary>
        private bool _isShown = false;
        #endregion Private Fields

        #region Constructors
        public GetCommentWindow()
        {
            InitializeComponent();
        }
        #endregion Constructors

        #region Handlers
        private void OkButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        #endregion Handlers

        #region Override
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);

            _isShown = true;
        }
        #endregion Override
    }
}
