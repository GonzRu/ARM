using System;

namespace WpfDashboards
{
   /// <summary>
   /// Базовый интерфейс лицевой части блока
   /// </summary>
   public interface IBaseFace
   {
      /// <summary>
      /// Установка первичного значения
      /// </summary>
      /// <param name="_bind">Имя привязки</param>
      /// <param name="_value">Значение</param>
      void SetFirstValue(String _bind, Boolean _value);
      /// <summary>
      /// Установка значения
      /// </summary>
      /// <param name="_guid">GUID номер</param>
      /// <param name="_bind">Имя привязки</param>
      /// <param name="_value">Значение</param>
      void SetValue(String _guid, String _bind, Boolean _value);
      /// <summary>
      /// Обновление всех свойств на форме
      /// </summary>
      void UpDateLabels();

      /// <summary>
      /// Получить или задать GUID номер блока
      /// </summary>
      String Guid { get; set; }
      /// <summary>
      /// Получить или задать модель блока
      /// </summary>
      String Model { get; set; }
      /// <summary>
      /// Получить параметер сигнала питание
      /// </summary>      
      LEDParameter LEDPower { get; }
   }
   /// <summary>
   /// Лицевая часть блока БМРЗ
   /// </summary>
   public interface IFaceBMRZ : IBaseFace
   {
      /// <summary>
      /// Получить параметер сигнала вкл
      /// </summary>      
      LEDParameter LEDOn { get; }
      /// <summary>
      /// Получить параметер сигнала выкл
      /// </summary>      
      LEDParameter LEDOff { get; }
      /// <summary>
      /// Получить параметер сигнала отказ
      /// </summary>      
      LEDParameter LEDFailure { get; }
      /// <summary>
      /// Получить параметер сигнала пуск
      /// </summary>      
      LEDParameter LEDRun { get; }
      /// <summary>
      /// Получить параметер сигнала выполнение
      /// </summary>      
      LEDParameter LEDOperation { get; }
      /// <summary>
      /// Получить параметер сигнала вызов
      /// </summary>      
      LEDParameter LEDCall { get; }
      /// <summary>
      /// Получить параметер сигнала местного управления
      /// </summary>      
      LEDParameter LEDLocal { get; }
   }
   /// <summary>
   /// Лицевая часть блока БММРЧ
   /// </summary>   
   public interface IFaceBMMRCH : IBaseFace
   {
      /// <summary>
      /// Получить параметер сигнала пуск
      /// </summary>      
      LEDParameter LEDRun { get; }
      /// <summary>
      /// Получить параметр сигнала Label1
      /// </summary>      
      LEDParameter LEDLabel1 { get; }
      /// <summary>
      /// Получить параметр сигнала Label2
      /// </summary>      
      LEDParameter LEDLabel2 { get; }
      /// <summary>
      /// Получить параметр сигнала Label3
      /// </summary>      
      LEDParameter LEDLabel3 { get; }
      /// <summary>
      /// Получить параметр сигнала Label4
      /// </summary>      
      LEDParameter LEDLabel4 { get; }
      /// <summary>
      /// Получить параметр сигнала Label5
      /// </summary>      
      LEDParameter LEDLabel5 { get; }
      /// <summary>
      /// Получить параметр сигнала Label6
      /// </summary>      
      LEDParameter LEDLabel6 { get; }
      /// <summary>
      /// Получить параметр сигнала Label7
      /// </summary>      
      LEDParameter LEDLabel7 { get; }
      /// <summary>
      /// Получить параметр сигнала Label8
      /// </summary>      
      LEDParameter LEDLabel8 { get; }
   }
   /// <summary>
   /// Лицевая часть блока Сириус-ОЗЗ
   /// </summary>
   public interface IFaceSiriusOZZ : IBaseFace
   {
      /// <summary>
      /// Получить параметр сигнала озз обнаружено
      /// </summary>
      LEDParameter LEDLocateOZZ { get; }
      /// <summary>
      /// Получить параметр сигнала земля на первой секции
      /// </summary>      
      LEDParameter LEDGroundOneSection { get; }
      /// <summary>
      /// Получить параметр сигнала земля на второй секции
      /// </summary>      
      LEDParameter LEDGroundTwoSection { get; }
      /// <summary>
      /// Получить параметр сигнала Label1
      /// </summary>      
      LEDParameter LEDLabel1 { get; }
      /// <summary>
      /// Получить параметр сигнала Label2
      /// </summary>      
      LEDParameter LEDLabel2 { get; }
      /// <summary>
      /// Получить параметр сигнала Label3
      /// </summary>      
      LEDParameter LEDLabel3 { get; }
      /// <summary>
      /// Получить параметр сигнала Label4
      /// </summary>      
      LEDParameter LEDLabel4 { get; }
      /// <summary>
      /// Получить параметр сигнала Label5
      /// </summary>      
      LEDParameter LEDLabel5 { get; }
      /// <summary>
      /// Получить параметр сигнала Label6
      /// </summary>      
      LEDParameter LEDLabel6 { get; }
      /// <summary>
      /// Получить параметр сигнала Label7
      /// </summary>      
      LEDParameter LEDLabel7 { get; }
      /// <summary>
      /// Получить параметр сигнала Label8
      /// </summary>      
      LEDParameter LEDLabel8 { get; }
      /// <summary>
      /// Получить параметр сигнала Label9
      /// </summary>      
      LEDParameter LEDLabel9 { get; }
      /// <summary>
      /// Получить параметр сигнала Label0
      /// </summary>      
      LEDParameter LEDLabel10 { get; }
      /// <summary>
      /// Получить параметр сигнала Label11
      /// </summary>      
      LEDParameter LEDLabel11 { get; }
      /// <summary>
      /// Получить параметр сигнала Label12
      /// </summary>      
      LEDParameter LEDLabel12 { get; }
      /// <summary>
      /// Получить параметр сигнала Label13
      /// </summary>      
      LEDParameter LEDLabel13 { get; }
      /// <summary>
      /// Получить параметр сигнала Label14
      /// </summary>      
      LEDParameter LEDLabel14 { get; }
      /// <summary>
      /// Получить параметр сигнала Label15
      /// </summary>      
      LEDParameter LEDLabel15 { get; }
      /// <summary>
      /// Получить параметр сигнала Label16
      /// </summary>      
      LEDParameter LEDLabel16 { get; }
      /// <summary>
      /// Получить параметр сигнала Label17
      /// </summary>      
      LEDParameter LEDLabel17 { get; }
      /// <summary>
      /// Получить параметр сигнала Label18
      /// </summary>      
      LEDParameter LEDLabel18 { get; }
      /// <summary>
      /// Получить параметр сигнала Label19
      /// </summary>      
      LEDParameter LEDLabel19 { get; }
      /// <summary>
      /// Получить параметр сигнала Label20
      /// </summary>      
      LEDParameter LEDLabel20 { get; }
      /// <summary>
      /// Получить параметр сигнала Label21
      /// </summary>      
      LEDParameter LEDLabel21 { get; }
      /// <summary>
      /// Получить параметр сигнала Label22
      /// </summary>      
      LEDParameter LEDLabel22 { get; }
      /// <summary>
      /// Получить параметр сигнала Label23
      /// </summary>      
      LEDParameter LEDLabel23 { get; }
      /// <summary>
      /// Получить параметр сигнала Label24
      /// </summary>      
      LEDParameter LEDLabel24 { get; }
   }
   /// <summary>
   /// Лицевая часть блока Сириус-ЦС
   /// </summary>
   public interface IFaceSiriusCS : IBaseFace
   {
      /// <summary>
      /// Получить параметр сигнала слежение
      /// </summary>
      LEDParameter LEDTracking { get; }
      /// <summary>
      /// Получить параметр сигнала программирование
      /// </summary>      
      LEDParameter LEDProgramming { get; }
      /// <summary>
      /// Получить параметр сигнала просмотр информации
      /// </summary>      
      LEDParameter LEDViewInformation { get; }
      /// <summary>
      /// Получить параметр сигнала сброс информации
      /// </summary>      
      LEDParameter LEDResetInformation { get; }
      /// <summary>
      /// Получить параметр сигнала состояние шинной АС
      /// </summary>      
      LEDParameter LEDStateTireAS { get; }
      /// <summary>
      /// Получить параметр сигнала состояние шинной ПС
      /// </summary>      
      LEDParameter LEDStateTirePS { get; }
      /// <summary>
      /// Получить параметр сигнала новая информация АС
      /// </summary>      
      LEDParameter LEDNewInformationAS { get; }
      /// <summary>
      /// Получить параметр сигнала новая информация ПС
      /// </summary>      
      LEDParameter LEDNewInformationPS { get; }
      /// <summary>
      /// Получить параметр сигнала Label1
      /// </summary>      
      LEDParameter LEDLabel1 { get; }
      /// <summary>
      /// Получить параметр сигнала Label2
      /// </summary>      
      LEDParameter LEDLabel2 { get; }
      /// <summary>
      /// Получить параметр сигнала Label3
      /// </summary>      
      LEDParameter LEDLabel3 { get; }
      /// <summary>
      /// Получить параметр сигнала Label4
      /// </summary>      
      LEDParameter LEDLabel4 { get; }
      /// <summary>
      /// Получить параметр сигнала Label5
      /// </summary>      
      LEDParameter LEDLabel5 { get; }
      /// <summary>
      /// Получить параметр сигнала Label6
      /// </summary>      
      LEDParameter LEDLabel6 { get; }
      /// <summary>
      /// Получить параметр сигнала Label7
      /// </summary>      
      LEDParameter LEDLabel7 { get; }
      /// <summary>
      /// Получить параметр сигнала Label8
      /// </summary>      
      LEDParameter LEDLabel8 { get; }
      /// <summary>
      /// Получить параметр сигнала Label9
      /// </summary>      
      LEDParameter LEDLabel9 { get; }
      /// <summary>
      /// Получить параметр сигнала Label0
      /// </summary>      
      LEDParameter LEDLabel10 { get; }
      /// <summary>
      /// Получить параметр сигнала Label11
      /// </summary>      
      LEDParameter LEDLabel11 { get; }
      /// <summary>
      /// Получить параметр сигнала Label12
      /// </summary>      
      LEDParameter LEDLabel12 { get; }
      /// <summary>
      /// Получить параметр сигнала Label13
      /// </summary>      
      LEDParameter LEDLabel13 { get; }
      /// <summary>
      /// Получить параметр сигнала Label14
      /// </summary>      
      LEDParameter LEDLabel14 { get; }
      /// <summary>
      /// Получить параметр сигнала Label15
      /// </summary>      
      LEDParameter LEDLabel15 { get; }
      /// <summary>
      /// Получить параметр сигнала Label16
      /// </summary>      
      LEDParameter LEDLabel16 { get; }
      /// <summary>
      /// Получить параметр сигнала Label17
      /// </summary>      
      LEDParameter LEDLabel17 { get; }
      /// <summary>
      /// Получить параметр сигнала Label18
      /// </summary>      
      LEDParameter LEDLabel18 { get; }
      /// <summary>
      /// Получить параметр сигнала Label19
      /// </summary>      
      LEDParameter LEDLabel19 { get; }
      /// <summary>
      /// Получить параметр сигнала Label20
      /// </summary>      
      LEDParameter LEDLabel20 { get; }
      /// <summary>
      /// Получить параметр сигнала Label21
      /// </summary>      
      LEDParameter LEDLabel21 { get; }
      /// <summary>
      /// Получить параметр сигнала Label22
      /// </summary>      
      LEDParameter LEDLabel22 { get; }
      /// <summary>
      /// Получить параметр сигнала Label23
      /// </summary>      
      LEDParameter LEDLabel23 { get; }
      /// <summary>
      /// Получить параметр сигнала Label24
      /// </summary>      
      LEDParameter LEDLabel24 { get; }
      /// <summary>
      /// Получить параметр сигнала Label25
      /// </summary>      
      LEDParameter LEDLabel25 { get; }
      /// <summary>
      /// Получить параметр сигнала Label26
      /// </summary>      
      LEDParameter LEDLabel26 { get; }
      /// <summary>
      /// Получить параметр сигнала Label27
      /// </summary>      
      LEDParameter LEDLabel27 { get; }
      /// <summary>
      /// Получить параметр сигнала Label28
      /// </summary>      
      LEDParameter LEDLabel28 { get; }
      /// <summary>
      /// Получить параметр сигнала Label29
      /// </summary>      
      LEDParameter LEDLabel29 { get; }
      /// <summary>
      /// Получить параметр сигнала Label30
      /// </summary>      
      LEDParameter LEDLabel30 { get; }
      /// <summary>
      /// Получить параметр сигнала Label31
      /// </summary>      
      LEDParameter LEDLabel31 { get; }
      /// <summary>
      /// Получить параметр сигнала Label32
      /// </summary>      
      LEDParameter LEDLabel32 { get; }
   }
   /// <summary>
   /// Лицевая часть блока Сириус-ИМФ
   /// </summary>
   public interface IFaceSiriusIMF : IBaseFace
   {
      /// <summary>
      /// Получить параметер сигнала пуск
      /// </summary>      
      LEDParameter LEDRun { get; }
      /// <summary>
      /// Получить параметер сигнала сигнал
      /// </summary>      
      LEDParameter LEDSignal { get; }
   }
   /// <summary>
   /// Лицевая часть блока Овод
   /// </summary>
   public interface IFaceOVOD : IBaseFace
   {
      /// <summary>
      /// Получить параметер сигнала выполнение
      /// </summary>      
      LEDParameter LEDOperation { get; }
      /// <summary>
      /// Получить параметер сигнала отказ
      /// </summary>      
      LEDParameter LEDFailure { get; }
      /// <summary>
      /// Получить параметер сигнала контроль по току выведен
      /// </summary>      
      LEDParameter LEDControlOfCurrentDrawn { get; }
      /// <summary>
      /// Получить параметер сигнала отключенные датчики
      /// </summary>      
      LEDParameter LEDDisconnectedSensors { get; }
   }
   /// <summary>
   /// Лицевая часть блока ЭКРА
   /// </summary>
   public interface IFaceEKRA : IBaseFace
   {
      /// <summary>
      /// Получить параметер сигнала отказ
      /// </summary>      
      LEDParameter LEDFailure { get; }
      /// <summary>
      /// Получить параметер сигнала контр. выход
      /// </summary>
      LEDParameter LEDExit { get; }
      /// <summary>
      /// Получить параметер сигнала пустой ячейки
      /// </summary>
      LEDParameter LEDEmpty { get; }
      /// <summary>
      /// Получить параметр сигнала Label1
      /// </summary>      
      LEDParameter LEDLabel1 { get; }
      /// <summary>
      /// Получить параметр сигнала Label2
      /// </summary>      
      LEDParameter LEDLabel2 { get; }
      /// <summary>
      /// Получить параметр сигнала Label3
      /// </summary>      
      LEDParameter LEDLabel3 { get; }
      /// <summary>
      /// Получить параметр сигнала Label4
      /// </summary>      
      LEDParameter LEDLabel4 { get; }
      /// <summary>
      /// Получить параметр сигнала Label5
      /// </summary>      
      LEDParameter LEDLabel5 { get; }
      /// <summary>
      /// Получить параметр сигнала Label6
      /// </summary>      
      LEDParameter LEDLabel6 { get; }
      /// <summary>
      /// Получить параметр сигнала Label7
      /// </summary>      
      LEDParameter LEDLabel7 { get; }
      /// <summary>
      /// Получить параметр сигнала Label8
      /// </summary>      
      LEDParameter LEDLabel8 { get; }
      /// <summary>
      /// Получить параметр сигнала Label9
      /// </summary>      
      LEDParameter LEDLabel9 { get; }
      /// <summary>
      /// Получить параметр сигнала Label0
      /// </summary>      
      LEDParameter LEDLabel10 { get; }
      /// <summary>
      /// Получить параметр сигнала Label11
      /// </summary>      
      LEDParameter LEDLabel11 { get; }
      /// <summary>
      /// Получить параметр сигнала Label12
      /// </summary>      
      LEDParameter LEDLabel12 { get; }
      /// <summary>
      /// Получить параметр сигнала Label13
      /// </summary>      
      LEDParameter LEDLabel13 { get; }
      /// <summary>
      /// Получить параметр сигнала Label14
      /// </summary>      
      LEDParameter LEDLabel14 { get; }
      /// <summary>
      /// Получить параметр сигнала Label15
      /// </summary>      
      LEDParameter LEDLabel15 { get; }
   }
}
