namespace Neat.Net;

public static class Utils
{
    public static T Sample<T>(this IEnumerable<T> source, Random? rnd = null)
    {
        rnd ??= new Random();
        return source.ElementAt(rnd.Next(0, source.Count()));
    }
    
    public static T SampleExcluding<T>(this IEnumerable<T> source, IEnumerable<T> exclude, Random? rnd = null)
    {
        var candidates = source.Except(exclude);
        return candidates.Sample(rnd);
    }
    
}