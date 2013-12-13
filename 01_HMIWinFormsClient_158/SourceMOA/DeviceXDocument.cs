using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;

namespace SourceMOA
{
    public class DeviceXDocument
    {
        XDocument xdoc_dev = null;

        public DeviceXDocument(string path2Device_cfg)
        {
            xdoc_dev = XDocument.Load(path2Device_cfg);
        }

        public XElement GetDescriptInfo() { return xdoc_dev.Element( "Device" ).Element( "DescriptInfo" ); }
        public XElement GetSpecificDeviceValues()
        {
            XElement xe_SpecificDeviceValues_Section = null;

            if (xdoc_dev.Element("Device").Elements("SpecificDeviceValues").Count() != 0)
                //xe_SpecificDeviceValues_Section = xdoc_dev.Element("Device").Element("SpecificDeviceValues");
                xe_SpecificDeviceValues_Section = new XElement(xdoc_dev.Element("Device").Element("SpecificDeviceValues"));

            return xe_SpecificDeviceValues_Section;
        }
        public XElement GetGroupsSection()
        {
            XElement xe_GroupsSection = null;

            xe_GroupsSection = new XElement(xdoc_dev.Element("Device").Element("Groups"));

            return xe_GroupsSection;
        }
        public bool TagsSectionExist()
        {
            return xdoc_dev.Element("Device").Elements("Tags").Count() > 0 ? true : false;
        }
        public bool CommandsSectionExist()
        {
            return xdoc_dev.Element("Device").Elements("Commands").Count() > 0 ? true : false;
        }
        public XElement GetCommandsSection()
        {
            XElement xe_CommandsSection = null;

            xe_CommandsSection = new XElement(xdoc_dev.Element("Device").Element("Commands"));

            return xe_CommandsSection;
        }

        public List<XElement> GetTagsList()
        {
            List<XElement> taglist = new List<XElement>();

			try
			{
                foreach (XElement xetagg in xdoc_dev.Element("Device").Element("Tags").Elements("Tag"))
                    taglist.Add(new XElement(xetagg));
			}
			catch(Exception ex)
			{
				TraceSourceLib.TraceSourceDiagMes.WriteDiagnosticMSG(ex );
			}

            return taglist;
        }
    }
}
