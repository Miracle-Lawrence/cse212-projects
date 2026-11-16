public class FeatureCollection
{
    public List<EarthquakeFeature> Features { get; set; } = new();
}

public class EarthquakeFeature
{
    public EarthquakeProperties Properties { get; set; }
}

public class EarthquakeProperties
{
    public double? Mag { get; set; }
    public string Place { get; set; }
}
