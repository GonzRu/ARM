using System;

namespace LibraryElements
{
    /// <summary>
    /// ����� ������������ ����������
    /// </summary>
    public class DynamicParameters
    {
        public DynamicParameters() { ToolTipMessage = Type = PathToExternalProgram = String.Empty; }

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

            #region New parameters
            DsGuidForCommandBinding = original.DsGuidForCommandBinding;
            DeviceGuidForCommandBinding = original.DeviceGuidForCommandBinding;
            CommandGuidForCommandBinding = original.CommandGuidForCommandBinding;
            #endregion

            IsExecExternalProgram = original.IsExecExternalProgram;
            PathToExternalProgram = original.PathToExternalProgram;
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

        #region ����� ���������. ��������� 31.01.2014
        #region CommandBinding
        /// <summary>
        /// DS guid
        /// </summary>
        public uint DsGuidForCommandBinding { get; set; }

        /// <summary>
        /// ����� ����������, ��� ������� ����� �����������
        /// </summary>
        public uint DeviceGuidForCommandBinding { get; set; }

        /// <summary>
        /// Command guid
        /// </summary>
        public uint CommandGuidForCommandBinding { get; set; }
        #endregion
        #endregion

        #region ����� ������� ��������� 20.02.2014
        /// <summary>
        /// ����, ������������ ���� �� �������� ������� ���������
        /// </summary>
        public bool IsExecExternalProgram { get; set; }

        /// <summary>
        /// Path to external programm
        /// </summary>
        public string PathToExternalProgram { get; set; }
        #endregion
    }
}