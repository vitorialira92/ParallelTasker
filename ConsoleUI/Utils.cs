using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    internal static class Utils
    {

        public static int ReadInteger(int min = int.MinValue, int max = int.MaxValue)
        {
            int number = 0;

            string minValueMessage = min != int.MinValue ? $"valor mínimo: {min}" : "não há valor mínimo";
            string maxValueMessage = min != int.MaxValue ? $"valor máximo: {max}" : "não há valor máximo";

            bool isValid = false;
            while (!isValid)
            {
                isValid = int.TryParse(Console.ReadLine(), out number);
                if(isValid)
                    isValid = number >= min && number <= max;

                if (!isValid)
                    Console.WriteLine($"Digite um valor válido!\nDeve ser um inteiro e seguir os requisitos: {minValueMessage} & {maxValueMessage}");

            }

            return number;
        }

    }
}
