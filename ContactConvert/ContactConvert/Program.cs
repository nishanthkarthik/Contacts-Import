using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LumiSoft.Net.Mime.vCard;

namespace ContactConvert
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = args.FirstOrDefault();
            List<ContactCard> contactCards = ContactCard.ReadCsv(fileName);
            DirectoryInfo directory = Directory.CreateDirectory(DateTime.Now.Ticks.ToString());
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Exporting contacts into VCF...");
            Console.ForegroundColor = ConsoleColor.Gray;
            if (contactCards != null)
                using (TextProgressBar progressBar = new TextProgressBar(contactCards.Count))
                {
                    foreach (ContactCard card in contactCards)
                    {
                        vCard vCard = new vCard
                        {
                            Name = new Name(string.Empty, card.ContactName, string.Empty, string.Empty, string.Empty),
                            FormattedName = card.ContactName,
                        };
                        vCard.PhoneNumbers.Add(PhoneNumberType_enum.Cellular, card.MobileNo);
                        string pathToStore = Path.Combine(directory.FullName, card.ContactName + ".vcf");
                        vCard.ToFile(pathToStore);
                        progressBar.Increment();
                    } 
                }
            Console.WriteLine("Export complete");
        }
    }
}
