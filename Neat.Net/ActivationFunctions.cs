namespace Neat.Net;

public static class ActivationFunctions
{
    public static float Sigmoid(float x)
    {
        return (float)(1f / (1f + Math.Exp(-x)));
    }
    
    public static float HyperbolicTangent(float x)
    {
        return (float)Math.Tanh(x);
    }
    
    public static float ReLu(float x)
    {
        return Math.Max(0, x);
    }

    public static float Linear(float x)
    {
        return x;
    }
}