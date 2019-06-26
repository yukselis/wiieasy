using System;
using System.Linq;
using Arwend.Web.View.Mvc.Models.Base;
using AutoMapper;
using Dalowe.Domain.Management;
using Dalowe.Domain.Visa;
using Dalowe.View.Web.Console.Models;
using Dalowe.View.Web.Framework;
using Dalowe.View.Web.Framework.Models.Account;
using Dalowe.View.Web.Framework.Models.Management;
using Dalowe.View.Web.Framework.Models.Visa;

namespace Dalowe.View.Web.Console
{
    public static class AutoMapperConfig
    {
        public static void Load(Client client)
        {
            Mapper.CreateMap<byte, bool>().ConvertUsing(b => b > 0 ? true : false);
            Mapper.CreateMap<bool, byte>().ConvertUsing(b => (byte)(b ? 1 : 0));
            Mapper.CreateMap<string, bool>().ConvertUsing(s => string.IsNullOrEmpty(s) ? false : bool.Parse(s));

            #region "Management"

            Mapper.CreateMap<Campaign, CampaignModel>()
                .AfterMap((src, dest) =>
                {
                    dest.CreateHistory(1, src.DateCreated, 0, src.DateModified);
                    //dest.History.UserCreatedName = client.Services.API.Visa.GetUser(src.UserCreatedID)?.Name;
                    //dest.History.UserModifiedName = client.Services.API.Visa.GetUser(src.UserModifiedID)?.Name;
                });
            Mapper.CreateMap<CampaignModel, Campaign>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.History.DateCreated))
                .AfterMap((src, dest) => dest.DateModified = DateTime.Now);

            Mapper.CreateMap<AccessNode, AccessNodeModel>()
                .AfterMap((src, dest) =>
                {
                    dest.CreateHistory(1, src.DateCreated, 0, src.DateModified);
                    //dest.History.UserCreatedName = client.Services.API.Visa.GetUser(src.UserCreatedID)?.Name;
                    //dest.History.UserModifiedName = client.Services.API.Visa.GetUser(src.UserModifiedID)?.Name;
                });
            Mapper.CreateMap<AccessNodeModel, AccessNode>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.History.DateCreated))
                .AfterMap((src, dest) => dest.DateModified = DateTime.Now);

            #endregion

            #region "Visa"

            Mapper.CreateMap<User, UserModel>()
                .AfterMap((src, dest) =>
                {
                    dest.CreateHistory(1, src.DateCreated, 0, src.DateModified);
                    //dest.History.UserCreatedName = client.Services.API.Visa.GetUser(src.UserCreatedID)?.Name;
                    //dest.History.UserModifiedName = client.Services.API.Visa.GetUser(src.UserModifiedID)?.Name;
                });
            Mapper.CreateMap<UserModel, User>()
                .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.History.DateCreated))
                .AfterMap((src, dest) => dest.DateModified = DateTime.Now);

            #endregion

            #region "Account"

            Mapper.CreateMap<User, LockedLoginModel>()
                .ForMember(dest => dest.Password, opt => opt.Ignore())
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));
            Mapper.CreateMap<User, ViewProfileModel>();
            Mapper.CreateMap<ViewProfileModel, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => client.CurrentUser.Password));
            //.ForMember(dest => dest.VisitCount, opt => opt.MapFrom(src => client.CurrentUser.VisitCount))
            //.ForMember(dest => dest.LastVisitDate, opt => opt.MapFrom(src => client.CurrentUser.LastVisitDate));

            #endregion
        }
    }
}