using AutoMapper;
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
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly IAuthorService _authorService;
        private readonly IMapper _mapper;
        public NewsController(INewsService newsService, IAuthorService authorService, IMapper mapper)
        {
            _newsService = newsService;
            _authorService = authorService;
            _mapper = mapper;
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(NewsDto model)
        {
            if (model.Image is null)
                return BadRequest();

            if (await _authorService.GetByAvailableIdAsync(model.AuthorId) is null)
                return NotFound();

            var news = await _newsService.CreateAsync(_mapper.Map<News>(model));

            if (news.Id == 0)
                return BadRequest();

            return Ok(_mapper.Map<NewsViewDto>(news));
        }
        [HttpPost("Update/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, NewsDto model)
        {
            string OldImage;
            string OldPublicId;

            var news = await _newsService.GetByIdAsync(id);
            if (news is null)
                return NotFound();

            OldImage = news.Image;
            OldPublicId = news.PublicId;

            if (await _authorService.GetByAvailableIdAsync(model.AuthorId) is null)
                return NotFound();

            news = _mapper.Map(model, news);

            if (model.Image is null ||model.PublicId is null)
            {
                news.Image = OldImage;
                news.PublicId = OldPublicId;
            }

            return Ok(_mapper.Map<NewsViewDto>(_newsService.Update(news)));
        }
        [AllowAnonymous]
        [HttpGet("GetAvailable")]
        public async Task<IActionResult> GetAvailableAsync()
        {
            var news=await _newsService.GetAvailableAsync();
            if (news.Count() == 0)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<NewsViewDto>>(news));
        }
        [HttpGet("GetDeleted")]
        public async Task<IActionResult> GetDeletedAsync()
        {
            var news = await _newsService.GetDeletedAsync();
            if (news.Count() == 0)
                return NotFound();

            return Ok(_mapper.Map<IEnumerable<NewsViewDto>>(news));
        }
        [AllowAnonymous]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var news = await _newsService.GetByIdAsync(id);
            if (news is null)
                return NotFound();

            return Ok(_mapper.Map<NewsViewDto>(news));
        }
        [HttpGet("Toggle/{id}")]
        public async Task<IActionResult> ToggleAsync(int id)
        {
            var news = await _newsService.GetByIdAsync(id);
            if (news is null)
                return NotFound();

            news.IsDeleted = !news.IsDeleted;

            return Ok(_mapper.Map<NewsViewDto>(_newsService.Update(news)));
        }
        [HttpGet("Delete/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var news = await _newsService.GetByIdAsync(id);
            if (news is null)
                return NotFound();

            return Ok(_mapper.Map<NewsViewDto>(_newsService.Delete(news)));
        }
    }
}
