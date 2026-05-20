namespace InventoryApp.Web.Models;

public class InventoryStatisticsViewModel
{
    public int InventoryId { get; set; }
    public string InventoryTitle { get; set; } = string.Empty;
    public int TotalItems { get; set; }
    public List<NumericFieldStat> NumericStats { get; set; } = [];
    public List<StringFieldStat> StringStats { get; set; } = [];
}

public class NumericFieldStat
{
    public string FieldTitle { get; set; } = string.Empty;
    public decimal? Average { get; set; }
    public decimal? Min { get; set; }
    public decimal? Max { get; set; }
}

public class StringFieldStat
{
    public string FieldTitle { get; set; } = string.Empty;
    public List<ValueCount> TopValues { get; set; } = [];
}

public class ValueCount
{
    public string Value { get; set; } = string.Empty;
    public int Count { get; set; }
}