using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

using HelperLibrary;
using LibraryElements;
using FileManager;

namespace WindowsForms
{
    public interface IBasePanel
    {
        /// <summary>
        /// Порлучить или задать родителя
        /// </summary>
        Control Parent { get; set; }
        /// <summary>
        /// Получить размер панели
        /// </summary>
        Size ClientSize { get; }
        /// <summary>
        /// Получить или задать цвет фона
        /// </summary>
        Color BackColor { get; set; }

        /// <summary>
        /// Событие срабатывания мыши
        /// </summary>
        event EventHandler PanelClick;
        /// <summary>
        /// Получить элемент
        /// </summary>
        BaseRegion Core { get; }
    }
    public interface IBasePanelCollection : IBasePanel
    {
        /// <summary>
        /// Получить признак загрузки схемы
        /// </summary>
        Boolean ErrorLoading { get; }
        /// <summary>
        /// Получить заголовок схемы
        /// </summary>
        String CaptionOfSchema { get; }
        /// <summary>
        /// Получить список расчетных элементов
        /// </summary>
        List<BaseRegion> CalculationElements { get; }
    }

    public abstract class BasePanel : UserControl, IBasePanel
    {
        /// <summary>
        /// Событие срабатывания мыши
        /// </summary>
        public event EventHandler PanelClick;

        protected BasePanel( )
        {
            SetStyle( ControlStyles.UserPaint, true );
            SetStyle( ControlStyles.AllPaintingInWmPaint, true );
            SetStyle( ControlStyles.DoubleBuffer, true );
        }
        /// <summary>
        /// Движение мыши над элементом
        /// </summary>
        protected override void OnMouseMove( MouseEventArgs e )
        {
            base.OnMouseMove( e );

            base.ContextMenuStrip = null;

            if ( Core != null )
            {
                Cursor = Cursors.Hand;
                base.ContextMenuStrip = Core.MenuStrip;

                if ( !Core.ToolTip.Active )
                {
                    Core.SetToolTipText( this, Core.ToolTipMessage );
                    Core.ToolTip.Active = true;
                }
            }
            else Cursor = Cursors.Arrow;
        }
        /// <summary>
        /// Клик мыши по рабочей области
        /// </summary>
        protected override sealed void OnMouseClick( MouseEventArgs e )
        {
            base.OnMouseClick( e );

            // если есть активный элемент, то срабатывает эвент
            if ( PanelClick != null && Core != null && e.Button == MouseButtons.Left )
                PanelClick( Core, e );
        }

        /// <summary>
        /// Получить элемент
        /// </summary>
        public BaseRegion Core { get; protected set; }
    }
    public abstract class BasePanelCollection : BasePanel, IBasePanelCollection
    {
        protected List<Element> Elements;

        protected BasePanelCollection( ) { CalculationElements = new List<BaseRegion>( ); }
        /// <summary>
        /// Сортировка элементов по уровню отображения
        /// </summary>
        protected static int ListCompare( Element elem1, Element elem2 )
        {
            if ( elem1.Level > elem2.Level ) return 0;
            if ( elem1.Level < elem2.Level ) return -1;
            return 1;
        }        
        /// <summary>
        /// Загрузка схемы
        /// </summary>
        protected void LoadingScheme( string path )
        {
            int winWidth = 640, winHeight = 480; //считываемый размер схемы
            this.Elements = new List<Element>( );

            using ( var file = new SchemasStream( ) )
            {
                file.LoadFile( path );

                if ( file.Error_Status )
                {
                    ErrorLoading = true;
                    MessageBox.Show( path + " not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                }
                else
                {
                    ErrorLoading = false;
                    file.ReadDatas( ref this.Elements, ref winWidth, ref winHeight );
                    CaptionOfSchema = file.GetMnenoCaption();

                    ClientSize = new Size( winWidth, winHeight );
                }
            }
        }
        /// <summary>
        /// Определение активных регионов
        /// </summary>
        /// <param name="scale">Масштаб</param>
        protected void GetRegions( PointF scale )
        {
            foreach ( var search in this.Elements )
            {
                search.Scale = scale;
                if ( search.ElementModel == Structure.Model.Dynamic )
                {
                    var region = BaseRegion.GetRegion( search );
                    if ( region != null )
                    {
                        //region.ChangeValue += ( sender, e ) => base.Refresh();
                        region.ChangeValue += (sender, e) => { search.DrawElement(this.CreateGraphics()); };
                        CalculationElements.Add( region );
                    }
                }
            }
        }
        /// <summary>
        /// Отрисовка всех элементов
        /// </summary>
        protected sealed override void OnPaint( PaintEventArgs e )
        {
            base.OnPaint( e );

            // отрисовка всех элементов
            foreach ( var sel in this.Elements )
            {
                e.Graphics.ScaleTransform( sel.Scale.X, sel.Scale.Y );
                sel.DrawElement( e.Graphics );
                e.Graphics.ResetTransform( );

                Application.DoEvents();
            }
        }
        /// <summary>
        /// Движение мыши над элементом
        /// </summary>
        protected sealed override void OnMouseMove( MouseEventArgs e )
        {
            Core = null;

            foreach ( var br in CalculationElements )
            {
                if ( e.Location.X / br.Scale.X >= br.Position.Left && e.Location.Y / br.Scale.Y >= br.Position.Top &&
                     e.Location.X / br.Scale.X <= br.Position.Right && e.Location.Y / br.Scale.Y <= br.Position.Bottom )
                    Core = br;
                else br.ToolTip.Active = false;
            }
            
            base.OnMouseMove( e );
        }

        /// <summary>
        /// Получить признак загрузки схемы
        /// </summary>
        public Boolean ErrorLoading { get; private set; }
        /// <summary>
        /// Получить заголовок схемы
        /// </summary>
        public String CaptionOfSchema { get; protected set; }
        /// <summary>
        /// Получить список расчетных элементов
        /// </summary>
        public List<BaseRegion> CalculationElements { get; private set; }
    }
}
