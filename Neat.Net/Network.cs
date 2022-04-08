namespace Neat.Net;

public class Network
{
    private Dictionary<Guid, Neuron> _neurons;
    private Dictionary<Guid, Node> _nodes;
    private Node[] _inputs;
    private Node[] _outputs;
    private Species _species;
    
    private Node.ActivationFunction _hiddenActivationFunction;

    public Network(Species species, int inputCount, int outputCount, Node.ActivationFunction hiddenActivationFunction, Node.ActivationFunction outputActivationFunction)
    {
        _species = species;
        _hiddenActivationFunction = hiddenActivationFunction;
        _neurons = new Dictionary<Guid, Neuron>();
        _nodes = new Dictionary<Guid, Node>();
        _inputs = new Node[inputCount+1];
        _outputs = new Node[outputCount];
        
        for(int i = 0; i < inputCount+1; i++)
        {
            _inputs[i] = new Node(this, null, 0);
            _nodes.Add(_inputs[i].Id, _inputs[i]);
        }
        
        _inputs[inputCount] = new Node(this, null, 0);
        _inputs[inputCount].Value = 1;
        
        for(int i = 0; i < outputCount; i++)
        {
            _outputs[i] = new Node(this, outputActivationFunction, 100000000000);
            _nodes.Add(_outputs[i].Id, _outputs[i]);
        }
    }

    public Network(Network network)
    {
        _species = network._species;
        _hiddenActivationFunction = network._hiddenActivationFunction;
        _neurons = new Dictionary<Guid, Neuron>();
        _nodes = new Dictionary<Guid, Node>();
        _inputs = network._inputs.Select(x => new Node(x)).ToArray();
        _outputs = network._outputs.Select(x => new Node(x)).ToArray();
        
        foreach (var (key, value) in network._neurons)
        {
            _neurons.Add(key, new Neuron(value));
        }
        
        foreach(var (key, value) in network._nodes)
        {
            if (_inputs.Contains(value) || _outputs.Contains(value)) continue;
            _nodes.Add(key, new Node(value));
        }
    }

    public float[] Evaluate(float[] inputs)
    {
        for (var i = 0; i < inputs.Length; i++)
        {
            _inputs[i].Value = inputs[i];
        }

        foreach (var node in _nodes.Values.Where( x => !_inputs.Contains(x)).OrderBy(x=>x.Priority))
        {
            node.Resolve();
        }

        return _outputs.Select(x => x.Value).ToArray();
    }

    public void AddNode(Neuron neuron)
    {
        neuron.Enabled = false;
        
        var input = _nodes[neuron.Input];
        var output = _nodes[neuron.Output];
        var priority = (input.Priority + output.Priority) / 2;
        
        var node = new Node(this, _hiddenActivationFunction, priority);
        _nodes.Add(node.Id, node);

        var inNeuron = AddNeuron(input, node, 1f);
        var outNeuron = AddNeuron(node, output, neuron.Weight);

        node.Inputs.Add(inNeuron.Id);
        node.Outputs.Add(outNeuron.Id);
    }

    public Neuron AddNeuron(Node inNode, Node outNode, float weight)
    {
        var neuron = new Neuron(this, inNode.Id, outNode.Id, weight);
        neuron.Id = _species.GetNeuronId(neuron);
        _neurons.Add(neuron.Id, neuron);

        return neuron;
    }

    public Neuron AddNeuron(Node inNode, Node outNode)
    {
        return AddNeuron(inNode, outNode, _species.GetWeight());
    }

    public void Init()
    {
        var in1 = _inputs.Sample();
        var out1 = in1;
        var in2 = _inputs.Sample();
        var out2 = in2;
        
        while(out1 == in1) out1 = _outputs.Sample();
        while(out2 == in2 || (in1==in2 && out1==out2)) out2 = _outputs.Sample();

        AddNeuron(in1, out1);
        AddNeuron(in2, out2);
        
        Mutate();
    }

    public void Mutate()
    {
        var settings = _species.Parameters;
        var addNeuronRate = _species.Size < settings.LargeSpeciesThreshold ? settings.SmallSpeciesAddNeuronRate : settings.LargeSpeciesAddNeuronRate;
        if (_species.NextFloat() <= addNeuronRate)
        {
            var len = _nodes.Count;
            Node? n1 = null;
            IEnumerable < Node >? cons = null;
            while (len >= _nodes.Count)
            {
                n1 = _nodes.Values.Sample();
                cons = n1.Outputs.Select(x =>
                {
                    var neuron = _neurons[x];
                    return _nodes[neuron.Output];
                }).Concat(_inputs);
                len = cons.Count();
            }

            var n2 = _nodes.Values.SampleExcluding(cons);
            AddNeuron(n1, n2);
        }

        if (_species.NextFloat() <= settings.AddNodeRate)
        {
            var neuron = _neurons.Values.Where(x=>x.Enabled).Sample();
            AddNode(neuron);
        }
    }
    
    public Node GetNode(Guid id) => _nodes[id];
    public Neuron GetNeuron(Guid id) => _neurons[id];
}