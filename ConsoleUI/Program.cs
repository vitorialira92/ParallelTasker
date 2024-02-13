using Microsoft.Extensions.DependencyInjection;
using ProcessadorTarefas.Entidades;
using ProcessadorTarefas.Servicos;
using ProcessadorTarefas.Repositorios.TarefaRepositories;
using SOLID_Example.Interfaces;
using System;
using ConsoleUI;

namespace SOLID_Example
{
    internal class Program
    {
        private static bool _running = false;
        private static bool finishApplication = false; 
        private static int menu = 1;

        private static readonly object consoleLock = new object();

        private static CancellationTokenSource cancelaExibicao = new CancellationTokenSource();

        static void Main(string[] args)
        {
            //configurações
            ConfigureOptionsAsync().Wait();
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton<IRepository<Tarefa>, InMemoryTarefaRepository>();
            serviceCollection.AddSingleton<IProcessadorTarefas, ProcessadorTarefas.Servicos.ProcessadorTarefas>();
            serviceCollection.AddSingleton<IGerenciadorTarefas, GerenciadorTarefas>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var processador = serviceProvider.GetService<ProcessadorTarefas.Servicos.IProcessadorTarefas>(); 
            var gerenciador = serviceProvider.GetService<ProcessadorTarefas.Servicos.IGerenciadorTarefas>();

            //tudo configurado
            Console.WriteLine(Menus.InitialMenu()); 

            //aplicação
            while(!finishApplication)
            {
                int op;
                lock (consoleLock)
                {
                    Console.Clear();
                    Console.SetCursorPosition(0, _running ? ConfigurationValues.QuantidadeDeTarefasExecutadas * 3 + 2 : 0);
                    Console.WriteLine(GetMenu());
                }
                Console.SetCursorPosition(0, _running ? ConfigurationValues.QuantidadeDeTarefasExecutadas * 3 + 12 : 12);
                op = Utils.ReadInteger(0, GetMaxMenuValue());
                ShowAnswerToMenuOption(op, processador, gerenciador).Wait();
            }
            Console.Clear();
            Console.WriteLine("Obrigada por utilizar nossos serviços! Volte sempre!");
        }

        private static async Task ShowAnswerToMenuOption(int op, IProcessadorTarefas processador, IGerenciadorTarefas gerenciador)
        {
            if (op == 0)
            {
                finishApplication = true;
                return;
            }
                
            if (menu == 1) 
            {
                switch (op)
                {
                    case 1:
                        Iniciar(processador);
                        ShowProgress(processador);
                        menu = 2;
                        return;
                    case 2:
                        menu = 3;
                        return;
                    case 3:
                        await CancelarTarefa(gerenciador); 
                        return;
                }
            }

            else if (menu == 2)
            {
                switch (op)
                {
                    case 1:
                        await Parar(processador);
                        return;
                    case 2:
                        menu = 3;
                        return;
                    case 3:
                        await PausarTarefa(processador, gerenciador);
                        return;
                    case 4:
                        await CancelarTarefa(gerenciador);
                        return;
                }
            }

            else if (menu == 3)
            {
                switch (op)
                {
                    case 1:
                        menu = 1;
                        return;
                    case 2:
                        await ListarTarefasAtivas(gerenciador);
                        return;
                    case 3:
                        await ListarTarefasInativas(gerenciador);
                        return;
                    case 4:
                        await ConsultarTarefaPorId(gerenciador);
                        return;

                }
            }

        }

        private static Task PausarTarefa(IProcessadorTarefas processador, IGerenciadorTarefas gerenciador)
        {
            lock (consoleLock)
            {
                Console.WriteLine("\nDigite o id da tarefa que deseja pausar:\nOBS: Uma tarefa só será pausada após a execução da subtarefa atual.");

                int id = Utils.ReadInteger();

                var tarefa = gerenciador.Consultar(id);

                if (tarefa == null)
                    Console.WriteLine("Tarefa não encontrada.");
                else
                {
                    processador.Pausar(id);
                    Console.WriteLine("Solicitação para pausa enviada.");
                }

                Console.WriteLine("Pressione qualquer tecla para continuar");

                Console.ReadKey(true);
            }
            
            return Task.CompletedTask;
        }

        private static Task Parar(IProcessadorTarefas processador)
        {
            lock (consoleLock)
            {
                cancelaExibicao.Cancel();
                processador.Encerrar();
                Console.WriteLine("Encerramento solicitado");
                menu = 1;
                _running = false;
            }
            return Task.CompletedTask;
        }

        private async static Task CancelarTarefa(IGerenciadorTarefas gerenciador)
        {
            lock (consoleLock)
            {
                Console.WriteLine("\nDigite o id da tarefa que deseja cancelar:\nOBS: Uma tarefa só será cancelada após a execução da subtarefa atual.");

                int id = Utils.ReadInteger();

                var tarefa = gerenciador.Consultar((long)id);

                if (tarefa == null)
                    Console.WriteLine("Tarefa não encontrada.");
                else
                {
                    gerenciador.Cancelar(tarefa.Result.Id);
                    Console.WriteLine("Solicitação de cancelamento enviada");
                }

                Console.WriteLine("Pressione qualquer tecla para continuar");

                Console.ReadKey(true);
            }
        }

        private static Task ConsultarTarefaPorId(IGerenciadorTarefas gerenciador)
        {
            lock (consoleLock)
            {
                Console.WriteLine("\nDigite o id da tarefa desejada: ");
                int id = Utils.ReadInteger();

                var tarefa = gerenciador.Consultar((long)id);

                if (tarefa == null)
                    Console.WriteLine("Tarefa não encontrada.");
                else
                    Console.WriteLine(tarefa.Result.ToString());

                Console.WriteLine("Pressione qualquer tecla para continuar");

                Console.ReadKey(true);
            }
            

            return Task.CompletedTask;
        }

        private static Task ListarTarefasInativas(IGerenciadorTarefas gerenciador)
        {
            lock (consoleLock)
            {
                Console.WriteLine("\nTodas as tarefas inativas: ");
                var items = gerenciador.ListarInativas().GetAwaiter().GetResult();

                foreach (var item in items)
                {
                    Console.WriteLine(item.ToString());
                }
                Console.WriteLine("Pressione qualquer tecla para continuar");

                Console.ReadKey(true);
            }
            

            return Task.CompletedTask;
        }

        private static Task ListarTarefasAtivas(IGerenciadorTarefas gerenciador)
        {
            lock (consoleLock)
            {
                Console.WriteLine("\nTodas as tarefas ativas: ");
                var items = gerenciador.ListarAtivas().GetAwaiter().GetResult();

                foreach (var item in items)
                {
                    Console.WriteLine(item.ToString());
                }
                Console.WriteLine("Pressione qualquer tecla para continuar");

                Console.ReadKey(true);
            }
            

            return Task.CompletedTask;
        }

        private static int GetMaxMenuValue()
        {
            return menu switch
            {
                1 => 3,
                2 => 4,
                3 => 4
            };
        }

        private static string GetMenu()
        {
            return menu switch
            {
                1 => Menus.InitialMenu(),
                2 => Menus.SecondaryMenu(),
                3 => Menus.ViewMenu()
            };
        }

        private static async Task ConfigureOptionsAsync()
        {
            Console.WriteLine("Bem-vindo ao Parallel Tasker\nAntes, vamos configurar algumas coisas:");
            Console.WriteLine("Qual é a quantidade máxima de tarefas que podem ser executadas ao mesmo tempo? No mínimo 5 e no máximo 15.");
            int maxTarefas = Utils.ReadInteger(5, 15);
            Console.WriteLine("Ótimo, qual é o máximo de subtarefas que uma tarefa pode ter? No mínimo 2 e no máximo 10.");
            int maxSubtarefas = Utils.ReadInteger(2, 10);

            ConfigurationValues.SetValues(maxTarefas, maxSubtarefas, TipoArmazenamento.EmMemoria);

            Console.WriteLine("Beleza, tudo configurado. Aproveite a estadia!\nAguarde...");

            await Task.Delay(2000);

            Console.Clear();

        }

        private static void ShowProgress(ProcessadorTarefas.Servicos.IProcessadorTarefas processador)
        {
            AtualizarBarraDeProgresso(processador);
            
        }

        static async Task Iniciar(IProcessadorTarefas processador)
        {
            cancelaExibicao = new CancellationTokenSource();
            _running = true;
            await processador.Iniciar();
        }

        
        static async Task AtualizarBarraDeProgresso(ProcessadorTarefas.Servicos.IProcessadorTarefas processador)
        {
            while (_running)
            {
                if(cancelaExibicao.IsCancellationRequested) 
                    return;

                lock (consoleLock)
                {
                    Console.SetCursorPosition(0, 0);

                    var progressos = processador.GetProgressoTarefas();
                    if (progressos.Count == 0)
                        Console.WriteLine("Não há nenhuma tarefa em execução");
                    else
                    {
                        Console.WriteLine("Tarefas em execução:");

                        foreach (var item in progressos)
                        {
                            Console.WriteLine($"\n >> Tarefa {item.Id}: ");
                            double percentage = 0;
                            if (item.TempoDeTotalDeSubtarefas > 0)
                            {
                                percentage = (double)item.TempoDeSubtarefasExecutadas / item.TempoDeTotalDeSubtarefas * 100;
                            }
                            DisplayProgressBar(100, (int)percentage);
                        }
                    }
                    Console.SetCursorPosition(0, ConfigurationValues.QuantidadeDeTarefasExecutadas * 3 + 12);

                }
                await Task.Delay(1000);
               
            }
        }

        public static void DisplayProgressBar(int progressBarWidth, double completed)
        {
            int percentage = (int)Math.Round(completed);
            int filledBarLength = (int)(percentage / 100.0 * progressBarWidth);
            filledBarLength = Math.Max(0, filledBarLength);
            filledBarLength = Math.Min(progressBarWidth, filledBarLength);
            string bar = new string('=', filledBarLength) + new string(' ', progressBarWidth - filledBarLength);
            Console.Write("[");
            Console.ForegroundColor = GetColor(percentage);
            Console.Write(bar);
            Console.ResetColor();
            Console.Write("]");
            Console.WriteLine($" {percentage:0.00}%");
        }

        private static ConsoleColor GetColor(double percentage)
        {
            if (percentage < 30)
                return ConsoleColor.Red;

            if (percentage < 70)
                return ConsoleColor.Yellow;

            return ConsoleColor.Green;

        }
    }
}
