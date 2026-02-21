using System.Xml.Serialization;

namespace Finance.Application.Abstractions.Crb;

[XmlRoot("ValCurs")]
public class CurrencyResponse
{
    [XmlElement("Valute")]
    public CurrencyRate[] Currencies { get; set; } = [];
}