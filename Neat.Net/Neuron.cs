namespace Neat.Net;

public class Neuron
{
    private Network _network;

    public Guid Id;
    public Guid Input;
    public Guid Output;

    public float Weight;

    public bool Enabled = true;

    public float Value => _network.GetNode(Input).Value * Weight;
    public bool IsLoopback => Input == Output;

    public Neuron(Network network, Guid input, Guid output, float weight)
    {
        this._network = network;
        Id = Guid.Empty;
        Input = input;
        Output = output;
        Weight = weight;
    }
    
    public Neuron(Neuron neuron)
    {
        this._network = neuron._network;
        Id = neuron.Id;
        Input = neuron.Input;
        Output = neuron.Output;
        Weight = neuron.Weight;
    }

    public override bool Equals(object? obj)
    {
        if(!(obj is Neuron neuron))
            return false;
        
        return Input.Equals(neuron.Input) && Output.Equals(neuron.Output);
    }
}