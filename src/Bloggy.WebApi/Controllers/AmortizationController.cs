using Bloggy.Database;
using Bloggy.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.WebApi.Controllers
{
    [ApiController]
    [Route("amortization")]
    public class AmortizationController : ControllerBase
    {
        private readonly BloggyDbContext _context;

        public AmortizationController(BloggyDbContext context)
        {
            _context = context;
        }

        [HttpGet("getSchedule")]
        public async Task<IActionResult> GetSchedule(
            [FromQuery] decimal loan = 36000m,
            [FromQuery] decimal interestPerAnnum = 8m,
            [FromQuery] int numberOfMonthlyPayments = 36,
            [FromQuery] decimal recycleInterestPerAnnum = 4.5m,
            [FromQuery] int recycleNumberOfMonthlyPayments = 48
            )
        {
            var schedule = await _context.Database.SqlQueryRaw<AmortizationSchedule>(
                "EXEC GetAmortizationSchedule @Loan, @InterestPerAnnum, @NumberOfMonthlyPayments, @RecycleInterestPerAnnum, @RecycleNumberOfMonthlyPayments",
                new SqlParameter("@Loan", loan),
                new SqlParameter("@InterestPerAnnum", interestPerAnnum),
                new SqlParameter("@NumberOfMonthlyPayments", numberOfMonthlyPayments),
                new SqlParameter("@RecycleInterestPerAnnum", recycleInterestPerAnnum),
                new SqlParameter("@RecycleNumberOfMonthlyPayments", recycleNumberOfMonthlyPayments)
                ).ToListAsync();

            return Ok(schedule);
        }
    }
}
