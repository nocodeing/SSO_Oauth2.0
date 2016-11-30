using Common.Base;
using OauthService.IBussiness;
using System;
using CommonTools;
using OauthService.Common.ViewModel;
using Master.IRepository;
using Model;

namespace OauthService.Bussiness
{
    public class UsersBussiness : BussinessBase, IUsersBussiness
    {
        public UsersModel GetModel(string userName, string password)
        {
            using (FactoryProxy)
            {
                var entity = FactoryRepository.CreateInstance<IUsersRepository>().Find(userName, password);
                return new UsersModel() {
                    UserName=entity.UserName,
                    Password=entity.PasswordHash,
                };
            }
        }

        public ReturnResult Register(UsersModel model)
        {
            using (FactoryProxy)
            {
                if (model == null)
                {
                    return new ReturnResult() { Flag = false, Message = "没有需要注册的用户信息" };
                }
                if (string.IsNullOrEmpty(model.UserName))
                {
                    return new ReturnResult() { Flag = false, Message = "用户名不能为空" };
                }
                if (string.IsNullOrEmpty(model.Password))
                {
                    return new ReturnResult() { Flag = false, Message = "密码不能为空" };
                }
                if (string.IsNullOrEmpty(model.ConfirmPassword))
                {
                    return new ReturnResult() { Flag = false, Message = "验证密码不能为空" };
                }
                if (model.ConfirmPassword != model.Password)
                {
                    return new ReturnResult() { Flag = false, Message = "两次密码输入不一致" };
                }
                var entity = new Users()
                {
                    Id = DateTime.Now.ToString("yyyyMMddHHmmssfff") + new Random().Next(100, 999),
                    UserName = model.UserName,
                    EmailConfirmed = false,
                    PasswordHash = model.Password,
                };
                var userRes = FactoryRepository.CreateInstance<IUsersRepository>();
                if (userRes.Insert(entity) != 1)
                {
                    return new ReturnResult() { Flag = false, Message = "注册失败" };
                }
                return new ReturnResult() { Flag = true, Message = "注册成功" };
            }

        }
    }
}
