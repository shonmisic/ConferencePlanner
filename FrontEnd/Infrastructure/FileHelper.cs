using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FrontEnd.Infrastructure
{
    public class FileHelper
    {
        public static async Task<string> ProcessFormFile(IFormFile formFile,
            ModelStateDictionary modelState)
        {
            var fileName = WebUtility.HtmlEncode(
                Path.GetFileName(formFile.FileName));

            if (formFile.Length == 0)
            {
                modelState.AddModelError(formFile.Name,
                    $"The file ({fileName}) is empty.");
            }
            else
            {
                try
                {
                    string fileContents;

                    using (
                        var reader =
                        new StreamReader(
                            formFile.OpenReadStream(),
                            new UTF8Encoding(encoderShouldEmitUTF8Identifier: false,
                                throwOnInvalidBytes: true),
                            detectEncodingFromByteOrderMarks: true))
                    {
                        fileContents = await reader.ReadToEndAsync();

                        if (fileContents.Length > 0)
                        {
                            return fileContents;
                        }
                        else
                        {
                            modelState.AddModelError(formFile.Name,
                                $"The file ({fileName}) must be a text file.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    modelState.AddModelError(formFile.Name,
                        $"The file ({fileName}) upload failed. " +
                        $"Please contact the Help Desk for support. Error: {ex.Message}");
                }
            }

            return string.Empty;
        }
    }
}
