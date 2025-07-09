namespace ArknightsApp.DTO;

public class PagedResult<T>
{
    public List<T> Items           { get; set; } = [];
    public int     TotalCount      { get; set; }
    public int     Page            { get; set; }
    public int     PageSize        { get; set; }
    public int     TotalPages      => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool    HasNextPage     => Page < TotalPages;
    public bool    HasPreviousPage => Page > 1;
}

public class SearchRequest
{
    public string? Name           { get; set; }
    public int?    Rarity         { get; set; }
    public string? ClassName      { get; set; }
    public string? FactionName    { get; set; }
    public int     Page           { get; set; } = 1;
    public int     PageSize       { get; set; } = 10;
    public string? SortBy         { get; set; } = "Name";
    public bool    SortDescending { get; set; } = false;
}