using MicroService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MicroService.Business
{
    public class UserService : IUserService
    {

        private List<UserModel> UserList => new List<UserModel>() {
                    new UserModel(){
                         Id = "1",
                         Age = 18,
                         Name = "fang"
                     },
                    new UserModel()
                    {
                        Id = "2",
                        Age = 22,
                        Name = "张三"
                    },
                    new UserModel()
                    {
                        Id = "3",
                        Age = 25,
                        Name = "guo姐"
                    }
            };
        public Task<UserModel> Get(string id)
        {
            var userInfo = UserList.FirstOrDefault(t => t.Id == id);
            if (userInfo == null)
                userInfo = UserList.FirstOrDefault();

            return Task.FromResult(userInfo);
        }

        public Task<List<UserModel>> GetList()
        {
            return Task.FromResult(UserList);
        }
    }

    public interface IUserService
    {
        Task<List<UserModel>> GetList();
        Task<UserModel> Get(string id);
    }
}
