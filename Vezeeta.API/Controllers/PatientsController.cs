using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.X86;
using System;
using Vezeeta.Core;
using Vezeeta.Core.Models;
using Vezeeta.Serivce;
using vezeeta.Repository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Vezeeta.Core.Dtos;

namespace Vezeeta.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]

    public class PatientsController : ControllerBase
    {
        private readonly IUnitOfWork _UnitOfWork;

        public PatientsController(IUnitOfWork UnitOfWork)
        {
            _UnitOfWork = UnitOfWork;
        }


        [HttpGet("GetAllPatients")]
        public async Task<IActionResult> GetAllPatients([FromQuery] PaginationDto paginationDto)
        {
            if (String.IsNullOrEmpty(paginationDto.search))
            {
                return Ok(await _UnitOfWork.ApplicationUser.GetAllSearch<PatientDto>
                    (paginationDto.page, paginationDto.pagesize, PatientDto.PatientSelector, criteria: u => u.AccountType == "Patient"));
            }
            var result = await _UnitOfWork.ApplicationUser.GetAllSearch<PatientDto>( paginationDto.page, paginationDto.pagesize, PatientDto.PatientSelector, PatientDto.PatientSearch(paginationDto.search) , u => u.AccountType == "Patient");
            return Ok(result);
        }


        [HttpGet("GetPateintById")]
        public async Task<IActionResult> GetPateintById(int id)
        {
            var result = _UnitOfWork.ApplicationUser.GetById<PatientWithBookingDto>(p => p.Id == id && p.AccountType == "Patient", PatientWithBookingDto.PatientWithBookingSelector);
            if (result == null)
            {
                return BadRequest(new { Message = "there is No Patient with that id " });
            }
            return Ok(result);
        }

    }
}
