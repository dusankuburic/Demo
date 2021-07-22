using System;
using System.Collections.Generic;
using System.Linq;
using TransportServer.Models;

namespace TransportServer.Data
{
    public class ItemSeed
    {
        public static void Do(TransportContext ctx)
        {
            if (!ctx.Items.Any())
            {
                var items = new List<Item>
                {
                    new Item
                    {
                        Id = 1,
                        Name = "Germanijum",
                        Description = "Otkriven je 1886 godine od strane Clemens Alexander Winklera",
                        Price = 1001.34f,
                        HazardStatus = RiskStatus.MODERATE,
                        DamageStatus = RiskStatus.LOW,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Item
                    {
                        Id = 2,
                        Name = "Fluor",
                        Description = "Elementarni fluor je gas žutozelene boje. On je najreaktivniji od svih elemenata. Direktno reaguje na sve metale i nemetale.",
                        Price = 300.11f,
                        HazardStatus = RiskStatus.LOW,
                        DamageStatus = RiskStatus.HIGH,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Item
                    {
                        Id = 3,
                        Name = "Volfram",
                        Description = "Volfram je metal prelazi metal VIB grupe. Ime je dobio po nemačkoj reči Wolfram koja označava bezvredan metal",
                        Price = 808,
                        HazardStatus = RiskStatus.MODERATE,
                        DamageStatus = RiskStatus.HIGH,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Item
                    {
                        Id = 4,
                        Name = "Polonijum",
                        Description = "Polonijum se u zemljinoj kori nalazi u tragovima i to samo u rudama urana.",
                        Price = 603.77f,
                        HazardStatus = RiskStatus.HIGH,
                        DamageStatus = RiskStatus.HIGH,
                        CreatedAt = DateTime.UtcNow
                    },
                    new Item
                    {
                        Id = 5,
                        Name = "Hrom",
                        Description = "Hrom je zastupljen u zemljinoj kori u količini od oko 102 ppm.",
                        Price = 710.55f,
                        HazardStatus = RiskStatus.LOW,
                        DamageStatus = RiskStatus.HIGH,
                        CreatedAt = DateTime.UtcNow
                    },

                };

                ctx.Items.AddRange(items);
                ctx.SaveChanges();
            }
        }
    }
}
