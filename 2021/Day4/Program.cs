using System;
using System.Linq;
using System.Text.Json;
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var input = System.IO.File.ReadLines("input.txt").ToList();

var numbers = input[0].Split(",").Select(x => Int32.Parse(x)).ToList();

var boards = GetBoards(input.Skip(2).ToList());

List<int[,]> GetBoards(List<string> input){
    var boards = new List<int[,]>();
    while(input.Any()){
        var board = new int[5,5];
        var boardRows = input.Take(5).Select(x => x.Split(" ",StringSplitOptions.RemoveEmptyEntries)).ToList();
        Console.WriteLine(JsonSerializer.Serialize(boardRows));
        for(var i = 0; i < 5; i++){
            for(var j = 0; j < 5; j++){
                board[i,j] = int.Parse(boardRows[i][j]);
            }
        }
        boards.Add(board);

        input = input.Skip(6).ToList();
    }
    return boards;
}

while(numbers.Any()){
    var draw = numbers.First();
    
}