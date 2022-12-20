using System;
using System.Linq;

namespace _1._2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var inputList = System.IO.File.ReadAllLines(@".\input.txt");

            var prevSum = -1;
            var increaseCount = 0;

            for(var i = 0; i < inputList.Length-2;i++){
                var inputSetAsInt = inputList.Skip(i).Take(3).Select(x => int.Parse(x));
                var sum = inputSetAsInt.Sum();

                if(prevSum < 0){
                    Console.WriteLine($"{sum} (start)");
                }
                else if(sum > prevSum){
                    increaseCount++;
                    Console.WriteLine($"{sum} (++)");
                }else{
                    Console.WriteLine($"{sum} (--)");
                }
                prevSum = sum;
            }

            Console.WriteLine(increaseCount);
        }
    }
}
