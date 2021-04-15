using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace TextXtractor.Ocr.Core
{
    /// <summary>
    /// Encapsulates an Ocr Service and provides common methods to be used across Services.
    /// </summary>
    public class OcrEngine : IOcrEngine
    {

        const string PAGEREGEX = @"page\s*(\d*)\s*of\s*(\d*)";
        protected IOcrService _ocrService;
        protected IBarcodeService _barcodeService;
        //protected ServiceSettings<TSetting> _serviceSettings;
        protected OcrEngineSetting _engineSetting;
        protected IMicrCodeService micrCodeService;
        protected IDrivingLicenseService drivingLicenseService;
        protected bool useMicrService;
        protected bool useDrivingLicenseService;



        /// <summary>
        /// Use Factory to instantiate
        /// </summary>
        /// <param name="ocrService"></param>
        public OcrEngine(OcrEngineSetting engineSetting, IOcrService ocrService/*, IBarcodeService barcodeService*/, IMicrCodeService micrCodeService, IDrivingLicenseService drivingLicenseService)
        {
            _ocrService = ocrService ?? throw new ArgumentNullException(nameof(ocrService));
            _engineSetting = engineSetting ?? throw new ArgumentNullException(nameof(engineSetting));
            //_barcodeService = barcodeService;
            this.micrCodeService = micrCodeService;
            this.drivingLicenseService = drivingLicenseService;
            useMicrService = false;
            useDrivingLicenseService = false;
        }

        public OcrEngine()
        {
        }

        ///// <summary>
        ///// Extract Text Out of OcrResults
        ///// </summary>
        ///// <param name="result"></param>
        ///// <returns></returns>
        //public string ExtractText(OcrResult result)
        //{
        //    return _ocrService.ExtractText(result);
        //}

        /// <summary>
        /// Initialize the Ocr Service
        /// </summary>
        public void Init()
        {
            _ocrService.Init();
        }

        public void InitMicrService(IMicrCodeService micrCodeService)
        {
            useMicrService = true;
            this.micrCodeService = micrCodeService;
        }

        public void InitDrivingLicenseService(IDrivingLicenseService drivingLicenseService)
        {
            useDrivingLicenseService = true;
            this.drivingLicenseService = drivingLicenseService;
        }
        /// <summary>
        /// Execute Ocr Service given an Image
        /// </summary>
        /// <param name="imagePath">File System path of an Image</param>
        /// <returns>Output of Ocr</returns>
        public OcrResult PerformRecognition(string imagePath)
        {
            FileInfo fileInfo = new FileInfo(imagePath);
            if (!fileInfo.Exists)
                throw new FileNotFoundException();

            FileStream stream = fileInfo.OpenRead();

            return PerformRecognition(stream);
        }

        public OcrResult PerformRecognition(byte[] imageData)
        {
            MemoryStream stream = new MemoryStream(imageData);

            return PerformRecognition(stream);
        }

        /// <summary>
        /// Executes Underlying OCR Service
        /// </summary>
        /// <param name="imageSteam">Stream of Image</param>
        /// <returns>Ocr Results</returns>
        public OcrResult PerformRecognition(Stream imageSteam)
        {
            OcrResult result = new OcrResult();
            if (!useMicrService && !useDrivingLicenseService)
            {

                int iteratorPartition = 0;
                if (!string.IsNullOrEmpty(_engineSetting.RedisConnectionString) && _ocrService.SettingsCount() > 1 && _engineSetting.IsServiceRatelimitEnabled)
                {

                    string serviceName = _ocrService.ServiceName;
                    iteratorPartition = GetIteratorPartition(serviceName, _ocrService.SettingsCount());
                }
                result = _ocrService.PerformRecognition(imageSteam, iteratorPartition);

            }

            //if(_barcodeService != null)
            //{
            //  List<BarcodeBox> barcodes =  _barcodeService.PerformRecognition(imageSteam);
            //    if (barcodes != null)
            //        result.Barcodes = barcodes;
            //}


            if (useMicrService && micrCodeService != null)
            {
                var micrCodes = micrCodeService.PerformRecognition(imageSteam);
                if (micrCodes != null)
                    result.MicrCodes.Add(micrCodes);
            }

            if (useDrivingLicenseService && drivingLicenseService != null)
            {
                try
                {
                    var drivingLicenseInfo = drivingLicenseService.PerformRecognition(imageSteam);
                    if (drivingLicenseInfo != null)
                        result.DrivingLicenses.Add(drivingLicenseInfo);
                }
                catch { }
            }


            return result;
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _ocrService.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~OcrEngine()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        public static (int?, int?) ExtractPageNumber(string text)
        {
            int? pageX = null;
            int? OfPageY = null;
            Regex rex = new Regex(PAGEREGEX, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            string cleanText = text.ToLower().Replace("\r", "").Replace("\n", "");
            Match match = rex.Match(cleanText);
            if (match.Groups.Count == 3)
            {
                int pageNumber;
                if (int.TryParse(match.Groups[1].Value, out pageNumber))
                    pageX = pageNumber;

                int pageCount;
                if (int.TryParse(match.Groups[2].Value, out pageCount))
                    OfPageY = pageCount;

            }
            return (pageX, OfPageY);
        }

        public int GetIteratorPartition(string serviceName, int partitions = 10)
        {
            int keyNumber = 0;
            try
            {
                ConnectionMultiplexer connectionMultiplexer = ConnectionMultiplexer.ConnectAsync(_engineSetting.RedisConnectionString).Result;
                var redisDb = connectionMultiplexer.GetDatabase();
                var val = redisDb.StringIncrement($"ServiceIterator.{serviceName}.{DateTime.UtcNow.Date.Ticks}");
                keyNumber = (int)(val % partitions);
            }
            catch (Exception ex)
            {
                var random = new Random(DateTime.UtcNow.Millisecond);
                keyNumber = random.Next(0, partitions);
            }

            return keyNumber;
        }

    }
}
