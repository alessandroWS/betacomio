using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using betacomio.Dtos.AdminRequest;
using betacomio.Models;
using betacomio.Services.AdminRequestService;

namespace betacomio.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AdminRequestController : ControllerBase
    {
        private readonly IAdminRequestService _requestservice;
        private readonly IMapper _mapper;

        public AdminRequestController(IAdminRequestService requestservice, IMapper mapper)
        {
            _requestservice = requestservice;
            _mapper = mapper;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<AdminRequest>>>> Get()
        {
            var adminRequest = await _requestservice.GetAllReq();
            var adminRequestDtos = adminRequest;
            return Ok(adminRequestDtos);
        }

        [HttpGet("GetAllCount")]
        public async Task<ServiceResponse<int>> GetCount()
        {
            var adminRequest = await _requestservice.GetAllReqCount();
            var adminRequestDtos = adminRequest;
            return adminRequestDtos;
        }

        [HttpGet("UserRequestCount")]
        public async Task<ServiceResponse<int>> GetUserRequestCount()
        {
            int userId = GetUserIdFromClaim();
            var adminRequestCount = await _requestservice.GetUserRequestCount(userId);
            return adminRequestCount;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<PutReqDto>>> UpdateReq(int id, [FromBody] PutReqDto putDto)
        {
            if (putDto == null)
            {
                return BadRequest("Il DTO PutReqDto è null.");
            }

            // Verifica se UserId è valido (diverso da zero o dal valore di default)
            if (putDto.UserId <= 0)
            {
                return BadRequest("UserId non è stato inizializzato nel DTO PutReqDto.");
            }

            var response = await _requestservice.UpdateReq(id, putDto);

            // Restituisce una risposta HTTP in base al risultato dell'aggiornamento
            return response.Data is null ? NotFound(response) : Ok(response);
        }

        [HttpPost("AddReq")]
        public async Task<ActionResult<ServiceResponse<AdminRequest>>> PostAdminRequest(PostAdminRequestDto postDto)
        {
            int userId = GetUserIdFromClaim(); // Metodo per ottenere UserId dal Claim del JWT
            if (userId == 0)
            {
                return BadRequest("UserId non trovato nel Claim del JWT.");
            }

            var adminRequest = new AdminRequest
            {
                UserId = userId,
            };

            var response = await _requestservice.CreateAdminRequest(adminRequest);
            return response.Success ? Ok(response) : BadRequest(response.Message);
        }
        [HttpDelete("DeleteReq")]
        public async Task<ActionResult<ServiceResponse<string>>> DeleteAdminRequest()
        {
            int userId = GetUserIdFromClaim();

            // Verifica se l'utente ha almeno una richiesta in sospeso
            var userRequestCount = await _requestservice.GetUserRequestCount(userId);
            if (userRequestCount.Data == 0)
            {
                return BadRequest("Non hai richieste da eliminare.");
            }

            // Elimina la prima richiesta in sospeso dell'utente loggato
            var response = await _requestservice.DeleteUserRequest(userId);
            return response.Success ? Ok("Richiesta eliminata con successo.") : BadRequest(response.Message);
        }

        private int GetUserIdFromClaim()
        {
            var claim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (claim != null && int.TryParse(claim.Value, out int userId))
            {
                return userId;
            }
            return 0;
        }
    }
}
