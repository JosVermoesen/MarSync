using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

public static class PeppolInvoiceRenderer
{
    public static string TransformXmlToHtml(string xmlPath, string xsltPath)
    {
        if (!File.Exists(xmlPath))
            throw new FileNotFoundException("XML not found", xmlPath);
        if (!File.Exists(xsltPath))
            throw new FileNotFoundException("XSLT not found", xsltPath);

        var xslt = new XslCompiledTransform();
        xslt.Load(xsltPath);

        using (var sw = new StringWriter())
        using (var xw = XmlWriter.Create(sw, xslt.OutputSettings))
        {
            xslt.Transform(xmlPath, xw);
            return sw.ToString();
        }
    }

    public static string TransformXmlStringToHtml(string xmlContent, string xsltPath)
    {
        var xslt = new XslCompiledTransform();
        xslt.Load(xsltPath);

        using (var sr = new StringReader(xmlContent))
        using (var xr = XmlReader.Create(sr))
        using (var sw = new StringWriter())
        using (var xw = XmlWriter.Create(sw, xslt.OutputSettings))
        {
            xslt.Transform(xr, xw);
            return sw.ToString();
        }
    }
}
