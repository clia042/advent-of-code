using System;

namespace _1._1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            var inputList = System.IO.File.ReadAllLines(@".\input.txt");

            var prevInput = "-1";
            var increaseCount = 0;

            foreach(var input in inputList){
                var prevInputAsInt = int.Parse(prevInput);
                var currentInputAsInt = int.Parse(input);

                if(prevInputAsInt < 0){
                    Console.WriteLine($"{input} (start)");
                }
                else if(currentInputAsInt > prevInputAsInt){
                    increaseCount++;
                    Console.WriteLine($"{input} (++)");
                }else{
                    Console.WriteLine($"{input} (--)");
                }
                prevInput = input;
            }

            Console.WriteLine(increaseCount);
        }
    }
}
