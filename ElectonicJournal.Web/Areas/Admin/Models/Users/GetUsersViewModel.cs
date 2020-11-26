using ElectronicJournal.Application.Authorization.Users.Dto.User;
using ElectronicJournal.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElectronicJournal.Web.Areas.Admin.Models.Users
{
    public class GetUsersViewModel
    {
        public GetUsersInput Input { get; set; }
        public ListResultDto<UserItemDto> Value { get; set; }

        public GetUsersViewModel()
        {
            Input = new GetUsersInput();
            Value = new ListResultDto<UserItemDto>();
        }
    }
}
