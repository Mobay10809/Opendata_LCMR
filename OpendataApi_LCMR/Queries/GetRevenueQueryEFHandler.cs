namespace OpendataApi_LCMR.Queries
{
    using AutoMapper;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using OpendataApi_LCMR.DTO;

    public class GetRevenueQueryEFHandler : IRequestHandler<GetRevenueQueryEF, List<RevenueDto>>
    {
        private readonly OpendataApiDbContext _context;
        private readonly IMapper _mapper;

        public GetRevenueQueryEFHandler(OpendataApiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<RevenueDto>> Handle(GetRevenueQueryEF request, CancellationToken cancellationToken)
        {
            var revenues = await _context.Revenues
                .FromSqlInterpolated($"EXEC sp_GetRevenue @DataYYYMM = {request.DataYYYMM}, @CompanyCode = {request.CompanyCode}")
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<RevenueDto>>(revenues);
        }
    }
}
