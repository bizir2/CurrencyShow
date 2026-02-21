using System.Globalization;
using System.Xml.Serialization;

namespace Finance.Application.Abstractions.Crb;

public class CurrencyRate
{
    [XmlElement("Name")]
    public string Name { get; set; } = null!;
    
    [XmlElement("CharCode")]
    public string CharCode { get; set; } = null!;

    [XmlElement("VunitRate")]
    public string ExchangeRateRaw { get; set; } = null!;
}