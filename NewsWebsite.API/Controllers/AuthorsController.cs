using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NewsWebsite.API.Dtos;
using NewsWebsite.API.Models;
using NewsWebsite.API.Services;

namespace NewsWebsite.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        [HttpGet("GetAvailable")]
        public async Task<IActionResult> GetAvailableAsync()
        {
            var authors = await _authorService.GetAvailableAsync();
            if (authors.Count() == 0)
                return NotFound();

            return Ok(authors);
        }

        [HttpGet("GetDeleted")]
        public async Task<IActionResult> GetDeletedAsync()
        {
            var authors = await _authorService.GetDeletedAsync();

            if (authors.Count() == 0)
                return NotFound();

            return Ok(authors);
        }
        
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var author = await _authorService.GetByIdAsync(id);

            if (author is null)
                return NotFound();

            return Ok(author);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(AuthorDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var author = new Author { Name = model.Name, Email = model.Email };

            var Created = await _authorService.CreateAsync(author);

            if (Created.Id == 0)
                return BadRequest();

            return Ok(Created);
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, AuthorDto model)
        {
            var author = await _authorService.GetByIdAsync(id);

            if (author is null)
                return NotFound();

            author.Name = model.Name;
            author.Email = model.Email;

            return Ok(_authorService.Update(author));
        }
        [HttpGet("Toggle/{id}")]
        public async Task<IActionResult> ToggleAsync(int id)
        {
            var author = await _authorService.GetByIdAsync(id);

            if (author is null)
                return NotFound();

            author.IsDeleted = !author.IsDeleted;

            return Ok(_authorService.Update(author));
        }
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var author = await _authorService.GetByIdAsync(id);

            if (author is null)
                return NotFound();

            return Ok(_authorService.Delete(author));
        }
    }
}
