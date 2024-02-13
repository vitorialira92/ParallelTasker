using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleUI
{
    internal static class Menus
    {
        public static string InitialMenu()
        {
            string extremidade = new string('=', 30);
            string barra = "|";
            string espaco = new string(' ', 30 - 2); 

            return $"{extremidade}\n" +
                          $"{barra}{espaco}{barra}\n" +
                          $"{barra} 1 - Iniciar execução       {barra}\n" +
                          $"{barra} 2 - Fazer consultas        {barra}\n" +
                          $"{barra} 3 - Cancelar uma tarefa    {barra}\n" +
                          $"{barra} 0 - Sair                   {barra}\n" +
                          $"{barra}{espaco}{barra}\n" +
                          $"{extremidade}";
        }
        public static string SecondaryMenu()
        {
            string extremidade = new string('=', 30);
            string barra = "|";
            string espaco = new string(' ', 30 - 2); 

            return $"{extremidade}\n" +
                          $"{barra}{espaco}{barra}\n" +
                          $"{barra} 1 - Parar execução         {barra}\n" +
                          $"{barra} 2 - Fazer consultas        {barra}\n" +
                          $"{barra} 3 - Pausar uma tarefa      {barra}\n" +
                          $"{barra} 4 - Cancelar uma tarefa    {barra}\n" +
                          $"{barra} 0 - Sair                   {barra}\n" +
                          $"{barra}{espaco}{barra}\n" +
                          $"{extremidade}";
        }
        
        public static string ViewMenu()
        {
            string extremidade = new string('=', 31);
            string barra = "|";
            string espaco = new string(' ', 31 - 2); 

            return $"{extremidade}\n" +
                          $"{barra}{espaco}{barra}\n" +
                          $"{barra} 1 - Ir para o menu principal{barra}\n" +
                          $"{barra} 2 - Listar tarefas ativas   {barra}\n" +
                          $"{barra} 3 - Listar tarefas inativas {barra}\n" +
                          $"{barra} 4 - Consultar tarefa por id {barra}\n" +
                          $"{barra} 0 - Sair da aplicação       {barra}\n" +
                          $"{barra}{espaco}{barra}\n" +
                          $"{extremidade}";
        }
    }
}
