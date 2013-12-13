using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using LibraryElements;
using LibraryElements.CalculationBlocks;
using Structure;
using InterfaceLibrary;

namespace HelperLibrary
{
    /// <summary>
    /// Базовый регион графического элемента
    /// </summary>
    public abstract class BaseRegion
    {
        /// <summary>
        /// Изменение значений
        /// </summary>
        public event EventHandler ChangeValue;
        protected readonly Element Core;

        protected BaseRegion( Element element )
        {
            ToolTip = new ToolTip
                {
                    InitialDelay = 200,
                    ReshowDelay = 50,
                    ToolTipTitle = "Подсказка:",
                    ToolTipIcon = ToolTipIcon.Info,
                    Active = false
                };

            ToolTipMessage = string.Empty;
            Core = element;
        }
        protected void ChangeValueMethod()
        {
            if ( ChangeValue != null )
                ChangeValue( Core, new EventArgs() );
        }
        /// <summary>
        /// Назначение текста всплывающей подсказке
        /// </summary>
        /// <param name="control">Контрол</param>
        /// <param name="text">Текст</param>
        public void SetToolTipText( Control control, String text )
        {
            if (control != null)
                ToolTip.SetToolTip( control, text );
        }
        /// <summary>
        /// Изменение значения
        /// </summary>
        /// <param name="format">Формат определяющий тэг</param>
        /// <param name="value">Значение</param>
        /// <param name="type">Тип тэга</param>
        /// <returns>true - значение присвоено</returns>        
        public abstract bool LinkSetText( String format, Object value, TypeOfTag type );

        public static BaseRegion GetRegion( Element element )
        {
            if ( element is CalculationFigure )
                return new DynamicElementRegion( element );
            if ( element is Trunk )
                return new TrunkRegion( element );
            if ( element is SchemaButton )
                return new ButtonRegion( element );

            return null;
        }

        public PointF Scale { get { return Core.Scale; } }
        public Rectangle Position { get { return ( Core is Figure ) ? ( (Figure)Core ).GetPosition() : Rectangle.Empty; } }
        public ToolTip ToolTip { get; private set; }
        public ContextMenuStrip MenuStrip { get; set; }
        public String ToolTipMessage { get; set; }
        /// <summary>
        /// Режим демонстрации
        /// </summary>
        public Boolean IsDemonstration { get; set; }
    }
    /// <summary>
    /// Регион расчетных данных
    /// </summary>
    public abstract class CalculationRegion : BaseRegion, ICalculationContext
    {
        protected CalculationRegion( Element element )
            : base( element ) { }
        /// <summary>
        /// Установка статуса протокола
        /// </summary>
        /// <param name="format">Формат определяющий тэг</param>
        /// <param name="value">Значение</param>
        /// <param name="type">Тип тэга</param>
        /// <returns>true - значение присвоено</returns>
        public bool LinkSetTextStatusDev( String format, Object value, TypeOfTag type )
        {
            var status = ProtocolStatus.Bad;
            switch ( type )
            {
                    case TypeOfTag.Analog: 
                        status = ( Convert.ToSingle( value ) < 4f ) ? ProtocolStatus.Bad : ProtocolStatus.Good;
                    break;
                    case TypeOfTag.Combo:
                    {
                        var idp = Core as IDynamicParameters;
                        if ( idp != null && idp.Parameters.DeviceGuid >= 25600 )
                        {
                            status = ( !string.Equals( value, "Есть" ) )
                                         ? ProtocolStatus.Bad
                                         : ProtocolStatus.Good;

                            if ( idp.Parameters.DeviceGuid == 25855 )
                                status = ProtocolStatus.Good;
                        }
                    }
                    break;
            }

            if ( CalculationContext == null ) return false;

            var isp = CalculationContext.Context as IStateProtocol;
            if ( isp == null ) return false;
            if ( isp.StateProtocol == status ) return false;

            isp.StateProtocol = status;
            ChangeValueMethod( );
            return true;
        }
        /// <summary>
        /// Изменение значения
        /// </summary>
        /// <param name="format">Формат определяющий тэг</param>
        /// <param name="value">Значение</param>
        /// <param name="type">Тип тэга</param>
        /// <returns>true - значение присвоено</returns>
        public sealed override bool LinkSetText( String format, Object value, TypeOfTag type )
        {
            if ( CalculationContext == null )
                return false;

            var flag = CalculationContext.Context.SetSignalValue( format, value );
            if ( flag ) ChangeValueMethod( );
            return flag;
        }
        /// <summary>
        /// Сброс статуса протокола
        /// </summary>
        /// <param name="collection">коллекция графических регионов</param>
        public static void ResetStatusProtocol( IEnumerable<BaseRegion> collection )
        {
            foreach ( var region in collection )
            {
                var calculationRegion = region as CalculationRegion;
                if ( calculationRegion != null )
                    calculationRegion.LinkSetTextStatusDev( string.Empty, 0, TypeOfTag.NaN );
            }
        }

        /// <summary>
        /// Расчетные данные
        /// </summary>
        public CalculationContext CalculationContext
        {
            get { return ( (ICalculationContext)Core ).CalculationContext; }
            set { ( (ICalculationContext)Core ).CalculationContext = value; }
        }
    }
    /// <summary>
    /// Регион динамического элемента
    /// </summary>
    public class DynamicElementRegion : CalculationRegion, IDynamicParameters
    {
        internal DynamicElementRegion( Element element )
            : base( element ) { }
        public void AdjustmentTags( ) { ( (DynamicElement)Core ).AdjustmentTags( ); }
        /// <summary>
        /// Получить параметры елемента
        /// </summary>
        public DynamicParameters Parameters { get { return ( (DynamicElement)Core ).Parameters; } }
    }
    /// <summary>
    /// Регион шины
    /// </summary>
    public class TrunkRegion : CalculationRegion
    {
        internal TrunkRegion( Element element )
            : base( element ) { }
    }
    /// <summary>
    /// Регион кнопки
    /// </summary>
    public class ButtonRegion : BaseRegion
    {
        public ButtonRegion( Element element ) : base( element ) { }
        /// <summary>
        /// Изменение значения
        /// </summary>
        /// <param name="format">Формат определяющий тэг</param>
        /// <param name="value">Значение</param>
        /// <param name="type">Тип тэга</param>
        /// <returns>true - значение присвоено</returns>
        public sealed override bool LinkSetText( string format, object value, TypeOfTag type ) { return false; }

        public String Group { get { return ( (BlockText)Core ).Group; } }
    }
}