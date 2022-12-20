using System;

namespace _2._1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            
            var inputCommandsList = System.IO.File.ReadAllLines(@".\input.txt");

            var position = 0;
            var depth = 0;
            var trueDepth = 0;
            var aim = 0;

            foreach(var input in inputCommandsList){
                var command = input.Split(' ');
                var units = int.Parse(command[1]);

                switch(command[0]){
                    case "forward":
                        position += units;
                        trueDepth += (units * aim);
                        break;
                    case "down":
                        depth += units;
                        aim += units;
                        break;
                    case "up":
                        depth -= units;
                        aim -= units;
                        break;
                    default:
                        throw new NotSupportedException();
                }
            }

            var result = position * trueDepth;

            Console.WriteLine(result);
        }
    }
}
