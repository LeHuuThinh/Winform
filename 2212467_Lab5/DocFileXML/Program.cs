using System;
using System.Xml;

public static class Program
{
    public static void Main(string[] args)
    {
        // Load XML file into XmlDocument instance
        var xmlDoc = new XmlDocument();
        xmlDoc.Load("..\\..\\fileXML.xml");
        // Get list of nodes whose name is Book
        var nodeList = xmlDoc.DocumentElement.SelectNodes("/catalog/book");
        foreach (XmlNode node in nodeList)
        {
            // Read attribute value
            var isbn = node.Attributes["ISBN"]?.Value;
            // Read child node value
            var title = node.SelectSingleNode("title")?.InnerText;
            var price = node.SelectSingleNode("price")?.InnerText;
            // Read the descendant node value
            var firstName = node.SelectSingleNode("author/first-name")?.InnerText;
            var lastName = node.SelectSingleNode("author/last-name")?.InnerText;
            Console.WriteLine("{0,-15}{1,-50}{2,-15}{3,-15}{4,6}",
            isbn, title, firstName, lastName, price);
        }
    }
}
