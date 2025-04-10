namespace OpendataApi_LCMR.Queries
{
    using MediatR;
    using OpendataApi_LCMR.DTO;

    public record GetRevenueQuery(string? DataYYYMM, string? CompanyCode) : IRequest<List<RevenueDto>>;
    public record GetRevenueQueryEF(string? DataYYYMM, string? CompanyCode) : IRequest<List<RevenueDto>>;
}
