using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Contract.V1;
using Tweetbook.Controllers.V1.Responses;
using Tweetbook.Domain;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = Policies.Roles.Admin + "," + Policies.Roles.Poster)] 
    public class TagsController : Controller
    {
        private readonly IDataService<Tag, string> tagService;

        public TagsController(IDataService<Tag, string> tagService)
        {
            this.tagService = tagService;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            var tags = await this.tagService.GetAllAsync();

            return Ok(tags.Select(tag => new TagResponse
            {
                Name = tag.Name,
                CreatorId = tag.CreatorId,
                CreatedOn = tag.CreatedOn
            }));
        }

        // 6a. However, only authorize Admin users to delete tags
        [HttpDelete(ApiRoutes.Tags.Delete)]
        [Authorize(Roles = Policies.Roles.Admin)]
        public async Task<IActionResult> Delete([FromRoute]string tagName)
        {
            if (!await this.tagService.DeleteAsync(tagName))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
