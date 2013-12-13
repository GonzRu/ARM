using System;

namespace LibraryElements
{
    /// <summary>
    /// ����� ������������ ����������
    /// </summary>
    public class DynamicParameters
    {
        public DynamicParameters() { ToolTipMessage = Type = String.Empty; }

        /// <summary>
        /// ����������� ��������
        /// </summary>
        /// <param name="original">������� �� ������ �������� �������� �����</param>
        public void CopyElement( DynamicParameters original )
        {
            ToolTipMessage = original.ToolTipMessage;
            Type = original.Type;
            DsGuid = original.DsGuid;
            DeviceGuid = original.DeviceGuid;
            Cell = original.Cell;
            ExternalDescription = original.ExternalDescription;
        }

        /// <summary>
        /// �������� ��� ������ ����� ��������� ��������� ��� ��������.
        /// (���� ��������� �� ������, ������������� ����������� ���������)
        /// </summary>
        public string ToolTipMessage { get; set; }
        /// <summary>
        /// �������� ��� ������ ��� ��������
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// �������� ��� ������ ����� 
        /// </summary>
        public uint DsGuid { get; set; }
        /// <summary>
        /// �������� ��� ������ ����� �������
        /// </summary>
        public uint DeviceGuid { get; set; }
        /// <summary>
        /// �������� ��� ������ ����� ������
        /// </summary>
        public uint Cell { get; set; }
        /// <summary>
        /// �������� ��� ������ ������� �������� ��������� ��� ��������
        /// </summary>
        public bool ExternalDescription { get; set; }
    }
}