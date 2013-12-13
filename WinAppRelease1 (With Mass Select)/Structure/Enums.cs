
namespace Structure
{
   /// <summary>
   /// ���������� ������
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
   /// ������ ������������/��������� ��������
   /// </summary>
   public enum Model
   {
      Static,
      Dynamic
   }
   /// <summary>
   /// ����� ����� �����
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
   /// ������ �����
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
   /// ����� ��������� �������� ����������� ������
   /// </summary>
   public enum DrawRotate
   {
      Up,
      Down,
      Left,
      Right
   }
   /// <summary>
   /// ��������� ���������
   /// </summary>
   public enum ProtocolStatus
   {
      Bad,
      Good
   }
}
