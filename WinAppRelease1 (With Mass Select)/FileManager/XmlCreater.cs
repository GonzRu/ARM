using System;
using System.IO;
using System.Xml;

namespace FileManager
{
   public class XmlCreater
   {
      #region Properties
      bool errorflag;
      XmlDocument xml_doc;
      XmlNodeList xml_nl;
      XmlWriterSettings xml_settings;
      XmlWriter xml_writer;

      XmlNode tmp_node;
      XmlAttribute tmp_atr;
      #endregion

      #region Xml Stream
      public XmlCreater()
      {
         xml_doc = new XmlDocument();
         errorflag = false;         
      }
      /// <summary>
      /// �������� ������ �� �����
      /// </summary>
      /// <param name="_fs">�������� �����</param>
      public void LoadXmlStream(FileStream _fs)
      {
         try
         {
            xml_doc.Load(_fs);
         }
         catch
         {
            errorflag = true;
         }

         errorflag = false;
      }
      /// <summary>
      /// ������ ������ � ����
      /// </summary>
      /// <param name="_fs">�������� �����</param>
      public void SaveXmlStream(FileStream _fs)
      {
         try
         {
            xml_settings = new XmlWriterSettings();
            xml_settings.Indent = true;
            xml_settings.IndentChars = "\t";

            xml_writer = XmlWriter.Create(_fs, xml_settings);
            xml_doc.Save(xml_writer);
         }
         catch
         {
            errorflag = true;
         }
      }
      #endregion

      #region Save Data
      /// <summary>
      /// ������� ����� �������
      /// </summary>
      /// <param name="_tag">��� ��������</param>
      /// <param name="_value">��������</param>
      /// <returns>����� �������</returns>
      public XmlAttribute CreateNodeAttribute(string _tag, string _value)
      {
         tmp_atr = xml_doc.CreateAttribute(_tag);
         tmp_atr.Value = _value;
         return tmp_atr;
      }
      /// <summary>
      /// ������� ����� �����
      /// </summary>
      /// <param name="_tag">���</param>
      /// <returns>����� �����</returns>
      public XmlNode CreateNode(string _tag)
      {
         tmp_node = xml_doc.CreateElement(_tag);
         return tmp_node;
      }
      /// <summary>
      /// ������� ����� �����
      /// </summary>
      /// <param name="_tag">���</param>
      /// <param name="_value">��������</param>
      /// <returns>����� �����</returns>
      public XmlNode CreateNode(string _tag, string _value)
      {
         tmp_node = xml_doc.CreateElement(_tag);
         tmp_node.InnerText = _value;
         return tmp_node;
      }
      /// <summary>
      /// �������� � ����� ��������
      /// </summary>
      /// <param name="_node">�����</param>
      /// <param name="_value">��������</param>
      public void AddInnerText(XmlNode _node, string _value)
      {
         XmlNode node = _node;
         node.InnerText = _value;
      }
      /// <summary>
      /// �������� ����� � ����� ������
      /// </summary>
      /// <param name="_node">�����</param>
      public void InsertNode(XmlNode _node)
      {
         XmlNode root = xml_doc.DocumentElement;
         root.InsertAfter(_node, root.LastChild);
      }
      /// <summary>
      /// ���������� Child ����� � Parent
      /// </summary>
      /// <param name="_parent">����� ��������</param>
      /// <param name="_child">����� �������</param>
      public void AddNode(XmlNode _parent, XmlNode _child)
      {
         XmlNode parent = _parent;
         parent.AppendChild(_child);
      }
      /// <summary>
      /// �������� ������� � �����
      /// </summary>
      /// <param name="_node">�����</param>
      /// <param name="_atr">�������</param>
      public void AddAttribute(XmlNode _node, XmlAttribute _atr)
      {
         XmlNode node = _node;
         node.Attributes.Append(_atr);
      }
      /// <summary>
      /// ������� ���������� � �������� ���� �����
      /// </summary>
      /// <param name="_node">�����</param>
      public void CreateXmlCloseDeclaration(XmlNode _node)
      {
         XmlNode docNode = xml_doc.CreateXmlDeclaration("1.0", "UTF-8", null);
         xml_doc.AppendChild(docNode);

         XmlNode productsNode = xml_doc.CreateElement("namespace");

         productsNode.AppendChild(_node);//add elements

         xml_doc.AppendChild(productsNode);
      }
      /// <summary>
      /// ��������� ��������
      /// </summary>
      /// <param name="_node">�����</param>
      /// <param name="_tag">���������� ���</param>
      /// <param name="_new_value">����� ��������</param>
      public void ChangeAttributeValue(XmlNode _node, string _tag, string _new_value)
      {
         XmlNode node = _node;

         for (int i = 0; i < node.Attributes.Count; i++)
         {
            if (node.Attributes[i].Name == _tag)
            {
               node.Attributes[i].Value = _new_value;
               return;
            }
         }
      }
      #endregion

      #region Read Data
      /// <summary>
      /// �������� ������ ������ ������ �� ����
      /// </summary>
      /// <param name="_tagname">���</param>
      /// <returns>������ ������</returns>
      public XmlNodeList GetNodeList(string _tagname)
      {
         xml_nl = xml_doc.GetElementsByTagName(_tagname);
         return xml_nl;
      }
      /// <summary>
      /// �������� ��������� �����
      /// </summary>
      /// <param name="_node">�����</param>
      /// <param name="_attribute_name">������� ��������</param>
      /// <returns>�������� ���������</returns>
      public string ReadAttribute(XmlNode _node, string _attribute_name)
      {
         for (int i = 0; i < _node.Attributes.Count; i++)
         {
            if (_node.Attributes[i].Name == _attribute_name)
               return _node.Attributes[i].Value;
         }
         return String.Empty;
      }
      /// <summary>
      /// �������� �������� ����� ������
      /// </summary>
      /// <param name="_node">�����</param>
      /// <param name="_search_name">��� �����</param>
      /// <returns>�������� �����</returns>
      public string ReadValue(XmlNode _node, string _search_name)
      {
         for (int i = 0; i < _node.ChildNodes.Count; i++)
         {
            if (_node.ChildNodes[i].ParentNode.Name/*.InnerText*/ == _search_name)
            {
               if (_node.ChildNodes[i].InnerText == "null") return String.Empty;
               else return _node.ChildNodes[i].InnerText;
            }
         }
         return String.Empty;
      }
      #endregion

      #region Delete Data
      /// <summary>
      /// ������� ��� �����
      /// </summary>
      /// <param name="_node">�����</param>
      public void DeleteAllChilds(XmlNode _node)
      {
         xml_doc.ChildNodes[1].RemoveChild(_node);
      }
      #endregion

      #region Properties
      /// <summary>
      /// ������ ���� ��� ������ � ���������
      /// </summary>
      public bool Error_Status
      {
         get { return errorflag; }
      }
      #endregion
   }
}
