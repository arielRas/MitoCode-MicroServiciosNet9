﻿using FastBuy.Stocks.Contracts.DTOs;
using FastBuy.Stocks.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace FastBuy.Stocks.Api.Routes
{
    public static class StocksRoute
    {
        public static IApplicationBuilder MapStocksRoute(this WebApplication app)
        {

            var group = app.MapGroup("/api/stocks")
                           .RequireAuthorization(policy => policy.RequireRole("Admin"))
                           .WithTags("Stocks");

            group.MapGet("/{id:Guid}", GetById);

            group.MapPatch("/{id:Guid}", SetStock);

            group.MapPatch("",  DecreaseStock);

            return app;
        }

        private static async Task<IResult> GetById([FromRoute] Guid id, IStockItemService service)
        {
            if(id == Guid.Empty)
                return Results.BadRequest("The id field cannot be empty or null");

            var stockItem = await service.GetByProductIdAsync(id);

            return Results.Ok(stockItem);
        }

        private static async Task<IResult> SetStock([FromRoute]Guid id, [FromBody] int stock, IStockItemService service)
        {
            if (id == Guid.Empty)
                return Results.BadRequest("The id field cannot be empty or null");

            if (stock < 0)
                return Results.BadRequest("The stock field cannot be less than zero");

            var result = await service.SetStockAsync(id, stock);

            return result ? Results.NoContent() : Results.BadRequest();
        }


        private static async Task<IResult> DecreaseStock([FromBody]StockDecreaseRequestDto stockDecreaseDto, IStockItemService service)
        {
            if(stockDecreaseDto == null)
                return Results.BadRequest("The request body cannot be empty");

            if(stockDecreaseDto.ProductId == Guid.Empty)
                return Results.BadRequest("The productId field cannot be empty or null");

            if(stockDecreaseDto.Quantity <= 0)
                return Results.BadRequest("The quantity field cannot be zero or less than zero");

            var result = await service.DecreaseStockAsync(stockDecreaseDto);

            return result ? Results.NoContent() 
                          : Results.BadRequest("the stock you are trying to subtract is greater than the existing one");
        }
    }
}
