using System;
using System.Xml.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;

using Structure;

namespace FileManager
{
   public abstract class LinqStream : IDisposable
   {
     #region Parameters
     protected bool error_flag;
     protected bool existfile;
     protected XDocument xdoc;
     protected XElement xroot;
     #endregion

     #region Class Methods
     public void LoadFile(String _path)
     {
       error_flag = false;

       if (!WorkFile.CheckExistFile(_path))
       {
          error_flag = true;
          return;
       }

       try
       {
          xdoc = XDocument.Load(_path);
       }
       catch { error_flag = true; }
     }
     public bool SaveFile(String _path)
     {
       try { xdoc.Save(_path); }
       catch { return false; }

       return true;
     }

     protected XElement CreateDeclaration()
     {
       xdoc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
       XElement ns = new XElement("namespace");
       xdoc.Add(ns);

       return ns;
     }
     protected void MessageError(String _message)
     {
        #warning Need to use TraceSourceLib
         Console.WriteLine(_message);
     }
     #endregion

     #region Virtual Method
     public virtual void Dispose()
     {
        xroot = null;
        xdoc = null;
     }
     #endregion

     #region Properties
     /// <summary>
     /// —татус сбо€ при работе с файловым потоком
     /// </summary>     
     public Boolean Error_Status
     {
       get { return error_flag; }
     }
     #endregion
   }
   public abstract class LinqXmlMethods : LinqStream
   {
     #region Class Methods
     /// <summary>
     /// —оздание ветви дерева типа
     /// </summary>
     /// <param name="_fig_name">им€ фигуры</param>
     /// <param name="_fig_type">тип фигуры</param>
     /// <returns>ветвь</returns>
     protected XElement CreateElementType(string _fig_name, string _fig_type)
     {
       return CreateElementType(_fig_name, _fig_type, null, false);
     }
     /// <summary>
     /// —оздание ветви дерева типа
     /// </summary>
     /// <param name="_fig_name">им€ фигуры</param>
     /// <param name="_fig_type">тип фигуры</param>
     /// <param name="_fgr_turn">положение фигуры</param>
     /// <returns>ветвь</returns>
     protected XElement CreateElementType(string _fig_name, string _fig_type, string _fgr_turn)
     {
        return CreateElementType(_fig_name, _fig_type, null, false);
     }
     /// <summary>
     /// —оздание ветви дерева типа
     /// </summary>
     /// <param name="_fig_name">им€ фигуры</param>
     /// <param name="_fig_type">тип фигуры</param>
     /// <param name="_fgr_turn">положение фигуры</param>
     /// <param name="_mirror"></param>
     /// <returns>ветвь</returns>
     protected XElement CreateElementType(string _fig_name, string _fig_type, string _fgr_turn, bool _mirror)
     {
        XAttribute xatr;
        XElement xnode = new XElement("type");

        xatr = new XAttribute("figure_name", _fig_name);
        xnode.Add(xatr);
        xatr = new XAttribute("figure_type", _fig_type);
        xnode.Add(xatr);
        
        if (_fgr_turn != null)
        {
           xatr = new XAttribute("turn", _fgr_turn);
           xnode.Add(xatr);
        }
        if (_mirror)
        {
           xatr = new XAttribute("mirror", _mirror);
           xnode.Add(xatr);
        }
        return xnode;
     }
     /// <summary>
     /// ѕолучение дополнительных данных о линии
     /// </summary>
     /// <param name="_xml_node">ветвь xml с данными</param>
     /// <param name="_line">интерфейс одиночной линии</param>
     protected void ParseLineAtributes(XElement _xml_node, IElementLine _line)
     {
       int w = 0;
       Color clr = Color.Black;
       DashStyle stl = DashStyle.Solid;

       clr = ConvertMethods.GetParseColor(_xml_node.Attribute("color").Value);
       w = Convert.ToInt32(_xml_node.Attribute("width").Value);
       stl = ConvertMethods.ParseLineStyle(_xml_node.Attribute("ln_style").Value);

       if (w == 0) w = 1;

       if (_line != null)
       {
         _line.Thickness = w;
         _line.ElementColor = clr;
         _line.LineStyle = stl;
       }
     }
     #endregion
   }
}
