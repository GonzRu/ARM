using System;
using System.Drawing;

using LibraryElements.CalculationBlocks;
using LibraryElements.Sources;

using Structure;

namespace LibraryElements
{
    public interface IDynamicParameters
    {
        /// <summary>
        /// Корректировака данных тэгов для конкретного элемента
        /// </summary>
        void AdjustmentTags( );
        /// <summary>
        /// Параметры
        /// </summary>
        DynamicParameters Parameters { get; }
    }

    /// <summary>
    /// Класс расчетных данных в представлении фигуры
    /// </summary>
    public abstract class CalculationFigure : Figure, ICalculationContext
    {
        protected CalculationFigure( int maxX, int maxY ) : base( maxX, maxY )
        {
            ElementModel = Model.Dynamic;
            ElementName = "CalculationFigure";
        }
        /// <summary>
        /// Метод отрисовки
        /// </summary>
        public override void DrawElement( Graphics g )
        {
            if ( CalculationContext != null )
            {
                var res = CalculationContext.Execute( this, g );
                if ( !res ) NoImage( g );
            }
            else
                NoImage( g );

            DrawSelected( g );
        }
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <param name="_original">Элемент на основе которого делается копия</param>
        public override void CopyElement( Element _original )
        {
            base.CopyElement( _original );
            CalculationContext = CalculationContext.GetCopy( ( (ICalculationContext)_original ).CalculationContext );
        }

        /// <summary>
        /// Расчетные данные 
        /// </summary>
        public CalculationContext CalculationContext { get; set; }
    }
    /// <summary>
    /// Класс функций динамического элемента
    /// </summary>
    public class DynamicElement : CalculationFigure, IDynamicParameters
    {
        public DynamicElement( int maxX = 200, int maxY = 200 )
            : base( maxX, maxY )
        {
            ElementModel = Model.Dynamic;
            ElementName = "DynamicElement";
            IsSelected = true;
            IsModify = true;
            Parameters = new DynamicParameters();
        }
        /// <summary>
        /// Корректировака данных тэгов для конкретного элемента
        /// </summary>
        public void AdjustmentTags( )
        {
            if (CalculationContext != null)
            {
                CalculationContext.AdjustmentTags(CalculationContext.Context, Parameters.DsGuid, Parameters.DeviceGuid);

                if (CalculationContext.IsDeviceFromDeviceBinding)
                {
                    CalculationContext.StateDSGuid = Parameters.DsGuid;
                    CalculationContext.StateDeviecGuid = Parameters.DeviceGuid;
                }
            }
        }
        public override Element CopyElement( )
        {
            var create = new DynamicElement();
            create.CopyElement( this );
            return create;
        }
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <param name="_original">Элемент на основе которого делается копия</param>
        public override void CopyElement( Element _original )
        {
            base.CopyElement( _original );
            Parameters.CopyElement( ( (IDynamicParameters)_original ).Parameters );
        }
        /// <summary>
        /// Параметры
        /// </summary>
        public DynamicParameters Parameters { get; private set; }
    }
    /// <summary>
    /// Класс статического элемента
    /// </summary>
    public class StaticElement : Figure
    {
        ImageData imgData;

        public StaticElement( int maxX = 200, int maxY = 200 )
            : base( maxX, maxY )
        {
            IsSelected = true;
            IsModify = true;
        }
        /// <summary>
        /// Присвоить изображение
        /// </summary>
        /// <param name="image">Данные изображения</param>
        public void SetImage( ImageData image )
        {
            if ( image != null )
            {
                imgData = image;
                if ( imgData.Image != null )
                    SetMaxSize( imgData.Image.Size );
            }
        }
        /// <summary>
        /// Взять изображение
        /// </summary>
        public ImageData GetImage()
        {
            return imgData;
        }
        /// <summary>
        /// Проверка на наличие изображения
        /// </summary>
        /// <returns>true - если есть изображение</returns>
        public Boolean ExistImage() { return imgData != null; }

        public override Element CopyElement()
        {
            var create = new StaticElement();
            create.CopyElement( this );
            return create;
        }
        /// <summary>
        /// Метод отрисовки
        /// </summary>
        public override void DrawElement( Graphics g )
        {
            lock ( this )
            {
                if ( imgData != null && imgData.Image != null )
                    g.DrawImage( imgData.Image, elem_rec );
                else NoImage( g );
            }

            DrawSelected( g );
        }
        /// <summary>
        /// Копирование элемента
        /// </summary>
        /// <param name="_original">Элемент на основе которого делается копия</param>
        public override void CopyElement( Element _original )
        {
            base.CopyElement( _original );
            imgData = new ImageData( ( (StaticElement)_original ).imgData );
        }
    }
}
