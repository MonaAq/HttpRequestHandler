namespace HttpRequestHandler.UnitTests.SubSectorInstrument.Models
{
    public class InstrumentsBySectorCodeResponse
    {
        public IList<SectorCodeInstrumentDto> SectorCodeInstruments { get; init; } = new List<SectorCodeInstrumentDto>();
    }
    public class SectorCodeInstrumentDto
    {
        public string Isin { get; init; }
        public string SymbolName { get; set; }
        public string SubSector { get; set; }
    }


}
