using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PhoneNumberParser.Data;
using PhoneParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.Extensions.Caching.Memory;
using System.IO;
using Microsoft.AspNetCore.Hosting;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PhoneNumberParser.Api
{
    /// <summary>
    /// Controller api per punto di immissione documento json contenete i numeri di telefono da processare
    /// Il metodo post permette di immettere il file json
    /// Il metodo get permette di ottenere i risultato.
    /// Il risultato è una collezione di record formati da questi campi:
    /// Input: Il record di input processato
    /// Id: l'identificativo del record di ingresso
    /// Phone: Il numero telefonico risultato del processo
    /// Result: Il risultato dell'operazione di processo
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class PhoneParserController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        IMemoryCache _memoryCache;
        Digester _digester;
        List<PhoneRecord> _records;
        public PhoneParserController(IConfiguration configuration, IWebHostEnvironment env, Digester digester)
        {
            _digester = digester;
            var digesterOptions = new DigesterOptions();
            configuration.GetSection(DigesterOptions.PhoneParser).Bind(digesterOptions);
            _digester.AcceptanceRules = digesterOptions.AcceptanceRules;
            _digester.CorrectionRules = digesterOptions.CorrectionRules;
            _digester.AllOtherRules = digesterOptions.AllOtherRules;

            _env = env;
        }

        // GET: api/<PhoneParserApi>
        [HttpGet]
        public async Task<string> Get()
        {
            string json = string.Empty;
            var path = Path.Combine(_env.ContentRootPath, "PhoneRecordsFile.json");
            if (System.IO.File.Exists(path))
            {
                json = await System.IO.File.ReadAllTextAsync(path);
                List<PhoneRecord> phoneRecords = JsonConvert.DeserializeObject<List<PhoneRecord>>(json);
                _digester.PhoneRecords = phoneRecords;
                await _digester.Digest();
            }
            return JsonConvert.SerializeObject(_digester.PhoneRecords, Formatting.Indented);
        }

        // POST: api/<PhoneParserApi>
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            string csv = string.Empty;
            try
            {
                using (var sr = new StreamReader(Request.Body))
                {
                    csv = await sr.ReadToEndAsync();
                }
                _records = ProcessCsv(csv);
                if (_records.Count() == 0)
                    return BadRequest("Nessun record valido riconosciuto");
                var jsonRecords = JsonConvert.SerializeObject(_records);
                var path = Path.Combine(_env.ContentRootPath, "PhoneRecordsFile.json");
                await System.IO.File.WriteAllTextAsync(path, jsonRecords);
                return Ok($"{_records.Count()} record caricati");
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        private List<PhoneRecord> ProcessCsv(string csv)
        {
            List<PhoneRecord> records = new List<PhoneRecord>();
            string[] lines = csv.Split(
                new[] { Environment.NewLine },
                StringSplitOptions.None);
            for (int i = 1; i < lines.Length; i++)
            {
                {
                    records.Add(new PhoneRecord
                    {
                        Input = lines[i]
                    });
                }

            }
            return records;
        }
    }
}
