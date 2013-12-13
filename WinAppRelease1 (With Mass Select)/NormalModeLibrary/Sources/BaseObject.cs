using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace NormalModeLibrary.Sources
{
    public abstract class BaseObject
    {
        protected BaseObject() { }
        internal abstract String GetTreeNodeText();
        internal abstract BaseObject Copy();
        public abstract void ParseXml( XElement xnode );
        public abstract XElement CreateXml();
    }
    public abstract class BaseObjectCollection : BaseObject
    {
        protected BaseObjectCollection()
        {
            Collection = new List<BaseObject>();
        }
        public List<BaseObject> Collection { get; private set; }
    }
}
