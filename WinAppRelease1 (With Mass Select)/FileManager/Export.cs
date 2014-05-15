using System;
using System.Collections.Generic;

using LibraryElements;

namespace FileManager
{
  public class Export
  {
    List<Element> lst;

    public Export(List<Element> _lst)
    {
      lst = _lst;
    }
    public Boolean SaveImage(String _path)
    {
       if (WorkMetaFile.SaveMetaFile(_path, lst))
          return true;
       else return false;
    }
  }
}
