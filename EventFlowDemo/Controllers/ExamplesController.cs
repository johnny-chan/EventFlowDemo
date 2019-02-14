using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Queries;
using EventFlow.ReadStores;
using Microsoft.AspNetCore.Mvc;

namespace EventFlowDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExamplesController : ControllerBase
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryProcessor _queryProcessor;
        

        public ExamplesController(
            ICommandBus commandBus, 
            IQueryProcessor queryProcessor)
        {
            _commandBus = commandBus;
            _queryProcessor = queryProcessor;
        }

        // GET api/examples/a6e02d4d-871e-4d18-be8a-b647706a2a11
        [HttpGet("{id}")]
        [ProducesResponseType(200)]
        public async Task<ActionResult<ExampleReadModel>> GetExample(string id)
        {
            var readModel = await _queryProcessor.ProcessAsync(new ReadModelByIdQuery<ExampleReadModel>(id), CancellationToken.None);

            return Ok(readModel);
        }

        // POST api/examples
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<ActionResult> Post([FromBody] int value)
        {
            var exampleCommand = new ExampleCommand(ExampleId.New, value);

            await _commandBus.PublishAsync(exampleCommand, CancellationToken.None);

            return CreatedAtAction(nameof(GetExample), new { id = exampleCommand.AggregateId.Value}, exampleCommand);
        }       
    }
}
