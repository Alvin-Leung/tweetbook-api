using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Contract.V1;
using Tweetbook.Controllers.V1.Responses;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1
{
    public class TagsController : Controller
    {
        private readonly IPostService postService;

        public TagsController(IPostService postService)
        {
            this.postService = postService;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var tags = await this.postService.GetAllTagsAsync();

            return Ok(tags.Select(tag => new TagResponse
            {
                Name = tag.Name,
                CreatorId = tag.CreatorId,
                CreatedOn = tag.CreatedOn
            }));
        }
    }
}
