namespace ArknightsApp.DTO;

public class ReferenceDto
{
    public int    Id   { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class ReferenceWithDescriptionDto : ReferenceDto
{
    public string  Description { get; set; } = string.Empty;
    public string? IconUrl     { get; set; }
}

public class OperatorClassDto : ReferenceDto
{
    public string  Description { get; set; } = string.Empty;
    public string? IconUrl     { get; set; }
}

public class SubClassDto : ReferenceDto
{
    public int    OperatorClassId   { get; set; }
    public string OperatorClassName { get; set; } = string.Empty;
}

public class FactionDto : ReferenceWithDescriptionDto
{
    public string? LogoUrl { get; set; }
}