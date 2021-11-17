﻿using Application.Interfaces;
using FileSignatures;
using Helpers.Enums;
using Helpers.Models;
using Helpers.Resources;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.Services
{
    public class FileValidator : IFileValidator
    {
        private readonly IFileFormatInspector _fileFormatInspector;

        public FileValidator(IFileFormatInspector fileFormatInspector)
        {
            _fileFormatInspector = fileFormatInspector;
        }
        public Result IsValidFile(byte[] file, int fileSizeInMega, FileExtensions[] allowedExtensions)
        {
            using(var stream = new MemoryStream(file))
                return IsValidFile(stream, fileSizeInMega, allowedExtensions);
        }

        public Result IsValidFile(MemoryStream file, int fileSizeInMega, FileExtensions[] allowedExtensions)
        {
            var isValidFileLength = file.Length <= fileSizeInMega * 1024 * 1024;

            if (!isValidFileLength) return Result.Fail(ResourceKeys.NotValidFileSize);

            var format = _fileFormatInspector.DetermineFileFormat(file);
            var isValidFileType = format != null && allowedExtensions.Any(ext => ext.ToString() == format.Extension.ToLower());

            if (!isValidFileType) return Result.Fail(ResourceKeys.NotValidFileType);

            return Result.Success();
        }

        public Result IsValidFile(IFormFile file, int fileSizeInMega, FileExtensions[] allowedExtensions)
        {
            using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                return IsValidFile(stream, fileSizeInMega, allowedExtensions);
            }
        }
    }
}
