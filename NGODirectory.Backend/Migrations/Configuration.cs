namespace NGODirectory.Backend.Migrations
{
    using Microsoft.Azure.Mobile.Server;
    using Microsoft.Azure.Mobile.Server.Tables;
    using NGODirectory.Backend.DataObjects;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Data.Entity.Validation;
    using System.Diagnostics;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NGODirectory.Backend.Models.MobileServiceContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "NGODirectory.Backend.Models.MobileServiceContext";
            SetSqlGenerator("System.Data.SqlClient", new EntityTableSqlGenerator());
        }

        protected override void Seed(NGODirectory.Backend.Models.MobileServiceContext context)
        {
            //  You can use the DbSet<T>.AddOrUpdate() helper extension method to avoid creating duplicate seed data. E.g.
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );

            try
            {
            //    List<Organization> organizations = new List<Organization>
            //{
            //    new Organization {
            //        Id = Guid.NewGuid().ToString(),
            //        Name = "Manos Unidas",
            //        Description = "Manos Unidas es la Asociación de la Iglesia Católica en España para la ayuda, promoción y desarrollo del Tercer Mundo. Es, a su vez, una Organización No Gubernamental para el Desarrollo, (ONGD), de voluntarios, católica y seglar.",
            //        Web = "www.manosunidas.org",
            //        Email = "info@manosunidas.org",
            //        Phone = "91 308 20 20",
            //        Address = "C/ Barquillo, 38. 3º, 28013 Madrid, España",
            //        Facebook = "https://es-la.facebook.com/manosunidas.ongd",
            //        Twitter = "https://twitter.com/manosunidasongd",
            //        Instagram = "https://www.instagram.com/manosunidas/",
            //    },
            //    new Organization {
            //        Id = Guid.NewGuid().ToString(),
            //        Name = "OXFAM Intermón",
            //        Description = "Oxfam Intermón somos personas que luchamos, con y para las poblaciones desfavorecidas y como parte de un amplio movimiento global, con el objetivo de erradicar la injusticia y la pobreza, y para lograr que todos los seres humanos puedan ejercer plenamente sus derechos y disfrutar de una vida digna.",
            //        Web = "http://www.oxfamintermon.org/",
            //        Email = "sedebcn1@oxfamintermon.org",
            //        Phone = "93 482 07 00",
            //        Address = "Gran Via de les Corts Catalanes, 641. C.P 08010, Barcelona, España",
            //        Facebook = "https://es-la.facebook.com/OxfamIntermon/",
            //        Twitter = "https://twitter.com/oxfamintermon",
            //        Instagram = "https://www.instagram.com/oxfamintermon/",
            //    },
            //    new Organization {
            //        Id = Guid.NewGuid().ToString(),
            //        Name = "Cruz Roja Española",
            //        Description = "Es una institución humanitaria cuyos objetivos son la búsqueda y el fomento de la paz, la cooperación nacional e internacional, la difusión y enseñanza del Derecho Internacional Humanitario; la defensa de los Derechos Humanos; la ayuda a las víctimas en situaciones de conflicto, accidentes o catástrofes; la atención a todas las personas que sufren; la promoción y colaboración en acciones de solidaridad, de cooperación al desarrollo y de bienestar social; el desarrollo de actividades formativas para conseguir la paz, el mutuo respeto y el entendimiento entre todos los hombres.",
            //        Web = "http://www.cruzroja.es",
            //        Email = "informa@cruzroja.es",
            //        Phone = "902 22 22 92",
            //        Address = "Avenida Reina Victoria, 26, 28003 Madrid, España",
            //        Facebook = "https://es-es.facebook.com/CruzRoja.es/",
            //        Twitter = "https://twitter.com/cruzrojaesp",
            //        Instagram = "https://www.instagram.com/cruzrojaesp/",
            //    },
            //};

            //    foreach (Organization organization in organizations)
            //    {
            //        AddOrUpdateOrganization(context.Organizations, organization);
            //    }

            //    #region announcements
            //    List<Announcement> announcements = new List<Announcement>()
            //{
            //    new Announcement
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        Title = "Manos Unidas fija para el día 26 su cena solidaria anual",
            //        Description = "La delegación Manos Unidas de Xàtiva ha pedido la colaboración de empresas y particulares para la cena solidaria que cada año celebramos en la capital de la Costera con la finalidad de colaborar y hacer realidad los proyectos de la ONG católica para aliviar el hambre y la pobreza en el mundo. Este año dicha cena tendrá lugar en la sala Lluna Events de Xàtiva el próximo viernes 26 de mayo a las 21.30 horas. El precio del tique por persona es de 12 euros y está a la venta a través de las voluntarias de dicha organización en las distintas parroquias de Xàtiva y en la librería de la iglesia de San Francesc, informa Manos Unidas.",
            //    },
            //    new Announcement
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        Title = "Manos Unidas Valdepeñas recaudó 612 euros con su VI Caminata Solidaria",
            //        Description = @"Manos Unidas Valdepeñas recaudó 612 euros con su VI Caminata Solidaria, que tuvo lugar el pasado sábado bajo el lema “El mundo no necesita más comida, necesita más gente comprometida”, eslogan de su campaña 58.  A la actividad asistieron unas 150 personas.\r\nEl recorrido se llevó a cabo desde la sede la Manos Unidas en la Plaza Constitución hasta el Parque Cervantes, donde hubo merienda, juegos, bailes, hinchables y sorteo de regalos.\r\nLas dorsales tuvieron un precio de tres euros.",
            //    },
            //    new Announcement
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        Title = "Eduardo Sánchez Arribas, nuevo presidente de Cruz Roja en Valladolid",
            //        Description = "Eduardo Sánchez Arribas ha tomado posesión esta mañana como presidente del Comité Provincial de Cruz Roja Española en Valladolid después de su nombramiento por parte del máximo representante de la Organización en la Comunidad, José Varela Rodríguez. Además del Presidente Autonómico, han asistido los vicepresidentes de Cruz Roja Española en Castilla y León, José Ignacio de Luis Páez y Jesús Juanes Galindo, así como varios presidentes provinciales de Castilla y León, miembros del Comité Provincial y directivos de la Organización, entre otros.",
            //    },
            //};

            //    foreach (Announcement announcement in announcements)
            //    {
            //        AddOrUpdateAnnouncement(context.Announcements, announcement);
            //    }

            //    #endregion

                base.Seed(context);
            }
            catch (DbEntityValidationException e)
            {
                
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }

        private void AddOrUpdateOrganization<T>(DbSet<T> set, T item) where T : Organization
        {
            var existing = set.Where(i => i.Name == item.Name).FirstOrDefault();
            if (existing != null)
            {
                item.CreatedAt = existing.CreatedAt;
            }
            set.AddOrUpdate(i => i.Name, item);
        }

        private void AddOrUpdateAnnouncement<T>(DbSet<T> set, T item) where T : Announcement
        {
            var existing = set.Where(i => i.Title == item.Title).FirstOrDefault();
            if (existing != null)
            {
                item.CreatedAt = existing.CreatedAt;
            }
            set.AddOrUpdate(i => i.Title, item);
        }
    }
}