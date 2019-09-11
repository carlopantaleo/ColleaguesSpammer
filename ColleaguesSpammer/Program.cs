using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using MailKit.Net.Smtp;
using MimeKit;

namespace ColleaguesSpammer {
    internal static class Program {
        private static void Main(string[] args) {
            try {
                DoMain(args);
            } catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }

        private static void DoMain(IReadOnlyList<string> args) {
            CheckAppSettings();
            (string email, int num, int interval) = ParseArgs(args);
            var messages = MakeMessages(email, num);
            SendMessages(messages, interval);
        }

        private static void CheckAppSettings() {
            const string missingAppSetting = "Missing AppSetting";

            if (ConfigurationManager.AppSettings["SmtpHost"] == null) {
                throw new ArgumentException($"{missingAppSetting} 'SmtpHost'");
            }

            if (ConfigurationManager.AppSettings["Sender"] == null) {
                throw new ArgumentException($"{missingAppSetting} 'Sender'");
            }
        }

        private static void SendMessages(IEnumerable<MimeMessage> messages, int interval) {
            using var client = new SmtpClient();
            client.Connect(ConfigurationManager.AppSettings["SmtpHost"],
                int.Parse(ConfigurationManager.AppSettings["SmtpPort"] ?? "0"));

            foreach (var message in messages) {
                client.Send(message);
                Console.WriteLine($"Sent mail. Subject: {message.Subject}");
                Thread.Sleep(interval * 1000);
            }

            client.Disconnect(true);
        }

        private static IEnumerable<MimeMessage> MakeMessages(string email, int num) {
            var messages = new List<MimeMessage>();
            var random = new Random();

            for (int i = 0; i < num; i++) {
                var message = new MimeMessage {
                    Subject = Phrases[0][random.Next(Phrases[0].Count)],
                    Body = new BodyBuilder {
                        HtmlBody = GenerateText(random)
                    }.ToMessageBody()
                };
                message.From.Add(InternetAddress.Parse(
                    $"{Names[random.Next(Names.Count)]} <{ConfigurationManager.AppSettings["Sender"]}>"));
                message.To.Add(InternetAddress.Parse(email));
                messages.Add(message);
            }

            return messages;
        }

        private static string GenerateText(Random random) {
            var sb = new StringBuilder("<p>Buongiorno.</p><p>");

            for (int i = 0; i < 3; i++) {
                sb
                    .Append(Phrases
                        .Select(elem => elem[random.Next(elem.Count)])
                        .Aggregate((a, b) => $"{a} {b}"))
                    .Append("<br/>");
            }

            sb
                .Append("</p><p>Cordiali saluti.</p><br/><br/>")
                .Append("<p>Generated with ❤ by ")
                .Append("<a href=\"https://github.com/carlopantaleo/ColleaguesSpammer\">ColleaguesSpammer</a></p>");

            return sb.ToString();
        }

        /// <summary>
        /// Parses the command-line arguments.
        /// </summary>
        /// <param name="args">The program arguments</param>
        /// <returns>
        ///     A tuple representing the mail address, the number of mails to send and the interval between the mails.
        /// </returns>
        /// <exception cref="ArgumentException">If arguments are invalid.</exception>
        private static (string, int, int) ParseArgs(IReadOnlyList<string> args) {
            const string usage =
                "Usage: ColleaguesSpammer <mail address> <number of mails> [<interval (in seconds)>]";

            if (args.Count < 2) {
                throw new ArgumentException(usage);
            }

            try {
                string email = args[0];
                int num = int.Parse(args[1]);
                int interval = args.Count > 2 ? int.Parse(args[2]) : 0;

                return (email, num, interval);
            } catch (Exception e) {
                throw new ArgumentException(usage, e);
            }
        }

        private static readonly List<List<string>> Phrases = new List<List<string>> {
            new List<string> {
                "L'utenza potenziale", "Il bisogno emergente", "Il quadro normativo", "La valenza epidemiologica",
                "Il nuovo soggeto sociale", "L'approccio programmatico", "L'assetto politico istituzionale",
                "Il criterio metodologico", "Il modello di sviluppo", "Il metodo partecipativo",
                "La configurazione epistemologica", "L'oggetto identificativo", "Il sostantivo auspicabile",
                "La volontà inconfutabile", "L'imprescindibile inconfutabilità dell'essere",
                "Il significato ultimo dell'incorreggibilità umana", "La catarsi vicendevole", "L'impianto antitetico"
            },
            new List<string> {
                "si caratterizza per", "privilegia", "prefigura", "riconduce a sintesi per", "persegue", "estrinseca",
                "si propone", "presuppone", "porta avanti", "auspica", "intende", "si analizza considerando", "estende",
                "difende", "offende", "annulla", "interessa"
            },
            new List<string> {
                "il ribaltamento della logica assistenziale preesistente",
                "il superamento di ogni ostacolo o resistenza passiva",
                "un organico collegamento interdisciplinare ed una prassi di lavoro di gruppo",
                "la puntuale corrispondenza tra obbiettivi e risorse",
                "la verifica critica degli obbiettivi istituzionali e l'individuazione di fini qualificanti",
                "il riorentamento delle linee di tendenza in atto",
                "l'accorpamento delle funzioni ed il decentramento decisionale",
                "la ricognizione del bisogno emergente e della domanda non soddisfatta",
                "la riconversione ed articolazione periferica dei servizi",
                "un corretto rapporto tra strutture e sovrastrutture",
                "un modello organizzativo di gruppo",
                "un supporto normativo paritetico",
                "una linea antropologica migliorativa volta agli interessi umani",
                "il progetto normativo assistenziale con fini ludici ambivalenti",
                "la conversazione articolata tra le parti",
                "il sunto epistemologico della diatriba palingenetica sull'origine non soddisfacente delle cose"
            },
            new List<string> {
                "nel primario interesse della popolazione", "senza pregiudicare l'attuale livello delle prestazioni",
                "al di sopra di interessi e pressioni di parte", "secondo un modulo di interdipendenza orizzontale",
                "in una visione organica e ricondotta a unità", "con criteri non dirigistici",
                "al di là delle contraddizioni e difficoltà iniziali", "in maniera articolata e non totalizzante",
                "attraverso i meccanismi della partecipazione", "senza precostituzione delle risposte",
                "con auspicabilità da parte dell'insieme delle parti", "con <i>se</i> e senza <i>ma</i>",
                "nell'ottica del comprendere e senza mai sottintendere"
            },
            new List<string> {
                "sostanziando e vitalizzando,", "recuperando ovvero rivalutando,", "ipotizzando e perseguendo,",
                "non assumendo mai come implicito,", "fattualizzando e concretizzando,",
                "non sottacendo ma anzi puntualizzando,", "potenziando ed incrementando,",
                "non dando certo per scontato,", "evidenziando ed esplicitando,", "attivando ed implementando,",
                "sviluppando e differenziando,", "ritrovando senza mezzi termini,",
                "venendo incontro alle esigenze costruttive,"
            },
            new List<string> {
                "nei tempi brevi, anzi brevissimi,", "in un ottica preventiva e non più curativa,",
                "in un ambito territoriale omogenico, ai diversi livelli,", "nel rispetto della normativa esistente,",
                "nel contesto di un sistema integrato,", "quale sua premessa indispensabile e condizionante,",
                "nella misura in cui ciò sia fattibile,", "con le dovute ed imprescindibili sottolineature,",
                "in termini di efficacia e di efficienza,", "a monte e a valle della situazione contingente,",
                "nella visione a 360 gradi della fattualità,"
            },
            new List<string> {
                "la trasparenza di ogni atto decisionale.", "la non sanitarizzazione delle risposte.",
                "un indispensabile salto di qualità.", "una congrua flessibilità delle strutture.",
                "l'annullamento di ogni ghettizzazione.", "il coinvolgimento attivo di operatori ed utenti.",
                "l'appianamento delle discrepanze e delle disgrazie esistenti.",
                "la ridefinizione di una nuova figura professionale.", "l'adozione di una metodologia differenziata.",
                "le demedicalizzazione del linguaggio.", "la determinazione unanime del modello integrativo unificato."
            }
        };

        private static readonly List<string> Names = new List<string> {
            "Kayden Hancock",
            "Jamya Mccullough",
            "Karissa Baird",
            "Corbin Simmons",
            "Brooklynn Sharp",
            "Azaria Mejia",
            "Robert Yu",
            "Mohammad Vargas",
            "Linda Monroe",
            "Monserrat Mcconnell",
            "Tyrese Mayer",
            "Ronan Hayes",
            "Miah King",
            "Shayla Contreras",
            "Regan Hayden",
            "Tyrone Richmond",
            "Lamont Huber",
            "Carina Rios",
            "Joe Holt",
            "Skyla Ortiz",
            "Alexa Holloway",
            "Brendon Ashley",
            "Bianca Gould",
            "Gianna Holder",
            "Adolfo Delacruz",
            "Chance Barrett",
            "Diamond Hodge",
            "Sadie Burch",
            "Jaydan Horne",
            "Nathanial Wade",
            "Rigoberto Washington",
            "Fernando Johnston",
            "Kian Zuniga",
            "London Baxter",
            "Darian Wood",
            "Brayan Winters",
            "Bradyn Archer",
            "Aydan Moore",
            "Vance Powers",
            "Corinne Boyd",
            "Xiomara Weber",
            "Jon Morton",
            "Ariella Ramirez",
            "Regina Chaney",
            "Zavier Swanson",
            "Tommy Douglas",
            "Cristopher Byrd",
            "Jaylen Reyes",
            "Meadow Bell",
            "Christopher Hammond",
            "Maeve Wiley",
            "Gordon Nunez",
            "Kiersten Murillo",
            "Jordan Mckee",
            "Bronson Cardenas",
            "Demarcus West",
            "Leilani Bowman",
            "Maeve Bean",
            "Beatrice Fernandez",
            "Genevieve Patton",
            "Marquise Richmond",
            "Taylor Lutz",
            "Charles Steele",
            "Siena Meadows",
            "Callum Sosa",
            "Paloma Hopkins",
            "Arianna Villarreal",
            "George Peterson",
            "Emanuel Thomas",
            "Laci Watkins",
            "Sydnee George",
            "Madisyn Benitez",
            "Lewis Lowery",
            "America Stevenson",
            "Marley Little",
            "Cale Bishop",
            "Odin Willis",
            "Jorge Newman",
            "Katherine Dickerson",
            "Myah Chen",
            "Lily Smith",
            "Curtis Navarro",
            "Yahir Miller",
            "Joey Macias",
            "Jason Blackwell",
            "Fernando Mejia",
            "Andrew Spence",
            "Mariela Erickson",
            "Jamar Perkins",
            "Tyson Campos",
            "Sidney Gardner",
            "Orion Benton",
            "Logan Smith",
            "Jaiden Simpson",
            "Paige Eaton",
            "Ruby Barron",
            "Kadyn Garrett",
            "Leslie Wang",
            "Samantha Colon",
            "Sabrina Hernandez"
        };
    }
}