using System.Linq;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Dynamic;

namespace Primes
{
	using System.Collections.Generic;
	using System.IO;

	class Program
	{
		static void Main(string[] args)
		{
			// Test 
			var primes = new Solver(2, 200);

			// Calc the primes and store in property
			primes.Calculate();

			// Write primes to file on desktop
			var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
			var file = File.OpenWrite(desktopPath + "\\PrimesOutput.txt");

			// TODO:  Test if this works!

			for (int i = 0; i < primes.Primes.Count() - 1; i++)
			{
				// Also write primes to console
				Console.WriteLine(primes.Primes.ElementAt(i).ToString());
				var writer = new StreamWriter(file, Encoding.ASCII, 64, true);
				writer.WriteLine(primes.Primes.ElementAt(i).ToString());
				writer.Flush();
			}
		}
	}

	internal class Solver
	{
		int first;
		int end;
		IEnumerable<int> primes;

		public Solver(int firstIn, int endIn)
		{
			first = firstIn;
			end = endIn;
			primes = new List<int>();
		}

		public void Calculate()
		{
			primes = WithIntegers_Internal(first, end);
		}

		public IEnumerable<int> Primes
		{
			get
			{
				return primes;
			}
		}

		private static List<int> WithIntegers_Internal(int first, int end)
		{
			// first has to be 2 or bigger 
			int valueToSkip, nextValue, lastValue;
			var integers = Enumerable.Range(first, end - first + 1);

			//var ints = new List<int>(); 
			//for (int i = first; i <= end; i++) 
			//{ 
			// ints.Add(i); 
			//}

			// Set up a dictionary of results.  The key is the integer itself, the value is a nullable 
			// boolean (3 states):  null -> not calculated, true -> is a prime, false -> is composite (not a prime)
			var intIsPrime = new Dictionary<int, bool?>();

			// Initialize collection
			foreach (var integer in integers)
			{
				intIsPrime.Add(integer, null);
			}

			// Loop 
			foreach (var integer in integers)
				if (!intIsPrime[integer].HasValue)
				{
					#region Check if prime

					bool cmpste = false;

					// Math trick FTW! 
					for (int current = 2; current <= Math.Sqrt(integer); current++)
					{
						cmpste = integer % current == 0;
						if (cmpste) break;
					}

					var isPrime = !cmpste;

					#endregion Check if prime

					if (isPrime)
					{
						// Set result 
						intIsPrime[integer] = true;

						// Another math trick 
						// Mark all multiples of prime not prime 
						for (int value = 2; (valueToSkip = value * integer) <= end; value++)
						{
							intIsPrime[valueToSkip] = false;
						}
					}
				}

			// Return the ones that are prime 
			return new List<int>(from n in intIsPrime let b = n.Value where b == true select n.Key);
		}
	}
}