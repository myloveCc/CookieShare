using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Linq;
using Microsoft.AspNetCore.DataProtection.Repositories;

namespace CookieInstance
{
    public class XmlRepository:IXmlRepository
    {
        private readonly string _KeyContentPath = "";

        public XmlRepository()
        {
            _KeyContentPath = Path.Combine(Directory.GetCurrentDirectory(),"ShareKeys", "key.xml");
        }

        public IReadOnlyCollection<XElement> GetAllElements()
        {
            var elements = new List<XElement>() { XElement.Load(_KeyContentPath) };
            return elements;
        }

        public void StoreElement(XElement element, string friendlyName)
        {
            
        }
    }
}
