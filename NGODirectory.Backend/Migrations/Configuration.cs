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
            //        Description = "Manos Unidas es la Asociaci�n de la Iglesia Cat�lica en Espa�a para la ayuda, promoci�n y desarrollo del Tercer Mundo. Es, a su vez, una Organizaci�n No Gubernamental para el Desarrollo, (ONGD), de voluntarios, cat�lica y seglar.",
            //        Web = "www.manosunidas.org",
            //        Email = "info@manosunidas.org",
            //        Phone = "91 308 20 20",
            //        Address = "C/ Barquillo, 38. 3�, 28013 Madrid, Espa�a",
            //        Facebook = "https://es-la.facebook.com/manosunidas.ongd",
            //        Twitter = "https://twitter.com/manosunidasongd",
            //        Instagram = "https://www.instagram.com/manosunidas/",
            //    },
            //    new Organization {
            //        Id = Guid.NewGuid().ToString(),
            //        Name = "OXFAM Interm�n",
            //        Description = "Oxfam Interm�n somos personas que luchamos, con y para las poblaciones desfavorecidas y como parte de un amplio movimiento global, con el objetivo de erradicar la injusticia y la pobreza, y para lograr que todos los seres humanos puedan ejercer plenamente sus derechos y disfrutar de una vida digna.",
            //        Web = "http://www.oxfamintermon.org/",
            //        Email = "sedebcn1@oxfamintermon.org",
            //        Phone = "93 482 07 00",
            //        Address = "Gran Via de les Corts Catalanes, 641. C.P 08010, Barcelona, Espa�a",
            //        Facebook = "https://es-la.facebook.com/OxfamIntermon/",
            //        Twitter = "https://twitter.com/oxfamintermon",
            //        Instagram = "https://www.instagram.com/oxfamintermon/",
            //    },
            //    new Organization {
            //        Id = Guid.NewGuid().ToString(),
            //        Name = "Cruz Roja Espa�ola",
            //        Description = "Es una instituci�n humanitaria cuyos objetivos son la b�squeda y el fomento de la paz, la cooperaci�n nacional e internacional, la difusi�n y ense�anza del Derecho Internacional Humanitario; la defensa de los Derechos Humanos; la ayuda a las v�ctimas en situaciones de conflicto, accidentes o cat�strofes; la atenci�n a todas las personas que sufren; la promoci�n y colaboraci�n en acciones de solidaridad, de cooperaci�n al desarrollo y de bienestar social; el desarrollo de actividades formativas para conseguir la paz, el mutuo respeto y el entendimiento entre todos los hombres.",
            //        Web = "http://www.cruzroja.es",
            //        Email = "informa@cruzroja.es",
            //        Phone = "902 22 22 92",
            //        Address = "Avenida Reina Victoria, 26, 28003 Madrid, Espa�a",
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
            //        Title = "Manos Unidas fija para el d�a 26 su cena solidaria anual",
            //        Description = "La delegaci�n Manos Unidas de X�tiva ha pedido la colaboraci�n de empresas y particulares para la cena solidaria que cada a�o celebramos en la capital de la Costera con la finalidad de colaborar y hacer realidad los proyectos de la ONG cat�lica para aliviar el hambre y la pobreza en el mundo. Este a�o dicha cena tendr� lugar en la sala Lluna Events de X�tiva el pr�ximo viernes 26 de mayo a las 21.30 horas. El precio del tique por persona es de 12 euros y est� a la venta a trav�s de las voluntarias de dicha organizaci�n en las distintas parroquias de X�tiva y en la librer�a de la iglesia de San Francesc, informa Manos Unidas.",
            //    },
            //    new Announcement
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        Title = "Manos Unidas Valdepe�as recaud� 612 euros con su VI Caminata Solidaria",
            //        Description = @"Manos Unidas Valdepe�as recaud� 612 euros con su VI Caminata Solidaria, que tuvo lugar el pasado s�bado bajo el lema �El mundo no necesita m�s comida, necesita m�s gente comprometida�, eslogan de su campa�a 58.  A la actividad asistieron unas 150 personas.\r\nEl recorrido se llev� a cabo desde la sede la Manos Unidas en la Plaza Constituci�n hasta el Parque Cervantes, donde hubo merienda, juegos, bailes, hinchables y sorteo de regalos.\r\nLas dorsales tuvieron un precio de tres euros.",
            //    },
            //    new Announcement
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        Title = "Eduardo S�nchez Arribas, nuevo presidente de Cruz Roja en Valladolid",
            //        Description = "Eduardo S�nchez Arribas ha tomado posesi�n esta ma�ana como presidente del Comit� Provincial de Cruz Roja Espa�ola en Valladolid despu�s de su nombramiento por parte del m�ximo representante de la Organizaci�n en la Comunidad, Jos� Varela Rodr�guez. Adem�s del Presidente Auton�mico, han asistido los vicepresidentes de Cruz Roja Espa�ola en Castilla y Le�n, Jos� Ignacio de Luis P�ez y Jes�s Juanes Galindo, as� como varios presidentes provinciales de Castilla y Le�n, miembros del Comit� Provincial y directivos de la Organizaci�n, entre otros.",
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