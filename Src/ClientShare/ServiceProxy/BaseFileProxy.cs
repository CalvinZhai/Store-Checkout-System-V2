using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GroceryCo.Checkout.Client
{
    // Local XML file based proxy
    public abstract class BaseFileProxy : BaseProxy, IFileLoader
    {
        public void Load(string filePath)
        {
            var xmlDoc = new XmlDocument();
            xmlDoc.Load(filePath);
            this.Parse(xmlDoc);
        }

        public virtual void Parse(XmlDocument xmlDoc) { }
    }
}
