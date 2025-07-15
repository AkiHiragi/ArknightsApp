namespace ArknightsApp.Models;

public class SkillTemplate
{
    public string                                   BaseDescription { get; set; } = string.Empty;
    public Dictionary<string, ParameterProgression> Parameters      { get; set; } = new();
}

public class ParameterProgression
{
    public double[] Values { get; set; } = new double[10];
}