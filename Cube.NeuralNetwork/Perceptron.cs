using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cube.NeuralNetwork
{
    public class Perceptron
    {
        public double[] Inputs { get; set; }
        public double[] Weights { get; set; }
        public Func<double, double> ActivationFuction { get; set; }
        public double Output { get; set; }
        public int InputCount { get; set; }
        public double LearningRate { get; set; }

        public Perceptron(int inputCount, Func<double, double> activationFuction = null)
        {
            this.InputCount = inputCount;
            this.LearningRate = 0.1;
            this.Inputs = new double[inputCount];
            this.Weights = new double[inputCount];
            this.ActivationFuction = activationFuction ?? this.SignActivationFuction;
            this.InitializeWeightsWithRandomValues();
        }

        public double ComputeOutput(double[] inputs)
        {
            if (inputs.Length != this.InputCount) throw new ArgumentException("Wrong inputs number");
            var sum = 1.0; // 1 is Bies
            for (int i = 0; i < this.Inputs.Length; i++)
            {
                this.Inputs[i] = inputs[i];
                sum += this.Inputs[i] * this.Weights[i];
            }

            this.Output = this.ActivationFuction(sum);
            return this.Output;
        }

        public void Train(double[] trainInputs, double targetValue)
        {
            if (trainInputs.Length != this.InputCount) throw new ArgumentException("Wrong inputs number");
            var guess = this.ComputeOutput(trainInputs);
            var error = targetValue - guess;

            for (int i = 0; i < this.Weights.Length; i++)
            {
                this.Weights[i] += error * this.Inputs[i] * this.LearningRate;
            }
        }

        private void InitializeWeightsWithRandomValues()
        {
            var random = new Random();
            for (int i = 0; i < this.Inputs.Length; i++)
            {
                this.Weights[i] = (random.NextDouble()*2)-1;
            }
        }

        private double SignActivationFuction(double input)
        {
            return Math.Sign(input);
        }
    }
}
