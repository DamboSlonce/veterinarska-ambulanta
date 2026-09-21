using System;
using System.Linq;

namespace Klase.Pomocne_klase
{
    public class InputSterilisanCheck
    {
        private readonly string[] pozitivniOdgovori = { "da", "jeste", "sterilisan", "sterilisana", "kastriran", "kastrirana", "yes", "true", "1" };
        private readonly string[] negativniOdgovori = { "ne", "nije", "no", "false", "0", "/" };

        public bool? proveraUnosaZaSterilizaciju(string unos)
        {
            if (string.IsNullOrWhiteSpace(unos))
                return false;

            string normalized = unos.Trim().ToLower();

            if (pozitivniOdgovori.Contains(normalized))
                return true;

            if (negativniOdgovori.Contains(normalized))
                return false;

            return null;
        }
    }
}
