using System;

namespace InterfaceLibrary
{
    public interface IDeviceForm
    {
        /// <summary>
        /// Активировать определенную вкладку
        /// </summary>
        void ActivateTabPage( string typetabpage );
        /// <summary>
        /// Действия по завершению чтения аварии 
        /// </summary>
        void reqAvar_OnReqExecuted( IRequestData req );
        /// <summary>
        /// Идентификатор блока
        /// </summary>
        UInt32 Guid { get; }
    }
}