using Grpc.Core;
using Production.API.Infrastructure;

namespace Production.API.Grpc;

public class ProductionService : ProductionGrpc.ProductionGrpcBase
{
    private readonly ProductionContext _productionContext;
    private readonly ILogger _logger;

    public ProductionService(ProductionContext dbContext, ILogger<ProductionService> logger)
    {
        _productionContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger;
    }

    public override Task<MilkMeasurementResponse> CreateMilkMeasurement(CreateMilkMeasurementRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Begin grpc call ProductionService.CreateMilkMeasurement for animal id {request.AnimalId}", request.AnimalId);


        //if OK
        //var customerBasket = MapToCustomerBasket(request);
        //var response = await _repository.UpdateBasketAsync(customerBasket);
        //return MapToCustomerBasketResponse(response);

        //if invalid
        //context.Status = new Status(StatusCode.NotFound, $"Basket with buyer id {request.Buyerid} do not exist");
        //return null;


        return base.CreateMilkMeasurement(request, context);
    }
}
