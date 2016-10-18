using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Org.Apache.REEF.IMRU.API;
using Org.Apache.REEF.IMRU.API.Fakes;
using Org.Apache.REEF.Tang.Implementations.Tang;
using Org.Apache.REEF.Tang.Interface;
using Org.Apache.REEF.Tang.Util;
using Xunit;

namespace Org.Apache.REEF.IMRU.Tests
{
    public class IMRUJobDefinitionBuilderTests
    {
        [Fact]
        public void JobDefinitionBuilder_CancellationConfigIsOptional()
        {
            var definition = CreateTestBuilder()
               .Build();

            Assert.Null(definition.JobCancelSignalConfiguration);
        }

        [Fact]
        public void JobDefinitionBuilder_CancellationConfigIsSetToNull()
        {
            var definition = CreateTestBuilder()
               .SetJobCancellationConfiguration(null)
               .Build();

            Assert.Null(definition.JobCancelSignalConfiguration);
        }

        [Fact]
        public void JobDefinitionBuilder_SetsJobCancellationConfig()
        {
            var cancelSignalConfig = TangFactory.GetTang().NewConfigurationBuilder()
                .BindImplementation(GenericType<IJobCancelledDetector>.Class,
                    GenericType<JobCancellationDetectorAlwaysFalse>.Class)
                .Build();

            var definition = CreateTestBuilder()
                .SetJobCancellationConfiguration(cancelSignalConfig)
                .Build();

            Assert.NotNull(definition.JobCancelSignalConfiguration);
            Assert.Same(cancelSignalConfig, definition.JobCancelSignalConfiguration);
        }

        private IMRUJobDefinitionBuilder CreateTestBuilder()
        {
            var testConfig = TangFactory.GetTang().NewConfigurationBuilder().Build();

            return new IMRUJobDefinitionBuilder()
                .SetJobName("Test")
                .SetMapFunctionConfiguration(testConfig)
                .SetMapInputCodecConfiguration(testConfig)
                .SetUpdateFunctionCodecsConfiguration(testConfig)
                .SetReduceFunctionConfiguration(testConfig)
                .SetUpdateFunctionConfiguration(testConfig);
        }
    }
}
