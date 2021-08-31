﻿using Application.Features.UserAccount.Commands;
using Application.Features.UserAccount.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [AllowAnonymous]
    public class UserDevicesController : BaseApiController
    {
        private const string baseRoute = "fcm-tokens";

        /// <summary>
        /// Add / update user device
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(baseRoute)]
        public async Task<IActionResult> AddUpdateDevice(AddUpdateUserDevice.Command command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Get paginated list of user devices
        /// </summary>
        /// <param name="page_number"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        [HttpGet(baseRoute)]
        public async Task<IActionResult> GetUserDevices([FromQuery]int page_number, [FromQuery]int page_size)
        {
            return Ok(await Mediator.Send(new GetUserDevices.Query(page_size, page_number)));
        }
    }
}