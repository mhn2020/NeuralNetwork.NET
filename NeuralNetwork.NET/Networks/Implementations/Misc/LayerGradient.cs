﻿using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NeuralNetworkNET.Helpers;

namespace NeuralNetworkNET.Networks.Implementations.Misc
{
    /// <summary>
    /// A simple struct that holds the gradient for each layer in a neural network
    /// </summary>
    internal struct LayerGradient
    {
        /// <summary>
        /// Gets the gradient with respect to the current weights
        /// </summary>
        public double[,] DJdw { get; }

        /// <summary>
        /// Gets the gradient with respect to the current biases
        /// </summary>
        public double[] Djdb { get; }

        public LayerGradient([NotNull] double[,] dJdw, [NotNull] double[] dJdb)
        {
            DJdw = dJdw;
            Djdb = dJdb;
        }
    }

    /// <summary>
    /// A small extension class for the <see cref="LayerGradient"/> structure
    /// </summary>
    internal static class LayerGradientExtensions
    {
        /// <summary>
        /// Flattens a list of layered gradients into a single linear array
        /// </summary>
        /// <param name="gradient">The gradients to flatten</param>
        [PublicAPI]
        [Pure, NotNull]
        [CollectionAccess(CollectionAccessType.Read)]
        public static double[] Flatten([NotNull] this IReadOnlyList<LayerGradient> gradient)
        {
            if (gradient.Count == 0) return new double[0];
            double[] result = new double[gradient.Sum(g => g.DJdw.Length + g.Djdb.Length)];
            int position = 0;
            foreach (LayerGradient layer in gradient)
            {
                int n = sizeof(double) * layer.DJdw.Length;
                Buffer.BlockCopy(layer.DJdw, 0, result, position, n);
                position += n;
                n = sizeof(double) * layer.Djdb.Length;
                Buffer.BlockCopy(layer.Djdb, 0, result, position, n);
                position += n;
            }
            return result;
        }
    }
}