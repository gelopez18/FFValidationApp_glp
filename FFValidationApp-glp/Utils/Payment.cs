using FFValidationApp_glp.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFValidationApp_glp.Utils
{
    public class Payment
    {
        public static void ProcessPayment(OrdersModel order)
        {
            var cc = AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter [green]Credit Card Number[/]?")
                    .PromptStyle("red")
                    .Secret());
          var exp =  AnsiConsole.Ask<string>("Experation Date:");
          var cvv = AnsiConsole.Prompt(
            new TextPrompt<string>("CVV:")
                .PromptStyle("red")
                .Secret());
            SendingDataToBankAsync(order.Total, cc, exp, cvv);            
        }

        private static void SendingDataToBankAsync(double total, string cc, string exp, string cvv)
        {
            AnsiConsole.Progress()
                        .Start(ctx =>
                        {
                            var task1 = ctx.AddTask("[blue]Processing Payment through the Bank[/]");
                          

                            for (int i = 0; i < 100; i++)
                            {
                                Thread.Sleep(50); 
                                task1.Increment(1); 
                            }
                        });
        }
    }
}
