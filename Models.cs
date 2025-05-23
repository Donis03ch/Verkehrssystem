using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

public class Kunde
{
    public int KundeId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
    public string Adresse { get; set; }
    public IList<Abonnement> Abonnements { get; set; } = new List<Abonnement>();
    public IList<Ticket> Tickets { get; set; } = new List<Ticket>();
}

public class Abonnement
{
    public int AbonnementId { get; set; }
    [Required]
    public int KundeId { get; set; }
    [Required]
    public string Typ { get; set; }
    [Required]
    public DateTime GueltigBis { get; set; }
    public Kunde Kunde { get; set; }
}

public class Ticket
{
    public int TicketId { get; set; }
    [Required]
    public int KundeId { get; set; }
    [Required]
    public DateTime Kaufdatum { get; set; }
    [Required]
    public decimal Preis { get; set; }
    public Kunde Kunde { get; set; }
}