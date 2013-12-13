
namespace Structure
{
   /// <summary>
   /// Разрешение экрана
   /// </summary>
   public enum Sanction
   {
      Unknown,
      Custom,
      Sanction_1024,
      Sanction_1280,
      Sanction_1600
   }
   /// <summary>
   /// Модель создаваемого/читаемого элемента
   /// </summary>
   public enum Model
   {
      Static,
      Dynamic
   }
   /// <summary>
   /// Выбор точки линии
   /// </summary>
   public enum SelectPoint
   {
      None,
      Start,
      Finish,
      StartIntermediate,
      FinishIntermediate
   }
   /// <summary>
   /// Статус линии
   /// </summary>
   public enum LineStatus
   {
      None,
      Add,
      Close,
      PointStart,
      PointFinish,
      Intermediate
   }
   /// <summary>
   /// Режим отрисовки поворота изображения фигуры
   /// </summary>
   public enum DrawRotate
   {
      Up,
      Down,
      Left,
      Right
   }
   /// <summary>
   /// Состояние протокола
   /// </summary>
   public enum ProtocolStatus
   {
      Bad,
      Good
   }
}
