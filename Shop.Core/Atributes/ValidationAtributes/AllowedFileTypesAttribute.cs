using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Core.Atributes.ValidationAtributes
{
    public class AllowedFileTypesAttribute : ValidationAttribute
    {
        private readonly string[] _types;

        public AllowedFileTypesAttribute(params string[] types)
        {
            _types = types;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            List<IFormFile> files = new List<IFormFile>();


            if (value is IFormFile)
                files.Add((IFormFile)value);
            else if (value is List<IFormFile>)
                files = value as List<IFormFile>;

            foreach (var item in files)
            {
                if (!_types.Contains(item.ContentType))
                    return new ValidationResult("File type must be one of the types: " + string.Join(",", _types));
            }

            return ValidationResult.Success;
        }
    }
}
