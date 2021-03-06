﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FormManager.Api.ViewModels;
using FormManager.Application.Forms.Queries;
using FormManager.Domain.Entities;
using FormManager.Application.Forms.Commands;
using FormManager.Application.Common.Models;

namespace FormManager.Api.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FormsController : BaseApiController
    {
        [HttpGet]
        [Authorize(Roles ="admin")]
        public async Task<ActionResult<Pagination<Form>>> GetForms([FromQuery]GetFormsQuery query)
        {
            return await Mediator.Send(query);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Form>> GetForm(Guid id)
        {
            return await Mediator.Send(new GetFormByIdQuery(id));
        }

        [HttpPost]
        public async Task<IActionResult> PostForm([FromBody]CreateFormCommand form)
        {
            Guid guid = await Mediator.Send(form);
            return CreatedAtAction("GetForm", new { id = guid}, form);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteForm(Guid id)
        {
            bool deleted = await Mediator.Send(new DeleteFormCommand(id));
            return deleted ? Ok() : BadRequest();
        }
    }
}
