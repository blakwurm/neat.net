namespace Neat.Net;

public class Node
{

    private Network _network;
    private ActivationFunction _activation;
    
    public delegate float ActivationFunction(float input);
    
    private float _lastValue = 0;
    public float Value = 0;

    public HashSet<Guid> Inputs;
    public HashSet<Guid> Outputs;
    public Guid Id;
    
    public float Priority;
    
    public Node(Network network, ActivationFunction activation, float priority)
    {
        _network = network;
        _activation = activation;
        Id = Guid.NewGuid();
        Inputs = new HashSet<Guid>();
        Outputs = new HashSet<Guid>();
        Priority = priority;
    }
    
    public void Resolve()
    {
        _lastValue = Value;
        Value = Inputs.Aggregate(0f, (v, i) =>
        {
            var neuron = _network.GetNeuron(i);
            if (!neuron.Enabled) return v;
            if (neuron.IsLoopback) return v + _lastValue;
            return v + neuron.Value;
        });
        Value = _activation.Invoke(Value);
    }
}