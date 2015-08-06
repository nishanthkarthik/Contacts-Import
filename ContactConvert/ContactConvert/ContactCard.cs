using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ContactConvert
{
    class ContactCard
    {
        public string ContactName { get; private set; }
        public string MobileNo { get; private set; }

        public static List<ContactCard> ReadCsv(string fileName)
        {
            var contactCards = new List<ContactCard>();
            using (var streamReader = new StreamReader(fileName))
            {
                while (!streamReader.EndOfStream)
                {
                    var readLine = streamReader.ReadLine();
                    var strings = readLine?.Split(',');
                    if (strings?.Length > 1)
                        contactCards.Add(new ContactCard()
                        {
                            ContactName = strings.ElementAt(0),
                            MobileNo = strings.ElementAt(1)
                        });
                }
            }
            return contactCards;
        }
    }
}
