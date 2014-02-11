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
        /// �������������� ������ ����� ��� ����������� ��������
        /// </summary>
        void AdjustmentTags( );
        /// <summary>
        /// ���������
        /// </summary>
        DynamicParameters Parameters { get; }
    }

    /// <summary>
    /// ����� ��������� ������ � ������������� ������
    /// </summary>
    public abstract class CalculationFigure : Figure, ICalculationContext
    {
        protected CalculationFigure( int maxX, int maxY ) : base( maxX, maxY )
        {
            ElementModel = Model.Dynamic;
            ElementName = "CalculationFigure";
        }
        /// <summary>
        /// ����� ���������
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
        /// ����������� ��������
        /// </summary>
        /// <param name="_original">������� �� ������ �������� �������� �����</param>
        public override void CopyElement( Element _original )
        {
            base.CopyElement( _original );
            CalculationContext = CalculationContext.GetCopy( ( (ICalculationContext)_original ).CalculationContext );
        }

        /// <summary>
        /// ��������� ������ 
        /// </summary>
        public CalculationContext CalculationContext { get; set; }
    }
    /// <summary>
    /// ����� ������� ������������� ��������
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
        /// �������������� ������ ����� ��� ����������� ��������
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
        /// ����������� ��������
        /// </summary>
        /// <param name="_original">������� �� ������ �������� �������� �����</param>
        public override void CopyElement( Element _original )
        {
            base.CopyElement( _original );
            Parameters.CopyElement( ( (IDynamicParameters)_original ).Parameters );
        }
        /// <summary>
        /// ���������
        /// </summary>
        public DynamicParameters Parameters { get; private set; }
    }
    /// <summary>
    /// ����� ������������ ��������
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
        /// ��������� �����������
        /// </summary>
        /// <param name="image">������ �����������</param>
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
        /// ����� �����������
        /// </summary>
        public ImageData GetImage()
        {
            return imgData;
        }
        /// <summary>
        /// �������� �� ������� �����������
        /// </summary>
        /// <returns>true - ���� ���� �����������</returns>
        public Boolean ExistImage() { return imgData != null; }

        public override Element CopyElement()
        {
            var create = new StaticElement();
            create.CopyElement( this );
            return create;
        }
        /// <summary>
        /// ����� ���������
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
        /// ����������� ��������
        /// </summary>
        /// <param name="_original">������� �� ������ �������� �������� �����</param>
        public override void CopyElement( Element _original )
        {
            base.CopyElement( _original );
            imgData = new ImageData( ( (StaticElement)_original ).imgData );
        }
    }
}
