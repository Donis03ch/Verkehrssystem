using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<VerkehrssystemContext>();
        optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        using (var context = new VerkehrssystemContext(optionsBuilder.Options))
        {
            context.Database.EnsureCreated(); // Erstellt die Datenbank, falls sie nicht existiert

            while (true)
            {
                Console.WriteLine("\nVerkehrssystem Menü:");
                Console.WriteLine("1. Kunde hinzufügen");
                Console.WriteLine("2. Kunde anzeigen");
                Console.WriteLine("3. Abonnement hinzufügen");
                Console.WriteLine("4. Ticket hinzufügen");
                Console.WriteLine("5. Alle Kunden anzeigen");
                Console.WriteLine("6. Beenden");
                Console.Write("Auswahl: ");

                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddKunde(context);
                        break;
                    case "2":
                        ShowKunde(context);
                        break;
                    case "3":
                        AddAbonnement(context);
                        break;
                    case "4":
                        AddTicket(context);
                        break;
                    case "5":
                        ShowAllKunden(context);
                        break;
                    case "6":
                        return;
                    default:
                        Console.WriteLine("Ungültige Eingabe!");
                        break;
                }
            }
        }
    }

    static void AddKunde(VerkehrssystemContext context)
    {
        Console.Write("Name: ");
        string name = Console.ReadLine();
        Console.Write("Email: ");
        string email = Console.ReadLine();
        Console.Write("Adresse: ");
        string adresse = Console.ReadLine();

        var kunde = new Kunde { Name = name, Email = email, Adresse = adresse };
        context.Kunden.Add(kunde);
        context.SaveChanges();
        Console.WriteLine("Kunde hinzugefügt!");
    }

    static void ShowKunde(VerkehrssystemContext context)
    {
        Console.Write("Kunden-ID eingeben: ");
        if (int.TryParse(Console.ReadLine(), out int id))
        {
            var kunde = context.Kunden
                .Include(k => k.Abonnements)
                .Include(k => k.Tickets)
                .FirstOrDefault(k => k.KundeId == id);

            if (kunde != null)
            {
                Console.WriteLine($"Kunde: {kunde.Name}, Email: {kunde.Email}, Adresse: {kunde.Adresse}");
                Console.WriteLine("Abonnements:");
                foreach (var abo in kunde.Abonnements)
                {
                    Console.WriteLine($" - {abo.Typ}, Gültig bis: {abo.GueltigBis.ToShortDateString()}");
                }
                Console.WriteLine("Tickets:");
                foreach (var ticket in kunde.Tickets)
                {
                    Console.WriteLine($" - Kaufdatum: {ticket.Kaufdatum.ToShortDateString()}, Preis: {ticket.Preis:C}");
                }
            }
            else
            {
                Console.WriteLine("Kunde nicht gefunden!");
            }
        }
        else
        {
            Console.WriteLine("Ungültige ID!");
        }
    }

    static void AddAbonnement(VerkehrssystemContext context)
    {
        Console.Write("Kunden-ID: ");
        if (int.TryParse(Console.ReadLine(), out int kundeId))
        {
            if (context.Kunden.Any(k => k.KundeId == kundeId))
            {
                Console.Write("Typ (z.B. Monatskarte): ");
                string typ = Console.ReadLine();
                Console.Write("Gültig bis (YYYY-MM-DD): ");
                if (DateTime.TryParse(Console.ReadLine(), out DateTime gueltigBis))
                {
                    var abo = new Abonnement { KundeId = kundeId, Typ = typ, GueltigBis = gueltigBis };
                    context.Abonnements.Add(abo);
                    context.SaveChanges();
                    Console.WriteLine("Abonnement hinzugefügt!");
                }
                else
                {
                    Console.WriteLine("Ungültiges Datum!");
                }
            }
            else
            {
                Console.WriteLine("Kunde nicht gefunden!");
            }
        }
        else
        {
            Console.WriteLine("Ungültige ID!");
        }
    }

    static void AddTicket(VerkehrssystemContext context)
    {
        Console.Write("Kunden-ID: ");
        if (int.TryParse(Console.ReadLine(), out int kundeId))
        {
            if (context.Kunden.Any(k => k.KundeId == kundeId))
            {
                Console.Write("Preis: ");
                if (decimal.TryParse(Console.ReadLine(), out decimal preis))
                {
                    var ticket = new Ticket { KundeId = kundeId, Kaufdatum = DateTime.Now, Preis = preis };
                    context.Tickets.Add(ticket);
                    context.SaveChanges();
                    Console.WriteLine("Ticket hinzugefügt!");
                }
                else
                {
                    Console.WriteLine("Ungültiger Preis!");
                }
            }
            else
            {
                Console.WriteLine("Kunde nicht gefunden!");
            }
        }
        else
        {
            Console.WriteLine("Ungültige ID!");
        }
    }

    static void ShowAllKunden(VerkehrssystemContext context)
    {
        var kunden = context.Kunden.ToList();
        if (kunden.Any())
        {
            foreach (var kunde in kunden)
            {
                Console.WriteLine($"ID: {kunde.KundeId}, Name: {kunde.Name}, Email: {kunde.Email}");
            }
        }
        else
        {
            Console.WriteLine("Keine Kunden gefunden!");
        }
    }
}