﻿using Newtonsoft.Json;
using System.Linq;
using TestDbApi.Data;
using Nancy;

namespace TestDbApi.Controllers
{
    public class UsersModule : NancyModule
    {
        UsersContext db;
        public UsersModule(UsersContext context)
        {
            db = context;
            //Получить пользователей по id группы (если id = 0, то всех пользователей)
            Get["/api/v1/groups/{id}/users"] = p =>
            {
                int groupId = p.id;
                if(groupId == 0)
                {
                    return JsonConvert.SerializeObject(
                        db.Users.ToList().Select(u => new { u.Id, u.GroupId, u.Name, BirthDay = u.BirthDay.ToString("yyyy-MM-dd") })
                        );
                }
                else
                {
                    return JsonConvert.SerializeObject(
                        db.Users.Where(u => u.GroupId == groupId).ToList().Select(u => new { u.Id, u.GroupId, u.Name, BirthDay = u.BirthDay.ToString("yyyy-MM-dd") })
                        );
                }

            };

            //Удалить пользователя по id
            Post["/api/v1/users/{id}/delete"] = parameters => {
                int userId = parameters.id;
                var user = db.Users.Find(userId);
                db.Users.Remove(user);
                db.SaveChanges();
                return HttpStatusCode.OK;
            };

            //Добавить нового пользователя
            Post["/api/v1/users/add"] = parameters => {
                int userId = parameters.id;
                var user = db.Users.Find(userId);
                db.Users.Remove(user);
                return HttpStatusCode.OK;
            };
            //Обновить данные пользователя
            Post["/api/v1/users/{id}/update"] = parameters => {
                int userId = parameters.id;
                var user = db.Users.Find(userId);
                db.Users.Remove(user);
                return HttpStatusCode.OK;
            };
        }
    }
}
