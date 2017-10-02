# FlappyBirdNeuralNetwork
 - Natural
 - Artificial
 - Activations
     - Binary Step
     - Sigmoid = 1 / 1-e^-x
     - TanH
 - Structure
     - Neuron, Bias
     - Dentrite
     - Layer
 - Build int[]
 - Run
     - for every layer, for every neuron
     - value = summation of previous layers value * weight of each previous layers 
     - value = sigmoid(value +_ bias)
 - Training
     - Supervised Training
         - BackPropogation
         - Move by learning rate
             - run input
             - neurons delta = nuron value * (1 - neuron value) * (output[i] - neuron value)
             - neuron bias += (Learning Rate * neuron delta)
             - every dentrite = weight + (learning Rate + previous layers neuron this dentrite is connected to value * this neuron delta)
     - Genetic Training
         - Kill off
         - Breed off
             - Swap single weights or layers
         - mutate
             - completely change, change by percentage, add or subtract, change sign, swap weights
         
 - Evaluation
     - Convex Optimization
