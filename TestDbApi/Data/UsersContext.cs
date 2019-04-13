namespace TestDbApi.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.IO;
    using System.Linq;


    //Генератор случаных дат (для дней рождений)
    static class DateGenerator
    {
        private static Random gen = new Random();
        public static DateTime RandomDay()
        {
            DateTime start = new DateTime(1980, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range));
        }
    }

    //Инициализатор базы данных (заполняем начальными данными)
    class MyContextInitializer : CreateDatabaseIfNotExists<UsersContext>
    {
        //Добавим началные значения в базу
        protected override void Seed(UsersContext db)
        {
            //Заполним таблиццу с группами пользователей
            db.Groups.Add(new Group() { Name = "Оператор" });
            db.Groups.Add(new Group() { Name = "Администратор" });
            db.Groups.Add(new Group() { Name = "Кассир" });
            db.Groups.Add(new Group() { Name = "Программист" });
            db.Groups.Add(new Group() { Name = "Охрана" });
            db.SaveChanges();

            //Прочитаем имена из файла
            string[] names = File.ReadAllLines("Names.txt");
            //Для случаного выбора группы пользователя
            Random gen = new Random();
            //Добавляем всех пользователей
            foreach (var name in names)
            {
                db.Users.Add(new User()
                {
                    Name = name,
                    BirthDay = DateGenerator.RandomDay(),
                    GroupId = gen.Next(5) + 1
                });
            }
            db.SaveChanges();
        }
    }

    public class UsersContext : DbContext
    {
        //Для инициализации
        static UsersContext()
        {
            Database.SetInitializer<UsersContext>(new MyContextInitializer());
        }

        public UsersContext()
            : base("name=UsersContext")
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }

        public int GroupId { get; set; }
        public virtual Group Group { get; set; }
        
        public int Age
        {
            get =>
            (DateTime.Now.DayOfYear >= BirthDay.DayOfYear) ?
            (DateTime.Now.Year - BirthDay.Year) :
            (DateTime.Now.Year - BirthDay.Year - 1);
        }
    }

    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual List<User> Users { get; set; }

        public Group()
        {
            Users = new List<User>();
        }
    }
}