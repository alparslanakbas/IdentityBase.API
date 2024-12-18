namespace Identity.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        readonly AppDbContext _context;
        readonly ILogger<UsersController> _logger;

        public UsersController(AppDbContext context, ILogger<UsersController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();

                if (!users.Any())
                {
                    return NotFound("Users not found");
                }

                _logger.LogInformation("Users fetched successfully");
                return Ok(users);
            }
            catch (Exception)
            {
                _logger.LogError("An error occurred while fetching users");
                return StatusCode(500, "An error occurred while fetching users");
            }
        }
    }
}
