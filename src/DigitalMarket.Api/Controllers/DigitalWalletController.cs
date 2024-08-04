﻿using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.DigitalWalletCommands;
using DigitalMarket.Business.CQRS.Queries.DigitalWalletQueries;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class DigitalWalletController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DigitalWalletController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ApiResponse<IEnumerable<DigitalWalletResponse>>> GetAll()
        {
            var response = await _mediator.Send(new GetAllDigitalWalletQuery());
            return response;
        }

        [HttpGet("{id}")]
        public async Task<ApiResponse<DigitalWalletResponse>> Get(long id)
        {
            var response = await _mediator.Send(new GetDigitalWalletByIdQuery(id));
            return response;
        }

        [HttpGet("point-balance/{userId}")]
        public async Task<ApiResponse<PointBalanceResponse>> GetPointBalance(long userId)
        {
            var response = await _mediator.Send(new GetPointBalanceQuery { UserId = userId });
            return response;
        }

        [HttpPut("{id}")]
        public async Task<ApiResponse<DigitalWalletResponse>> Put(long id, [FromBody] DigitalWalletRequest digitalWalletRequest)
        {
            var response = await _mediator.Send(new UpdateDigitalWalletCommand(id, digitalWalletRequest));
            return response;
        }
    }
}
