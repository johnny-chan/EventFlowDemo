using EventFlow.Core;

namespace EventFlowDemo
{
    public class ExampleId : Identity<ExampleId>
    {
        public ExampleId(string value) : base(value) { }
    }
}
