namespace Neat.Net;

public class TrainingParameters
{
    public float ExcessGeneCoefficient = 1.0f;
    public float DisjointGeneCoefficient = 1.0f;
    public float CompatabilityCoefficient = 0.4f;
    public float CompatabilityThreshold = 3.0f;
    public float StagnationRate = 1;
    public float StagnationThreshold = 15;
    public float WeightMutationRate = 0.8f;
    public float WeightOverwriteRate = 0.1f;
    public float WeightPerturbation = 0.05f;
    public float HereditaryDisableRate = 0.75f;
    public float AddNodeRate = 0.03f;
    public float SmallSpeciesAddNeuronRate = 0.05f;
    public float LargeSpeciesAddNeuronRate = 0.3f;
    public int LargeSpeciesThreshold = 15;
}