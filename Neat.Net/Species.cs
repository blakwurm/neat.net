namespace Neat.Net;

public class Species
{
    private List<Neuron> _neurons;
    private Random _random;
    private TrainingParameters _parameters;

    public int Size => 0;
    public TrainingParameters Parameters => _parameters;

    public Species()
    {
        _neurons = new List<Neuron>();
        _random = new Random();
    }

    public Guid GetNeuronId(Neuron neuron)
    {
        var i = _neurons.IndexOf(neuron);
        if (i >= 0) return _neurons[i].Id;
        
        _neurons.Add(neuron);
        return Guid.NewGuid();
    }

    public float GetWeight()
    {
        return (float)(_random.NextDouble() * 2 - 1);
    }
    
    public float NextFloat()
    {
        return (float)_random.NextDouble();
    }
}