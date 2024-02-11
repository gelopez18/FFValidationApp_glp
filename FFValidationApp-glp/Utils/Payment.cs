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
            var res = SendingDataToBankAsync(order.Total, cc, exp, cvv);            
        }

        private static async Task SendingDataToBankAsync(double total, string cc, string cvv, string exp)
        {
            await AnsiConsole.Progress()
                 .StartAsync(async ctx =>
                 {

                     var task1 = ctx.AddTask("[blue]Sending Payment Waiting for the bank[/]");                     
                     while (!ctx.IsFinished)
                     {
                         await Task.Delay(300);
                         task1.Increment(1.5);                        
                     }
                 });
        }
    }
}
