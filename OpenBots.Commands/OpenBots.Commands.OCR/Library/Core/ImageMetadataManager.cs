using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TextXtractor.Ocr.Core
{
    public class ImageMetaData
    {
        public string Directory { get; set; }

        public string Tag { get; set; }

        public string Description { get; set; }
    }


    public class ImageMetadataManager
    {
        public List<ImageMetaData> Extract(Stream imageStream)
        {
            List<ImageMetaData> metaDatas = new List<ImageMetaData>();
            IEnumerable<MetadataExtractor.Directory> directories = ImageMetadataReader.ReadMetadata(imageStream);
            foreach (var directory in directories)
            {
                foreach (var tag in directory.Tags)
                {
                    ImageMetaData mData = new ImageMetaData()
                    {
                        Directory = directory.Name,
                        Tag = tag.Name,
                        Description = tag.Description
                    };
                    metaDatas.Add(mData);
                }
            }

            return metaDatas;
        }
    }
}
