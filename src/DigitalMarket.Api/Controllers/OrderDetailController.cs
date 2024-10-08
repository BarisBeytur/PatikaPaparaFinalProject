﻿using DigitalMarket.Base.Response;
using DigitalMarket.Business.CQRS.Commands.OrderDetailCommands;
using DigitalMarket.Business.CQRS.Queries.OrderDetailQueries;
using DigitalMarket.Schema.Request;
using DigitalMarket.Schema.Response;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DigitalMarket.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderDetailController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<IEnumerable<OrderDetailResponse>>> GetAll()
        {
            var response = await _mediator.Send(new GetAllOrderDetailQuery());
            return response;
        }

        [HttpGet("Order/{orderId}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse<List<OrderDetailResponse>>> GetByOrderId(long orderId)
        {
            var response = await _mediator.Send(new GetOrderDetailByOrderIdQuery { OrderId = orderId });
            return response;
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse<OrderDetailResponse>> Get(long id)
        {
            var response = await _mediator.Send(new GetOrderDetailByIdQuery(id));
            return response;
        }


        [HttpPost]
        [Authorize(Roles = "admin,user")]
        public async Task<ApiResponse<OrderDetailResponse>> Post(OrderDetailRequest couponRequest)
        {
            var response = await _mediator.Send(new CreateOrderDetailCommand(couponRequest));
            return response;
        }

        [HttpDelete]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse> Delete(long id)
        {
            var response = await _mediator.Send(new DeleteOrderDetailCommand { Id = id });
            return response;
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ApiResponse<OrderDetailResponse>> Put(long id, [FromBody] OrderDetailRequest couponRequest)
        {
            var response = await _mediator.Send(new UpdateOrderDetailCommand(id, couponRequest));
            return response;
        }
    }
}
