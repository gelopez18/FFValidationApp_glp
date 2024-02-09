using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;

namespace FFValidationApp_glp.Models
{
    public class CustomerModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long customerId {  get; set; }
        public string customerName { get; set; }
        public string customerPhone { get; set; }
        public string customerZipCode { get; set; }

        public static object CustomerCreation()
        {
           var res = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Would you like to create a [green]customer[/] or proceed as [blue]Guess[/]?")
                    .PageSize(10)
                    .MoreChoicesText("[grey](Move up and down [green] Enter to Pick)[/]")
                    .AddChoices(new List<string>()
                    {
                        "Guest",
                        "New Customer"
                    }));
            if (res== "New Customer")
            {
                var name = AnsiConsole.Ask<string>("What's the [green]Customer's name[/]?");
                var phone = AnsiConsole.Ask<string>("What's the [green]Customer's Phone number[/]?");
                var zipCode = AnsiConsole.Ask<string>("What's the [green]Customer's ZipCode[/]?");
                return new CustomerModel()
                {
                    customerName = name,
                    customerPhone = phone,
                    customerZipCode = zipCode
                };
            }
            else
            {
                var table = AnsiConsole.Ask<string>("What's the [green]Customer's Table[/]?");
                return new CustomerModel()
                {
                    customerName=$"Guest-T{table}",
                    customerPhone="NA",
                    customerZipCode="NA"
                };
            }
            
           
        }
    }
}
