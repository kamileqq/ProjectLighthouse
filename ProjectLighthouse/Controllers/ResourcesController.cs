using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Kettu;
using LBPUnion.ProjectLighthouse.Helpers;
using LBPUnion.ProjectLighthouse.Logging;
using LBPUnion.ProjectLighthouse.Types.Files;
using LBPUnion.ProjectLighthouse.Types.Lists;
using Microsoft.AspNetCore.Mvc;
using IOFile = System.IO.File;

namespace LBPUnion.ProjectLighthouse.Controllers
{
    [ApiController]
    [Produces("text/xml")]
    [Route("LITTLEBIGPLANETPS3_XML")]
    public class ResourcesController : ControllerBase
    {
        [HttpPost("showModerated")]
        public IActionResult ShowModerated() => this.Ok(new ResourcesList(new List<string>()));

        [HttpPost("filterResources")]
        [HttpPost("showNotUploaded")]
        public async Task<IActionResult> FilterResources()
        {
            string bodyString = await new StreamReader(this.Request.Body).ReadToEndAsync();

            XmlSerializer serializer = new(typeof(ResourceList));
            ResourceList resourceList = (ResourceList)serializer.Deserialize(new StringReader(bodyString));

            if (resourceList == null) return this.BadRequest();

            List<string> resources = resourceList.Resources.Where(s => !FileHelper.ResourceExists(s)).ToList();
            return this.Ok(new ResourcesList(resources));
        }

        [ResponseCache(Duration = 86400)]
        [HttpGet("/gameAssets/{hash}")]
        [HttpGet("r/{hash}")]
        public IActionResult GetResource(string hash)
        {
            string path = FileHelper.GetResourcePath(hash);

            if (FileHelper.ResourceExists(hash)) return this.File(IOFile.OpenRead(path), "application/octet-stream");

            return this.NotFound();
        }

        // TODO: check if this is a valid hash
        [HttpPost("upload/{hash}")]
        public async Task<IActionResult> UploadResource(string hash)
        {
            string assetsDirectory = FileHelper.ResourcePath;
            string path = FileHelper.GetResourcePath(hash);

            FileHelper.EnsureDirectoryCreated(assetsDirectory);
            if (FileHelper.ResourceExists(hash)) this.Ok(); // no reason to fail if it's already uploaded

            Logger.Log($"Processing resource upload (hash: {hash})", LoggerLevelResources.Instance);
            LbpFile file = new(await BinaryHelper.ReadFromPipeReader(this.Request.BodyReader));

            if (!FileHelper.IsFileSafe(file))
            {
                Logger.Log($"File is unsafe (hash: {hash}, type: {file.FileType})", LoggerLevelResources.Instance);
                return this.UnprocessableEntity();
            }

            string calculatedHash = HashHelper.Sha1Hash(file.Data).ToLower();
            if (calculatedHash != hash)
            {
                Logger.Log
                (
                    $"File hash does not match the uploaded file! (hash: {hash}, calculatedHash: {calculatedHash}, type: {file.FileType})",
                    LoggerLevelResources.Instance
                );
                return this.Conflict();
            }

            Logger.Log($"File is OK! (hash: {hash}, type: {file.FileType})", LoggerLevelResources.Instance);
            await IOFile.WriteAllBytesAsync(path, file.Data);
            return this.Ok();
        }
    }
}