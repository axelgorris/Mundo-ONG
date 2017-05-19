using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using Microsoft.Azure.Mobile.Server;
using Microsoft.Azure.Mobile.Server.Authentication;
using Microsoft.Azure.Mobile.Server.Config;
using NGODirectory.Backend.DataObjects;
using NGODirectory.Backend.Models;
using Owin;

namespace NGODirectory.Backend
{
    public partial class Startup
    {
        public static void ConfigureMobileApp(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();

            new MobileAppConfiguration()
                .AddTablesWithEntityFramework()
                .ApplyTo(config);

            // Use Entity Framework Code First to create database tables based on your DbContext
            Database.SetInitializer(new MobileServiceInitializer());

            MobileAppSettingsDictionary settings = config.GetMobileAppSettingsProvider().GetMobileAppSettings();

            if (string.IsNullOrEmpty(settings.HostName))
            {
                app.UseAppServiceAuthentication(new AppServiceAuthenticationOptions
                {
                    // This middleware is intended to be used locally for debugging. By default, HostName will
                    // only have a value when running in an App Service application.
                    SigningKey = ConfigurationManager.AppSettings["SigningKey"],
                    ValidAudiences = new[] { ConfigurationManager.AppSettings["ValidAudience"] },
                    ValidIssuers = new[] { ConfigurationManager.AppSettings["ValidIssuer"] },
                    TokenHandler = config.GetAppServiceTokenHandler()
                });
            }

            app.UseWebApi(config);
        }
    }

    public class MobileServiceInitializer : CreateDatabaseIfNotExists<MobileServiceContext>
    {
        protected override void Seed(MobileServiceContext context)
        {
            //List<Announcement> announcements1 = new List<Announcement>()
            //{
            //    new Announcement
            //    {
            //        Title = "Manos Unidas fija para el día 26 su cena solidaria anual",
            //        Description = "La delegación Manos Unidas de Xàtiva ha pedido la colaboración de empresas y particulares para la cena solidaria que cada año celebramos en la capital de la Costera con la finalidad de colaborar y hacer realidad los proyectos de la ONG católica para aliviar el hambre y la pobreza en el mundo. Este año dicha cena tendrá lugar en la sala Lluna Events de Xàtiva el próximo viernes 26 de mayo a las 21.30 horas. El precio del tique por persona es de 12 euros y está a la venta a través de las voluntarias de dicha organización en las distintas parroquias de Xàtiva y en la librería de la iglesia de San Francesc, informa Manos Unidas.",
            //        ImageUrl = "",
            //    },
            //    new Announcement
            //    {
            //        Title = "Manos Unidas Valdepeñas recaudó 612 euros con su VI Caminata Solidaria",
            //        Description = @"Manos Unidas Valdepeñas recaudó 612 euros con su VI Caminata Solidaria, que tuvo lugar el pasado sábado bajo el lema “El mundo no necesita más comida, necesita más gente comprometida”, eslogan de su campaña 58.  A la actividad asistieron unas 150 personas.
            //                        El recorrido se llevó a cabo desde la sede la Manos Unidas en la Plaza Constitución hasta el Parque Cervantes, donde hubo merienda, juegos, bailes, hinchables y sorteo de regalos.
            //                        Las dorsales tuvieron un precio de tres euros.",
            //        ImageUrl = "",
            //    },
            //    new Announcement
            //    {
            //        Title = "Eduardo Sánchez Arribas, nuevo presidente de Cruz Roja en Valladolid",
            //        Description = "Eduardo Sánchez Arribas ha tomado posesión esta mañana como presidente del Comité Provincial de Cruz Roja Española en Valladolid después de su nombramiento por parte del máximo representante de la Organización en la Comunidad, José Varela Rodríguez. Además del Presidente Autonómico, han asistido los vicepresidentes de Cruz Roja Española en Castilla y León, José Ignacio de Luis Páez y Jesús Juanes Galindo, así como varios presidentes provinciales de Castilla y León, miembros del Comité Provincial y directivos de la Organización, entre otros.",
            //        ImageUrl = "",
            //    },
            //};
            //List<Announcement> announcements2 = new List<Announcement>()
            //{
            //    new Announcement
            //    {
            //        Title = "Eduardo Sánchez Arribas, nuevo presidente de Cruz Roja en Valladolid",
            //        Description = "Eduardo Sánchez Arribas ha tomado posesión esta mañana como presidente del Comité Provincial de Cruz Roja Española en Valladolid después de su nombramiento por parte del máximo representante de la Organización en la Comunidad, José Varela Rodríguez. Además del Presidente Autonómico, han asistido los vicepresidentes de Cruz Roja Española en Castilla y León, José Ignacio de Luis Páez y Jesús Juanes Galindo, así como varios presidentes provinciales de Castilla y León, miembros del Comité Provincial y directivos de la Organización, entre otros.",
            //        ImageUrl = "",
            //    },
            //};
            //context.Set<Announcement>().AddRange(announcements1);
            //context.Set<Announcement>().AddRange(announcements2);

            List<Organization> organizations = new List<Organization>
            {
                new Organization {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Manos Unidas",
                    Description = "Manos Unidas es la Asociación de la Iglesia Católica en España para la ayuda, promoción y desarrollo del Tercer Mundo. Es, a su vez, una Organización No Gubernamental para el Desarrollo, (ONGD), de voluntarios, católica y seglar.",
                    Web = "www.manosunidas.org",
                    Email = "info@manosunidas.org",
                    Phone = "91 308 20 20",
                    Address = "C/ Barquillo, 38. 3º, 28013 Madrid, España",
                    Facebook = "https://es-la.facebook.com/manosunidas.ongd",
                    Twitter = "https://twitter.com/manosunidasongd",
                    Instagram = "https://www.instagram.com/manosunidas/",
                    //Announcements = announcements1,
                },
                new Organization {
                    Id = Guid.NewGuid().ToString(),
                    Name = "OXFAM Intermón",
                    Description = "Oxfam Intermón somos personas que luchamos, con y para las poblaciones desfavorecidas y como parte de un amplio movimiento global, con el objetivo de erradicar la injusticia y la pobreza, y para lograr que todos los seres humanos puedan ejercer plenamente sus derechos y disfrutar de una vida digna.",
                    Web = "http://www.oxfamintermon.org/",
                    Email = "sedebcn1@oxfamintermon.org",
                    Phone = "93 482 07 00",
                    Address = "Gran Via de les Corts Catalanes, 641. C.P 08010, Barcelona, España",
                    Facebook = "https://es-la.facebook.com/OxfamIntermon/",
                    Twitter = "https://twitter.com/oxfamintermon",
                    Instagram = "https://www.instagram.com/oxfamintermon/",
                    //Announcements = announcements2,
                },
                new Organization {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Cruz Roja Española",
                    Description = "Es una institución humanitaria cuyos objetivos son la búsqueda y el fomento de la paz, la cooperación nacional e internacional, la difusión y enseñanza del Derecho Internacional Humanitario; la defensa de los Derechos Humanos; la ayuda a las víctimas en situaciones de conflicto, accidentes o catástrofes; la atención a todas las personas que sufren; la promoción y colaboración en acciones de solidaridad, de cooperación al desarrollo y de bienestar social; el desarrollo de actividades formativas para conseguir la paz, el mutuo respeto y el entendimiento entre todos los hombres.",
                    Web = "http://www.cruzroja.es",
                    Email = "informa@cruzroja.es",
                    Phone = "902 22 22 92",
                    Address = "Avenida Reina Victoria, 26, 28003 Madrid, España",
                    Facebook = "https://es-es.facebook.com/CruzRoja.es/",
                    Twitter = "https://twitter.com/cruzrojaesp",
                    Instagram = "https://www.instagram.com/cruzrojaesp/",
                    //Announcements = new List<Announcement>(),
                },
            };

            foreach (Organization organization in organizations)
            {
                context.Set<Organization>().Add(organization);
            }

            base.Seed(context);
        }
    }
}

