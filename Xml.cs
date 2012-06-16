using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml.Linq;

namespace WpfBaggage
{
    public static class OwnXmlSerializer
    {
        public static void CreateXmlFile(string fileName, string rootName, BindingList<string> list)
        {
            var array = list.Select(item => item.ToString())
                            .ToList();

            CreateXmlFile(fileName,rootName,array);
        }

        public static void CreateXmlFile(string fileName,string rootName,List<string> list)
        {
            var doc = new XDocument(new XElement(rootName));

            foreach (var elem in list.Select(item => new XElement("item") {Value = item}))
                doc.Root.Add(elem);

            doc.Save(fileName);
        }
    }
}
